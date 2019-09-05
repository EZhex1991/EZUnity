/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-09-02 17:43:21
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZhex1991.EZUnity;
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CreateAssetMenu(fileName = "EZTextureTwirl",
        menuName = EZTextureProcessorUtility.MenuName_TextureProcessor + "EZTextureTwirl",
        order = (int)EZAssetMenuOrder.EZTextureTwirl)]
    public class EZTextureTwirl : EZTextureProcessor
    {
        private const string PropertyName_TwirlCenter = "_TwirlCenter";
        private const string PropertyName_TwirlStrength = "_TwirlStrength";

        public override string defaultShaderName { get { return "Hidden/EZTextureProcessor/Distort_Twirl"; } }

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

        public Vector2 twirlCenter = new Vector2(0.5f, 0.5f);
        public float twirlStrength = 10f;

        public override void ProcessTexture(Texture sourceTexture, RenderTexture destinationTexture)
        {
            if (material != null)
            {
                material.SetVector(PropertyName_TwirlCenter, twirlCenter);
                material.SetFloat(PropertyName_TwirlStrength, twirlStrength);
                Graphics.Blit(sourceTexture, destinationTexture, material);
            }
            else
            {
                Graphics.Blit(sourceTexture, destinationTexture);
            }
        }
    }
}
