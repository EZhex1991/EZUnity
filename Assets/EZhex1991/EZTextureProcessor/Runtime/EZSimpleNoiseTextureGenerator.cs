/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-06-06 15:12:45
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZhex1991.EZUnity;
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CreateAssetMenu(
        fileName = "EZSimpleNoiseTextureGenerator",
        menuName = EZTextureProcessorUtility.MenuName_TextureGenerator + "EZSimpleNoiseTextureGenerator",
        order = (int)EZAssetMenuOrder.EZSimpleNoiseTextureGenerator)]
    public class EZSimpleNoiseTextureGenerator : EZTextureProcessor
    {
        private const string PropertyName_NoiseDensity = "_NoiseDensity";

        public override string defaultShaderName { get { return "Hidden/EZTextureProcessor/Noise_Simple"; } }

        public override Texture inputTexture { get { return null; } }

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

        public Vector2 noiseDensity = new Vector2(50, 50);

        public override void ProcessTexture(Texture sourceTexture, RenderTexture destinationTexture)
        {
            if (material != null)
            {
                material.SetVector(PropertyName_NoiseDensity, noiseDensity);
                Graphics.Blit(sourceTexture, destinationTexture, material);
            }
            else
            {
                Graphics.Blit(sourceTexture, destinationTexture);
            }
        }
    }
}
