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
        /*
         * 之前是在AddComponent之后把luatable赋值给“self”，但这样意味着Component在Awake时“self”为空，所以Awake无法动态绑定
         * 现在用静态词典在AddComponent前记录绑定的luatable，保证Awake可以获取这个“self”
         * 但这样意味着一个GameObject只能绑定一个luatable，所以多个lua脚本响应同一事件需要在lua端进行消息分发
         * 当然，改为Dictionary<GameObject, List<LuaTable>>可以实现和之前一样的效果，只是个人认为在lua上实现会更方便一点
         * 关于这个LuaTable变量的命名问题，自己怎么改都可以，个人在lua上基本只用冒号声明方法，这里的self只是方便在lua上的理解
         * PS：不管在C#上还是Lua上，纠结于self这个“变量名称”只能说明对Lua的self理解不够
         */
        public LuaTable self;
        public static Dictionary<GameObject, LuaTable> bindings = new Dictionary<GameObject, LuaTable>();
        public static T Bind(GameObject obj, LuaTable self, string traceback = "") // traceback用于定位重复绑定的调用位置，lua传入debug.traceback()即可
        {
            if (bindings.ContainsKey(obj))
            {
                Debug.LogException(new InvalidOperationException(typeof(T).ToString() + "already exist, bind failed." + traceback), obj);
                return obj.GetComponent<T>();
            }
            bindings.Add(obj, self);
            T behaviour = obj.AddComponent<T>();    // 在这一句实际上Awake已经执行了
            behaviour.self = self;
            return behaviour;
        }
        public static void Remove(GameObject obj)
        {
            if (obj == null) return;
            Remove(obj.GetComponent<T>());
        }
        public static void Remove(T behaviour)
        {
            if (behaviour == null) return;
            Destroy(behaviour);
        }
        protected virtual void OnDestroy()
        {
            bindings.Remove(gameObject);
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