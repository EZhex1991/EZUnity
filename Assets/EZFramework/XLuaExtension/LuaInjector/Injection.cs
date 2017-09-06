/*
 * Author:      熊哲
 * CreateTime:  9/4/2017 6:41:45 PM
 * Description:
 * 
*/
using System;
using System.Collections.Generic;
using System.Reflection;

namespace EZFramework.LuaInjector
{
    [Serializable]
    public class Injection
    {
        public string key;
        public UnityEngine.Object value;
        public string typeName;

        public static List<Type> typeList = new List<Type>
        {
            typeof(UnityEngine.Object),
            typeof(UnityEngine.GameObject),
            typeof(UnityEngine.RectTransform),
            typeof(UnityEngine.Transform),
            typeof(UnityEngine.UI.Button),
            typeof(UnityEngine.UI.Image),
            typeof(UnityEngine.UI.Slider),
            typeof(UnityEngine.UI.Text),
            typeof(UnityEngine.UI.Toggle),
            typeof(UnityEngine.UI.ToggleGroup),
        };

        public static Type GetType(string typeName, bool deepSearch = true)
        {
            foreach (Type type in typeList)
            {
                if (type.FullName == typeName) return type;
            }
            if (deepSearch)
            {
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    Type type = assembly.GetType(typeName);
                    if (type != null) return type;
                }
            }
            return null;
        }
    }
}