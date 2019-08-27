/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-06-06 15:12:45
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZhex1991.EZUnity;
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CreateAssetMenu(
        fileName = "EZRandomNoiseTextureGenerator",
        menuName = EZTextureProcessorUtility.MenuName_TextureGenerator + "EZRandomNoiseTextureGenerator",
        order = (int)EZAssetMenuOrder.EZRandomNoiseTextureGenerator)]
    public class EZRandomNoiseTextureGenerator : EZTextureGenerator
    {
        public bool colored;
        [EZCurveRect]
        public AnimationCurve outputCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public override void SetTexturePixels(Texture2D texture)
        {
            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    Color color = Color.white;
                    if (colored)
                    {
                        color.r = outputCurve.Evaluate(Random.value);
                        color.g = outputCurve.Evaluate(Random.value);
                        color.b = outputCurve.Evaluate(Random.value);
                        color.a = outputCurve.Evaluate(Random.value);
                    }
                    else
                    {
                        color = Color.white * outputCurve.Evaluate(Random.value);
                    }
                    texture.SetPixel(x, y, color);
                }
            }
        }
    }
}
