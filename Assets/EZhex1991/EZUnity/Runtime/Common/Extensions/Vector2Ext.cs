/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-11-28 15:21:54
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public static class Vector2Ext
    {
        public static float RangeLerp(this Vector2 v, float t)
        {
            return Mathf.Lerp(v.x, v.y, t);
        }
        public static float RangeInverseLerp(this Vector2 v, float value)
        {
            return Mathf.InverseLerp(v.x, v.y, value);
        }
        public static float RangeRandom(this Vector2 v)
        {
            return UnityEngine.Random.Range(v.x, v.y);
        }

        public static float ComponentMin(this Vector2 v)
        {
            return Mathf.Min(v.x, v.y);
        }
        public static float ComponentMax(this Vector2 v)
        {
            return Mathf.Max(v.x, v.y);
        }
        public static Vector2 ComponentAbs(this Vector2 v)
        {
            return new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
        }
        public static Vector2 ComponentMultiply(this Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x * v2.x, v1.y * v2.y);
        }

        public static Vector2 Round(this Vector2 v)
        {
            return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
        }
        public static Vector2 Round(this Vector2 v, int digits)
        {
            return new Vector2((float)Math.Round(v.x, digits), (float)Math.Round(v.y, digits));
        }
        public static Vector2Int RoundToInt(this Vector2 v)
        {
            return new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
        }
    }
}
