/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-10-24 14:11:14
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using UnityEngine.Playables;

namespace EZhex1991.EZUnity.Playables
{
    public class EZMaterialPropertyBlockClip : PlayableAsset
    {
        [EZLockedFoldout]
        public EZMaterialPropertyBlockBehaviour template = new EZMaterialPropertyBlockBehaviour();

        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            return ScriptPlayable<EZMaterialPropertyBlockBehaviour>.Create(graph, template);
        }
    }
}