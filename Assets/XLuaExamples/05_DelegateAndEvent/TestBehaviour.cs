/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-10-11 13:14:44
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace EZhex1991.EZUnity.XLuaExample
{
    public class TestBehaviour : MonoBehaviour
    {
        public delegate void OnTriggerEnterAction(Collider collider);
        public class OnCollisionEnterEvent
        {
            private event Action<Collision> m_Event;
            public void Add(Action<Collision> action)
            {
                m_Event += action;
            }
            public void Remove(Action<Collision> action)
            {
                m_Event -= action;
            }
            public void Call(Collision collision)
            {
                if (m_Event != null) m_Event(collision);
            }
        }

        public Action updateAction;   // System.Action
        public OnTriggerEnterAction onTriggerEnterAction = delegate (Collider other) { print(other.name); };   // 自定义delegate
        public OnCollisionEnterEvent onCollisionEnterEvent = new OnCollisionEnterEvent(); // 名为Event，实际是class，例如Button.onClick(UnityEngine.UI.Button.ButtonClickedEvent)
        public event Action onDestroyEvent;  // event
        public event Action<int> testEvent;

        void Update()
        {
            if (updateAction != null) updateAction();
        }
        void OnTriggerEnter(Collider collider)
        {
            if (onTriggerEnterAction != null) onTriggerEnterAction(collider);
        }
        void OnCollisionEnter(Collision collision)
        {
            onCollisionEnterEvent.Call(collision);  // 这里没有判断是否为null，因为Call方法会判断
        }
        void OnDestroy()
        {
            if (onDestroyEvent != null) onDestroyEvent();
            if (testEvent != null) testEvent(1);
        }

        public void NonStaticFunction()
        {
            print("createdelegate, non-static test");
        }
        public static void StaticFunction(int test)
        {
            print("createdelegate, static test " + test);
        }
    }

    public static class GenList
    {
        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp = new List<Type>()
        {
            typeof(TestBehaviour),
            typeof(TestBehaviour.OnCollisionEnterEvent),  // 注意，这是个class
        };
        [CSharpCallLua] // 因为是lua传过来的方法适配到了delegate由CSharp去调用，所以是CSharpCallLua
        public static List<Type> CSharpCallLua = new List<Type>()
        {
            // 这个其实很容易理解：lua给你传过来一个function，你需要用什么类型的delegate去接收它，那么就把这个类型加到这里
            // 对于System.Action<T>这种无法打标签的，必须通过这种列表方式添加，具体看文档
            typeof(System.Action),  // update和onDestroy都需要这个
            typeof(TestBehaviour.OnTriggerEnterAction), // onTriggerEnterEvent需要这个，其实可以换成Action<Collider>
            typeof(System.Action<Collision>),   // OnCollisionEnterEvent里需要这个
            typeof(System.Action<int>), // testEvent需要这个
        };
    }
}