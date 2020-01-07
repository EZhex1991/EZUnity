/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-09-02 16:25:53
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CreateAssetMenu(fileName = nameof(EZVoronoiTextureGenerator),
        menuName = MenuName_TextureGenerator + nameof(EZVoronoiTextureGenerator))]
    public class EZVoronoiTextureGenerator : EZTextureProcessor
    {
        private static class Uniforms
        {
            public const string Keyword_FillType = "_FillType";
            public static readonly int PropertyID_VoronoiAngleOffset = Shader.PropertyToID("_VoronoiAngleOffset");
            public static readonly int PropertyID_VoronoiDensity = Shader.PropertyToID("_VoronoiDensity");
        }

        public enum FillType { Gradient, Random }

        public override string defaultShaderName { get { return "Hidden/EZTextureProcessor/Noise_Voronoi"; } }

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

        public FillType fillType = FillType.Gradient;
        public float angleOffset = 2;
        public Vector2 voronoiDensity = new Vector2(10, 10);

        public override void ProcessTexture(Texture sourceTexture, RenderTexture destinationTexture)
        {
            if (material != null)
            {
                material.SetKeyword(Uniforms.Keyword_FillType, fillType);
                material.SetFloat(Uniforms.PropertyID_VoronoiAngleOffset, angleOffset);
                material.SetVector(Uniforms.PropertyID_VoronoiDensity, voronoiDensity);
                Graphics.Blit(sourceTexture, destinationTexture, material);
            }
            else
            {
                Graphics.Blit(sourceTexture, destinationTexture);
            }
        }
    }
}
