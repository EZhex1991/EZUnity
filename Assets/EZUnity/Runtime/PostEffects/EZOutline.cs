/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-08-31 16:30:04
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZUnity.PostEffects
{
    [RequireComponent(typeof(Camera)), ExecuteInEditMode]
    public class EZOutline : MonoBehaviour
    {
        [SerializeField]
        private float m_SampleDistance = 0.5f;
        public float sampleDistance
        {
            get { return m_SampleDistance; }
            set { m_SampleDistance = value; material.SetFloat("_SampleDistance", m_SampleDistance); }
        }

        [SerializeField]
        private float m_DepthSensitivity = 10;
        public float depthSensitivity
        {
            get { return m_DepthSensitivity; }
            set { m_DepthSensitivity = value; material.SetFloat("_DepthSensitivity", m_DepthSensitivity); }
        }

        [SerializeField]
        private float m_NormalSensitivity = 10;
        public float normalSensitivity
        {
            get { return m_NormalSensitivity; }
            set { m_NormalSensitivity = value; material.SetFloat("_NormalSensitivity", m_NormalSensitivity); }
        }

        [SerializeField]
        private Color m_CoverColor = Color.black;
        public Color corverColor
        {
            get { return m_CoverColor; }
            set { m_CoverColor = value; material.SetColor("_FadeOutColor", m_CoverColor); }
        }

        [SerializeField, Range(0, 1)]
        private float m_CoverStrength = 0.5f;
        public float coverStrength
        {
            get { return m_CoverStrength; }
            set { m_CoverStrength = value; material.SetFloat("_FadeOutStrength", m_CoverStrength); }
        }

        [SerializeField]
        private Color m_OutlineColor = Color.blue;
        public Color outlineColor
        {
            get { return m_OutlineColor; }
            set { m_OutlineColor = value; material.SetColor("_OutlineColor", m_OutlineColor); }
        }

        [SerializeField, Range(0, 1)]
        private float m_OutlineStrength = 1;
        public float outlineStrength
        {
            get { return m_OutlineStrength; }
            set { m_OutlineStrength = value; material.SetFloat("_OutlineStrength", m_OutlineStrength); }
        }

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

        private Material m_Material;
        public Material material
        {
            get
            {
                if (m_Material == null)
                {
                    Shader outlineShader = Shader.Find("EZUnity/Effects/EZOutline");
                    m_Material = new Material(outlineShader);
                }
                return m_Material;
            }
        }

        private void Awake()
        {
            camera.depthTextureMode |= DepthTextureMode.DepthNormals;
        }

        [ImageEffectOpaque]
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (material == null)
            {

            }
            else
            {
                Graphics.Blit(source, destination, material);
            }
        }

        private void OnValidate()
        {
            material.SetFloat("_SampleDistance", m_SampleDistance);
            material.SetFloat("_DepthSensitivity", m_DepthSensitivity);
            material.SetFloat("_NormalSensitivity", m_NormalSensitivity);
            material.SetColor("_CoverColor", m_CoverColor);
            material.SetFloat("_CoverStrength", m_CoverStrength);
            material.SetColor("_OutlineColor", m_OutlineColor);
            material.SetFloat("_OutlineStrength", m_OutlineStrength);
        }
    }
}
