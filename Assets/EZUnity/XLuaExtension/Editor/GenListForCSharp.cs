/*
 * Author:      熊哲
 * CreateTime:  4/19/2017 2:20:04 PM
 * Description:
 * 
*/
#if XLUA
using System;
using System.Collections.Generic;
using XLua;

namespace EZUnity.XLuaExtension
{
    public static class GenListForCSharp
    {
        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp = new List<Type>()
        {

        };

        [CSharpCallLua]
        public static List<Type> CSharpCallLua = new List<Type>()
        {
            typeof(System.Action),
            typeof(System.Action<int>),
            typeof(System.Action<float>),
            typeof(System.Action<string>),
            typeof(System.Action<bool>),
            typeof(System.Collections.IEnumerator),
        };

        [BlackList]
        public static List<List<string>> BlackList = new List<List<string>>()
        {

        };
    }
}
#endif
