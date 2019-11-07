/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-11-06 17:49:08
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
    public class EZTransformConstraintTrack : TrackAsset
    {
        [EZLockedFoldout]
        public EZTransformConstraintMixer template;

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var playable = ScriptPlayable<EZTransformConstraintMixer>.Create(graph, template, inputCount);
            template = playable.GetBehaviour();
            return playable;
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