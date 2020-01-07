/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-13 13:40:06
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CreateAssetMenu(fileName = nameof(EZTextureBlurProcessor),
        menuName = MenuName_TextureProcessor + nameof(EZTextureBlurProcessor))]
    public class EZTextureBlurProcessor : EZTextureProcessor
    {
        private static class Uniforms
        {
            public static readonly string ShaderName = "Hidden/EZTextureProcessor/MotionBlur";
            public static readonly int PropertyID_BlurWeightTex = Shader.PropertyToID("_BlurWeightTex");
            public static readonly int PropertyID_BlurRadius = Shader.PropertyToID("_BlurRadius");
            public static readonly int PropertyID_BlurDirection = Shader.PropertyToID("_BlurDirection");
        }

        public override string defaultShaderName { get { return Uniforms.ShaderName; } }

        [SerializeField]
        protected Texture m_InputTexture;
        public override Texture inputTexture { get { return m_InputTexture; } }

        [System.NonSerialized]
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
                material.SetTexture(Uniforms.PropertyID_BlurWeightTex, blurWeightTexture);
                RenderTexture tempTexture = RenderTexture.GetTemporary(sourceTexture.width, sourceTexture.height);

                material.SetInt(Uniforms.PropertyID_BlurRadius, blurRadius.y);
                material.SetVector(Uniforms.PropertyID_BlurDirection, new Vector4(0, 1, 0, 0));
                Graphics.Blit(sourceTexture, tempTexture, material);

                material.SetInt(Uniforms.PropertyID_BlurRadius, blurRadius.x);
                material.SetVector(Uniforms.PropertyID_BlurDirection, new Vector4(1, 0, 0, 0));
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
