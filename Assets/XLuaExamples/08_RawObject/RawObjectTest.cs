/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-11-22 10:05:40
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using XLua;

namespace EZUnity.Example
{
    public class RawObjectTest : MonoBehaviour
    {
        public static string GetTypeName(object o)
        {
            return o.GetType().ToString();
        }

        private void Start()
        {
            LuaEnv luaenv = new LuaEnv();
            luaenv.DoString(@"
                local RawByteArray = CS.EZUnity.Example.RawByteArray
                local RawObjectTest = CS.EZUnity.Example.RawObjectTest
                -- 直接传递byte[]，lua会作为string处理，通过继承RawObject的RawByteArray，实现string以byte[]方式传递
                local byteArray = RawByteArray.GetBytes('string from lua')
                local rawByteArray = RawByteArray('string from lua')
                print(RawObjectTest.GetTypeName(byteArray), byteArray)
                print(RawObjectTest.GetTypeName(rawByteArray), rawByteArray)
            ");
            luaenv.Dispose();
        }
    }
}
