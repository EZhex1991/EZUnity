/* Author:          熊哲
 * CreateTime:      2018-04-17 10:35:36
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace EZUnity.Playables
{
    public abstract class KeyframeBehaviour<T> : PlayableBehaviour
        where T : IKeyframe
    {
        public List<T> keyframes { get; set; }
        public T outputFrame { get; set; }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            if (keyframes == null || keyframes.Count == 0) return;
            float time = (float)playable.GetTime();
            int frameIndex = KeyframeUtility.FindKeyframe(time, keyframes);
            T startFrame = keyframes[frameIndex];
            T endFrame = frameIndex >= keyframes.Count - 1 ? keyframes[frameIndex] : keyframes[frameIndex + 1];
            float process = Mathf.InverseLerp(startFrame.time, endFrame.time, time);
            outputFrame = InterpolateFrame(startFrame, endFrame, process);
        }

        protected abstract T InterpolateFrame(T startFrame, T endFrame, float process);
    }
}
