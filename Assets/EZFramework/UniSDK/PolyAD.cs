/*
 * Author:      熊哲
 * CreateTime:  7/24/2017 2:43:03 PM
 * Description:
 * 
*/
using UnityEngine;
using System.Collections.Generic;
using System;
#if POLYAD
using Polymer;
#endif

namespace EZFramework.UniSDK
{
    public class PolyAD : Base.PolyAD
    {
#if POLYAD
        private Queue<Action> callbackQueue = new Queue<Action>();

        public override void Init()
        {
            // init
            PolyADSDK.AvidlySDKInitFinishedCallback = _OnInitFinished;
            // reward video
            PolyADSDK.setRewardVideoLoadCallback(_OnRewardVideoLoadSucceeded, _OnRewardVideoLoadFailed);
            PolyADSDK.AvidlyRewardDidOpenCallback = _OnRewardVideoShow;
            PolyADSDK.AvidlyRewardDidCloseCallback = _OnRewardVideoClose;
            PolyADSDK.AvidlyRewardDidClickCallback = _OnRewardVideoClick;
            PolyADSDK.AvidlyRewardDidGivenCallback = _OnRewardVideoReward;
            PolyADSDK.AvidlyRewardDidAbandonCallback = _OnRewardVideoAbandon;
            // interstitial
            PolyADSDK.AvidlyInterstitialDidShowCallback = _OnInterstitialShow;
            PolyADSDK.AvidlyInterstitialDidCloseCallback = _OnInterstitialClose;
            PolyADSDK.AvidlyInterstitialDidClickCallback = _OnInterstitialClick;
            // banner
            PolyADSDK.AvidlyBannerDidShowCallback = _OnBannerShow;
            PolyADSDK.AvidlyBannerDidRemoveCallback = _OnBannerRemove;
            PolyADSDK.AvidlyBannerDidClickCallback = _OnBannerClick;
            // init
            PolyADSDK.setManifestPackageName(Application.identifier);
            PolyADSDK.initPolyAdSDK(PolyADSDK.SDKZONE_AUTO);
        }
        void Update()
        {
            while (callbackQueue.Count > 0)
            {
                callbackQueue.Dequeue().Invoke();
            }
        }

        public override bool IsRewardVideoReady()
        {
            return PolyADSDK.isRewardReady();
        }
        public override bool IsInterstitialReady(string placeId)
        {
            return PolyADSDK.isInterstitialReady(placeId);
        }

        public override void ShowRewardVideo(string placeId)
        {
            PolyADSDK.showRewardAd(placeId);
        }

        public override void ShowInterstitial(string placeId)
        {
            PolyADSDK.showIntersitialAd(placeId);
        }

        public override void ShowBannerAtTop(string placeId)
        {
            PolyADSDK.showBannerAdAtTop(placeId);
        }
        public override void ShowBannerAtBottom(string placeId)
        {
            PolyADSDK.showBannerAdAtBottom(placeId);
        }
        public override void RemoveBanner(string placeId)
        {
            PolyADSDK.removeBannerAdAt(placeId);
        }

        public override void InitConfig(string accountId, bool completeTask, int isPaid, string channel = "", string gender = "", int age = -1)
        {
            PolyADSDK.initAbtConfigJson(accountId, completeTask, isPaid, channel, gender, age, null);
        }
        public override string GetConfig(string placeId)
        {
            return PolyADSDK.getAbtConfig(placeId);
        }
#endif
    }
}