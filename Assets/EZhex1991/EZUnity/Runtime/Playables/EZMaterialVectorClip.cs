/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-15 20:49:52
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using UnityEngine.Playables;

namespace EZhex1991.EZUnity.Playables
{
    public class EZMaterialVectorClip : PlayableAsset
    {
        protected virtual Vector4 defaultValue { get { return Vector4.zero; } }

        [EZLockedFoldout]
        public EZMaterialVectorPlayableBehaviour template = new EZMaterialVectorPlayableBehaviour();

        protected virtual void Reset()
        {
            template.value = defaultValue;
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            return ScriptPlayable<EZMaterialVectorPlayableBehaviour>.Create(graph, template);
        }
    }

    [System.Serializable]
    public class EZMaterialVectorPlayableBehaviour : PlayableBehaviour
    {
        [EZSingleLineVector4]
        public Vector4 value;
    }
}