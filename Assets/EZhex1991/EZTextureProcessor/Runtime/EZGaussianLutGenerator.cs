/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-27 17:26:31
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CreateAssetMenu(fileName = nameof(EZGaussianLutGenerator),
        menuName = MenuName_TextureGenerator + nameof(EZGaussianLutGenerator))]
    public class EZGaussianLutGenerator : EZTextureProcessor
    {
        private static class Uniforms
        {
            public static readonly string Keyword_GaussianTextureType = "_GaussianTextureType";
            public static readonly int PropertyID_GaussianRangeX = Shader.PropertyToID("_GaussianRangeX");
            public static readonly int PropertyID_GaussianSigmaX = Shader.PropertyToID("_GaussianSigmaX");
            public static readonly int PropertyID_GaussianRangeY = Shader.PropertyToID("_GaussianRangeY");
            public static readonly int PropertyID_GaussianSigmaY = Shader.PropertyToID("_GaussianSigmaY");
        }

        public enum TextureType { Wave, Lut1D, Lut2D }

        public override string defaultShaderName { get { return "Hidden/EZTextureProcessor/GaussianDistribution"; } }

        public override Texture inputTexture { get { return null; } }

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

        public TextureType textureType = TextureType.Wave;
        public Vector2 rangeX = new Vector2(-3f, 3f);
        public float sigmaX = 1f;
        public Vector2 rangeY = new Vector2(-3f, 3f);
        public float sigmaY = 1f;

        public override TextureWrapMode defaultWrapMode { get { return TextureWrapMode.Clamp; } }

        public override void ProcessTexture(Texture sourceTexture, RenderTexture destinationTexture)
        {
            if (material != null)
            {
                material.SetKeyword(Uniforms.Keyword_GaussianTextureType, textureType);
                material.SetVector(Uniforms.PropertyID_GaussianRangeX, rangeX);
                material.SetFloat(Uniforms.PropertyID_GaussianSigmaX, sigmaX);
                material.SetVector(Uniforms.PropertyID_GaussianRangeY, rangeY);
                material.SetFloat(Uniforms.PropertyID_GaussianSigmaY, sigmaY);
                Graphics.Blit(sourceTexture, destinationTexture, material);
            }
            else
            {
                Graphics.Blit(sourceTexture, destinationTexture);
            }
        }

        public static float Gaussian1DWeight(float x, float sigma)
        {
            float sigma2 = sigma * sigma;
            float left = Mathf.Sqrt(1 / (2 * sigma2 * Mathf.PI));
            float right = Mathf.Exp(-x * x / (2 * sigma2));
            return left * right;
        }
        public static float Gaussian2DWeight(float x, float y, float sigma)
        {
            float sigma2 = sigma * sigma;
            float left = 1 / (2 * sigma2 * Mathf.PI);
            float right = Mathf.Exp(-(x * x + y * y) / (2 * sigma2));
            return left * right;
        }
    }
}
