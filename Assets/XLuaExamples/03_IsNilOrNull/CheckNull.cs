/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-06-09 16:09:42
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using XLua;

namespace EZhex1991.EZUnity.XLuaExample
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