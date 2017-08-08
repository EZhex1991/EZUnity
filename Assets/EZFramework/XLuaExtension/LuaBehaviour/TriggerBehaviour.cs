/*
 * Author:      熊哲
 * CreateTime:  6/13/2017 6:08:32 PM
 * Description:
 * 
*/
using System;
using UnityEngine;
using XLua;

namespace EZFramework.LuaBehaviour
{
    [LuaCallCSharp]
    public class TriggerBehaviour : _LuaBehaviour<TriggerBehaviour>
    {
        private Action<LuaTable, Collider> luaOnTriggerEnter;
        private Action<LuaTable, Collider> luaOnTriggerStay;
        private Action<LuaTable, Collider> luaOnTriggerExit;
        void Awake()
        {
            bindings[gameObject].Get("LuaOnTriggerEnter", out luaOnTriggerEnter);
            bindings[gameObject].Get("LuaOnTriggerStay", out luaOnTriggerStay);
            bindings[gameObject].Get("LuaOnTriggerExit", out luaOnTriggerExit);
        }
        void OnTriggerEnter(Collider collider)
        {
            if (luaOnTriggerEnter != null) luaOnTriggerEnter(self, collider);
        }
        void OnTriggerStay(Collider collider)
        {
            if (luaOnTriggerStay != null) luaOnTriggerStay(self, collider);
        }
        void OnTriggerExit(Collider collider)
        {
            if (luaOnTriggerExit != null) luaOnTriggerExit(self, collider);
        }
    }
}