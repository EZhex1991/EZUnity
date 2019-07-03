/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-04-19 14:20:04
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections.Generic;
using XLua;

namespace EZhex1991.EZUnity.Example
{
    public static class GenListForCSharp
    {
        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp = new List<Type>()
        {
            typeof(System.Collections.Generic.List<int>),
            typeof(System.Collections.Generic.List<float>),
            typeof(System.Collections.Generic.List<string>),
            typeof(System.Collections.Generic.List<bool>),
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
