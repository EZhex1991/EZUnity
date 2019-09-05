/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-09-02 16:25:53
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZhex1991.EZUnity;
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CreateAssetMenu(fileName = "EZVoronoiTextureGenerator",
        menuName = EZTextureProcessorUtility.MenuName_TextureGenerator + "EZVoronoiTextureGenerator",
        order = (int)EZAssetMenuOrder.EZVoronoiTextureGenerator)]
    public class EZVoronoiTextureGenerator : EZTextureProcessor
    {
        private const string Keyword_FillType = "_FillType";
        private const string PropertyName_VoronoiAngleOffset = "_VoronoiAngleOffset";
        private const string PropertyName_VoronoiDensity = "_VoronoiDensity";

        public enum FillType { Gradient, Random }

        public override string defaultShaderName { get { return "Hidden/EZTextureProcessor/Noise_Voronoi"; } }

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

        public FillType fillType = FillType.Gradient;
        public float angleOffset = 2;
        public Vector2 voronoiDensity = new Vector2(10, 10);

        public override void ProcessTexture(Texture sourceTexture, RenderTexture destinationTexture)
        {
            if (material != null)
            {
                material.SetKeyword(Keyword_FillType, fillType);
                material.SetFloat(PropertyName_VoronoiAngleOffset, angleOffset);
                material.SetVector(PropertyName_VoronoiDensity, voronoiDensity);
                Graphics.Blit(sourceTexture, destinationTexture, material);
            }
            else
            {
                Graphics.Blit(sourceTexture, destinationTexture);
            }
        }
    }
}
