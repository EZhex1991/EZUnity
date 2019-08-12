/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-01-16 11:54:38
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZhex1991.EZUnity.UniSDK
{
    public class NotificationBase : EZMonoBehaviourSingleton<NotificationBase>
    {
        [Range(0, 30)]
        public int timeout = 10;

        public delegate int NotificationProvider(ref string message);
        public List<NotificationProvider> onPauseProviderList = new List<NotificationProvider>();

        protected override void Init()
        {
            ClearLocalNotifications();
        }

        protected virtual IEnumerator OnApplicationPause(bool pauseStatus)
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
                yield return null;
            }
            else
            {
                yield return null;
                ClearLocalNotifications();
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

        public virtual void ClearLocalNotifications()
        {

        }
    }
}