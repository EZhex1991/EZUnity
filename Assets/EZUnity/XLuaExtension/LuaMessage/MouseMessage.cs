/*
 * Author:      熊哲
 * CreateTime:  9/26/2017 5:44:07 PM
 * Description:
 * 
*/
#if XLUA
using UnityEngine;

namespace EZUnity.XLuaExtension
{
    [DisallowMultipleComponent]
    public class MouseMessage : _Message<MouseMessage>
    {
        public class MouseEvent : OnMessageEvent { }

        public MouseEvent onMouseEnter = new MouseEvent();
        public MouseEvent onMouseOver = new MouseEvent();
        public MouseEvent onMouseExit = new MouseEvent();
        public MouseEvent onMouseDown = new MouseEvent();
        public MouseEvent onMouseDrag = new MouseEvent();
        public MouseEvent onMouseUp = new MouseEvent();
        public MouseEvent onMouseUpAsButton = new MouseEvent();

        void OnMouseEnter()
        {
            onMouseEnter.Invoke();
        }
        void OnMouseOver()
        {
            onMouseOver.Invoke();
        }
        void OnMouseExit()
        {
            onMouseExit.Invoke();
        }
        void OnMouseDown()
        {
            onMouseDown.Invoke();
        }
        void OnMouseDrag()
        {
            onMouseDrag.Invoke();
        }
        void OnMouseUp()
        {
            onMouseUp.Invoke();
        }
        void OnMouseUpAsButton()
        {
            onMouseUpAsButton.Invoke();
        }
    }
}
#endif
