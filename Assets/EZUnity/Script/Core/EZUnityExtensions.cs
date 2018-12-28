/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-06-13 19:19:11
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZUnity
{
    public static partial class EZUnityExtensions
    {
        public static bool Contains(this LayerMask mask, int layer)
        {
            return (mask | (1 << layer)) == mask;
        }

        public static Vector2 GetSize(this RectTransform rt)
        {
            Vector2 anchorDistance = rt.anchorMax - rt.anchorMin;
            Vector2 rectSize = new Vector2(rt.rect.size.x * anchorDistance.x, rt.rect.size.y * anchorDistance.y);
            return rectSize + rt.sizeDelta;
        }

        public static void SetAnchoredPositionX(this RectTransform rt, float x)
        {
            Vector2 anchoredPosition = rt.anchoredPosition;
            anchoredPosition.x = x;
            rt.anchoredPosition = anchoredPosition;
        }
        public static void SetAnchoredPositionY(this RectTransform rt, float y)
        {
            Vector2 anchoredPosition = rt.anchoredPosition;
            anchoredPosition.y = y;
            rt.anchoredPosition = anchoredPosition;
        }

        public static void SetPositionX(this Transform tf, float x)
        {
            Vector3 position = tf.position;
            position.x = x;
            tf.position = position;
        }
        public static void SetPositionY(this Transform tf, float y)
        {
            Vector3 position = tf.position;
            position.y = y;
            tf.position = position;
        }
        public static void SetPositionZ(this Transform tf, float z)
        {
            Vector3 position = tf.position;
            position.z = z;
            tf.position = position;
        }
        public static void SetLocalPositionX(this Transform tf, float x)
        {
            Vector3 position = tf.localPosition;
            position.x = x;
            tf.localPosition = position;
        }
        public static void SetLocalPositionY(this Transform tf, float y)
        {
            Vector3 position = tf.localPosition;
            position.y = y;
            tf.localPosition = position;
        }
        public static void SetLocalPositionZ(this Transform tf, float z)
        {
            Vector3 position = tf.localPosition;
            position.z = z;
            tf.localPosition = position;
        }

        public static void SetEulerAngleX(this Transform tf, float x)
        {
            Vector3 angles = tf.eulerAngles;
            angles.x = x;
            tf.eulerAngles = angles;
        }
        public static void SetEulerAngleY(this Transform tf, float y)
        {
            Vector3 angles = tf.eulerAngles;
            angles.y = y;
            tf.eulerAngles = angles;
        }
        public static void SetEulerAngleZ(this Transform tf, float z)
        {
            Vector3 angles = tf.eulerAngles;
            angles.z = z;
            tf.eulerAngles = angles;
        }
        public static void SetLocalEulerAngleX(this Transform tf, float x)
        {
            Vector3 angles = tf.localEulerAngles;
            angles.x = x;
            tf.localEulerAngles = angles;
        }
        public static void SetLocalEulerAngleY(this Transform tf, float y)
        {
            Vector3 angles = tf.localEulerAngles;
            angles.y = y;
            tf.localEulerAngles = angles;
        }
        public static void SetLocalEulerAngleZ(this Transform tf, float z)
        {
            Vector3 angles = tf.localEulerAngles;
            angles.z = z;
            tf.localEulerAngles = angles;
        }

        public static Vector2 Abs(this Vector2 v)
        {
            return new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
        }

        public static Vector3 EulerNormalize(this Vector3 angles)
        {
            for (int i = 0; i < 3; i++)
            {
                if (angles[i] < -180) angles[i] = angles[i] + 360;
                else if (angles[i] > 180) angles[i] = angles[i] - 360;
            }
            return angles;
        }
        public static Vector3 Abs(this Vector3 v)
        {
            return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }

        public static float Magnitude(this Quaternion rotation)
        {
            return Mathf.Sqrt((Quaternion.Dot(rotation, rotation)));
        }
        public static Quaternion Add(this Quaternion q1, Quaternion q2)
        {
            q1.x += q2.x;
            q1.y += q2.y;
            q1.z += q2.z;
            q1.w += q2.w;
            return q1;
        }
        public static Quaternion Scale(this Quaternion rotation, float multiplier)
        {
            rotation.x *= multiplier;
            rotation.y *= multiplier;
            rotation.z *= multiplier;
            rotation.w *= multiplier;
            return rotation;
        }
#if !UNITY_2018_2_OR_NEWER
        public static Quaternion Normalize(this Quaternion rotation)
        {
            float magnitude = rotation.Magnitude();
            if (magnitude > 0f)
            {
                return rotation.Scale(1f / magnitude);
            }
            else
            {
                Debug.LogWarning("Cannot normalize a quaternion with zero magnitude.");
                return Quaternion.identity;
            }
        }
#endif
    }
}
