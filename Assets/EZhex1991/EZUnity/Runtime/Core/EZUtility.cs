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

        public static Quaternion QuaternionCumulate(Quaternion q1, Quaternion q2)
        {
            if (Quaternion.Dot(q1, q2) < 0)
            {
                return QuaternionAdd(q1, new Quaternion(-q2.x, -q2.y, -q2.z, -q2.w));
            }
            else
            {
                return QuaternionAdd(q1, q2);
            }
        }
        public static Quaternion QuaternionAdd(Quaternion q1, Quaternion q2)
        {
            return new Quaternion(
                q1.x + q2.x,
                q1.y + q2.y,
                q1.z + q2.z,
                q1.w + q2.w
            );
        }

        public static Vector2 Abs(Vector2 v2)
        {
            return new Vector2(Mathf.Abs(v2.x), Mathf.Abs(v2.y));
        }
        public static Vector3 Abs(Vector3 v3)
        {
            return new Vector3(Mathf.Abs(v3.x), Mathf.Abs(v3.y), Mathf.Abs(v3.z));
        }
        public static Vector4 Abs(Vector4 v4)
        {
            return new Vector4(Mathf.Abs(v4.x), Mathf.Abs(v4.y), Mathf.Abs(v4.z), Mathf.Abs(v4.w));
        }
        public static float MaxComponent(Vector2 v2)
        {
            return Mathf.Max(v2.x, v2.y);
        }
        public static float MaxComponent(Vector3 v3)
        {
            return Mathf.Max(v3.x, v3.y, v3.z);
        }
        public static float MaxComponent(Vector4 v4)
        {
            return Mathf.Max(v4.x, v4.y, v4.z, v4.w);
        }
        public static float MinComponent(Vector2 v2)
        {
            return Mathf.Min(v2.x, v2.y);
        }
        public static float MinComponent(Vector3 v3)
        {
            return Mathf.Min(v3.x, v3.y, v3.z);
        }
        public static float MinComponent(Vector4 v4)
        {
            return Mathf.Min(v4.x, v4.y, v4.z, v4.w);
        }

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