/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-06-18 11:46:59
 * Organization:    #ORGANIZATION#
 * Description:     
 */
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
using UnityEngine;

namespace EZhex1991.EZUnity.Rendering
{
    [RequireComponent(typeof(Camera))]
    [ExecuteInEditMode]
    public abstract class EZImageEffect : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        protected Shader m_Shader;
        public Shader shader { get { return m_Shader; } }

        protected Material m_Material;
        public Material material
        {
            get
            {
                if (m_Material == null && shader != null)
                {
                    m_Material = new Material(shader);
                }
                return m_Material;
            }
        }

        protected Camera m_Camera;
        public Camera camera
        {
            get
            {
                if (m_Camera == null)
                    m_Camera = GetComponent<Camera>();
                return m_Camera;
            }
        }

        [ImageEffectOpaque]
        public virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!enabled || material == null)
            {
                Graphics.Blit(source, destination);
            }
            else
            {
                SetMaterial();
                Graphics.Blit(source, destination, material);
            }
        }
        protected abstract void SetMaterial();
    }
}
