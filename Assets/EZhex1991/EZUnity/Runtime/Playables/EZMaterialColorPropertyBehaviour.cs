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
    public class EZMaterialColorPropertyBehaviour : PlayableBehaviour
    {
#if UNITY_2018_1_OR_NEWER
        [ColorUsage(true, true)]
#else
        [ColorUsage(true, true, 0, 8, 0.125f, 3)]
#endif
        public Color value = Color.white;
    }
}
