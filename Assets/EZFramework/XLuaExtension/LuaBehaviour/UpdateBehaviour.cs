/*
 * Author:      熊哲
 * CreateTime:  6/13/2017 6:06:48 PM
 * Description:
 * 
*/
using System;
using XLua;

namespace EZFramework.LuaBehaviour
{
    [LuaCallCSharp]
    public class UpdateBehaviour : _LuaBehaviour<UpdateBehaviour>
    {
        private Action<LuaTable> luaUpdate;
        private Action<LuaTable> luaFixedUpdate;
        private Action<LuaTable> luaLateUpdate;
        void Awake()
        {
            bindings[gameObject].Get("LuaUpdate", out luaUpdate);
            bindings[gameObject].Get("LuaFixedUpdate", out luaFixedUpdate);
            bindings[gameObject].Get("LuaLateUpdate", out luaLateUpdate);
        }
        void Update()
        {
            if (luaUpdate != null) luaUpdate(self);
        }
        void FixedUpdate()
        {
            if (luaFixedUpdate != null) luaFixedUpdate(self);
        }
        void LateUpdate()
        {
            if (luaLateUpdate != null) luaLateUpdate(self);
        }
    }
}