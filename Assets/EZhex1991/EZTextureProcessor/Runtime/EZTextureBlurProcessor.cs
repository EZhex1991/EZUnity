/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-13 13:40:06
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZhex1991.EZUnity;
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CreateAssetMenu(fileName = "EZTextureBlurProcessor",
        menuName = EZTextureProcessorUtility.MenuName_TextureProcessor + "EZTextureBlurProcessor",
        order = (int)EZAssetMenuOrder.EZTextureBlurProcessor)]
    public class EZTextureBlurProcessor : EZTextureProcessor
    {
        private const string PropertyName_BlurWeightTex = "_BlurWeightTex";
        private const string PropertyName_BlurRadius = "_BlurRadius";
        private const string PropertyName_BlurDirection = "_BlurDirection";

        public Texture2D blurWeightTexture;
        public Vector2Int blurRadius = new Vector2Int(5, 5);

        public void BlurTexture(Texture sourceTexture, RenderTexture destinationTexture)
        {
            if (material != null)
            {
                material.SetTexture(PropertyName_BlurWeightTex, blurWeightTexture);
                RenderTexture tempTexture = RenderTexture.GetTemporary(sourceTexture.width, sourceTexture.height);

                material.SetInt(PropertyName_BlurRadius, blurRadius.y);
                material.SetVector(PropertyName_BlurDirection, new Vector4(0, 1, 0, 0));
                Graphics.Blit(sourceTexture, tempTexture, material);

                material.SetInt(PropertyName_BlurRadius, blurRadius.x);
                material.SetVector(PropertyName_BlurDirection, new Vector4(1, 0, 0, 0));
                Graphics.Blit(tempTexture, destinationTexture, material);

                RenderTexture.ReleaseTemporary(tempTexture);
            }
            else
            {
                Graphics.Blit(sourceTexture, destinationTexture);
            }
        }

        public override void SetTexturePixels(Texture2D texture)
        {
            RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height);
            BlurTexture(inputTexture, renderTexture);
            RenderTexture.active = renderTexture;
            texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(renderTexture);
        }
        public override void SetPreviewTexture(Texture2D previewTexture, RenderTexture renderTexture)
        {
            BlurTexture(inputTexture, renderTexture);
        }

        private void OnValidate()
        {
            blurRadius.x = Mathf.Max(1, blurRadius.x);
            blurRadius.y = Mathf.Max(1, blurRadius.y);
        }
    }
}
