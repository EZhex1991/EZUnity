/*
 * Author:      熊哲
 * CreateTime:  6/9/2017 4:09:42 PM
 * Description:
 * 
*/
using UnityEngine;
using XLua;

namespace EZhex1991.XLuaExample
{
    [LuaCallCSharp]
    public class CheckNull
    {
        public static bool IsNull(Object o)
        {
            return o == null;
        }
    }
}