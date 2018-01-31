/*
 * Author:      熊哲
 * CreateTime:  7/19/2017 10:53:58 AM
 * Description:
 * 
*/
using System.Collections.Generic;
using UnityEngine;
#if UNITYPURCHASING
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
#endif

namespace EZFramework.UniSDK.UnityNative
{
    public class InAppPurchase : UniSDK.InAppPurchase
#if UNITYPURCHASING
    , IStoreListener
#endif
    {
#if UNITYPURCHASING
        private IStoreController m_Controller;
        private IAppleExtensions m_AppleExtensions;

        public override void Init(List<CustomProduct> products)
        {
            StandardPurchasingModule module = StandardPurchasingModule.Instance();
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);
            builder.Configure<IGooglePlayConfiguration>().SetPublicKey(googlePlayPublicKey);
            foreach (CustomProduct product in products)
            {
                builder.AddProduct(product.id, (ProductType)product.type, GetStoreIDs(product));
            }
            UnityPurchasing.Initialize(this, builder);
        }
        private IDs GetStoreIDs(CustomProduct product)
        {
            IDs ids = new IDs();
            foreach (KeyValuePair<string, string> storeIDs in product.storeIDs)
            {
                string storeName = storeIDs.Key;
                string storeID = storeIDs.Value;
                ids.Add(storeID, storeName);
            }
            return ids;
        }
        public override void Purchase(string productId, string payload = "")
        {
            if (m_Controller == null)
            {
                Log("Purchase Failed. Not Initialized.");
                m_OnPurchaseFailed("Null", "Not Initialized.");
                return;
            }
            Product product = m_Controller.products.WithID(productId);
            Purchase(product, payload);
        }
        private void Purchase(Product product, string payload = "")
        {
            if (m_Controller == null)
            {
                Log("Purchase Failed. Not Initialized.");
                m_OnPurchaseFailed("Null", "Not Initialized.");
                return;
            }
            if (product == null || !product.availableToPurchase)
            {
                Log("Purchase Failed. Product not available.");
                m_OnPurchaseFailed("Null", "Product not available.");
                return;
            }
            if (inProgress == true)
            {
                Log("Purchase Failed. Purchasing in progress.");
                m_OnPurchaseFailed("Null", "Purchasing in progress.");
                return;
            }
            Log("Starting Purchase Flow...");
            inProgress = true;
            m_OnPurchaseFlowStarted(product.definition.id, payload);
            m_Controller.InitiatePurchase(product, payload);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            m_Controller = controller;
            m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
            m_AppleExtensions.RegisterPurchaseDeferredListener(OnDeferred);
            foreach (var product in controller.products.all)
            {
                if (product.availableToPurchase)
                {
                    m_OnProductInfo(string.Join(" - ", new string[]{
                        product.metadata.localizedTitle,
                        product.metadata.localizedDescription,
                        product.metadata.isoCurrencyCode,
                        product.metadata.localizedPrice.ToString(),
                        product.metadata.localizedPriceString,
                        product.transactionID,
                        product.receipt,
                    }));
                }
            }
            Log("Initialize Complete.");
            m_OnInitSucceeded();
        }
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            string message = "Billing failed to initialize!\n";
            switch (error)
            {
                case InitializationFailureReason.AppNotKnown:
                    message += "Is your App correctly uploaded on the relevant publisher console?";
                    break;
                case InitializationFailureReason.PurchasingUnavailable:
                    message += "Billing disabled!";
                    break;
                case InitializationFailureReason.NoProductsAvailable:
                    message += "No products available for purchase!";
                    break;
            }
            Log("Initialize Failed. " + message);
            m_OnInitFailed("", message);
        }
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
        {
            if (Validate(e))
            {
                Log("Purchase Succeed.");
                m_OnPurchaseSucceeded(e.purchasedProduct.definition.id, e.purchasedProduct.receipt);
            }
            else
            {
                Log("Invalid receipt!");
                m_OnPurchaseFailed(e.purchasedProduct.definition.id, "Invalid receipt!");
            }
            return PurchaseProcessingResult.Complete;
        }
        public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
        {
            Log("Purchase Failed. product=" + product.definition.id + ", reason=" + reason);
            m_OnPurchaseFailed(product.definition.id, reason.ToString());
        }

        private void OnDeferred(Product product)
        {
            Log("Deferred.");
            m_OnDeferred(product.definition.id);
        }
        private bool Validate(PurchaseEventArgs e)
        {
            Log(string.Format("definitionId: {0}, receipt: {1}", e.purchasedProduct.definition.id, e.purchasedProduct.receipt));
            try
            {
                CrossPlatformValidator validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);
                IPurchaseReceipt[] result = validator.Validate(e.purchasedProduct.receipt);
                foreach (IPurchaseReceipt productReceipt in result)
                {
                    Log(string.Format("productId: {0}, purchaseDate: {1}, transactionId: {2}", productReceipt.productID, productReceipt.purchaseDate, productReceipt.transactionID));
                }
                return true;
            }
            catch (IAPSecurityException)
            {
#if UNITY_EDITOR
                return positiveEvent;
#else
                return false;
#endif
            }
        }
#endif
    }
}