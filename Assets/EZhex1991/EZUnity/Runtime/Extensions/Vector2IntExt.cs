/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-11-28 15:30:12
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public static class Vector2IntExt
    {
        public static float RangeLerp(this Vector2Int v, float t)
        {
            return Mathf.Lerp(v.x, v.y, t);
        }
        public static float RangeInverseLerp(this Vector2Int v, float value)
        {
            return Mathf.InverseLerp(v.x, v.y, value);
        }
        public static int RangeRandom(this Vector2Int v)
        {
            return UnityEngine.Random.Range(v.x, v.y);
        }

        public static int ComponentMax(this Vector2Int v)
        {
            return Mathf.Max(v.x, v.y);
        }
        public static int ComponentMin(this Vector2Int v)
        {
            return Mathf.Min(v.x, v.y);
        }
        public static Vector2Int ComponentAbs(this Vector2Int v)
        {
            return new Vector2Int(Mathf.Abs(v.x), Mathf.Abs(v.y));
        }
        public static Vector2Int ComponentMultiply(this Vector2Int v1, Vector2Int v2)
        {
            return new Vector2Int(v1.x * v2.x, v1.y * v2.y);
        }
    }
}
