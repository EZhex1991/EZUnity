/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-12-10 16:20:16
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EZhex1991.EZUnity.Framework
{
    public class EZInputManager : EZMonoBehaviourSingleton<EZInputManager>
    {
        public bool simulateMouseWithTouches = true;
        public float moveThreshold = 5f;

        private bool touched;
        private Vector3 touchPosition;
        private bool moved;

        public delegate void InputBlocker(Vector3 mousePosition, ref bool blocked);
        public event InputBlocker inputBlockers;
        public event Action<Vector3> onTouchDown;
        public event Action<Vector3> onTouchHover;
        public event Action<Vector3> onTouchMove;
        public event Action onTouchEnd;
        public event Action<Vector3> onTouchClick;
        public event Action<Touch, Touch> onDoubleTouch;

        protected override void Init()
        {
            Input.simulateMouseWithTouches = simulateMouseWithTouches;
        }

        protected virtual void Update()
        {
            if (Input.touchCount == 2)
            {
                touched = false;
                moved = false;
                if (onDoubleTouch != null) onDoubleTouch(Input.touches[1], Input.touches[0]);
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                // ui block
                if (EventSystem.current.IsPointerOverGameObject() || EventSystem.current.IsPointerOverGameObject(0))
                {
                    return;
                }
                // touch block
                bool blocked = false;
                if (inputBlockers != null) inputBlockers(Input.mousePosition, ref blocked);
                if (blocked) return;

                if (onTouchDown != null) onTouchDown(Input.mousePosition);
                touched = true;
                moved = false;
                touchPosition = Input.mousePosition;
            }
            else if (touched && Input.GetMouseButton(0))
            {
                Vector3 delta = Input.mousePosition - touchPosition;
                if (delta.magnitude > moveThreshold) moved = true;
                if (onTouchHover != null) onTouchHover(Input.mousePosition);
                if (moved && onTouchMove != null) onTouchMove(delta);
                touchPosition = Input.mousePosition;
            }
            else if (touched && Input.GetMouseButtonUp(0))
            {
                if (onTouchEnd != null) onTouchEnd();
                if (!moved && onTouchClick != null) onTouchClick(Input.mousePosition);
                touched = false;
                moved = false;
            }
        }
    }
}
