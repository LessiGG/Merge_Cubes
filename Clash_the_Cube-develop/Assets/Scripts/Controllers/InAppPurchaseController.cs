using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyMobile;
using Framework.Events;
using Databox;

namespace ClashTheCube
{
    public class InAppPurchaseController : MonoBehaviour
    {
        public static InAppPurchaseController Instance;

        [SerializeField] private DataboxObject databox;
        [SerializeField] private GameEvent purchaseCompletedEvent;
        [SerializeField] private GameEvent abilityPurchaseCompletedEvent;
        [SerializeField] private GameEvent purchaseFailedEvent;

        public bool IsSingleton
        {
            get
            {
                return this == Instance;
            }
        }

        void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(this);
                return;
            }

            if (!RuntimeManager.IsInitialized())
            {
                RuntimeManager.Init();
            }
        }

        private void Start()
        {
            if (!IsSingleton) return;

            InitPurchaseData();
        }

        private void OnEnable()
        {
            if (!IsSingleton) return;

            InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
            InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;

            InAppPurchasing.RestoreCompleted += RestoreCompletedHandler;
            InAppPurchasing.RestoreFailed += RestoreFailedHandler;
        }

        private void OnDisable()
        {
            if (!IsSingleton) return;

            InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
            InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;

            InAppPurchasing.RestoreCompleted -= RestoreCompletedHandler;
            InAppPurchasing.RestoreFailed -= RestoreFailedHandler;
        }

        public void PurchaseRemoveADs()
        {
            //InAppPurchasing.Purchase(EM_IAPConstants.Product_Remove_ADs);
        }

        public bool IsRemoveADsProductOwned()
        {
            return false;//InAppPurchasing.IsProductOwned(EM_IAPConstants.Product_Remove_ADs);
        }

        public void PurchaseAddAbility()
        {
            //InAppPurchasing.Purchase(EM_IAPConstants.Product_Add_Ability);
        }

        public void RestorePurchases()
        {
            InAppPurchasing.RestorePurchases();
        }

        private void PurchaseCompletedHandler(IAPProduct product)
        {
            SavePurchaseData(product);

            GameEvent gameEvent = null;

            //switch (product.Name)
            //{
            //    case EM_IAPConstants.Product_Remove_ADs:
            //        gameEvent = purchaseCompletedEvent;
            //        break;

            //    case EM_IAPConstants.Product_Add_Ability:
            //        gameEvent = abilityPurchaseCompletedEvent;
            //        break;
            //}

            if (gameEvent)
            {
                gameEvent.Raise();
            }

            Debug.Log(product.Name + " was purchased.");
        }

        private void PurchaseFailedHandler(IAPProduct product, string info)
        {
            if (purchaseFailedEvent)
            {
                purchaseFailedEvent.Raise();
            }

            Debug.Log("The purchase of product " + product.Name + " has failed.");
        }

        private void RestoreCompletedHandler()
        {
            Debug.Log("All purchases have been restored successfully.");
        }

        private void RestoreFailedHandler()
        {
            Debug.Log("The purchase restoration has failed.");
        }

        private void InitPurchaseData()
        {
            var table = DataBaseController.Data_Table;
            var entry = DataBaseController.Purchases_Entry;
            var removeADsField = DataBaseController.Purchases_RemoveADsField;

            if (!databox.EntryExists(table, entry))
            {
                databox.AddData(table, entry, removeADsField, new BoolType(false));
                databox.SaveDatabase();
            }
        }

        private void SavePurchaseData(IAPProduct product)
        {
            var table = DataBaseController.Data_Table;
            var entry = DataBaseController.Purchases_Entry;
            var removeADsField = DataBaseController.Purchases_RemoveADsField;

            //switch (product.Name)
            //{
            //    case EM_IAPConstants.Product_Remove_ADs:
            //        databox.AddData(table, entry, removeADsField, new BoolType(true));
            //        break;

            //    default:
            //        break;
            //}

            databox.SaveDatabase();
        }
    }
}
