/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-06-06 13:55:50
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZhex1991.EZUnity;
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CreateAssetMenu(
        fileName = "EZPerlinNoiseTextureGenerator",
        menuName = EZTextureProcessorUtility.MenuName_TextureGenerator + "EZPerlinNoiseTextureGenerator",
        order = (int)EZAssetMenuOrder.EZPerlinNoiseTextureGenerator)]
    public class EZPerlinNoiseTextureGenerator : EZTextureProcessor
    {
        private const string PropertyName_NoiseDensity = "_NoiseDensity";

        public override string defaultShaderName { get { return "Hidden/EZTextureProcessor/Noise_Perlin"; } }

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

        public Vector2 noiseDensity = new Vector2(10, 10);

        protected override void SetupMaterial(Material material)
        {
            material.SetVector(PropertyName_NoiseDensity, noiseDensity);
        }
    }
}
