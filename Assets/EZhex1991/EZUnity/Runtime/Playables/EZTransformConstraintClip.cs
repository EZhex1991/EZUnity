/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-11-06 17:48:00
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using UnityEngine.Playables;

namespace EZhex1991.EZUnity.Playables
{
    public class EZTransformConstraintClip : PlayableAsset
    {
        public ExposedReference<Transform> target;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            var behaviour = new EZTransformConstraintBehaviour
            {
                target = target.Resolve(graph.GetResolver())
            };
            return ScriptPlayable<EZTransformConstraintBehaviour>.Create(graph, behaviour);
        }
    }

    public class EZTransformConstraintBehaviour : PlayableBehaviour
    {
        public Transform target;
    }
}