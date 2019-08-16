/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-15 20:49:52
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using UnityEngine.Playables;

namespace EZhex1991.EZUnity.Playables
{
    public class EZMaterialColorPropertyClip : PlayableAsset
    {
        [EZLockedFoldout]
        public EZMaterialColorPropertyBehaviour template;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            return ScriptPlayable<EZMaterialColorPropertyBehaviour>.Create(graph, template);
        }
    }
}