/*
 * Author:      熊哲
 * CreateTime:  3/15/2017 1:39:06 PM
 * Description:
 * 
*/
using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace EZFramework
{
    public abstract class TLuaBehaviour<T> : MonoBehaviour
        where T : TLuaBehaviour<T>
    {
        public LuaTable self;
        public static void Bind(GameObject obj, LuaTable self)
        {
            T behaviour = obj.AddComponent<T>();
            behaviour.self = self;
        }
        public static void Remove(GameObject obj)
        {
            Destroy(obj.GetComponent<T>());
        }
    }

    [LuaCallCSharp]
    public class LuaBehaviour : TLuaBehaviour<LuaBehaviour>
    {
        private Action<LuaTable, GameObject> luaStart;
        private Action<LuaTable, GameObject> luaOnEnable;
        private Action<LuaTable, GameObject> luaOnDisable;
        private Action<LuaTable, GameObject> luaOnDestroy;
        void Start()
        {
            self.Get("LuaStart", out luaStart);
            self.Get("LuaOnEnable", out luaOnEnable);
            self.Get("LuaOnDisable", out luaOnDisable);
            self.Get("LuaOnDestroy", out luaOnDestroy);
            if (luaStart != null) luaStart(self, gameObject);
        }
        void OnEnable()
        {
            if (luaOnEnable != null) luaOnEnable(self, gameObject);
        }
        void OnDisable()
        {
            if (luaOnDisable != null) luaOnDisable(self, gameObject);
        }
        void OnDestroy()
        {
            if (luaOnDestroy != null) luaOnDestroy(self, gameObject);
        }
    }

    [LuaCallCSharp]
    public class LuaUpdateBehaviour : TLuaBehaviour<LuaUpdateBehaviour>
    {
        private Action<LuaTable, GameObject> luaUpdate;
        private Action<LuaTable, GameObject> luaFixedUpdate;
        private Action<LuaTable, GameObject> luaLateUpdate;
        void Start()
        {
            self.Get("LuaUpdate", out luaUpdate);
            self.Get("LuaFixedUpdate", out luaFixedUpdate);
            self.Get("LuaLateUpdate", out luaLateUpdate);
        }
        void Update()
        {
            if (luaUpdate != null) luaUpdate(self, gameObject);
        }
        void FixedUpdate()
        {
            if (luaFixedUpdate != null) luaFixedUpdate(self, gameObject);
        }
        void LateUpdate()
        {
            if (luaLateUpdate != null) luaLateUpdate(self, gameObject);
        }
    }

    [LuaCallCSharp]
    public class LuaCollisionBehaviour : TLuaBehaviour<LuaCollisionBehaviour>
    {
        private Action<LuaTable, GameObject, Collision> luaOnCollisionEnter;
        private Action<LuaTable, GameObject, Collision> luaOnCollisionStay;
        private Action<LuaTable, GameObject, Collision> luaOnCollisionExit;
        void Start()
        {
            self.Get("LuaOnCollisionEnter", out luaOnCollisionEnter);
            self.Get("LuaOnCollisionStay", out luaOnCollisionStay);
            self.Get("LuaOnCollisionExit", out luaOnCollisionExit);
        }
        void OnCollisionEnter(Collision collision)
        {
            if (luaOnCollisionEnter != null) luaOnCollisionEnter(self, gameObject, collision);
        }
        void OnCollisionStay(Collision collision)
        {
            if (luaOnCollisionStay != null) luaOnCollisionStay(self, gameObject, collision);
        }
        void OnCollisionExit(Collision collision)
        {
            if (luaOnCollisionExit != null) luaOnCollisionExit(self, gameObject, collision);
        }
    }

    [LuaCallCSharp]
    public class LuaTriggerBehaviour : TLuaBehaviour<LuaTriggerBehaviour>
    {
        private Action<LuaTable, GameObject, Collider> luaOnTriggerEnter;
        private Action<LuaTable, GameObject, Collider> luaOnTriggerStay;
        private Action<LuaTable, GameObject, Collider> luaOnTriggerExit;
        void Start()
        {
            self.Get("LuaOnTriggerEnter", out luaOnTriggerEnter);
            self.Get("LuaOnTriggerStay", out luaOnTriggerStay);
            self.Get("LuaOnTriggerExit", out luaOnTriggerExit);
        }
        void OnTriggerEnter(Collider collider)
        {
            if (luaOnTriggerEnter != null) luaOnTriggerEnter(self, gameObject, collider);
        }
        void OnTriggerStay(Collider collider)
        {
            if (luaOnTriggerStay != null) luaOnTriggerStay(self, gameObject, collider);
        }
        void OnTriggerExit(Collider collider)
        {
            if (luaOnTriggerExit != null) luaOnTriggerExit(self, gameObject, collider);
        }
    }

    public static class XLuaBehaviourGen
    {
        [CSharpCallLua]
        public static List<Type> CSharpCallLua = new List<Type>()
        {
            typeof(System.Action<LuaTable, GameObject>),
            typeof(System.Action<LuaTable, GameObject, Collision>),
            typeof(System.Action<LuaTable, GameObject, Collider>),
        };
    }
}