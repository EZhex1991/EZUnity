/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-09-04 17:15:16
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CreateAssetMenu(
        fileName = nameof(EZMaterialToTexture),
        menuName = "EZUnity/" + nameof(EZMaterialToTexture))]
    public class EZMaterialToTexture : EZTextureProcessor
    {
        public override string defaultShaderName { get { return ""; } }

        [SerializeField]
        private Texture m_InputTexture;
        public override Texture inputTexture { get { return m_InputTexture; } }

        [SerializeField, EZNestedEditor]
        private Material m_Material;
        public override Material material { get { return m_Material; } }

        public override void ProcessTexture(Texture sourceTexture, RenderTexture destinationTexture)
        {
            if (material != null)
            {
                Graphics.Blit(sourceTexture, destinationTexture, material);
            }
            else
            {
                Graphics.Blit(sourceTexture, destinationTexture);
            }
        }
    }
}
