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
    /* 
     * 因为在Lua中可能出现多个GameObject绑定了同一个LuaTable，
     * 也就是不同GameObject的Message可能调用了同一个LuaFunction，
     * 所以在每个方法上我都将GameObject传了回去；
     */
    public abstract class TLuaBehaviour<T> : MonoBehaviour
        where T : TLuaBehaviour<T>
    {
        public LuaTable self;
        public static void Bind(GameObject obj, LuaTable self)
        {
            T behaviour = obj.AddComponent<T>();    // 在这一句实际上Awake已经执行了，所以Awake方法无法被动态绑定；
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

    [LuaCallCSharp]
    public class LuaApplicationBehaviour : TLuaBehaviour<LuaApplicationBehaviour>
    {
        private Action<LuaTable, GameObject, bool> luaOnApplicationFocus;
        private Action<LuaTable, GameObject, bool> luaOnApplicationPause;
        void Start()
        {
            self.Get("LuaOnApplicationFocus", out luaOnApplicationFocus);
            self.Get("LuaOnApplicationPause", out luaOnApplicationPause);
        }
        void OnApplicationFocus(bool focusStatus)
        {
            if (luaOnApplicationFocus != null) luaOnApplicationFocus(self, gameObject, focusStatus);
        }
        void OnApplicationPause(bool pauseStatus)
        {
            if (luaOnApplicationPause != null) luaOnApplicationPause(self, gameObject, pauseStatus);
        }
    }

    [LuaCallCSharp]
    public class LuaMouseBehaviour : TLuaBehaviour<LuaMouseBehaviour>
    {
        private Action<LuaTable, GameObject> luaOnMouseEnter;
        private Action<LuaTable, GameObject> luaOnMouseOver;
        private Action<LuaTable, GameObject> luaOnMouseDown;
        private Action<LuaTable, GameObject> luaOnMouseDrag;
        private Action<LuaTable, GameObject> luaOnMouseUp;
        private Action<LuaTable, GameObject> luaOnMouseExit;
        private Action<LuaTable, GameObject> luaOnMouseUpAsButton;
        void Start()
        {
            self.Get("LuaOnMouseEnter", out luaOnMouseEnter);
            self.Get("LuaOnMouseOver", out luaOnMouseOver);
            self.Get("LuaOnMouseDown", out luaOnMouseDown);
            self.Get("LuaOnMouseDrag", out luaOnMouseDrag);
            self.Get("LuaOnMouseUp", out luaOnMouseUp);
            self.Get("LuaOnMouseExit", out luaOnMouseExit);
            self.Get("LuaOnMouseUpAsButton", out luaOnMouseUpAsButton);
        }
        void OnMouseEnter()
        {
            if (luaOnMouseEnter != null) luaOnMouseEnter(self, gameObject);
        }
        void OnMouseOver()
        {
            if (luaOnMouseOver != null) luaOnMouseOver(self, gameObject);
        }
        void OnMouseDown()
        {
            if (luaOnMouseDown != null) luaOnMouseDown(self, gameObject);
        }
        void OnMouseDrag()
        {
            if (luaOnMouseDrag != null) luaOnMouseDrag(self, gameObject);
        }
        void OnMouseUp()
        {
            if (luaOnMouseUp != null) luaOnMouseUp(self, gameObject);
        }
        void OnMouseExit()
        {
            if (luaOnMouseExit != null) luaOnMouseExit(self, gameObject);
        }
        void OnMouseUpAsButton()
        {
            if (luaOnMouseUpAsButton != null) luaOnMouseUpAsButton(self, gameObject);
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
            typeof(System.Action<LuaTable, GameObject, bool>),
        };
    }
}