/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-06-13 19:19:11
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public enum ColorChannel { R, G, B, A }
    public enum TextureEncoding { PNG, JPG, TGA }

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

        public static Vector3 WorldCenter(this BoxCollider collider)
        {
            return collider.transform.TransformPoint(collider.center);
        }
        public static Vector3 WorldCenter(this SphereCollider collider)
        {
            return collider.transform.TransformPoint(collider.center);
        }
        public static Vector3 WorldCenter(this CapsuleCollider collider)
        {
            return collider.transform.TransformPoint(collider.center);
        }

        public static Vector3 WorldExtents(this BoxCollider collider)
        {
            return collider.transform.TransformVector(collider.size / 2);
        }

        public static Vector2 Abs(this Vector2 v)
        {
            return new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
        }
        public static float Max(this Vector2 v)
        {
            return Mathf.Max(v.x, v.y);
        }
        public static float Min(this Vector2 v)
        {
            return Mathf.Min(v.x, v.y);
        }
        public static float Lerp(this Vector2 v, float t)
        {
            return Mathf.Lerp(v.x, v.y, t);
        }
        public static float InverseLerp(this Vector2 v, float value)
        {
            return Mathf.InverseLerp(v.x, v.y, value);
        }

        public static Vector3 Abs(this Vector3 v)
        {
            return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }
        public static float Max(this Vector3 v)
        {
            return Mathf.Max(v.x, v.y, v.z);
        }
        public static float Min(this Vector3 v)
        {
            return Mathf.Min(v.x, v.y, v.z);
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

        public static Vector4 Abs(this Vector4 v)
        {
            return new Vector4(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z), Mathf.Abs(v.w));
        }
        public static float Max(this Vector4 v)
        {
            return Mathf.Max(v.x, v.y, v.z, v.w);
        }
        public static float Min(this Vector4 v)
        {
            return Mathf.Min(v.x, v.y, v.z, v.w);
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

        public static void Clear(this AnimationCurve curve)
        {
            System.Array.Clear(curve.keys, 0, curve.length);
        }
        public static void Merge(this AnimationCurve curve, AnimationCurve other)
        {
            for (int i = 0; i < other.length; i++)
            {
                curve.AddKey(other[i]);
            }
        }
        public static float GetLastTime(this AnimationCurve curve)
        {
            return curve.length > 0 ? curve.keys[curve.length - 1].time : 0;
        }
        public static float GetTimeSpan(this AnimationCurve curve)
        {
            if (curve.length <= 1) return 0;
            return curve.keys[curve.length - 1].time - curve.keys[0].time;
        }

        public static float GetChannel(this Color color, ColorChannel channel)
        {
            switch (channel)
            {
                case ColorChannel.R: return color.r;
                case ColorChannel.G: return color.g;
                case ColorChannel.B: return color.b;
                case ColorChannel.A: return color.a;
            }
            return 0;
        }

        public static byte[] Encode(this Texture2D texture, TextureEncoding encoding)
        {
            switch (encoding)
            {
                case TextureEncoding.PNG: return texture.EncodeToPNG();
                case TextureEncoding.JPG: return texture.EncodeToJPG();
                case TextureEncoding.TGA:
#if UNITY_2018_3_OR_NEWER
                    return texture.EncodeToTGA();
#else
                    Debug.LogError("TGA encoding is not available on Unity2018.2 or earlier version, PNG encoding will be used");
                    break;
#endif
            }
            return texture.EncodeToPNG();
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

        public static string FormatKeyword(string prefix, Enum selection)
        {
            return string.Format("{0}_{1}", prefix, selection).ToUpper();
        }
        public static void SetKeyword(this Material mat, string prefix, Enum selection)
        {
            foreach (Enum value in Enum.GetValues(selection.GetType()))
            {
                mat.DisableKeyword(FormatKeyword(prefix, value));
            }
            mat.EnableKeyword(FormatKeyword(prefix, selection));
        }
        public static void SetKeyword(this Material mat, string keyword, bool value)
        {
            if (value) { mat.EnableKeyword(keyword); }
            else mat.DisableKeyword(keyword);
        }
        public static bool IsKeywordEnabled(this Material mat, string prefix, Enum selection)
        {
            return mat.IsKeywordEnabled(FormatKeyword(prefix, selection));
        }
    }
}
