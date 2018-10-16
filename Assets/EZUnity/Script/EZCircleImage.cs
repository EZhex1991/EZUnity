/*
 * Author:      熊哲
 * CreateTime:  9/30/2017 2:43:23 PM
 * Description:
 * 用于圆形响应按钮的Image
*/
using UnityEngine;
using UnityEngine.UI;

namespace EZUnity
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class EZCircleImage : Image
    {
        private CircleCollider2D m_Circle;
        private CircleCollider2D circle
        {
            get
            {
                if (m_Circle == null)
                {
                    m_Circle = GetComponent<CircleCollider2D>();
                }
                return m_Circle;
            }
        }

        public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            return IsPointInside(screenPoint, eventCamera);
        }
        // 判断鼠标是否在多边形区域内
        protected bool IsPointInside(Vector2 point, Camera eventCamera)
        {
            Vector3 center = (Vector3)circle.offset + transform.position;
            if (eventCamera != null)
            {
                center = eventCamera.WorldToScreenPoint(center);
            }
            return (point - (Vector2)center).magnitude <= circle.radius;
        }
    }
}