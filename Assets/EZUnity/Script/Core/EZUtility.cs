/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-01-24 14:53:33
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZUnity
{
    public static partial class EZUtility
    {
        public const int AssetOrder = 1000;

        public static bool IsNetAvailable
        {
            get { return Application.internetReachability != NetworkReachability.NotReachable; }
        }
        public static bool IsNetLocal
        {
            get { return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork; }
        }

        public static void DrawGizmosPolyline(bool close, params Vector3[] vertices)
        {
            int i = 0;
            while (i < vertices.Length - 1)
            {
                Gizmos.DrawLine(vertices[i], vertices[i + 1]);
                i++;
            }
            if (close) Gizmos.DrawLine(vertices[i], vertices[0]);
        }

        public static void DrawGizmosArrow(Vector3 startPoint, Vector3 direction, float halfWidth, Vector3 normal)
        {
            Vector3 sideStep = Vector3.Cross(direction, normal).normalized * halfWidth * 0.5f;
            Vector3[] vertices = new Vector3[7];
            vertices[3] = startPoint + direction;
            vertices[0] = startPoint + sideStep;
            vertices[6] = startPoint - sideStep;
            vertices[1] = vertices[0] + direction * 0.5f;
            vertices[5] = vertices[6] + direction * 0.5f;
            vertices[2] = vertices[1] + sideStep;
            vertices[4] = vertices[5] - sideStep;
            DrawGizmosPolyline(true, vertices);
        }
        public static void DrawGizmosArrowTriangle(Vector3 startPoint, Vector3 direction, float halfWidth, Vector3 normal)
        {
            Vector3 sideDirection = Vector3.Cross(direction, normal).normalized * halfWidth;
            Vector3[] vertices = new Vector3[3];
            vertices[0] = startPoint + direction;
            vertices[1] = startPoint + sideDirection;
            vertices[2] = startPoint - sideDirection;
            DrawGizmosPolyline(true, vertices);
        }
    }
}