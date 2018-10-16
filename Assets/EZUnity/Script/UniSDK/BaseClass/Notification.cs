/*
 * Author:      熊哲
 * CreateTime:  1/16/2018 11:54:38 AM
 * Description:
 * 
*/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZUnity.UniSDK.Base
{
    public class Notification : _EZMonoBehaviourSingleton<Notification>
    {
        [Range(0, 30)]
        public int timeout = 10;

        public delegate int NotificationProvider(ref string message);
        public List<NotificationProvider> onPauseProviderList = new List<NotificationProvider>();

        protected override void Init()
        {
            ClearNotifications();
        }
        protected override void Dispose()
        {

        }

        protected virtual void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                ResetBadgeNumber();
                for (int i = 0; i < onPauseProviderList.Count; i++)
                {
                    string message = "";
                    int seconds = onPauseProviderList[i](ref message);
                    if (seconds >= 10)
                        ScheduleNotification(message, seconds);
                }
            }
            else
            {
                ClearNotifications();
            }
        }

        public void AppendProvider(NotificationProvider provider)
        {
            onPauseProviderList.Add(provider);
        }
        public void RemoveProvider(NotificationProvider provider)
        {
            onPauseProviderList.Remove(provider);
        }
        public void ClearAllProviders()
        {
            onPauseProviderList.Clear();
        }

        public virtual void ScheduleNotification(string message, int seconds)
        {

        }
        public virtual void ScheduleNotification(string message, DateTime time)
        {

        }
        public virtual void RepeatNotification(string message, int hour, int minute, int second)
        {

        }
        public virtual void RepeatNotification(string message, DateTime time)
        {

        }

        public virtual void ResetBadgeNumber()
        {

        }

        public virtual void ClearNotifications()
        {

        }
    }
}