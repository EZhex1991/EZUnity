/*
 * Author:      熊哲
 * CreateTime:  6/13/2017 5:57:36 PM
 * Description:
 * 
*/
using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace EZFramework.LuaBehaviour
{
    /* 
     * ----- 以下说明为基于网络资料的个人理解，不保证可靠性 -----
     * MonoBehaviour的内部Message是一种反射调用的消息机制，但是并非所有的MonoBehaviour都需要响应所有消息；
     * Unity会判断哪些MonoBehaviour需要响应对应的消息（判断是否存在对应的事件方法）来避免无意义的调用；
     * 官方也建议，在MonoBehaviour中去除不用的事件方法来避免额外的性能消耗；
     * 因此，这里对MonoBehaviour传入Lua的消息进行了分类封装；
     * 另外，分类之后能很清楚地看到某个物体需要在lua上响应哪些事件，也能很方便地移除不需要继续响应的事件；
     */
    public abstract class _LuaBehaviour<T> : MonoBehaviour
        where T : _LuaBehaviour<T>
    {
        public LuaTable self;
        public static T Bind(GameObject obj, LuaTable self)
        {
            T behaviour = obj.AddComponent<T>();    // 在这一句实际上Awake已经执行了，所以Awake方法无法被动态绑定；
            behaviour.self = self;
            return behaviour;
        }
        public static void Remove(GameObject obj)
        {
            if (obj == null) return;
            foreach (T behaviour in obj.GetComponents<T>())
            {
                Destroy(behaviour);
            }
        }
        public static void Remove(T behaviour)
        {
            if (behaviour == null) return;
            Destroy(behaviour);
        }
    }

    public static class XLuaBehaviourGen
    {
        [CSharpCallLua]
        public static List<Type> CSharpCallLua = new List<Type>()
        {
            typeof(System.Action<LuaTable>),
            typeof(System.Action<LuaTable, Collision>),
            typeof(System.Action<LuaTable, Collider>),
            typeof(System.Action<LuaTable, bool>),
        };
    }
}