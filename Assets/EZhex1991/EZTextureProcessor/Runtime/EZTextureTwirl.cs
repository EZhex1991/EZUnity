/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-09-02 17:43:21
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CreateAssetMenu(fileName = nameof(EZTextureTwirl),
        menuName = MenuName_TextureProcessor + nameof(EZTextureTwirl))]
    public class EZTextureTwirl : EZTextureProcessor
    {
        private static class Uniforms
        {
            public static readonly int PropertyID_TwirlCenter = Shader.PropertyToID("_TwirlCenter");
            public static readonly int PropertyID_TwirlStrength = Shader.PropertyToID("_TwirlStrength");
        }

        public override string defaultShaderName { get { return "Hidden/EZTextureProcessor/Distort_Twirl"; } }

        [SerializeField]
        protected Texture m_InputTexture;
        public override Texture inputTexture { get { return m_InputTexture; } }

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

        public Vector2 twirlCenter = new Vector2(0.5f, 0.5f);
        public float twirlStrength = 10f;

        public override void ProcessTexture(Texture sourceTexture, RenderTexture destinationTexture)
        {
            if (material != null)
            {
                material.SetVector(Uniforms.PropertyID_TwirlCenter, twirlCenter);
                material.SetFloat(Uniforms.PropertyID_TwirlStrength, twirlStrength);
                Graphics.Blit(sourceTexture, destinationTexture, material);
            }
            else
            {
                Graphics.Blit(sourceTexture, destinationTexture);
            }
        }
    }
}
