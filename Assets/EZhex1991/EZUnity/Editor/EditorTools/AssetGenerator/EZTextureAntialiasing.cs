/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-13 13:40:06
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity.AssetGenerator
{
    [CreateAssetMenu(fileName = "EZTextureAntialiasing", menuName = "EZUnity/EZTextureAntialiasing", order = (int)EZAssetMenuOrder.EZTextureAntialiasing)]
    public class EZTextureAntialiasing : EZTextureGenerator
    {
        public enum Antialiasing { X2, X4, X8 }

        public Texture2D referenceTexture;
        [Range(1, 8)]
        public int antialiasingX = 2;
        [Range(1, 8)]
        public int antialiasingY = 2;

        public override void SetTexturePixels(Texture2D texture)
        {
            if (referenceTexture == null) return;

            int maxX = texture.width - 1;
            int maxY = texture.height - 1;
            float stepX = 1f / antialiasingX;
            float stepY = 1f / antialiasingY;
            float[] u = new float[antialiasingX];
            float[] v = new float[antialiasingY];

            for (int x = 0; x < texture.width; x++)
            {
                for (int du = 0; du < antialiasingX; du++)
                {
                    u[du] = Mathf.Clamp(x - 0.5f + stepX * 0.5f + (stepX * du), 0, maxX);
                }
                for (int y = 0; y < texture.height; y++)
                {
                    for (int dv = 0; dv < antialiasingY; dv++)
                    {
                        v[dv] = Mathf.Clamp(y - 0.5f + stepY * 0.5f + (stepY * dv), 0, maxY);
                    }

                    Color color = Color.black;
                    for (int i = 0; i < antialiasingX; i++)
                    {
                        for (int j = 0; j < antialiasingY; j++)
                        {
                            color += referenceTexture.GetPixelBilinear(u[i] / maxX, v[j] / maxY);
                        }
                    }
                    color /= antialiasingX * antialiasingY;
                    texture.SetPixel(x, y, color);
                }
            }
        }
    }
}
