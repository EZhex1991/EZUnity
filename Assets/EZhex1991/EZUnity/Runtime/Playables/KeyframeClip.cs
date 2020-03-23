/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-04-17 10:35:16
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace EZhex1991.EZUnity.Playables
{
    public abstract class KeyframeClip<T> : PlayableAsset
        where T : IKeyframe
    {
        public List<T> keyframes = new List<T>();

        public override double duration
        {
            get
            {
                try
                {
                    return keyframes[keyframes.Count - 1].time;
                }
                catch
                {
                    return 0;
                }
            }
        }

        public abstract override Playable CreatePlayable(PlayableGraph graph, GameObject go);
    }

    public abstract class KeyframeBehaviour<T> : PlayableBehaviour
    where T : IKeyframe
    {
        public KeyframeClip<T> clip;
        public virtual List<T> keyframes { get { return clip.keyframes; } }
        protected List<T> tempFrames = new List<T>();
        public T outputFrame { get; set; }

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            Clear();
        }
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (tempFrames.Count > 0)
            {
#if UNITY_EDITOR
                UnityEditor.Undo.RegisterCompleteObjectUndo(clip, "Record " + typeof(T).Name);
#endif
                KeyframeUtility.Replace(clip.keyframes, tempFrames);
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(clip);
                UnityEditor.AssetDatabase.SaveAssets();
#endif
            }
            Clear();
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            if (keyframes == null || keyframes.Count == 0) return;
            float time = (float)playable.GetTime();
            int frameIndex = KeyframeUtility.GetIndex(time, keyframes);
            T startFrame = keyframes[frameIndex];
            T endFrame = frameIndex >= keyframes.Count - 1 ? keyframes[frameIndex] : keyframes[frameIndex + 1];
            float process = Mathf.InverseLerp(startFrame.time, endFrame.time, time);
            InterpolateFrame(startFrame, endFrame, process);
        }

        protected abstract void InterpolateFrame(T startFrame, T endFrame, float process);

        public void Record(T frame)
        {
            tempFrames.Add(frame);
        }
        public void Clear()
        {
            tempFrames.Clear();
        }
    }
}