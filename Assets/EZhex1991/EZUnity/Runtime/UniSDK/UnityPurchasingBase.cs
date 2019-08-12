/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-01-12 12:24:57
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEngine;

namespace EZhex1991.EZUnity.UniSDK
{
    public class UnityPurchasingBase : EZMonoBehaviourSingleton<UnityPurchasingBase>
    {
        [TextArea(5, 10)]
        public string googlePlayPublicKey;

        public bool positiveEvent = true;

        public bool inProgress { get; protected set; }

        public event OnResultCallback onInitFinishedEvent;
        public event OnEventCallback2 onPurchaseFlowStartedEvent;
        public event OnEventCallback2 onPurchaseSucceededEvent;
        public event OnEventCallback2 onPurchaseFailedEvent;
        public event OnEventCallback2 onDeferredEvent;

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
            _OnInitFinished(positiveEvent, "Test Mode");
        }
        public virtual void Purchase(string productId, string payload = "")
        {
            inProgress = true;
            _OnPurchaseFlowStarted(productId, payload);
            if (positiveEvent) _OnPurchaseSucceeded(productId, "Test Mode");
            else _OnPurchaseFailed(productId, "Test Mode");
        }

        protected virtual void _OnInitFinished(bool result, string message)
        {
            Log(string.Format("InitFinished:\n result={0}\n message={1}", result, message));
            if (onInitFinishedEvent != null) onInitFinishedEvent(result, message);
        }
        protected virtual void _OnPurchaseFlowStarted(string productId, string payload)
        {
            Log(string.Format("PurchaseFlowStarted:\n productId={0}\n payload={1}", productId, payload));
            if (onPurchaseFlowStartedEvent != null) onPurchaseFlowStartedEvent(productId, payload);
        }
        protected virtual void _OnPurchaseSucceeded(string productId, string receipt)
        {
            Log(string.Format("PurchaseSucceeded:\n productId={0}\n receipt={1}", productId, receipt));
            if (onPurchaseSucceededEvent != null) onPurchaseSucceededEvent(productId, receipt);
            inProgress = false;
        }
        protected virtual void _OnPurchaseFailed(string productId, string message)
        {
            Log(string.Format("PurchaseFailed:\n productId={0}\n message={1}", productId, message));
            if (onPurchaseFailedEvent != null) onPurchaseFailedEvent(productId, message);
            inProgress = false;
        }
        protected virtual void _OnDeferred(string productId)
        {
            Log(string.Format("Deferred:\n productId={0}", productId));
            if (onDeferredEvent != null) onDeferredEvent(productId, "");
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