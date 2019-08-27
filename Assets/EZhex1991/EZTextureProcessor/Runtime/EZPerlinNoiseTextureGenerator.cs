/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-06-06 13:55:50
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZhex1991.EZUnity;
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CreateAssetMenu(
        fileName = "EZPerlinNoiseTextureGenerator",
        menuName = EZTextureProcessorUtility.MenuName_TextureGenerator + "EZPerlinNoiseTextureGenerator",
        order = (int)EZAssetMenuOrder.EZPerlinNoiseTextureGenerator)]
    public class EZPerlinNoiseTextureGenerator : EZTextureGenerator
    {
        [EZCurveRect(0, 0, 1, 1)]
        public AnimationCurve outputCurve = AnimationCurve.Linear(0, 0, 1, 1);
        public Vector2 density = new Vector2(5, 5);

        public override void SetTexturePixels(Texture2D texture)
        {
            float maxX = texture.width - 1;
            float maxY = texture.height - 1;
            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    float u = x / maxX;
                    float v = y / maxY;
                    u *= density.x;
                    v *= density.y;
                    Color color = Color.white * outputCurve.Evaluate(Mathf.PerlinNoise(u, v));
                    texture.SetPixel(x, y, color);
                }
            }
        }
    }
}
