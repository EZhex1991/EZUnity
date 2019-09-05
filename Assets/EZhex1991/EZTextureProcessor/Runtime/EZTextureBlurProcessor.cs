/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-13 13:40:06
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZhex1991.EZUnity;
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CreateAssetMenu(fileName = "EZTextureBlurProcessor",
        menuName = EZTextureProcessorUtility.MenuName_TextureProcessor + "EZTextureBlurProcessor",
        order = (int)EZAssetMenuOrder.EZTextureBlurProcessor)]
    public class EZTextureBlurProcessor : EZTextureProcessor
    {
        private const string PropertyName_BlurWeightTex = "_BlurWeightTex";
        private const string PropertyName_BlurRadius = "_BlurRadius";
        private const string PropertyName_BlurDirection = "_BlurDirection";

        public override string defaultShaderName { get { return "Hidden/EZTextureProcessor/MotionBlur"; } }

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

        public Texture2D blurWeightTexture;
        public Vector2Int blurRadius = new Vector2Int(5, 5);

        public override void ProcessTexture(Texture sourceTexture, RenderTexture destinationTexture)
        {
            if (sourceTexture != null && material != null)
            {
                material.SetTexture(PropertyName_BlurWeightTex, blurWeightTexture);
                RenderTexture tempTexture = RenderTexture.GetTemporary(sourceTexture.width, sourceTexture.height);

                material.SetInt(PropertyName_BlurRadius, blurRadius.y);
                material.SetVector(PropertyName_BlurDirection, new Vector4(0, 1, 0, 0));
                Graphics.Blit(sourceTexture, tempTexture, material);

                material.SetInt(PropertyName_BlurRadius, blurRadius.x);
                material.SetVector(PropertyName_BlurDirection, new Vector4(1, 0, 0, 0));
                Graphics.Blit(tempTexture, destinationTexture, material);

                RenderTexture.ReleaseTemporary(tempTexture);
            }
            else
            {
                Graphics.Blit(sourceTexture, destinationTexture);
            }
        }

        private void OnValidate()
        {
            blurRadius.x = Mathf.Max(0, blurRadius.x);
            blurRadius.y = Mathf.Max(0, blurRadius.y);
        }
    }
}
