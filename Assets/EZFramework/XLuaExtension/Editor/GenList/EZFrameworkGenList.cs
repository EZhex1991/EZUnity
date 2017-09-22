/*
 * Author:      熊哲
 * CreateTime:  3/31/2017 6:11:46 PM
 * Description:
 * 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        };
        [CSharpCallLua]
        public static List<Type> CSharpCallLua = new List<Type>()
        {
            typeof(System.Action),
            typeof(System.Action<UnityEngine.Object>),
            typeof(System.Action<string, byte[]>),
        };

        [LuaCallCSharp]
        public static List<Type> EZComponent_LuaCallCSharp = new List<Type>()
        {
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
        public static List<Type> EZComponent_CSharpCallLua = new List<Type>()
        {
            typeof(System.Action),
            typeof(System.Action<int>),
            typeof(System.Action<float>),
            typeof(System.Action<Color>),
            typeof(System.Action<Vector2>),
            typeof(System.Action<Vector3>),
            typeof(System.Action<int, int>),
        };

        [BlackList]
        public static List<List<string>> BlackList = new List<List<string>>()
        {

        };
    }
}