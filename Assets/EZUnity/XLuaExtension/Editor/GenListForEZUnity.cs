/*
 * Author:      熊哲
 * CreateTime:  3/31/2017 6:11:46 PM
 * Description:
 * 
*/
#if XLUA
using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace EZUnity.XLuaExtension
{
    public static class GenListForEZUnity
    {
        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp = new List<Type>()
        {
            typeof(EZUnity.Framework.EZApplication),
            typeof(EZUnity.Framework.EZDatabase),
            typeof(EZUnity.Framework.EZLogHandler),
            typeof(EZUnity.Framework.EZResources),
            typeof(EZUnity.Framework.EZWWWQueue),
            typeof(EZUnity.Framework.EZWWWQueue.Status),
            typeof(EZUnity.Framework.EZWWWQueue.Task),

            typeof(EZUnity.UniSDK.Base.PolyAD),
            typeof(EZUnity.UniSDK.Base.Notification),
            typeof(EZUnity.UniSDK.Base.UnityAnalytics),
            typeof(EZUnity.UniSDK.Base.UnityAnalytics.CustomEvent),
            typeof(EZUnity.UniSDK.Base.UnityPurchasing),
            typeof(EZUnity.UniSDK.Base.UnityPurchasing.CustomProduct),
            typeof(EZUnity.UniSDK.Base.UnityPurchasing.ReceiptContent),
            typeof(EZUnity.UniSDK.Base.UnityPurchasing.PayloadContent),
            typeof(EZUnity.UniSDK.Base.UnityPurchasing.JsonContent),

            typeof(EZUnity.XLuaExtension.EZLua),
            typeof(EZUnity.XLuaExtension.EZLuaBehaviour),
            typeof(EZUnity.XLuaExtension.EZLuaInjector),
            typeof(EZUnity.XLuaExtension.EZLuaUtility),
            typeof(EZUnity.XLuaExtension.ActivityMessage),
            typeof(EZUnity.XLuaExtension.ActivityMessage.ActivityEvent),
            typeof(EZUnity.XLuaExtension.ApplicationMessage),
            typeof(EZUnity.XLuaExtension.ApplicationMessage.ApplicationEvent),
            typeof(EZUnity.XLuaExtension.CollisionMessage),
            typeof(EZUnity.XLuaExtension.CollisionMessage.CollisionEvent),
            typeof(EZUnity.XLuaExtension.MouseMessage),
            typeof(EZUnity.XLuaExtension.MouseMessage.MouseEvent),
            typeof(EZUnity.XLuaExtension.TriggerMessage),
            typeof(EZUnity.XLuaExtension.TriggerMessage.TriggerEvent),
            typeof(EZUnity.XLuaExtension.UpdateMessage),
            typeof(EZUnity.XLuaExtension.UpdateMessage.UpdateEvent),

            typeof(EZUnity.EZTimer),
            typeof(EZUnity.EZTimer.Task),
            typeof(EZUnity.EZTimer.TickMode),

            typeof(EZUnity.Animation.EZGraphicColorAnimation),
            typeof(EZUnity.Animation.EZRectTransformAnimation),
            typeof(EZUnity.Animation.EZTransformAnimation),

            typeof(EZUnity.EZUISorter),
            typeof(EZUnity.EZGridLayout2D),
            typeof(EZUnity.EZOutstand),
            typeof(EZUnity.EZScrollRect),
            typeof(EZUnity.EZSizeDriver),
            typeof(EZUnity.EZTransition),
            typeof(EZUnity.EZDictionary),
            typeof(EZUnity.EZSerializableProperty),

            typeof(EZUnity.EZLoadingPanel),
            typeof(EZUnity.EZUtility),
            typeof(EZUnity.EZExtensions),
        };

        [CSharpCallLua]
        public static List<Type> CSharpCallLua = new List<Type>()
        {
            typeof(EZUnity.Framework.EZApplication.OnApplicationAction),
            typeof(EZUnity.Framework.EZApplication.OnApplicationStatusAction),
            typeof(EZUnity.Framework.EZResources.OnAssetLoadedAction),
            typeof(EZUnity.Framework.EZResources.OnSceneChangedAction),
            typeof(EZUnity.Framework.EZResources.OnBundleLoadedAction),
            typeof(EZUnity.Framework.EZWWWQueue.Task.OnProgressAction),
            typeof(EZUnity.Framework.EZWWWQueue.Task.OnStopAction),

            typeof(EZUnity.UniSDK.Base.OnResultCallback),
            typeof(EZUnity.UniSDK.Base.OnDataCallback),
            typeof(EZUnity.UniSDK.Base.OnEventCallback1),
            typeof(EZUnity.UniSDK.Base.OnEventCallback2),
            typeof(EZUnity.UniSDK.Base.Notification.NotificationProvider),


            typeof(EZUnity.XLuaExtension.LuaRequire),
            typeof(EZUnity.XLuaExtension.LuaAction),
            typeof(EZUnity.XLuaExtension.LuaAction<LuaTable>),
            typeof(EZUnity.XLuaExtension.EZLuaBehaviour.LuaAwake),
            typeof(EZUnity.XLuaExtension.OnMessageAction),
            typeof(EZUnity.XLuaExtension.OnMessageAction<bool>),
            typeof(EZUnity.XLuaExtension.OnMessageAction<Collider>),
            typeof(EZUnity.XLuaExtension.OnMessageAction<Collision>),

            typeof(EZUnity.Animation.OnAnimationEndAction),

            typeof(EZUnity.EZScrollRect.OnBeginScrollAction),
            typeof(EZUnity.EZScrollRect.OnEndScrollAction),
        };

        [BlackList]
        public static List<List<string>> BlackList = new List<List<string>>()
        {

        };
    }
}
#endif
