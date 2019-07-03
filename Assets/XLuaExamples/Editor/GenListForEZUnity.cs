/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-03-31 18:11:46
 * Organization:    #ORGANIZATION#
 * Description:     
 */
#if XLUA
using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace EZhex1991.EZUnity.Example
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
            typeof(EZUnity.Framework.Events.EZEvent),
            typeof(EZUnity.Framework.Events.EZEventHub),

            typeof(EZUnity.UniSDK.PolyADBase),
            typeof(EZUnity.UniSDK.NotificationBase),
            typeof(EZUnity.UniSDK.UnityPurchasingBase),
            typeof(EZUnity.UniSDK.UnityPurchasingBase.CustomProduct),
            typeof(EZUnity.UniSDK.UnityPurchasingBase.ReceiptContent),
            typeof(EZUnity.UniSDK.UnityPurchasingBase.PayloadContent),
            typeof(EZUnity.UniSDK.UnityPurchasingBase.JsonContent),

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

            typeof(EZUnity.EZUISorter),
            typeof(EZUnity.EZGridLayout2D),
            typeof(EZUnity.EZOutstand),
            typeof(EZUnity.EZScrollRect),
            typeof(EZUnity.EZTransition),
            typeof(EZUnity.EZPropertyList),
            typeof(EZUnity.EZProperty),

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
            typeof(EZUnity.Framework.Events.EZEventHandler),

            typeof(EZUnity.UniSDK.OnResultCallback),
            typeof(EZUnity.UniSDK.OnDataCallback),
            typeof(EZUnity.UniSDK.OnEventCallback1),
            typeof(EZUnity.UniSDK.OnEventCallback2),
            typeof(EZUnity.UniSDK.NotificationBase.NotificationProvider),


            typeof(EZUnity.XLuaExtension.LuaRequire),
            typeof(EZUnity.XLuaExtension.LuaAction),
            typeof(EZUnity.XLuaExtension.LuaAction<LuaTable>),
            typeof(EZUnity.XLuaExtension.EZLuaBehaviour.LuaAwake),
            typeof(EZUnity.XLuaExtension.OnMessageAction),
            typeof(EZUnity.XLuaExtension.OnMessageAction<bool>),
            typeof(EZUnity.XLuaExtension.OnMessageAction<Collider>),
            typeof(EZUnity.XLuaExtension.OnMessageAction<Collision>),

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
