/* Author:          ezhex1991@outlook.com
 * CreateTime:      2020-03-25 17:11:20
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public static class EZCompatibility
    {
#if UNITY_2018_1_OR_NEWER
#else
        public static void GetPropertyBlock(this Renderer renderer, MaterialPropertyBlock properties, int materialIndex)
        {
            renderer.GetPropertyBlock(properties);
        }
        public static void SetPropertyBlock(this Renderer renderer, MaterialPropertyBlock properties, int materialIndex)
        {
            renderer.SetPropertyBlock(properties);
        }
#endif
    }
}
