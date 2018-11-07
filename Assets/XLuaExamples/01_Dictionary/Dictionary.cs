/*
 * Author:      #AUTHORNAME#
 * CreateTime:  #CREATETIME#
 * Description:
 * 
*/
using System.Collections;

namespace EZhex1991.XLuaExample
{
    [XLua.LuaCallCSharp]
    public class Dictionary
    {
        // 词典不能使用string和object索引，getter可使用TryGetValue，setter用该方法代替
        public static void SetItem(IDictionary dict, object key, object value)
        {
            dict[key] = value;
        }
    }
}