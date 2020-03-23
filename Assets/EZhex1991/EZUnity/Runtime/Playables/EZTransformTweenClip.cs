/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-06-22 21:36:01
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace EZhex1991.EZUnity.Playables
{
    public class EZTransformTweenClip : PlayableAsset, ITimelineClipAsset
    {
        public ExposedReference<Transform> startPoint;
        public ExposedReference<Transform> endPoint;
        [EZLockedFoldout]
        public EZTransformTweenBehaviour templateBehaviour = new EZTransformTweenBehaviour();

        public ClipCaps clipCaps { get { return ClipCaps.Blending | ClipCaps.Looping; } }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            ScriptPlayable<EZTransformTweenBehaviour> playable = ScriptPlayable<EZTransformTweenBehaviour>.Create(graph, templateBehaviour);
            EZTransformTweenBehaviour behaviour = playable.GetBehaviour();
            behaviour.startPoint = startPoint.Resolve(graph.GetResolver());
            behaviour.endPoint = endPoint.Resolve(graph.GetResolver());
            return playable;
        }
    }

    [Serializable]
    public class EZTransformTweenBehaviour : PlayableBehaviour
    {
        [NonSerialized]
        public Transform startPoint;
        [NonSerialized]
        public Transform endPoint;

        public bool tweenPosition = true;
        public bool tweenRotation = true;
        public bool tweenScale = true;
        public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [NonSerialized]
        public Vector3 startPosition;
        [NonSerialized]
        public Quaternion startRotation = Quaternion.identity;
        [NonSerialized]
        public Vector3 startScale;

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            if (startPoint != null)
            {
                startPosition = startPoint.position;
                startRotation = startPoint.rotation;
                startScale = startPoint.localScale;
            }
        }
    }
}