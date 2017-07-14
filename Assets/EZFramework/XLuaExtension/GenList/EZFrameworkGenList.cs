/*
 * Author:      熊哲
 * CreateTime:  3/31/2017 6:11:46 PM
 * Description:
 * 
*/
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using XLua;

namespace EZFramework.XLuaGen
{
    public static class EZFrameworkGenList
    {
        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp = new List<Type>()
        {
            typeof(EZFramework.EZUtility),

            typeof(EZFramework.EZFacade),
            typeof(EZFramework.EZDatabase),
            typeof(EZFramework.EZNetwork),
            typeof(EZFramework.EZResource),
            typeof(EZFramework.EZSound),
            typeof(EZFramework.EZUpdate),
            typeof(EZFramework.EZUI),
            typeof(EZFramework.EZUIExtensions),
            typeof(EZFramework.WWWTask),

            typeof(EZUnityTools.EZScrollRect),
            typeof(EZUnityTools.EZScrollRect.Status),

        };

        [CSharpCallLua]
        public static List<Type> CSharpCallLua = new List<Type>()
        {

        };

        [BlackList]
        public static List<List<string>> BlackList = new List<List<string>>()
        {

        };
    }
}