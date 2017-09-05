/*
 * Author:      熊哲
 * CreateTime:  6/13/2017 6:06:29 PM
 * Description:
 * 
*/
using System;
using XLua;

namespace EZFramework.LuaBehaviour
{
    [LuaCallCSharp]
    public class LuaBehaviour : _LuaBehaviour<LuaBehaviour>
    {
        private Action<LuaTable> luaAwake;
        private Action<LuaTable> luaStart;
        private Action<LuaTable> luaOnEnable;
        private Action<LuaTable> luaOnDisable;
        private Action<LuaTable> luaOnDestroy;
        void Awake()
        {
            bindings[gameObject].Get("LuaAwake", out luaAwake);
            bindings[gameObject].Get("LuaStart", out luaStart);
            bindings[gameObject].Get("LuaOnEnable", out luaOnEnable);
            bindings[gameObject].Get("LuaOnDisable", out luaOnDisable);
            bindings[gameObject].Get("LuaOnDestroy", out luaOnDestroy);
            if (luaAwake != null) luaAwake(bindings[gameObject]);
        }
        void Start()
        {
            if (luaStart != null) luaStart(self);
        }
        void OnEnable()
        {
            if (luaOnEnable != null) luaOnEnable(self);
        }
        void OnDisable()
        {
            if (luaOnDisable != null) luaOnDisable(self);
        }
        protected override void OnDestroy()
        {
            if (luaOnDestroy != null) luaOnDestroy(self);
            base.OnDestroy();
        }
    }
}