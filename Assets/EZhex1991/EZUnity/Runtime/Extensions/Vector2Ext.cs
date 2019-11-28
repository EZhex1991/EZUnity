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
        public static float Lerp(this Vector2 v2, float t)
        {
            return Mathf.Lerp(v2.x, v2.y, t);
        }
        public static float InverseLerp(this Vector2 v2, float value)
        {
            return Mathf.InverseLerp(v2.x, v2.y, value);
        }
        public static float Random(this Vector2 v2)
        {
            return UnityEngine.Random.Range(v2.x, v2.y);
        }

        public static float MaxComponent(this Vector2 v2)
        {
            return Mathf.Max(v2.x, v2.y);
        }
        public static float MinComponent(this Vector2 v2)
        {
            return Mathf.Min(v2.x, v2.y);
        }

        public static Vector2 Abs(this Vector2 v2)
        {
            return new Vector2(Mathf.Abs(v2.x), Mathf.Abs(v2.y));
        }

        public static Vector2 Round(this Vector2 v2)
        {
            return new Vector2(Mathf.Round(v2.x), Mathf.Round(v2.y));
        }
        public static Vector2 Round(this Vector2 v2, int digits)
        {
            return new Vector2((float)Math.Round(v2.x, digits), (float)Math.Round(v2.y, digits));
        }
        public static Vector2Int RoundToInt(this Vector2 v2)
        {
            return new Vector2Int(Mathf.RoundToInt(v2.x), Mathf.RoundToInt(v2.y));
        }
    }
}
