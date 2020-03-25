/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-15 20:49:52
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using UnityEngine.Playables;

namespace EZhex1991.EZUnity.Playables
{
    public class EZMaterialColorClip : PlayableAsset
    {
        protected virtual Color defaultValue { get { return Color.yellow; } }

        [EZLockedFoldout]
        public EZMaterialColorPlayableBehaviour template = new EZMaterialColorPlayableBehaviour();

        protected virtual void Reset()
        {
            template.value = defaultValue;
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            return ScriptPlayable<EZMaterialColorPlayableBehaviour>.Create(graph, template);
        }
    }

    [System.Serializable]
    public class EZMaterialColorPlayableBehaviour : PlayableBehaviour
    {
#if UNITY_2018_1_OR_NEWER
        [ColorUsage(true, true)]
#else
        [ColorUsage(true, true, 0, 8, 0.125f, 3)]
#endif
        public Color value;
    }
}