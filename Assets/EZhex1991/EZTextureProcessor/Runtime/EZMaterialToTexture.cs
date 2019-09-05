/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-09-04 17:15:16
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZhex1991.EZUnity;
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CreateAssetMenu(
        fileName = "EZMaterialToTexture",
        menuName = "EZUnity/EZMaterialToTexture",
        order = (int)EZAssetMenuOrder.EZMaterialToTexture)]
    public class EZMaterialToTexture : EZTextureProcessor
    {
        [SerializeField]
        private Texture m_InputTexture;
        public override Texture inputTexture { get { return m_InputTexture; } }

        [SerializeField, EZNestedEditor]
        private Material m_Material;
        public override Material material { get { return m_Material; } }

        protected override void SetupMaterial(Material material)
        {

        }
    }
}
