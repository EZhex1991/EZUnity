/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-11-28 15:36:18
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public static class TransformExt
    {
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

        public static Quaternion TransformRotation(this Transform tf, Quaternion q)
        {
            return tf.rotation * q;
        }
        public static Quaternion InverseTransformRotation(this Transform tf, Quaternion q)
        {
            return Quaternion.Inverse(tf.rotation) * q;
        }

        public static void CopyTRSFrom(this Transform tf, Transform target, bool recursive)
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RegisterFullObjectHierarchyUndo(tf, "Copy TRS");
#endif
            tf.localPosition = target.localPosition;
            tf.localRotation = target.localRotation;
            tf.localScale = target.localScale;
            if (recursive)
            {
                foreach (Transform child in tf)
                {
                    Transform childTarget = target.Find(child.name);
                    if (childTarget != null)
                    {
                        child.CopyTRSFrom(childTarget, recursive);
                    }
                }
            }
        }
    }
}
