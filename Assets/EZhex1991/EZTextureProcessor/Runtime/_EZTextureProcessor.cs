/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-27 16:34:04
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    public abstract class EZTextureProcessor : EZTextureGenerator
    {
        [SerializeField]
        protected Shader m_Shader;
        public Shader shader
        {
            get
            {
                if (m_Shader == null && !string.IsNullOrEmpty(defaultShaderName))
                {
                    m_Shader = Shader.Find(defaultShaderName);
                }
                return m_Shader;
            }
        }

        public abstract string defaultShaderName { get; }
        public abstract Texture inputTexture { get; }
        public abstract Material material { get; }

        public abstract void ProcessTexture(Texture sourceTexture, RenderTexture destinationTexture);

        public sealed override void SetTexturePixels(Texture2D texture)
        {
            RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height);
            ProcessTexture(inputTexture, renderTexture);
            RenderTexture.active = renderTexture;
            texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(renderTexture);
        }
    }
}
