/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-06-22 21:36:33
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace EZhex1991.EZUnity.Playables
{
    [TrackBindingType(typeof(Transform))]
    [TrackClipType(typeof(EZTransformTweenClip))]
    public class EZTransformTweenTrack : TrackAsset
    {
        [EZLockedFoldout]
        public EZTransformTweenMixer mixerTemplate;

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<EZTransformTweenMixer>.Create(graph, mixerTemplate, inputCount);
        }

        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            var binding = director.GetGenericBinding(this) as Transform;
            if (binding == null) return;
            driver.AddFromComponent(binding.gameObject, binding);
            base.GatherProperties(director, driver);
        }
    }
}