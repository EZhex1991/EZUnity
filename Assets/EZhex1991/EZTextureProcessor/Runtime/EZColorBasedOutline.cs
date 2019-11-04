/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-09-06 10:35:40
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZhex1991.EZUnity;
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CreateAssetMenu(fileName = "EZColorBasedOutline",
        menuName = EZTextureProcessorUtility.MenuName_TextureProcessor + "EZColorBasedOutline",
        order = (int)EZAssetMenuOrder.EZColorBasedOutline)]
    public class EZColorBasedOutline : EZTextureProcessor
    {
        private static class Uniforms
        {
            public static readonly int PropertyID_GrayWeight = Shader.PropertyToID("_GrayWeight");
            public static readonly int PropertyID_Tolerance = Shader.PropertyToID("_Tolerance");
            public static readonly int PropertyID_OutlineColor = Shader.PropertyToID("_OutlineColor");
            public static readonly int PropertyID_OutlineThickness = Shader.PropertyToID("_OutlineThickness");
        }

        public override string defaultShaderName { get { return "Hidden/EZTextureProcessor/ColorBasedOutline"; } }

        [SerializeField]
        protected Texture m_InputTexture;
        public override Texture inputTexture { get { return m_InputTexture; } }

        protected Material m_Material;
        public override Material material
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

        public Color grayWeight = new Color(0.299f, 0.587f, 0.114f);
        [Range(0, 255)]
        public float tolerance = 50f;
        public Color outlineColor = new Color(0, 0, 0, 0.5f);
        public int outlineThickness = 1;

        public override void ProcessTexture(Texture sourceTexture, RenderTexture destinationTexture)
        {
            if (material != null)
            {
                material.SetVector(Uniforms.PropertyID_GrayWeight, grayWeight);
                material.SetFloat(Uniforms.PropertyID_Tolerance, tolerance);
                material.SetColor(Uniforms.PropertyID_OutlineColor, outlineColor);
                material.SetInt(Uniforms.PropertyID_OutlineThickness, outlineThickness);
                Graphics.Blit(sourceTexture, destinationTexture, material);
            }
            else
            {
                Graphics.Blit(sourceTexture, destinationTexture);
            }
        }

        private void OnValidate()
        {
            outlineThickness = Mathf.Max(0, outlineThickness);
        }
    }
}
