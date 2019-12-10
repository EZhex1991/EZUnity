/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-10-11 13:14:44
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XLua;

namespace EZhex1991.EZUnity.XLuaExample
{
    public class DelegateTest : MonoBehaviour
    {
        public Button button;
        public Toggle toggle;

        public Action testAction; // System.Action
        public event Action<string> testEvent; // event System.Action<System.String>

        void Start()
        {
            if (testAction != null) testAction();
            if (testEvent != null) testEvent(name);
        }

        public void NonStaticFunction()
        {
            print("NonStaticFunction");
        }
        public static void StaticFunction(bool value)
        {
            print("StaticFunction, Value = " + value);
        }
    }

    public static class DelegateTestConfig
    {
        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp = new List<Type>()
        {
            typeof(DelegateTest),
            typeof(UnityEngine.UI.Button),
            typeof(UnityEngine.UI.Toggle),
        };
        [CSharpCallLua] // 因为是lua传过来的方法适配到了delegate由CSharp去调用，所以是CSharpCallLua
        public static List<Type> CSharpCallLua = new List<Type>()
        {
            // 这个其实很容易理解：lua给你传过来一个function，你需要用什么类型的delegate去接收它，那么就把这个类型加到这里
            // 对于System.Action<T>这种无法打标签的，必须通过这种列表方式添加，具体看文档
            typeof(System.Action), // testAction需要这个
            typeof(System.Action<string>), // testEvent需要这个
            typeof(UnityEngine.Events.UnityAction), // Button需要这个
            typeof(UnityEngine.Events.UnityAction<bool>), // Toggle需要这个
        };
    }
}