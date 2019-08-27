/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-27 17:26:31
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZhex1991.EZUnity;
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CreateAssetMenu(fileName = "EZGaussianLutGenerator",
        menuName = EZTextureProcessorUtility.MenuName_TextureGenerator + "EZGaussianLutGenerator",
        order = (int)EZAssetMenuOrder.EZGaussianLutGenerator)]
    public class EZGaussianLutGenerator : EZTextureGenerator
    {
        public enum TextureType { Wave, Lut1D, Lut2D }
        private delegate Color Sampler(float x, float y);

        public TextureType textureType = TextureType.Wave;
        public Vector2 rangeX = new Vector2(-3f, 3f);
        public float sigmaX = 1f;
        public Vector2 rangeY = new Vector2(-3f, 3f);
        public float sigmaY = 1f;

        public Color color0 = Color.black;
        public Color color1 = Color.white;

        public override TextureWrapMode defaultWrapMode { get { return TextureWrapMode.Clamp; } }

        public static float Gaussian1DWeight(float x, float sigma)
        {
            float sigma2 = sigma * sigma;
            float left = Mathf.Sqrt(1 / (2 * sigma2 * Mathf.PI));
            float right = Mathf.Exp(-x * x / (2 * sigma2));
            return left * right;
        }
        public static float Gaussian2DWeight(float x, float y, float sigma)
        {
            float sigma2 = sigma * sigma;
            float left = 1 / (2 * sigma2 * Mathf.PI);
            float right = Mathf.Exp(-(x * x + y * y) / (2 * sigma2));
            return left * right;
        }
        public static float Gaussian2DWeight(float x, float y, float sigmaX, float sigmaY)
        {
            return Gaussian1DWeight(x, sigmaX) * Gaussian1DWeight(y, sigmaY);
        }

        public override void SetTexturePixels(Texture2D texture)
        {
            switch (textureType)
            {
                case TextureType.Wave:
                    SetPixels(texture, SamplerWave);
                    break;
                case TextureType.Lut1D:
                    SetPixels(texture, SamplerLut1D);
                    break;
                case TextureType.Lut2D:
                    SetPixels(texture, SamplerLut2D);
                    break;
            }
        }
        private void SetPixels(Texture2D texture, Sampler sampler)
        {
            float maxX = texture.width - 1;
            float maxY = texture.height - 1;
            for (int x = 0; x < texture.width; x++)
            {
                float u = x / maxX;
                for (int y = 0; y < texture.height; y++)
                {
                    float v = y / maxY;
                    texture.SetPixel(x, y, sampler(u, v));
                }
            }
        }

        private Color SamplerWave(float x, float y)
        {
            x = Mathf.Lerp(rangeX.x, rangeX.y, x);
            return y > Gaussian1DWeight(x, sigmaX) ? color1 : color0;
        }
        private Color SamplerLut1D(float x, float y)
        {
            x = Mathf.Lerp(rangeX.x, rangeX.y, x);
            return Color.Lerp(color0, color1, Gaussian1DWeight(x, sigmaX));
        }
        private Color SamplerLut2D(float x, float y)
        {
            x = Mathf.Lerp(rangeX.x, rangeX.y, x);
            y = Mathf.Lerp(rangeY.x, rangeY.y, y);
            return Color.Lerp(color0, color1, Gaussian2DWeight(x, y, sigmaX, sigmaY));
        }

        private void OnValidate()
        {
            sigmaX = Mathf.Max(0, sigmaX);
            sigmaY = Mathf.Max(0, sigmaY);
        }
    }
}
