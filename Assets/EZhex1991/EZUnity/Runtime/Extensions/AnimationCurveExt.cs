/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-11-28 15:37:38
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public static class AnimationCurveExt
    {
        public static void Clear(this AnimationCurve curve)
        {
            System.Array.Clear(curve.keys, 0, curve.length);
        }
        public static void Merge(this AnimationCurve curve, AnimationCurve other)
        {
            for (int i = 0; i < other.length; i++)
            {
                curve.AddKey(other[i]);
            }
        }
        public static float GetLastTime(this AnimationCurve curve)
        {
            return curve.length > 0 ? curve.keys[curve.length - 1].time : 0;
        }
        public static float GetTimeSpan(this AnimationCurve curve)
        {
            if (curve.length <= 1) return 0;
            return curve.keys[curve.length - 1].time - curve.keys[0].time;
        }
    }
}
