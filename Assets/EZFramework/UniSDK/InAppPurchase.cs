/*
 * Author:      熊哲
 * CreateTime:  1/12/2018 12:24:57 PM
 * Description:
 * 
*/
using System.Collections.Generic;
using UnityEngine;

namespace EZFramework.UniSDK
{
    public class InAppPurchase : EZSingleton<InAppPurchase>
    {
        [TextArea(5, 10)]
        public string googlePlayPublicKey;

        public bool positiveEvent = true;

        public bool inProgress { get; protected set; }

        public delegate void OnEventCallback(string info, string msg);

        public event OnEventCallback onInitSucceededEvent;
        public event OnEventCallback onInitFailedEvent;
        public event OnEventCallback onProductInfoEvent;

        public event OnEventCallback onPurchaseFlowStartedEvent;
        public event OnEventCallback onPurchaseSucceededEvent;
        public event OnEventCallback onPurchaseFailedEvent;
        public event OnEventCallback onDeferredEvent;

        public class CustomProduct
        {
            public string id;
            public int type;
            public Dictionary<string, string> storeIDs;
            public CustomProduct(string id, int type = 0)
            {
                this.id = id;
                this.type = type;
                this.storeIDs = new Dictionary<string, string>();
            }
            public void SetStoreID(string storeName, string id)
            {
                storeIDs.Add(storeName, id);
            }
        }

        public virtual void Init(List<CustomProduct> products)
        {
            Log("Init");
            if (positiveEvent) m_OnInitSucceeded("", "IAP disabled, positive events will be triggered.");
            else m_OnInitFailed("", "IAP disabled, negative events will be triggered.");
        }
        public virtual void Purchase(string productId, string payload = "")
        {
            inProgress = true;
            Log(string.Format("{0}\n{1}\n{2}", "Purchase", productId, payload));
            if (positiveEvent) m_OnPurchaseSucceeded(productId, payload);
            else m_OnPurchaseFailed(productId, "IAP disabled");
        }

        protected virtual void m_OnInitSucceeded(string info = "", string msg = "")
        {
            if (onInitSucceededEvent != null) onInitSucceededEvent(info, msg);
        }
        protected virtual void m_OnInitFailed(string info = "", string msg = "")
        {
            if (onInitFailedEvent != null) onInitFailedEvent(info, msg);
        }
        protected virtual void m_OnProductInfo(string info = "", string msg = "")
        {
            if (onProductInfoEvent != null) onProductInfoEvent(info, msg);
        }
        protected virtual void m_OnPurchaseFlowStarted(string info = "", string msg = "")
        {
            if (onPurchaseFlowStartedEvent != null) onPurchaseFlowStartedEvent(info, msg);
        }
        protected virtual void m_OnPurchaseSucceeded(string info = "", string msg = "")
        {
            if (onPurchaseSucceededEvent != null) onPurchaseSucceededEvent(info, msg);
            inProgress = false;
        }
        protected virtual void m_OnPurchaseFailed(string info = "", string msg = "")
        {
            if (onPurchaseFailedEvent != null) onPurchaseFailedEvent(info, msg);
            inProgress = false;
        }
        protected virtual void m_OnDeferred(string info = "", string msg = "")
        {
            if (onDeferredEvent != null) onDeferredEvent(info, msg);
        }

        public class ReceiptContent
        {
            public string Store;
            public string TransactionID;
            public string Payload;
        }
        public class PayloadContent
        {
            public string json;
            public string signature;
        }
        public class JsonContent
        {
            public string orderId;
            public string packageName;
            public string productId;
            public System.DateTime purchaseTime;
            public int purchaseState;
            public string developerPayload;
            public string purchaseToken;
        }
        public static ReceiptContent DecodeReceipt(string receiptText)
        {
            return JsonUtility.FromJson<ReceiptContent>(receiptText);
        }
        public static PayloadContent DecodePayload(string payloadText)
        {
            return JsonUtility.FromJson<PayloadContent>(payloadText);
        }
        public static JsonContent DecodeJson(string jsonText)
        {
            return JsonUtility.FromJson<JsonContent>(jsonText);
        }
    }
}