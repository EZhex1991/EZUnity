/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-07-19 10:53:58
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEngine;
#if UNITYPURCHASING
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
#endif

namespace EZhex1991.EZUnity.UniSDK
{
    public class UnityPurchasing : UnityPurchasingBase
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
            UnityEngine.Purchasing.UnityPurchasing.Initialize(this, builder);
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
                _OnPurchaseFailed(productId, "Not Initialized.");
                return;
            }
            Product product = m_Controller.products.WithID(productId);
            if (product == null || !product.availableToPurchase)
            {
                _OnPurchaseFailed(product.definition.id, "Product not available.");
                return;
            }
            if (inProgress == true)
            {
                _OnPurchaseFailed(product.definition.id, "Purchasing in progress.");
                return;
            }
            inProgress = true;
            _OnPurchaseFlowStarted(product.definition.id, payload);
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
                    Log(string.Join(" - ", new string[]{
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
            _OnInitFinished(true, "");
        }
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            _OnInitFinished(false, error.ToString());
        }
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
        {
            if (Validate(e))
            {
                _OnPurchaseSucceeded(e.purchasedProduct.definition.id, e.purchasedProduct.receipt);
            }
            else
            {
                _OnPurchaseFailed(e.purchasedProduct.definition.id, "Invalid receipt!");
            }
            return PurchaseProcessingResult.Complete;
        }
        public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
        {
            _OnPurchaseFailed(product.definition.id, reason.ToString());
        }

        private void OnDeferred(Product product)
        {
            _OnDeferred(product.definition.id);
        }
        private bool Validate(PurchaseEventArgs e)
        {
            Log(string.Format("Validate:\n definitionId={0}\n receipt={1}", e.purchasedProduct.definition.id, e.purchasedProduct.receipt));
            try
            {
                CrossPlatformValidator validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);
                IPurchaseReceipt[] result = validator.Validate(e.purchasedProduct.receipt);
                foreach (IPurchaseReceipt productReceipt in result)
                {
                    Log(string.Format("Validate Result:\n productId={0}\n purchaseDate={1}\n transactionId={2}", productReceipt.productID, productReceipt.purchaseDate, productReceipt.transactionID));
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