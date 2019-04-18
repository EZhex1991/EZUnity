/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-03-29 18:05:26
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace EZUnity.Animation
{
    [TrackClipType(typeof(EZAnimationClip))]
    [TrackBindingType(typeof(EZGraphicColorAnimation))]
    public class EZGraphicColorAnimationTrack : TrackAsset
    {
        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
#if UNITY_EDITOR
            var controller = director.GetGenericBinding(this) as EZGraphicColorAnimation;
            if (controller == null || controller.targetGraphic == null) return;
            driver.AddFromName<Graphic>(controller.targetGraphic.gameObject, "m_Color");
#endif
            base.GatherProperties(director, driver);
        }
    }
}