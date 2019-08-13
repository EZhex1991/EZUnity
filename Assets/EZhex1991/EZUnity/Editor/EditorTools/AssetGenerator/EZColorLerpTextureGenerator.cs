/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-13 13:38:55
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity.AssetGenerator
{
    [CreateAssetMenu(fileName = "EZColorLerpTextureGenerator", menuName = "EZUnity/EZColorLerpTextureGenerator", order = (int)EZAssetMenuOrder.EZColorLerpTextureGenerator)]
    public class EZColorLerpTextureGenerator : EZTextureGenerator
    {
        [EZCurveRect(0, 0, 1, 1)]
        public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        public Color color0 = Color.black;
        public Color color1 = Color.white;

        public override void SetTexturePixels(Texture2D texture)
        {
            int maxX = texture.width - 1;
            for (int x = 0; x < texture.width; x++)
            {
                float u = (float)x / maxX;
                for (int y = 0; y < texture.height; y++)
                {
                    Color color = Color.Lerp(color0, color1, curve.Evaluate(u));
                    texture.SetPixel(x, y, color);
                }
            }
        }
    }
}
