/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-04-12 15:20:40
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEngine;

namespace EZhex1991.EZUnity.Playables
{
    public static class KeyframeUtility
    {
        public static void Replace<T>(List<T> frames, List<T> other) where T : IKeyframe
        {
            if (other.Count == 0) return;
            if (other[0].time > other[other.Count - 1].time)
            {
                Debug.LogErrorFormat("Invalid Replace Range: {0} - {1}", other[0].time, other[other.Count - 1].time);
                return;
            }
            if (frames.Count == 0)
            {
                frames.InsertRange(0, other);
            }
            else
            {
                Vector2 frameRange = new Vector2(frames[0].time, frames[frames.Count - 1].time);
                Vector2 mergeRange = new Vector2(other[0].time, other[other.Count - 1].time);
                if (mergeRange[1] < frameRange[0])
                {
                    frames.InsertRange(0, other);
                }
                else if (mergeRange[0] > frameRange[1])
                {
                    frames.InsertRange(frames.Count, other);
                }
                else
                {
                    int startIndex = GetIndex(mergeRange.x, frames);
                    int endIndex = GetIndex(mergeRange.y, frames);
                    if (mergeRange.x == frames[startIndex].time)
                    {
                        startIndex -= 1;
                    }
                    frames.RemoveRange(startIndex + 1, endIndex - startIndex);
                    frames.InsertRange(startIndex + 1, other);
                }
            }
        }
        public static void Replace(AnimationCurve curve, AnimationCurve other)
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

        public static int GetIndex<T>(float time, IList<T> frames) where T : IKeyframe
        {
            if (frames.Count == 0 || time <= frames[0].time) return 0;
            int last = frames.Count - 1;
            if (time >= frames[last].time) return last;
            return GetIndex(time, frames, 0, last);
        }
        public static int GetIndex(float time, Keyframe[] frames)
        {
            if (frames.Length == 0 || time < frames[0].time) return 0;
            int last = frames.Length - 1;
            if (time >= frames[last].time) return last;
            return GetIndex(time, frames, 0, last);
        }

        public static int GetIndex<T>(float time, IList<T> frames, int min, int max) where T : IKeyframe
        {
            int mid = (min + max) / 2;
            if (mid == min) return min;
            if (time < frames[mid].time) return GetIndex(time, frames, min, mid);
            else if (time > frames[mid].time) return GetIndex(time, frames, mid, max);
            else return mid;
        }
        public static int GetIndex(float time, Keyframe[] frames, int min, int max)
        {
            int mid = (min + max) / 2;
            if (mid == min) return min;
            if (time < frames[mid].time) return GetIndex(time, frames, min, mid);
            else if (time > frames[mid].time) return GetIndex(time, frames, mid, max);
            else return mid;
        }
    }
}
