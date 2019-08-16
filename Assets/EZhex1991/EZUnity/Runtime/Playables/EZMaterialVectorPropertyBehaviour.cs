/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-15 20:48:46
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using UnityEngine.Playables;

namespace EZhex1991.EZUnity.Playables
{
    [System.Serializable]
    public class EZMaterialVectorPropertyBehaviour : PlayableBehaviour
    {
        [EZSingleLineVector4]
        public Vector4 value = Vector4.zero;
    }
}
