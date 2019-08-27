/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-05-15 15:48:51
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZhex1991.EZUnity;
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CreateAssetMenu(fileName = "EZTextureCombiner",
        menuName = EZTextureProcessorUtility.MenuName_TextureProcessor + "EZTextureCombiner",
        order = (int)EZAssetMenuOrder.EZTextureCombiner)]
    public class EZTextureCombiner : EZTextureGenerator
    {
        public Vector2Int cellSize = new Vector2Int(2, 2);
        public Texture2D[] inputTextures = new Texture2D[36];

        public override void SetTexturePixels(Texture2D texture)
        {
            float subTextureWidth = (float)texture.width / cellSize.x;
            float subTextureheight = (float)texture.height / cellSize.y;

            for (int cellX = 0; cellX < cellSize.x; cellX++)
            {
                for (int cellY = 0; cellY < cellSize.y; cellY++)
                {
                    int textureIndex = cellY * 6 + cellX;
                    Texture2D subTexture = inputTextures[textureIndex];

                    for (int x = 0; x < subTextureWidth; x++)
                    {
                        for (int y = 0; y < subTextureheight; y++)
                        {
                            float coordX = (float)x / (subTextureWidth - 1);
                            float coordY = (float)y / (subTextureheight - 1);
                            Color color = subTexture == null ? Color.white : subTexture.GetPixelBilinear(coordX, coordY);
                            int pixelX = (int)(cellX * subTextureWidth + x);
                            int pixelY = (int)(cellY * subTextureheight + y);
                            texture.SetPixel(pixelX, pixelY, color);
                        }
                    }
                }
            }
        }
    }
}
