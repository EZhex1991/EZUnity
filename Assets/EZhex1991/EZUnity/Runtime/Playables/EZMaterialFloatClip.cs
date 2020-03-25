/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-15 20:49:52
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using UnityEngine.Playables;

namespace EZhex1991.EZUnity.Playables
{
    public class EZMaterialFloatClip : PlayableAsset
    {
        protected virtual float defaultValue { get { return 0; } }

        [EZLockedFoldout]
        public EZMaterialFloatPlayableBehaviour template = new EZMaterialFloatPlayableBehaviour();

        protected virtual void Reset()
        {
            template.value = defaultValue;
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            return ScriptPlayable<EZMaterialFloatPlayableBehaviour>.Create(graph, template);
        }
    }

    [System.Serializable]
    public class EZMaterialFloatPlayableBehaviour : PlayableBehaviour
    {
        public float value = 0;
    }
}