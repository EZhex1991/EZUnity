/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-06-18 11:46:59
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity.Rendering
{
    [RequireComponent(typeof(Camera))]
    [ExecuteInEditMode]
    public abstract class EZImageEffect : MonoBehaviour
    {
        private Camera m_Camera;
        public Camera camera
        {
            get
            {
                if (m_Camera == null)
                    m_Camera = GetComponent<Camera>();
                return m_Camera;
            }
        }

        public abstract Material material { get; }

        [ImageEffectOpaque]
        public void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!enabled || material == null)
            {
                Graphics.Blit(source, destination);
            }
            else
            {
                Graphics.Blit(source, destination, material);
            }
        }
    }
}
