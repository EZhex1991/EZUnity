/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-06-06 13:55:50
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CreateAssetMenu(
        fileName = nameof(EZPerlinNoiseTextureGenerator),
        menuName = MenuName_TextureGenerator + nameof(EZPerlinNoiseTextureGenerator))]
    public class EZPerlinNoiseTextureGenerator : EZTextureProcessor
    {
        private static class Uniforms
        {
            public static readonly int PropertyID_NoiseDensity = Shader.PropertyToID("_NoiseDensity");
        }

        public override string defaultShaderName { get { return "Hidden/EZTextureProcessor/Noise_Perlin"; } }

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

        public Vector2 noiseDensity = new Vector2(10, 10);

        public override void ProcessTexture(Texture sourceTexture, RenderTexture destinationTexture)
        {
            if (material != null)
            {
                material.SetVector(Uniforms.PropertyID_NoiseDensity, noiseDensity);
                Graphics.Blit(sourceTexture, destinationTexture, material);
            }
            else
            {
                Graphics.Blit(sourceTexture, destinationTexture);
            }
        }
    }
}
