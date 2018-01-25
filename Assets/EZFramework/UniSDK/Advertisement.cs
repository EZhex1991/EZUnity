/*
 * Author:      熊哲
 * CreateTime:  1/8/2018 6:19:35 PM
 * Description:
 * 1. 统一广告的接入方式和事件传递的参数类型，保证SDK更换后接口不用重新设计
 * 2. 对Editor模式做特殊处理，保证业务逻辑在不依赖SDK相关文件的情况下能正常执行
 * 3. 使用时继承该类，在自定义宏中对方法进行重写，宏定义即为启用SDK的开关（宏的使用请在Unity Manual中搜索Scripting Define Symbols）。
 * 例：
 * public class ADSDK : Advertisement
 * {
 * #if ADSDK // 不加宏就会使用默认逻辑，不会依赖到SDK相关文件
 *      public override void Init()
 *      {
 *          ADSDK.Init();
 *          ADSDK.onAdOpen = (placeId, rewardType, rewardAmount)=>
 *          {
 *              // 自己对参数进行处理
 *              OnRewardVideoShow(placeId, string.Format("{0}, {1}", rewardType, rewardAmount));
 *          }
 *      }
 *      public override void ShowRewardVideo(string placeId, string msg)
 *      {
 *          // 无用参数可以舍弃
 *          ADSDK.ShowRewardVideo(placeId);
 *      }
 *      public override void OnRewardVideoShow(string info, string msg)
 *      {
 *          Debug.Log("ADSDK");
 *      }
 * #endif
 * }
*/
using UnityEngine;

namespace EZFramework.UniSDK
{
    public class Advertisement : EZSingleton<Advertisement>
    {
        public bool positiveEvent = true;
        public delegate void OnEventCallback(string info, string msg);
        // init
        public event OnEventCallback onInitSucceededEvent;
        public event OnEventCallback onInitFailedEvent;
        // reward video
        public event OnEventCallback onRewardVideoLoadedEvent;
        public event OnEventCallback onRewardVideoLoadFailedEvent;
        public event OnEventCallback onRewardVideoShowEvent;
        public event OnEventCallback onRewardVideoCloseEvent;
        public event OnEventCallback onRewardVideoClickEvent;
        public event OnEventCallback onRewardVideoRewardEvent;
        public event OnEventCallback onRewardVideoAbandonEvent;
        // interstitial
        public event OnEventCallback onInterstitialLoadedEvent;
        public event OnEventCallback onInterstitialLoadFailedEvent;
        public event OnEventCallback onInterstitialShowEvent;
        public event OnEventCallback onInterstitialCloseEvent;
        public event OnEventCallback onInterstitialClickEvent;
        // banner
        public event OnEventCallback onBannerLoadedEvent;
        public event OnEventCallback onBannerLoadFailedEvent;
        public event OnEventCallback onBannerShowEvent;
        public event OnEventCallback onBannerCloseEvent;
        public event OnEventCallback onBannerClickEvent;
        // exit ad
        public event OnEventCallback onExitAdLoadedEvent;
        public event OnEventCallback onExitAdLoadFailedEvent;
        public event OnEventCallback onExitAdShowEvent;
        public event OnEventCallback onExitAdCloseEvent;
        public event OnEventCallback onExitAdClickEvent;
        // native ad
        public event OnEventCallback onNativeAdLoadedEvent;
        public event OnEventCallback onNativeAdLoadFailedEvent;

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
            if (positiveEvent) m_OnInitSucceeded("", "Ad disabled, positive events will be triggered.");
            else m_OnInitFailed("", "Ad disabled, negative events will be triggered.");
        }

        public virtual void LoadRewardVideo()
        {
            Log("LoadRewardVideo");
            if (positiveEvent) m_OnRewardVideoLoaded("", "");
            else m_OnRewardVideoLoadFailed("", "");
        }
        public virtual void ShowRewardVideo(string info = "", string msg = "")
        {
            Log(string.Format("{0}\n{1}\n{2}", "Show RewardVideo", info, msg));
            if (positiveEvent)
            {
                m_OnRewardVideoShow(info, msg);
                m_OnRewardVideoReward(info, msg);
                m_OnRewardVideoClose(info, msg);
            }
            else
            {
                m_OnRewardVideoShow(info, msg);
                m_OnRewardVideoAbandon(info, msg);
                m_OnRewardVideoClose(info, msg);
            }
        }

        public virtual void ShowInterstitial(string info = "", string msg = "")
        {
            Log(string.Format("{0}\n{1}\n{2}", "ShowInterstitial", info, msg));
            if (positiveEvent)
            {
                m_OnInterstitialShow(info, msg);
                m_OnInterstitialClose(info, msg);
            }
        }

        public virtual void ShowBanner(string info = "", string msg = "")
        {
            Log(string.Format("{0}\n{1}\n{2}", "ShowBanner", info, msg));
            if (positiveEvent) m_OnBannerShow(info, msg);
        }
        public virtual void RemoveBanner(string info = "", string msg = "")
        {
            Log(string.Format("{0}\n{1}\n{2}", "RemoveBanner", info, msg));
            if (positiveEvent) m_OnBannerClose(info, msg);
        }

        public virtual void LoadNativeAd(string info = "", string msg = "")
        {
            Log(string.Format("{0}\n{1}\n{2}", "LoadNativeAd", info, msg));
            if (positiveEvent) m_OnNativeAdLoaded(info, msg);
            else m_OnNativeAdLoadFailed(info, msg);
        }
        public virtual Texture2D GetNativeAdTexture(string info = "")
        {
            return new Texture2D(100, 100);
        }
        public virtual string GetNativeAdText(string info = "")
        {
            return info;
        }
        public virtual void PerformClick(string info = "")
        {
            Log(string.Format("{0}\n{1}", "PerformClick", info));
        }

        public virtual bool IsRewardVideoReady(string info = "")
        {
            Log(string.Format("{0}\n{1}", "IsRewardVideoReady", info));
            return positiveEvent;
        }
        public virtual bool IsInterstitialReady(string info = "")
        {
            Log(string.Format("{0}\n{1}", "IsInterstitialReady", info));
            return positiveEvent;
        }
        public virtual bool IsBannerReady(string info = "")
        {
            Log(string.Format("{0}\n{1}", "IsBannerReady", info));
            return positiveEvent;
        }
        public virtual bool IsExitAdReady(string info = "")
        {
            Log(string.Format("{0}\n{1}", "IsExitAdReady", info));
            return positiveEvent;
        }

        // 统一事件传递方式
        // init
        protected virtual void m_OnInitSucceeded(string info, string msg)
        {
            if (onInitSucceededEvent != null) onInitSucceededEvent("", msg);
        }
        protected virtual void m_OnInitFailed(string info, string msg)
        {
            if (onInitFailedEvent != null) onInitFailedEvent("", msg);
        }
        // reward video
        protected virtual void m_OnRewardVideoLoaded(string info, string msg)
        {
            if (onRewardVideoLoadedEvent != null) onRewardVideoLoadedEvent(info, msg);
        }
        protected virtual void m_OnRewardVideoLoadFailed(string info, string msg)
        {
            if (onRewardVideoLoadFailedEvent != null) onRewardVideoLoadFailedEvent(info, msg);
        }
        protected virtual void m_OnRewardVideoShow(string info, string msg)
        {
            if (onRewardVideoShowEvent != null) onRewardVideoShowEvent(info, msg);
#if UNITY_IOS
            PauseTime();
#endif
        }
        protected virtual void m_OnRewardVideoClose(string info, string msg)
        {
            if (onRewardVideoCloseEvent != null) onRewardVideoCloseEvent(info, msg);
#if UNITY_IOS
            ResumeTime();
#endif
        }
        protected virtual void m_OnRewardVideoClick(string info, string msg)
        {
            if (onRewardVideoClickEvent != null) onRewardVideoClickEvent(info, msg);
        }
        protected virtual void m_OnRewardVideoReward(string info, string msg)
        {
            if (onRewardVideoRewardEvent != null) onRewardVideoRewardEvent(info, msg);
        }
        protected virtual void m_OnRewardVideoAbandon(string info, string msg)
        {
            if (onRewardVideoAbandonEvent != null) onRewardVideoAbandonEvent(info, msg);
        }
        // interstitial
        protected virtual void m_OnInterstitialLoaded(string info, string msg)
        {
            if (onInterstitialLoadedEvent != null) onInterstitialLoadedEvent(info, msg);
        }
        protected virtual void m_OnInterstitialLoadFailed(string info, string msg)
        {
            if (onInterstitialLoadFailedEvent != null) onInterstitialLoadFailedEvent(info, msg);
        }
        protected virtual void m_OnInterstitialShow(string info, string msg)
        {
            if (onInterstitialShowEvent != null) onInterstitialShowEvent(info, msg);
#if UNITY_IOS
            PauseTime();
#endif
        }
        protected virtual void m_OnInterstitialClose(string info, string msg)
        {
            if (onInterstitialCloseEvent != null) onInterstitialCloseEvent(info, msg);
#if UNITY_IOS
            ResumeTime();
#endif
        }
        protected virtual void m_OnInterstitialClick(string info, string msg)
        {
            if (onInterstitialClickEvent != null) onInterstitialClickEvent(info, msg);
        }
        // banner
        protected virtual void m_OnBannerLoaded(string info, string msg)
        {
            if (onBannerLoadedEvent != null) onBannerLoadedEvent(info, msg);
        }
        protected virtual void m_OnBannerLoadFailed(string info, string msg)
        {
            if (onBannerLoadFailedEvent != null) onBannerLoadFailedEvent(info, msg);
        }
        protected virtual void m_OnBannerShow(string info, string msg)
        {
            if (onBannerShowEvent != null) onBannerShowEvent(info, msg);
        }
        protected virtual void m_OnBannerClose(string info, string msg)
        {
            if (onBannerCloseEvent != null) onBannerCloseEvent(info, msg);
        }
        protected virtual void m_OnBannerClick(string info, string msg)
        {
            if (onBannerClickEvent != null) onBannerClickEvent(info, msg);
        }
        // exit ad
        protected virtual void m_OnExitAdLoaded(string info, string msg)
        {
            if (onExitAdLoadedEvent != null) onExitAdLoadedEvent(info, msg);
        }
        protected virtual void m_OnExitAdLoadFailed(string info, string msg)
        {
            if (onExitAdLoadFailedEvent != null) onExitAdLoadFailedEvent(info, msg);
        }
        protected virtual void m_OnExitAdShow(string msg)
        {
            if (onExitAdShowEvent != null) onExitAdShowEvent("", msg);
        }
        protected virtual void m_OnExitAdClose(string msg)
        {
            if (onExitAdCloseEvent != null) onExitAdCloseEvent("", msg);
        }
        protected virtual void m_OnExitAdClick(string msg)
        {
            if (onExitAdClickEvent != null) onExitAdClickEvent("", msg);
        }
        // native ad
        protected virtual void m_OnNativeAdLoaded(string info, string msg)
        {
            if (onNativeAdLoadedEvent != null) onNativeAdLoadedEvent(info, msg);
        }
        protected virtual void m_OnNativeAdLoadFailed(string info, string msg)
        {
            if (onNativeAdLoadFailedEvent != null) onNativeAdLoadFailedEvent(info, msg);
        }
    }
}