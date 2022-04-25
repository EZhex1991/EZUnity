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
        public static float ComponentMax(this Vector4 v)
        {
            return Mathf.Max(Mathf.Max(v.x, v.y), Mathf.Max(v.z, v.w));
        }
        public static float ComponentMin(this Vector4 v)
        {
            return Mathf.Min(Mathf.Min(v.x, v.y), Mathf.Min(v.z, v.w));
        }
        public static Vector4 ComponentAbs(this Vector4 v)
        {
            return new Vector4(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z), Mathf.Abs(v.w));
        }
        public static Vector4 ComponentMultiply(this Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z, v1.w * v2.w);
        }

        public static Vector4 Round(this Vector4 v)
        {
            return new Vector4(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z), Mathf.Round(v.w));
        }
        public static Vector4 Round(this Vector4 v, int digits)
        {
            return new Vector4((float)Math.Round(v.x, digits), (float)Math.Round(v.y, digits), (float)Math.Round(v.z, digits), (float)Math.Round(v.w, digits));
        }
    }
}
