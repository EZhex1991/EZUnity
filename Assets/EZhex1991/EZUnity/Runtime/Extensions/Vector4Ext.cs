/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-11-28 14:55:17
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public static class Vector4Ext
    {
        public static float MaxComponent(this Vector4 v4)
        {
            return Mathf.Max(Mathf.Max(v4.x, v4.y), Mathf.Max(v4.z, v4.w));
        }
        public static float MinComponent(this Vector4 v4)
        {
            return Mathf.Min(Mathf.Min(v4.x, v4.y), Mathf.Min(v4.z, v4.w));
        }

        public static Vector4 Abs(this Vector4 v4)
        {
            return new Vector4(Mathf.Abs(v4.x), Mathf.Abs(v4.y), Mathf.Abs(v4.z), Mathf.Abs(v4.w));
        }

        public static Vector4 Round(this Vector4 v4)
        {
            return new Vector4(Mathf.Round(v4.x), Mathf.Round(v4.y), Mathf.Round(v4.z), Mathf.Round(v4.w));
        }
        public static Vector4 Round(this Vector4 v4, int digits)
        {
            return new Vector4((float)Math.Round(v4.x, digits), (float)Math.Round(v4.y, digits), (float)Math.Round(v4.z, digits), (float)Math.Round(v4.w, digits));
        }
    }
}
