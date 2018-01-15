/*
 * Author:      熊哲
 * CreateTime:  1/9/2018 2:59:55 PM
 * Description:
 * 
*/
using System.Collections.Generic;
using UnityEngine;

namespace EZFramework.UniSDK
{
    public class DataAnalytics : EZSingleton<DataAnalytics>
    {
        public const string LOG_TAG = "DA ---> ";

        public class CustomEvent
        {
            public string eventName { get; private set; }
            public Dictionary<string, object> eventData { get; private set; }

            public CustomEvent(string eventName)
            {
                this.eventName = eventName;
                this.eventData = new Dictionary<string, object>();
            }

            public void SetData(string key, object data)
            {
                eventData[key] = data;
            }
            public void SetData<T>(string key, T data)
            {
                eventData[key] = data;
            }
            public void Clear()
            {
                eventData.Clear();
            }
        }

        public virtual void SetUserInfo(string userId, int gender = 2, int birthYear = 0)
        {
            Debug.Log(LOG_TAG + "Set user info");
        }
        public virtual void Transaction(string productId, decimal amount, string currency)
        {
            Debug.Log(LOG_TAG + "Transaction");
        }
        public virtual void Transaction(string productId, decimal amount, string currency, string receiptPurchaseData, string signature)
        {
            Debug.Log(LOG_TAG + "Transaction");
        }
        public virtual void Submit(CustomEvent customEvent)
        {
            Debug.Log(LOG_TAG + "CustomEvent: " + customEvent.eventName);
        }
    }
}