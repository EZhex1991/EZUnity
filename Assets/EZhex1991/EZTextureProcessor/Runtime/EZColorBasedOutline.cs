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
        private const string PropertyName_GrayWeight = "_GrayWeight";
        private const string PropertyName_Tolerance = "_Tolerance";
        private const string PropertyName_OutlineColor = "_OutlineColor";
        private const string PropertyName_OutlineThickness = "_OutlineThickness";

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
                material.SetVector(PropertyName_GrayWeight, grayWeight);
                material.SetFloat(PropertyName_Tolerance, tolerance);
                material.SetColor(PropertyName_OutlineColor, outlineColor);
                material.SetInt(PropertyName_OutlineThickness, outlineThickness);
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
