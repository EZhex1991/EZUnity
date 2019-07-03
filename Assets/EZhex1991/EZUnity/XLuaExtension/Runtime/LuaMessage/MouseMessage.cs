/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-09-26 17:44:07
 * Organization:    #ORGANIZATION#
 * Description:     
 */
#if XLUA
using UnityEngine;

namespace EZhex1991.EZUnity.XLuaExtension
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
