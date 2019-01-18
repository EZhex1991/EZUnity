/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-10-16 16:13:31
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using UnityEngine.Playables;

namespace EZUnity.Playables
{
    public class EZActivationClip : PlayableAsset
    {
        public ExposedReference<GameObject> gameObject;
        public EZActivationBehaviour template = new EZActivationBehaviour();

        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            template.gameObject = gameObject.Resolve(graph.GetResolver());
            return ScriptPlayable<EZActivationBehaviour>.Create(graph, template);
        }
    }
}