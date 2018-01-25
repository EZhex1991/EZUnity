/*
 * Author:      熊哲
 * CreateTime:  1/24/2018 2:53:33 PM
 * Description:
 * 
*/
using UnityEngine;

namespace EZFramework
{
    public static class EZFrameworkUtility
    {
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
    }
}