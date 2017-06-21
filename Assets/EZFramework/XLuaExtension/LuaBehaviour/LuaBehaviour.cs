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
        private Action<LuaTable> luaStart;
        private Action<LuaTable> luaOnEnable;
        private Action<LuaTable> luaOnDisable;
        private Action<LuaTable> luaOnDestroy;
        void Start()
        {
            self.Get("LuaStart", out luaStart);
            self.Get("LuaOnEnable", out luaOnEnable);
            self.Get("LuaOnDisable", out luaOnDisable);
            self.Get("LuaOnDestroy", out luaOnDestroy);
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
        void OnDestroy()
        {
            if (luaOnDestroy != null) luaOnDestroy(self);
        }
    }
}