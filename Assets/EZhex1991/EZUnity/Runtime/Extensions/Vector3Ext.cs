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

        public static float MaxComponent(this Vector3 v3)
        {
            return Mathf.Max(v3.x, Mathf.Max(v3.y, v3.z));
        }
        public static float MinComponent(this Vector3 v3)
        {
            return Mathf.Min(v3.x, Mathf.Min(v3.y, v3.z));
        }

        public static Vector3 Abs(this Vector3 v3)
        {
            return new Vector3(Mathf.Abs(v3.x), Mathf.Abs(v3.y), Mathf.Abs(v3.z));
        }

        public static Vector3 Round(this Vector3 v3)
        {
            return new Vector3(Mathf.Round(v3.x), Mathf.Round(v3.y), Mathf.Round(v3.z));
        }
        public static Vector3 Round(this Vector3 v3, int digits)
        {
            return new Vector3((float)Math.Round(v3.x, digits), (float)Math.Round(v3.y, digits), (float)Math.Round(v3.z, digits));
        }
        public static Vector3Int RoundToInt(this Vector3 v3)
        {
            return new Vector3Int(Mathf.RoundToInt(v3.x), Mathf.RoundToInt(v3.y), Mathf.RoundToInt(v3.z));
        }

        public static Color ToColor(this Vector3 v3)
        {
            return new Color(v3.x, v3.y, v3.z);
        }
        public static Color ToColor256(this Vector3 v3)
        {
            return new Color(v3.x / 255f, v3.y / 255f, v3.z / 255f);
        }
    }
}
