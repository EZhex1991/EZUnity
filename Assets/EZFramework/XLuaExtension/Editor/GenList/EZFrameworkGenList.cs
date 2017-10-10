/*
 * Author:      熊哲
 * CreateTime:  3/31/2017 6:11:46 PM
 * Description:
 * 
*/
using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace EZFramework.XLuaConfig
{
    public static class EZFrameworkGenList
    {
        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp = new List<Type>()
        {
            typeof(EZFramework.EZFacade),
            typeof(EZFramework.EZDatabase),
            typeof(EZFramework.EZNetwork),
            typeof(EZFramework.EZResource),
            typeof(EZFramework.EZSound),
            typeof(EZFramework.EZUpdate),
            typeof(EZFramework.EZUI),
            typeof(EZFramework.EZWWWTask),

            typeof(EZFramework.XLuaExtension.LuaInjector),
            typeof(EZFramework.XLuaExtension.ExtensionFunctions),
            typeof(EZFramework.XLuaExtension.LuaUtility),
            typeof(EZFramework.XLuaExtension.ActivityMessage),
            typeof(EZFramework.XLuaExtension.ActivityMessage.ActivityEvent),
            typeof(EZFramework.XLuaExtension.ApplicationMessage),
            typeof(EZFramework.XLuaExtension.ApplicationMessage.ApplicationEvent),
            typeof(EZFramework.XLuaExtension.CollisionMessage),
            typeof(EZFramework.XLuaExtension.CollisionMessage.CollisionEvent),
            typeof(EZFramework.XLuaExtension.MouseMessage),
            typeof(EZFramework.XLuaExtension.MouseMessage.MouseEvent),
            typeof(EZFramework.XLuaExtension.TriggerMessage),
            typeof(EZFramework.XLuaExtension.TriggerMessage.TriggerEvent),
            typeof(EZFramework.XLuaExtension.UpdateMessage),
            typeof(EZFramework.XLuaExtension.UpdateMessage.UpdateEvent),

            typeof(EZComponent.EZProcess.EZGraphicColorProcessor),
            typeof(EZComponent.EZProcess.EZRectTransformProcessor),
            typeof(EZComponent.EZProcess.EZTransformProcessor),
            typeof(EZComponent.UI.EZGridLayout2D),
            typeof(EZComponent.UI.EZOutstand),
            typeof(EZComponent.UI.EZScrollRect),
            typeof(EZComponent.UI.EZSizeDriver),
            typeof(EZComponent.UI.EZTransition),
        };

        [CSharpCallLua]
        public static List<Type> CSharpCallLua = new List<Type>()
        {
            typeof(EZFramework.EZFacade.OnApplicationStatusAction),
            typeof(EZFramework.EZFacade.OnApplicationQuitAction),
            typeof(EZFramework.EZResource.OnAssetLoadedAction),
            typeof(EZFramework.EZResource.OnSceneLoadedAction),
            typeof(EZFramework.EZLua.LuaAction),
            typeof(EZFramework.EZLua.LuaCoroutineCallback),
            typeof(EZFramework.EZWWWTask.OnProgressAction),
            typeof(EZFramework.EZWWWTask.OnStopAction),

            typeof(EZFramework.XLuaExtension.OnMessageAction),
            typeof(EZFramework.XLuaExtension.OnMessageAction<bool>),
            typeof(EZFramework.XLuaExtension.OnMessageAction<Collider>),
            typeof(EZFramework.XLuaExtension.OnMessageAction<Collision>),

            typeof(EZComponent.EZProcess.OnPhaseUpdateAction<Color>),
            typeof(EZComponent.EZProcess.OnPhaseUpdateAction<Vector2>),
            typeof(EZComponent.EZProcess.OnPhaseUpdateAction<Vector3>),
            typeof(EZComponent.UI.EZScrollRect.OnBeginScrollAction),
            typeof(EZComponent.UI.EZScrollRect.OnEndScrollAction),
        };

        [BlackList]
        public static List<List<string>> BlackList = new List<List<string>>()
        {

        };
    }
}