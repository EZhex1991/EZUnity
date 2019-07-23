/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-07-04 11:16:05
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class _MaterialPreview : MonoBehaviour
    {
        public Renderer[] renderers;
        [EZNestedEditor]
        public Material material;

        private void OnValidate()
        {
            if (material != null)
            {
                gameObject.name = material.name;
                foreach (Renderer renderer in renderers)
                {
                    if (renderer != null) renderer.sharedMaterial = material;
                }
            }
        }
    }
}
