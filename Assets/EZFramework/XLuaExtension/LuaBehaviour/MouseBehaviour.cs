/*
 * Author:      熊哲
 * CreateTime:  6/13/2017 6:09:05 PM
 * Description:
 * 
*/
using System;
using XLua;

namespace EZFramework.LuaBehaviour
{
    [LuaCallCSharp]
    public class MouseBehaviour : _LuaBehaviour<MouseBehaviour>
    {
        private Action<LuaTable> luaOnMouseEnter;
        private Action<LuaTable> luaOnMouseOver;
        private Action<LuaTable> luaOnMouseDown;
        private Action<LuaTable> luaOnMouseDrag;
        private Action<LuaTable> luaOnMouseUp;
        private Action<LuaTable> luaOnMouseExit;
        private Action<LuaTable> luaOnMouseUpAsButton;
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
            if (luaOnMouseEnter != null) luaOnMouseEnter(self);
        }
        void OnMouseOver()
        {
            if (luaOnMouseOver != null) luaOnMouseOver(self);
        }
        void OnMouseDown()
        {
            if (luaOnMouseDown != null) luaOnMouseDown(self);
        }
        void OnMouseDrag()
        {
            if (luaOnMouseDrag != null) luaOnMouseDrag(self);
        }
        void OnMouseUp()
        {
            if (luaOnMouseUp != null) luaOnMouseUp(self);
        }
        void OnMouseExit()
        {
            if (luaOnMouseExit != null) luaOnMouseExit(self);
        }
        void OnMouseUpAsButton()
        {
            if (luaOnMouseUpAsButton != null) luaOnMouseUpAsButton(self);
        }
    }
}