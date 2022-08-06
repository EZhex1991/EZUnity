/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-11-28 15:22:14
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public static class Vector3Ext
    {
        public static Vector3 EulerNormalize(this Vector3 angles)
        {
            for (int i = 0; i < 3; i++)
            {
                if (angles[i] < -180) angles[i] = angles[i] + 360;
                else if (angles[i] > 180) angles[i] = angles[i] - 360;
            }
            return angles;
        }

        public static float ComponentMax(this Vector3 v)
        {
            return Mathf.Max(v.x, Mathf.Max(v.y, v.z));
        }
        public static float ComponentMin(this Vector3 v)
        {
            return Mathf.Min(v.x, Mathf.Min(v.y, v.z));
        }
        public static Vector3 ComponentAbs(this Vector3 v)
        {
            return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }
        public static Vector3 ComponentMultiply(this Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        public static Vector3 Round(this Vector3 v)
        {
            return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
        }
        public static Vector3 Round(this Vector3 v, int digits)
        {
            return new Vector3((float)Math.Round(v.x, digits), (float)Math.Round(v.y, digits), (float)Math.Round(v.z, digits));
        }
        public static Vector3Int RoundToInt(this Vector3 v)
        {
            return new Vector3Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
        }

        public static Color ToColor(this Vector3 v)
        {
            return new Color(v.x, v.y, v.z);
        }
        public static Color ToColor256(this Vector3 v)
        {
            return new Color(v.x / 255f, v.y / 255f, v.z / 255f);
        }
    }
}
