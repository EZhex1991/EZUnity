/*
 * Author:      熊哲
 * CreateTime:  6/13/2017 6:08:23 PM
 * Description:
 * 
*/
using System;
using UnityEngine;
using XLua;

namespace EZFramework.LuaBehaviour
{
    [LuaCallCSharp]
    public class CollisionBehaviour : _LuaBehaviour<CollisionBehaviour>
    {
        private Action<LuaTable, Collision> luaOnCollisionEnter;
        private Action<LuaTable, Collision> luaOnCollisionStay;
        private Action<LuaTable, Collision> luaOnCollisionExit;
        void Start()
        {
            self.Get("LuaOnCollisionEnter", out luaOnCollisionEnter);
            self.Get("LuaOnCollisionStay", out luaOnCollisionStay);
            self.Get("LuaOnCollisionExit", out luaOnCollisionExit);
        }
        void OnCollisionEnter(Collision collision)
        {
            if (luaOnCollisionEnter != null) luaOnCollisionEnter(self, collision);
        }
        void OnCollisionStay(Collision collision)
        {
            if (luaOnCollisionStay != null) luaOnCollisionStay(self, collision);
        }
        void OnCollisionExit(Collision collision)
        {
            if (luaOnCollisionExit != null) luaOnCollisionExit(self, collision);
        }
    }
}