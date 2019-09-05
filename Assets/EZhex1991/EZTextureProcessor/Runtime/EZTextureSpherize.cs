/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-09-02 17:04:08
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZhex1991.EZUnity;
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CreateAssetMenu(fileName = "EZTextureSpherize",
        menuName = EZTextureProcessorUtility.MenuName_TextureProcessor + "EZTextureSpherize",
        order = (int)EZAssetMenuOrder.EZTextureSpherize)]
    public class EZTextureSpherize : EZTextureProcessor
    {
        private const string PropertyName_SpherizePower = "_SpherizePower";
        private const string PropertyName_SpherizeCenter = "_SpherizeCenter";
        private const string PropertyName_SpherizeStrength = "_SpherizeStrength";

        public override string defaultShaderName { get { return "Hidden/EZTextureProcessor/Distort_Spherize"; } }

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

        public float spherizePower = 4;
        public Vector2 spherizeCenter = new Vector2(0.5f, 0.5f);
        public Vector2 spherizeStrength = new Vector2(10, 10);

        public override void ProcessTexture(Texture sourceTexture, RenderTexture destinationTexture)
        {
            if (material != null)
            {
                material.SetFloat(PropertyName_SpherizePower, spherizePower);
                material.SetVector(PropertyName_SpherizeCenter, spherizeCenter);
                material.SetVector(PropertyName_SpherizeStrength, spherizeStrength);
                Graphics.Blit(sourceTexture, destinationTexture, material);
            }
            else
            {
                Graphics.Blit(sourceTexture, destinationTexture);
            }
        }
    }
}
