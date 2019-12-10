/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-09-26 17:19:35
 * Organization:    #ORGANIZATION#
 * Description:     
 * 
 * xlua原LuaBehaviour只是样例，这个LuaMessage为了防止重名，但仍然是！示例！
 * 全部以event方式由Unity的Message驱动，不会用delegate和event的见上一个例子
 */
using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace EZhex1991.EZUnity.XLuaExample
{
    public class LuaMessage : MonoBehaviour
    {
        // 给一个GameObject添加Message组件 —— 换种说法就是获取一个GameObject的MonoBehaviour消息
        public static LuaMessage Require(GameObject obj)
        {
            LuaMessage transmitter = obj.GetComponent<LuaMessage>();
            if (transmitter == null)
            {
                transmitter = obj.AddComponent<LuaMessage>();
            }
            return transmitter;
        }
        public static void Dismiss(GameObject obj)
        {
            if (obj == null) return;
            LuaMessage message = obj.GetComponent<LuaMessage>();
            if (message == null) return;
            message.Dismiss();
        }
        public void Dismiss()
        {
            Destroy(this);
        }

        // 该例子我随便挑了几个Message，实际上常用Message很多，我对其进行了分类封装，参考链接：
        // https://github.com/EZhex1991/EZUnity/tree/master/Assets/EZUnity/XLuaExtension/LuaMessage

        // Start
        public OnMessageEvent start = new OnMessageEvent();
        // OnTriggerEnter
        public OnMessageEvent<Collider> onTriggerEnter = new OnMessageEvent<Collider>();
        // OnCollisionEnter
        public OnMessageEvent<Collision> onCollisionEnter = new OnMessageEvent<Collision>();

        private void Start()
        {
            start.Invoke();
        }
        private void OnTriggerEnter(Collider other)
        {
            onTriggerEnter.Invoke(other);
        }
        private void OnCollisionEnter(Collision collision)
        {
            onCollisionEnter.Invoke(collision);
        }
    }

    // 以下为事件的封装
    public delegate void OnMessageAction();
    public class OnMessageEvent
    {
        private event OnMessageAction m_Event;

        public void AddAction(OnMessageAction action)
        {
            m_Event += action;  // 把OnMessageAction加到LuaCallCSharp，就可以在lua侧直接message:AddAction(func)来添加方法
        }
        public void RemoveAction(OnMessageAction action)
        {
            m_Event -= action;  // 如果方法里会用到self，可以使用xlua.util.bind：message:AddAction(bind(self.func, self))
        }
        public void Clear()
        {
            m_Event = null;
        }
        public void Invoke()
        {
            if (m_Event != null) m_Event.Invoke();
        }
    }
    // 一个参数的泛型事件的封装
    public delegate void OnMessageAction<T>(T arg);
    public class OnMessageEvent<T>
    {
        private event OnMessageAction<T> m_Event;

        public void AddAction(OnMessageAction<T> action)
        {
            m_Event += action;
        }
        public void RemoveAction(OnMessageAction<T> action)
        {
            m_Event -= action;
        }
        public void Clear()
        {
            m_Event = null;
        }
        public void Invoke(T arg)
        {
            if (m_Event != null) m_Event.Invoke(arg);
        }
    }

    // 以下为配置
    public static class LuaMessageConfig
    {
        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp = new List<Type>()
        {
            typeof(LuaMessage),
            typeof(OnMessageEvent),
            typeof(Collider),
            typeof(Collision),
            typeof(OnMessageEvent<Collider>),
            typeof(OnMessageEvent<Collision>),
        };
        [CSharpCallLua]
        public static List<Type> CSharpCallLua = new List<Type>()
        {
            typeof(OnMessageAction),
            typeof(OnMessageAction<Collider>),
            typeof(OnMessageAction<Collision>),
        };
    }
}