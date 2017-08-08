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
        void Awake()
        {
            bindings[gameObject].Get("LuaOnMouseEnter", out luaOnMouseEnter);
            bindings[gameObject].Get("LuaOnMouseOver", out luaOnMouseOver);
            bindings[gameObject].Get("LuaOnMouseDown", out luaOnMouseDown);
            bindings[gameObject].Get("LuaOnMouseDrag", out luaOnMouseDrag);
            bindings[gameObject].Get("LuaOnMouseUp", out luaOnMouseUp);
            bindings[gameObject].Get("LuaOnMouseExit", out luaOnMouseExit);
            bindings[gameObject].Get("LuaOnMouseUpAsButton", out luaOnMouseUpAsButton);
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