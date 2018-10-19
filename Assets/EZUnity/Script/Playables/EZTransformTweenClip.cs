/* Author:          熊哲
 * CreateTime:      2018-06-22 21:36:01
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace EZUnity.Playables
{
    public class EZTransformTweenClip : PlayableAsset, ITimelineClipAsset
    {
        public ExposedReference<Transform> startPoint;
        public ExposedReference<Transform> endPoint;
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
}