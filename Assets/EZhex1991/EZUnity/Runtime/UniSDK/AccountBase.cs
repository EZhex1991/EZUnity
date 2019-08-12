/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-01-09 17:51:53
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity.UniSDK
{
    public class AccountBase : EZMonoBehaviourSingleton<AccountBase>
    {
        public bool positiveEvent;

        public event OnEventCallback1 onInitSucceededEvent;
        public event OnEventCallback1 onInitFailedEvent;
        public event OnEventCallback1 onLoginSucceededEvent;
        public event OnEventCallback1 onLoginFailedEvent;

        protected override void Init()
        {
            Log("Init");
            if (positiveEvent) _OnInitSucceeded("Test Mode");
            else _OnInitFailed("Test Mode");
        }

        public virtual bool IsLoggedIn()
        {
            return positiveEvent;
        }

        public virtual void Login()
        {
            Log("Login");
            if (positiveEvent) _OnLoginSucceeded("Test Mode");
            else _OnLoginFailed("Test Mode");
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
            return null;
        }

        protected virtual void _OnInitSucceeded(string message)
        {
            Log(string.Format("InitSucceeded:\n message={0}", message));
            if (onInitSucceededEvent != null) onInitSucceededEvent(message);
        }
        protected virtual void _OnInitFailed(string message)
        {
            Log(string.Format("InitFailed:\n message={0}", message));
            if (onInitFailedEvent != null) onInitFailedEvent(message);
        }
        protected virtual void _OnLoginSucceeded(string message)
        {
            Log(string.Format("LoginSucceeded:\n message={0}", message));
            if (onLoginSucceededEvent != null) onLoginSucceededEvent(message);
        }
        protected virtual void _OnLoginFailed(string message)
        {
            Log(string.Format("LoginFailed:\n message={0}", message));
            if (onLoginFailedEvent != null) onLoginFailedEvent(message);
        }
    }
}