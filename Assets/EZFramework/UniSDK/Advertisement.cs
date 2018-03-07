/* Author:          熊哲
 * CreateTime:      2018-01-08 18:19:35
 * Orgnization:     #ORGNIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZFramework.UniSDK
{
    public class Advertisement : EZSingleton<Advertisement>
    {
        public bool positiveEvent = true;
        public delegate void OnInitResultCallback(bool result, string msg);
        public delegate void OnEventCallback(string info, string msg);
        // init
        public event OnInitResultCallback onInitFinishedEvent;
        // reward video
        public event OnEventCallback onRewardVideoLoadSucceedEvent;
        public event OnEventCallback onRewardVideoLoadFailedEvent;
        public event OnEventCallback onRewardVideoShowEvent;
        public event OnEventCallback onRewardVideoCloseEvent;
        public event OnEventCallback onRewardVideoClickEvent;
        public event OnEventCallback onRewardVideoRewardEvent;
        public event OnEventCallback onRewardVideoAbandonEvent;
        // interstitial
        public event OnEventCallback onInterstitialShowEvent;
        public event OnEventCallback onInterstitialCloseEvent;
        public event OnEventCallback onInterstitialClickEvent;
        // banner
        public event OnEventCallback onBannerShowEvent;
        public event OnEventCallback onBannerRemoveEvent;
        public event OnEventCallback onBannerClickEvent;

        private float timeScale;
        private void PauseTime()
        {
            if (Time.timeScale == 0) return;
            timeScale = Time.timeScale;
            Time.timeScale = 0;
        }
        private void ResumeTime()
        {
            Time.timeScale = timeScale;
        }

        public virtual void Init()
        {
            Log("Init");
            _OnInitFinished(positiveEvent, "Test Mode");
        }

        public virtual bool IsRewardVideoReady()
        {
            Log("IsRewardVideoReady");
            return positiveEvent;
        }
        public virtual bool IsInterstitialReady(string placeId)
        {
            Log(string.Format("IsInterstitialReady:\n placeId: {0}", placeId));
            return positiveEvent;
        }

        public virtual void ShowRewardVideo(string placeId)
        {
            Log(string.Format("ShowRewardVideo:\n placeId: {0}", placeId));
            if (positiveEvent)
            {
                _OnRewardVideoShow(placeId, "Test Mode");
                _OnRewardVideoReward(placeId, "Test Mode");
                _OnRewardVideoClose(placeId, "Test Mode");
            }
            else
            {
                _OnRewardVideoShow(placeId, "Test Mode");
                _OnRewardVideoAbandon(placeId, "Test Mode");
                _OnRewardVideoClose(placeId, "Test Mode");
            }
        }

        public virtual void ShowInterstitial(string placeId)
        {
            Log(string.Format("ShowInterstitial:\n placeId: {0}", placeId));
            if (positiveEvent)
            {
                _OnInterstitialShow(placeId, "Test Mode");
                _OnInterstitialClose(placeId, "Test Mode");
            }
        }

        public virtual void ShowBannerAtTop(string placeId)
        {
            Log(string.Format("ShowBannerAtTop:\n placeId: {0}", placeId));
            if (positiveEvent) _OnBannerShow(placeId, "Test Mode");
        }
        public virtual void ShowBannerAtBottom(string placeId)
        {
            Log(string.Format("ShowBannerAtBottom:\n placeId: {0}", placeId));
            if (positiveEvent) _OnBannerShow(placeId, "Test Mode");
        }
        public virtual void RemoveBanner(string placeId)
        {
            Log(string.Format("RemoveBanner:\n placeId: {0}", placeId));
            if (positiveEvent) _OnBannerRemove(placeId, "Test Mode");
        }

        public virtual void InitConfig(string accountId, bool completeTask, int isPaid, string channel, string gender, int age)
        {
            Log(string.Format("InitConfig:\n accountId: {0}\n completeTask: {1}\n isPaid: {2}\n channel: {3}\n gender: {4}\n age: {5}",
                accountId, completeTask, isPaid, channel, gender, age));
        }
        public virtual string GetConfig(string placeId)
        {
            Log(string.Format("GetConfig:\n placeId: {0}", placeId));
            return null;
        }

        // init
        protected virtual void _OnInitFinished(bool result, string msg)
        {
            if (onInitFinishedEvent != null) onInitFinishedEvent(result, msg);
        }
        // reward video
        protected virtual void _OnRewardVideoLoadSucceed(string placeId, string msg)
        {
            if (onRewardVideoLoadSucceedEvent != null) onRewardVideoLoadSucceedEvent(placeId, msg);
        }
        protected virtual void _OnRewardVideoLoadFailed(string placeId, string msg)
        {
            if (onRewardVideoLoadFailedEvent != null) onRewardVideoLoadFailedEvent(placeId, msg);
        }
        protected virtual void _OnRewardVideoShow(string placeId, string msg)
        {
            if (onRewardVideoShowEvent != null) onRewardVideoShowEvent(placeId, msg);
#if UNITY_IOS
            PauseTime();
#endif
        }
        protected virtual void _OnRewardVideoClose(string placeId, string msg)
        {
            if (onRewardVideoCloseEvent != null) onRewardVideoCloseEvent(placeId, msg);
#if UNITY_IOS
            ResumeTime();
#endif
        }
        protected virtual void _OnRewardVideoClick(string placeId, string msg)
        {
            if (onRewardVideoClickEvent != null) onRewardVideoClickEvent(placeId, msg);
        }
        protected virtual void _OnRewardVideoReward(string placeId, string msg)
        {
            if (onRewardVideoRewardEvent != null) onRewardVideoRewardEvent(placeId, msg);
        }
        protected virtual void _OnRewardVideoAbandon(string placeId, string msg)
        {
            if (onRewardVideoAbandonEvent != null) onRewardVideoAbandonEvent(placeId, msg);
        }
        // interstitial
        protected virtual void _OnInterstitialShow(string placeId, string msg)
        {
            if (onInterstitialShowEvent != null) onInterstitialShowEvent(placeId, msg);
#if UNITY_IOS
            PauseTime();
#endif
        }
        protected virtual void _OnInterstitialClose(string placeId, string msg)
        {
            if (onInterstitialCloseEvent != null) onInterstitialCloseEvent(placeId, msg);
#if UNITY_IOS
            ResumeTime();
#endif
        }
        protected virtual void _OnInterstitialClick(string placeId, string msg)
        {
            if (onInterstitialClickEvent != null) onInterstitialClickEvent(placeId, msg);
        }
        // banner
        protected virtual void _OnBannerShow(string placeId, string msg)
        {
            if (onBannerShowEvent != null) onBannerShowEvent(placeId, msg);
        }
        protected virtual void _OnBannerRemove(string placeId, string msg)
        {
            if (onBannerRemoveEvent != null) onBannerRemoveEvent(placeId, msg);
        }
        protected virtual void _OnBannerClick(string placeId, string msg)
        {
            if (onBannerClickEvent != null) onBannerClickEvent(placeId, msg);
        }
    }
}