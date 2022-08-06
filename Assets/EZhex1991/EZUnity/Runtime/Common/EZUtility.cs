/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-01-24 14:53:33
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public static partial class EZUtility
    {
        public static bool IsNetAvailable
        {
            get { return Application.internetReachability != NetworkReachability.NotReachable; }
        }
        public static bool IsNetLocal
        {
            get { return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork; }
        }

        public static void DrawGizmosPolyLine(params Vector3[] vertices)
        {
            for (int i = 0; i < vertices.Length - 1; i++)
            {
                Gizmos.DrawLine(vertices[i], vertices[i + 1]);
            }
        }

        public static void DrawGizmosArrow(Vector3 startPoint, Vector3 direction, float halfWidth, Vector3 normal)
        {
            Vector3 sideDir = Vector3.Cross(direction, normal).normalized * halfWidth;
            Vector3[] vertices = new Vector3[8];
            vertices[0] = startPoint + sideDir * 0.5f;
            vertices[1] = vertices[0] + direction * 0.5f;
            vertices[2] = vertices[1] + sideDir * 0.5f;
            vertices[3] = startPoint + direction;
            vertices[4] = startPoint - sideDir + direction * 0.5f;
            vertices[5] = vertices[4] + sideDir * 0.5f;
            vertices[6] = startPoint - sideDir * 0.5f;
            vertices[7] = vertices[0];
            DrawGizmosPolyLine(vertices);
        }
        public static void DrawGizmosCameraFrustum(Camera camera)
        {
            Gizmos.matrix = Matrix4x4.TRS(camera.transform.position, camera.transform.rotation, Vector3.one);
            if (camera.orthographic)
            {
                float spread = camera.farClipPlane - camera.nearClipPlane;
                float center = (camera.farClipPlane + camera.nearClipPlane) * 0.5f;
                Gizmos.DrawWireCube(new Vector3(0, 0, center), new Vector3(camera.orthographicSize * 2 * camera.aspect, camera.orthographicSize * 2, spread));
            }
            else
            {
                Gizmos.DrawFrustum(Vector3.zero, camera.fieldOfView, camera.farClipPlane, camera.nearClipPlane, camera.aspect);
            }
        }

        public static void GetReflectionMatrix(Vector4 plane, ref Matrix4x4 reflectionMatrix)
        {
            reflectionMatrix.m00 = 1f - 2f * plane[0] * plane[0];
            reflectionMatrix.m01 = -2f * plane[0] * plane[1];
            reflectionMatrix.m02 = -2f * plane[0] * plane[2];
            reflectionMatrix.m03 = -2f * plane[0] * plane[3];

            reflectionMatrix.m10 = -2f * plane[1] * plane[0];
            reflectionMatrix.m11 = 1 - 2f * plane[1] * plane[1];
            reflectionMatrix.m12 = -2f * plane[1] * plane[2];
            reflectionMatrix.m13 = -2f * plane[1] * plane[3];

            reflectionMatrix.m20 = -2f * plane[2] * plane[0];
            reflectionMatrix.m21 = -2f * plane[2] * plane[1];
            reflectionMatrix.m22 = 1 - 2f * plane[2] * plane[2];
            reflectionMatrix.m23 = -2f * plane[2] * plane[3];

            reflectionMatrix.m30 = 0;
            reflectionMatrix.m31 = 0;
            reflectionMatrix.m32 = 0;
            reflectionMatrix.m33 = 1;
        }
    }
}