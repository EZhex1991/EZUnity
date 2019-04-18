/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-03-29 18:05:26
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace EZUnity.Animation
{
    [TrackClipType(typeof(EZAnimationClip))]
    [TrackBindingType(typeof(EZTransformAnimation))]
    public class EZTransformAnimationTrack : TrackAsset
    {
        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
#if UNITY_EDITOR
            var controller = director.GetGenericBinding(this) as EZTransformAnimation;
            if (controller == null || controller.targetTransform == null) return;
            driver.AddFromComponent(controller.targetTransform.gameObject, controller.targetTransform);
#endif
            base.GatherProperties(director, driver);
        }
    }
}