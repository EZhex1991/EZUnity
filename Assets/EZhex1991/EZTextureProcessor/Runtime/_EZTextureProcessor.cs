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
        public Shader shader { get { return m_Shader; } }

        public virtual Texture inputTexture { get; }
        public virtual Material material { get; }

        protected abstract void SetupMaterial(Material material);
        public virtual void ProcessTexture(Texture sourceTexture, RenderTexture destinationTexture)
        {
            if (material != null)
            {
                SetupMaterial(material);
                Graphics.Blit(sourceTexture, destinationTexture, material);
            }
            else
            {
                Graphics.Blit(sourceTexture, destinationTexture);
            }
        }

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
