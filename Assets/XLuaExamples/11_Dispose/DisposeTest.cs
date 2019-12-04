/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-11-25 13:36:11
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace EZhex1991.EZUnity.XLuaExample
{
    /// <summary>
    /// -----Dispose并不是必要的-----
    /// -----Dispose并不是必要的-----
    /// -----Dispose并不是必要的-----
    /// 通常来说，整个应用只需要一个LuaEnv，并且这个LuaEnv的生命周期与该应用一致，换句话说，Dispose大部分时候意味着应用要退出了
    /// 你根本不用在乎这个LuaEnv在退出前是否被合理释放 - 它是应用的一部分，自然会随着应用的结束而被系统处理
    /// 
    /// 如果你真的要手动Dispose，你只需要释放掉被引用的lua方法即可，具体说明请查看官方FAQ
    /// </summary>
    [LuaCallCSharp]
    public class DisposeTest : LuaManager
    {
        public static Action testAction;
        public static event Action testEvent;

        public static Action unregister;

        private void Start()
        {
            unregister = luaEnv.Global.Get<Action>("Unregister");

            unregister();
            // C#变量引用的lua function置为null
            unregister = null;
        }

        private void OnDestroy()
        {
            luaEnv.Dispose();
            print("LuaEnv Disposed");
        }

        public static void TestFunction()
        {
            print("C# Function");
        }
    }

    public static class DisposeTestConfig
    {
        public static List<Type> CSharpCallLua = new List<Type>()
        {
            typeof(Action),
        };
    }
}
