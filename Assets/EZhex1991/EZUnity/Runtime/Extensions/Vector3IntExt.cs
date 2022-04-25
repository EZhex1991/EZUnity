/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-11-28 15:30:25
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public static class Vector3IntExt
    {
        public static Vector3Int EulerNormalize(this Vector3Int angles)
        {
            for (int i = 0; i < 3; i++)
            {
                if (angles[i] < -180) angles[i] = angles[i] + 360;
                else if (angles[i] > 180) angles[i] = angles[i] - 360;
            }
            return angles;
        }

        public static int ComponentMax(this Vector3Int v)
        {
            return Mathf.Max(v.x, Mathf.Max(v.y, v.z));
        }
        public static int ComponentMin(this Vector3Int v)
        {
            return Mathf.Min(v.x, Mathf.Min(v.y, v.z));
        }
        public static Vector3Int ComponentAbs(this Vector3Int v)
        {
            return new Vector3Int(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }
        public static Vector3Int ComponentMultiply(this Vector3Int v1, Vector3Int v2)
        {
            return new Vector3Int(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }
    }
}
