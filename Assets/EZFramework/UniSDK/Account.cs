/*
 * Author:      熊哲
 * CreateTime:  1/9/2018 5:51:53 PM
 * Description:
 * 
*/
using UnityEngine;

namespace EZFramework.UniSDK
{
    public class Account : EZSingleton<Account>
    {
        public bool positiveEvent;

        public delegate void OnEventCallback(string msg);
        public event OnEventCallback onInitSucceededEvent;
        public event OnEventCallback onInitFailedEvent;
        public event OnEventCallback onLoginSucceededEvent;
        public event OnEventCallback onLoginFailedEvent;

        public virtual void Init()
        {
            Log("Init");
            if (positiveEvent) m_OnInitSucceeded("Account disabled, positive events will be triggered.");
            else m_OnInitFailed("Account disabled, negative events will be triggered.");
        }

        public virtual bool IsLoggedIn()
        {
            return positiveEvent;
        }

        public virtual void Login()
        {
            Log("Login");
            if (positiveEvent)
            {
                m_OnLoginSucceeded("");
            }
            else
            {
                m_OnLoginFailed("");
            }
        }

        public virtual string GetNickName()
        {
            Log("GetNickName");
            return "Guest";
        }
        public virtual string GetRealName()
        {
            Log("GetRealName");
            return "Guest";
        }
        public virtual int GetGender()
        {
            Log("GetGender");
            return 2;
        }
        public virtual Texture GetIcon()
        {
            Log("GetIcon");
            return new Texture();
        }

        protected virtual void m_OnInitSucceeded(string msg)
        {
            if (onInitSucceededEvent != null) onInitSucceededEvent(msg);
        }
        protected virtual void m_OnInitFailed(string msg)
        {
            if (onInitFailedEvent != null) onInitFailedEvent(msg);
        }
        protected virtual void m_OnLoginSucceeded(string msg)
        {
            if (onLoginSucceededEvent != null) onLoginSucceededEvent(msg);
        }
        protected virtual void m_OnLoginFailed(string msg)
        {
            if (onLoginFailedEvent != null) onLoginFailedEvent(msg);
        }
    }
}