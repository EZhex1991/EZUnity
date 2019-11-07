/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-11-07 11:24:34
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace EZhex1991.EZUnity.Playables
{
    [TrackBindingType(typeof(Transform))]
    [TrackClipType(typeof(EZTransformConstraintClip))]
    public class EZTransformRotationTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<EZTransformRotationMixer>.Create(graph, inputCount);
        }

        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            var binding = director.GetGenericBinding(this) as Transform;
            if (binding == null) return;
            driver.AddFromName<Transform>(binding.gameObject, "m_LocalRotation");
            base.GatherProperties(director, driver);
        }
    }
}
