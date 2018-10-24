/* Author:          熊哲
 * CreateTime:      2018-10-24 14:11:14
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using UnityEngine.Playables;

namespace EZUnity.Playables
{
    public class EZMaterialClip : PlayableAsset
    {
        public EZMaterialBehaviour template = new EZMaterialBehaviour();

        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            return ScriptPlayable<EZMaterialBehaviour>.Create(graph, template);
        }
    }
}