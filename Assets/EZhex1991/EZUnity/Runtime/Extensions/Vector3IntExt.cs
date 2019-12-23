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

        public static int MaxComponent(this Vector3Int v3)
        {
            return Mathf.Max(v3.x, Mathf.Max(v3.y, v3.z));
        }
        public static int MinComponent(this Vector3Int v3)
        {
            return Mathf.Min(v3.x, Mathf.Min(v3.y, v3.z));
        }

        public static Vector3Int Abs(this Vector3Int v3)
        {
            return new Vector3Int(Mathf.Abs(v3.x), Mathf.Abs(v3.y), Mathf.Abs(v3.z));
        }
    }
}
