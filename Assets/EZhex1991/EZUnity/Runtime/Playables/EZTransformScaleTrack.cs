/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-11-07 11:25:59
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
    public class EZTransformScaleTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<EZTransformScaleMixer>.Create(graph, inputCount);
        }

        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            var binding = director.GetGenericBinding(this) as Transform;
            if (binding == null) return;
            driver.AddFromName<Transform>(binding.gameObject, "m_LocalScale");
            base.GatherProperties(director, driver);
        }
    }
}
