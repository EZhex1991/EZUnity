/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-06-06 15:12:45
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity.AssetGenerator
{
    [CreateAssetMenu(fileName = "EZNoiseGenerator", menuName = "EZUnity/EZNoiseGenerator", order = (int)EZAssetMenuOrder.EZNoiseGenerator)]
    public class EZNoiseGenerator : EZTextureGenerator
    {
        public bool colored;
        [EZCurveRect(0, 0, 1, 1)]
        public AnimationCurve outputCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public override void ApplyToTexture(Texture2D texture)
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
