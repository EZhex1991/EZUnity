/*
 * Author:      熊哲
 * CreateTime:  6/13/2017 6:08:50 PM
 * Description:
 * 
*/
using System;
using XLua;

namespace EZFramework.LuaBehaviour
{
    [LuaCallCSharp]
    public class ApplicationBehaviour : _LuaBehaviour<ApplicationBehaviour>
    {
        private Action<LuaTable, bool> luaOnApplicationFocus;
        private Action<LuaTable, bool> luaOnApplicationPause;
        void Start()
        {
            self.Get("LuaOnApplicationFocus", out luaOnApplicationFocus);
            self.Get("LuaOnApplicationPause", out luaOnApplicationPause);
        }
        void OnApplicationFocus(bool focusStatus)
        {
            if (luaOnApplicationFocus != null) luaOnApplicationFocus(self, focusStatus);
        }
        void OnApplicationPause(bool pauseStatus)
        {
            if (luaOnApplicationPause != null) luaOnApplicationPause(self, pauseStatus);
        }
    }
}