/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-04-12 15:20:40
 * Organization:    SmileTech
 * Description:     
 */
using System.Collections.Generic;
using UnityEngine;

namespace EZUnity.Playables
{
    public static class KeyframeUtility
    {
        public static void Replace<T>(List<T> frames, List<T> other) where T : IKeyframe
        {
            if (other.Count == 0)
            {
                Debug.LogWarning("no frames");
                return;
            }
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
                if (mergeRange[0] > frameRange[1])
                {
                    frames.InsertRange(frames.Count, other);
                }
                else if (mergeRange[1] < frameRange[0])
                {
                    frames.InsertRange(0, other);
                }
                else
                {
                    Vector2Int range = GetRangeByTime(frames, mergeRange);
                    if (other[0].time <= frames[range[0]].time)
                    {
                        range[0] -= 1;
                    }
                    frames.RemoveRange(range[0] + 1, range[1] - range[0]);
                    frames.InsertRange(range[0] + 1, other);
                }
            }
        }

        public static Vector2Int GetRangeByTime<T>(List<T> frames, Vector2 timeRange) where T : IKeyframe
        {
            if (frames.Count == 0) return Vector2Int.zero;
            int startIndex = GetIndexByTime(frames, timeRange.x);
            int endIndex = GetIndexByTime(frames, timeRange.y);
            return new Vector2Int(startIndex, endIndex);
        }

        public static int GetIndexByTime<T>(List<T> frames, float time) where T : IKeyframe
        {
            if (frames.Count == 0) return 0;
            return FindKeyframe(time, frames);
        }

        public static int FindKeyframe<T>(float time, List<T> frames) where T : IKeyframe
        {
            if (frames.Count == 0 || time <= frames[0].time) return 0;
            int last = frames.Count - 1;
            if (time >= frames[last].time) return last;
            return FindKeyframe(time, frames, 0, last);
        }

        public static int FindKeyframe<T>(float time, List<T> frames, int min, int max) where T : IKeyframe
        {
            int mid = (min + max) / 2;
            if (min == mid) return min;
            else if (time < frames[mid].time) return FindKeyframe(time, frames, min, mid);
            else return FindKeyframe(time, frames, mid, max);
        }
    }
}
