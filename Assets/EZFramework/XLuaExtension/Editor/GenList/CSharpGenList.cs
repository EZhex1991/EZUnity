/*
 * Author:      熊哲
 * CreateTime:  4/19/2017 2:20:04 PM
 * Description:
 * 
*/
using System;
using System.Collections.Generic;
using XLua;

namespace EZFramework.XLuaConfig
{
    public static class CSharpGenList
    {
        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp = new List<Type>()
        {

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