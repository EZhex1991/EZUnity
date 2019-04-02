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
    [TrackBindingType(typeof(EZRectTransformAnimation))]
    public class EZRectTransformAnimationTrack : TrackAsset
    {
        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
#if UNITY_EDITOR
            var controller = director.GetGenericBinding(this) as EZRectTransformAnimation;
            if (controller == null || controller.rectTransform == null) return;
            driver.AddFromComponent(controller.rectTransform.gameObject, controller.rectTransform);
#endif
            base.GatherProperties(director, driver);
        }
    }
}