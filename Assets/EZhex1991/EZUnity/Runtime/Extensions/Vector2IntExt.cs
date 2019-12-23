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
        public static float Lerp(this Vector2Int v2, float t)
        {
            return Mathf.Lerp(v2.x, v2.y, t);
        }
        public static int Random(this Vector2Int v2)
        {
            return UnityEngine.Random.Range(v2.x, v2.y);
        }

        public static int MaxComponent(this Vector2Int v2)
        {
            return Mathf.Max(v2.x, v2.y);
        }
        public static int MinComponent(this Vector2Int v2)
        {
            return Mathf.Min(v2.x, v2.y);
        }
        public static Vector2Int Abs(this Vector2Int v2)
        {
            return new Vector2Int(Mathf.Abs(v2.x), Mathf.Abs(v2.y));
        }
    }
}
