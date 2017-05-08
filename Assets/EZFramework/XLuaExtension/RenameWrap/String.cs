/*
 * Author:      熊哲
 * CreateTime:  4/11/2017 4:24:54 PM
 * Description:
 * 
*/
using UnityEngine;
using XLua;

namespace EZFramework
{
    [LuaCallCSharp]
    public static class String
    {
        public static string Format(string format, params object[] args)
        {
            return System.String.Format(format, args);
        }
    }
}