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
            typeof(EZFramework.EZUIExtensions),

            typeof(EZFramework.EZWWWTask),
        };

        [CSharpCallLua]
        public static List<Type> CSharpCallLua = new List<Type>()
        {
            typeof(System.Action),
            typeof(System.Action<string>),
            typeof(System.Action<double>),
            typeof(System.Action<float>),
            typeof(System.Action<GameObject>),
            typeof(System.Action<string, byte[]>),

            typeof(System.Action<int, int>),
        };

        [LuaCallCSharp]
        public static List<Type> EZComponent_CSCallLua
        {
            get
            {
                return (Assembly.Load("Assembly-CSharp").GetTypes()
                    .Where(type => type.FullName.StartsWith("EZComponent")))
                    .ToList();
            }
        }
        [CSharpCallLua]
        public static List<Type> EZComponent_LuaCallCS = new List<Type>()
        {
            typeof(System.Action),
            typeof(System.Action<int>),
            typeof(System.Action<float>),
            typeof(System.Action<Color>),
            typeof(System.Action<Vector2>),
            typeof(System.Action<Vector3>),
        };

        [BlackList]
        public static List<List<string>> BlackList = new List<List<string>>()
        {

        };
    }
}