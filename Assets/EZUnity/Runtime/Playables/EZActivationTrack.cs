/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-10-16 16:04:35
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace EZUnity.Playables
{
    [TrackBindingType(typeof(Transform))]
    [TrackClipType(typeof(EZActivationClip))]
    public class EZActivationTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<EZActivationMixer>.Create(graph, inputCount);
        }

        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
#if UNITY_EDITOR
            Transform parent = director.GetGenericBinding(this) as Transform;
            if (parent == null) return;
            for (int i = 0; i < parent.childCount; i++)
            {
                driver.AddFromName(parent.GetChild(i).gameObject, "m_IsActive");
            }
#endif
            base.GatherProperties(director, driver);
        }
    }
}