/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-01-08 18:19:35
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity.UniSDK
{
    public class PolyADBase : EZMonoBehaviourSingleton<PolyADBase>
    {
        public bool positiveEvent = true;
        // init
        public event OnResultCallback onInitFinishedEvent;
        // reward video
        public event OnEventCallback2 onRewardVideoLoadSucceededEvent;
        public event OnEventCallback2 onRewardVideoLoadFailedEvent;
        public event OnEventCallback2 onRewardVideoShowEvent;
        public event OnEventCallback2 onRewardVideoCloseEvent;
        public event OnEventCallback2 onRewardVideoClickEvent;
        public event OnEventCallback2 onRewardVideoRewardEvent;
        public event OnEventCallback2 onRewardVideoAbandonEvent;
        // interstitial
        public event OnEventCallback2 onInterstitialShowEvent;
        public event OnEventCallback2 onInterstitialCloseEvent;
        public event OnEventCallback2 onInterstitialClickEvent;
        // banner
        public event OnEventCallback2 onBannerShowEvent;
        public event OnEventCallback2 onBannerRemoveEvent;
        public event OnEventCallback2 onBannerClickEvent;

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

        protected override void Init()
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
            Log(string.Format("IsInterstitialReady:\n placeId={0}", placeId));
            return positiveEvent;
        }

        public virtual void ShowRewardVideo(string placeId)
        {
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
            if (positiveEvent)
            {
                _OnInterstitialShow(placeId, "Test Mode");
                _OnInterstitialClose(placeId, "Test Mode");
            }
        }

        public virtual void ShowBannerAtTop(string placeId)
        {
            if (positiveEvent) _OnBannerShow(placeId, "Test Mode");
        }
        public virtual void ShowBannerAtBottom(string placeId)
        {
            if (positiveEvent) _OnBannerShow(placeId, "Test Mode");
        }
        public virtual void RemoveBanner(string placeId)
        {
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
        protected virtual void _OnInitFinished(bool result, string message)
        {
            Log(string.Format("InitFinished:\n result={0}\n message={1}", result, message));
            if (onInitFinishedEvent != null) onInitFinishedEvent(result, message);
        }
        // reward video
        protected virtual void _OnRewardVideoLoadSucceeded(string placeId, string message)
        {
            Log(string.Format("RewardVideoLoadSucceeded:\n placeId={0}\n message={1}", placeId, message));
            if (onRewardVideoLoadSucceededEvent != null) onRewardVideoLoadSucceededEvent(placeId, message);
        }
        protected virtual void _OnRewardVideoLoadFailed(string placeId, string message)
        {
            Log(string.Format("RewardVideoLoadFailed:\n placeId={0}\n message={1}", placeId, message));
            if (onRewardVideoLoadFailedEvent != null) onRewardVideoLoadFailedEvent(placeId, message);
        }
        protected virtual void _OnRewardVideoShow(string placeId, string message)
        {
            Log(string.Format("RewardVideoShow:\n placeId={0}\n message={1}", placeId, message));
            if (onRewardVideoShowEvent != null) onRewardVideoShowEvent(placeId, message);
#if UNITY_IOS
            PauseTime();
#endif
        }
        protected virtual void _OnRewardVideoClose(string placeId, string message)
        {
            Log(string.Format("RewardVideoClose:\n placeId={0}\n message={1}", placeId, message));
            if (onRewardVideoCloseEvent != null) onRewardVideoCloseEvent(placeId, message);
#if UNITY_IOS
            ResumeTime();
#endif
        }
        protected virtual void _OnRewardVideoClick(string placeId, string message)
        {
            Log(string.Format("RewardVideoClick:\n placeId={0}\n message={1}", placeId, message));
            if (onRewardVideoClickEvent != null) onRewardVideoClickEvent(placeId, message);
        }
        protected virtual void _OnRewardVideoReward(string placeId, string message)
        {
            Log(string.Format("RewardVideoReward:\n placeId={0}\n message={1}", placeId, message));
            if (onRewardVideoRewardEvent != null) onRewardVideoRewardEvent(placeId, message);
        }
        protected virtual void _OnRewardVideoAbandon(string placeId, string message)
        {
            Log(string.Format("RewardVideoAbandon:\n placeId={0}\n message={1}", placeId, message));
            if (onRewardVideoAbandonEvent != null) onRewardVideoAbandonEvent(placeId, message);
        }
        // interstitial
        protected virtual void _OnInterstitialShow(string placeId, string message)
        {
            Log(string.Format("InterstitialShow:\n placeId={0}\n message={1}", placeId, message));
            if (onInterstitialShowEvent != null) onInterstitialShowEvent(placeId, message);
#if UNITY_IOS
            PauseTime();
#endif
        }
        protected virtual void _OnInterstitialClose(string placeId, string message)
        {
            Log(string.Format("InterstitialClose:\n placeId={0}\n message={1}", placeId, message));
            if (onInterstitialCloseEvent != null) onInterstitialCloseEvent(placeId, message);
#if UNITY_IOS
            ResumeTime();
#endif
        }
        protected virtual void _OnInterstitialClick(string placeId, string message)
        {
            Log(string.Format("InterstitialClick:\n placeId={0}\n message={1}", placeId, message));
            if (onInterstitialClickEvent != null) onInterstitialClickEvent(placeId, message);
        }
        // banner
        protected virtual void _OnBannerShow(string placeId, string message)
        {
            Log(string.Format("BannerShow:\n placeId={0}\n message={1}", placeId, message));
            if (onBannerShowEvent != null) onBannerShowEvent(placeId, message);
        }
        protected virtual void _OnBannerRemove(string placeId, string message)
        {
            Log(string.Format("BannerRemove:\n placeId={0}\n message={1}", placeId, message));
            if (onBannerRemoveEvent != null) onBannerRemoveEvent(placeId, message);
        }
        protected virtual void _OnBannerClick(string placeId, string message)
        {
            Log(string.Format("BannerClick:\n placeId={0}\n message={1}", placeId, message));
            if (onBannerClickEvent != null) onBannerClickEvent(placeId, message);
        }
    }
}