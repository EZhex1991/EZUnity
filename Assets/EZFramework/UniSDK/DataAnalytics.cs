/*
 * Author:      熊哲
 * CreateTime:  1/9/2018 2:59:55 PM
 * Description:
 * 
*/
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace EZFramework.UniSDK
{
    public class DataAnalytics : EZSingleton<DataAnalytics>
    {
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
            Log(string.Format("{0}\n{1}\n{2}\n{3}", "SetUserInfo", userId, gender, birthYear));
        }
        public virtual void Transaction(string productId, decimal amount, string currency)
        {
            Log(string.Format("{0}\n{1}\n{2}\n{3}", "Transaction", productId, amount, currency));
        }
        public virtual void Transaction(string productId, decimal amount, string currency, string receiptPurchaseData, string signature)
        {
            Log(string.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}", "Transaction", productId, amount, currency, receiptPurchaseData, signature));
        }
        public virtual void Submit(CustomEvent customEvent)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("CustomEvent: " + customEvent.eventName);
            foreach (var data in customEvent.eventData)
            {
                sb.AppendLine(data.Key + ": " + data.Value);
            }
            Log(sb.ToString());
        }
    }
}