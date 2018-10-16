/* Author:          熊哲
 * CreateTime:      2018-08-31 16:30:04
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZUnity.PostEffects
{
    [RequireComponent(typeof(Camera)), ExecuteInEditMode]
    public class EZDepthEffects : MonoBehaviour
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

        public Material targetMaterial;

        private void Awake()
        {
            camera.depthTextureMode |= DepthTextureMode.DepthNormals;
        }

        [ImageEffectOpaque]
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (targetMaterial == null)
            {
                Graphics.Blit(source, destination);
            }
            else
            {
                Graphics.Blit(source, destination, targetMaterial);
            }
        }
    }
}
