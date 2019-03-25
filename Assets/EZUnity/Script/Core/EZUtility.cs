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

        public static Gradient GradientFadeOut()
        {
            GradientColorKey[] colorKeys = new GradientColorKey[]
            {
                new GradientColorKey(Color.white, 0),
                new GradientColorKey(Color.black, 1),
            };
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[]
            {
                new GradientAlphaKey(1, 0),
                new GradientAlphaKey(0, 1),
            };

            Gradient gradient = new Gradient();
            gradient.SetKeys(colorKeys, alphaKeys);
            return gradient;
        }
        public static Gradient GradientFadeIn()
        {
            GradientColorKey[] colorKeys = new GradientColorKey[]
            {
                new GradientColorKey(Color.black, 0),
                new GradientColorKey(Color.white, 1),
            };
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[]
            {
                new GradientAlphaKey(0, 0),
                new GradientAlphaKey(1, 1),
            };

            Gradient gradient = new Gradient();
            gradient.SetKeys(colorKeys, alphaKeys);
            return gradient;
        }

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
        public static void DrawGizmosCamera(Camera camera)
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