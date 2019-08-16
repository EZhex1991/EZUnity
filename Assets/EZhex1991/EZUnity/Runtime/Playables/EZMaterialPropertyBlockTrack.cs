/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-10-24 14:14:25
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace EZhex1991.EZUnity.Playables
{
    [TrackBindingType(typeof(Renderer))]
    [TrackClipType(typeof(EZMaterialPropertyBlockClip))]
    public class EZMaterialPropertyBlockTrack : TrackAsset
    {
        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            var controller = director.GetGenericBinding(this) as Renderer;
            if (controller == null) return;
            driver.AddFromComponent(controller.gameObject, controller);
            base.GatherProperties(director, driver);
        }
    }
}