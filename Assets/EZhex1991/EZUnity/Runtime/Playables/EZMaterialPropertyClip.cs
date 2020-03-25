/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-10-24 14:11:14
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using UnityEngine.Playables;

namespace EZhex1991.EZUnity.Playables
{
    public class EZMaterialPropertyClip : PlayableAsset
    {
        [EZLockedFoldout]
        public EZMaterialPropertyPlayableBehaviour template = new EZMaterialPropertyPlayableBehaviour();

        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            return ScriptPlayable<EZMaterialPropertyPlayableBehaviour>.Create(graph, template);
        }
    }

    [System.Serializable]
    public class EZMaterialPropertyPlayableBehaviour : PlayableBehaviour
    {
        public EZMaterialFloatProperty[] floatProperties;
        public EZMaterialColorProperty[] colorProperties;
        public EZMaterialVectorProperty[] vectorProperties;
    }
}