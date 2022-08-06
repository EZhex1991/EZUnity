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
        public static void Replace(this AnimationCurve curve, AnimationCurve other)
        {
            if (other.length == 0) return;
            if (other[0].time > other[other.length - 1].time)
            {
                Debug.LogErrorFormat("Invalid Replace Range: {0} - {1}", other[0].time, other[other.length - 1].time);
                return;
            }
            if (curve.length == 0)
            {
                curve.Merge(other);
            }
            else
            {
                Vector2 frameRange = new Vector2(curve[0].time, curve[curve.length - 1].time);
                Vector2 mergeRange = new Vector2(other[0].time, other[other.length - 1].time);
                if (mergeRange[1] < frameRange[0] || mergeRange[0] > frameRange[1])
                {
                    curve.Merge(other);
                }
                else
                {
                    int startIndex = GetIndex(mergeRange.x, curve.keys);
                    int endIndex = GetIndex(mergeRange.y, curve.keys);
                    if (mergeRange.x == curve[startIndex].time)
                    {
                        startIndex -= 1;
                    }
                    for (int i = startIndex; i < endIndex; i++)
                    {
                        curve.RemoveKey(startIndex + 1);
                    }
                    curve.Merge(other);
                }
            }
        }

        public static float GetTimeSpan(this AnimationCurve curve)
        {
            if (curve.length <= 1) return 0;
            return curve.keys[curve.length - 1].time - curve.keys[0].time;
        }

        public static bool GetLastFrame(this AnimationCurve curve, out Keyframe keyframe)
        {
            if (curve.length > 0)
            {
                keyframe = curve.keys[curve.length - 1];
                return true;
            }
            else
            {
                keyframe = new Keyframe();
                return false;
            }
        }
        public static void AppendKey(this AnimationCurve curve, float time, float value)
        {
            Keyframe key = new Keyframe(time, value);
            int lastIndex = curve.length - 1;
            if (curve.length > 1 && curve.keys[lastIndex].value == value && curve.keys[lastIndex - 1].value == value)
            {
                curve.RemoveKey(lastIndex);
            }
            curve.AddKey(key);
        }

        private static int GetIndex(float time, Keyframe[] frames)
        {
            if (frames.Length == 0 || time < frames[0].time) return 0;
            int last = frames.Length - 1;
            if (time >= frames[last].time) return last;
            return GetIndex(time, frames, 0, last);
        }
        private static int GetIndex(float time, Keyframe[] frames, int min, int max)
        {
            int mid = (min + max) / 2;
            if (mid == min) return min;
            if (time < frames[mid].time) return GetIndex(time, frames, min, mid);
            else if (time > frames[mid].time) return GetIndex(time, frames, mid, max);
            else return mid;
        }
    }
}
