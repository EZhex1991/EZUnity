/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-03-29 20:41:23
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using UnityEngine.Playables;

namespace EZUnity.Animation
{
    public class EZAnimationClip : PlayableAsset
    {
        public EZAnimationPlayableBehaviour template = new EZAnimationPlayableBehaviour();

        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            return ScriptPlayable<EZAnimationPlayableBehaviour>.Create(graph, template);
        }
    }
}