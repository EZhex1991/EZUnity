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

namespace EZFramework.XLuaGen
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
            typeof(EZFramework.EZFacade.OnApplicationFocusEventHandler),
            typeof(EZFramework.EZFacade.OnApplicationQuitEventHandler),
            typeof(EZFramework.EZResource.OnAssetLoadedAction),
            typeof(EZFramework.EZResource.OnSceneLoadedAction),
            typeof(EZFramework.EZLua.LuaEntry),
            typeof(EZFramework.EZLua.LuaCoroutineCallback),
            typeof(EZFramework.EZWWWTask.OnProgressAction),
            typeof(EZFramework.EZWWWTask.OnStopAction),

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