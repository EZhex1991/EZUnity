/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-09-30 13:36:20
 * Organization:    #ORGANIZATION#
 * Description:     用于多边形响应按钮的Image
 */
using UnityEngine;
using UnityEngine.UI;

namespace EZhex1991.EZUnity
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class EZPolygonImage : Image
    {
        private PolygonCollider2D m_Polygon;
        private PolygonCollider2D polygon
        {
            get
            {
                if (m_Polygon == null)
                {
                    m_Polygon = GetComponent<PolygonCollider2D>();
                }
                return m_Polygon;
            }
        }

        public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            return IsPointInside(screenPoint, eventCamera);
        }
        // 判断鼠标是否在多边形区域内
        protected bool IsPointInside(Vector2 point, Camera eventCamera)
        {
            int intersections = 0;
            Vector2[] vertexes = polygon.points;
            Vector3 offset = transform.position;
            for (int i = 0; i < vertexes.Length; i++)
            {
                Vector3 startPoint = (Vector3)vertexes[i] + offset;
                Vector3 endPoint = (i + 1 == vertexes.Length ? (Vector3)vertexes[0] : (Vector3)vertexes[i + 1]) + offset;
                if (eventCamera != null)
                {
                    startPoint = eventCamera.WorldToScreenPoint(startPoint);
                    endPoint = eventCamera.WorldToScreenPoint(endPoint);
                }
                // 交叉判断
                if (CheckHorizontalIntersection(point, startPoint, endPoint))
                {
                    // 统计单侧交点数
                    float slope = (endPoint.y - startPoint.y) / (endPoint.x - startPoint.x);
                    if ((point.y - startPoint.y) / slope + startPoint.x <= point.x)
                        intersections++;
                }
            }
            return intersections % 2 != 0;
        }
        // 判断向量是否与点所在水平线（也可以换成垂直线）交叉
        protected bool CheckHorizontalIntersection(Vector2 checkPoint, Vector2 startPoint, Vector2 endPoint)
        {
            return (startPoint.y <= checkPoint.y && endPoint.y >= checkPoint.y) || (startPoint.y >= checkPoint.y && endPoint.y <= checkPoint.y);
        }
    }
}