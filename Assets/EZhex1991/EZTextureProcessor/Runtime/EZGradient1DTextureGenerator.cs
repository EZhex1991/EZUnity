/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-20 17:00:14
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZhex1991.EZUnity;
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CreateAssetMenu(
        fileName = "EZGradient1DTextureGenerator",
        menuName = EZTextureProcessorUtility.MenuName_TextureGenerator + "EZGradient1DTextureGenerator",
        order = (int)EZAssetMenuOrder.EZGradient1DTextureGenerator)]
    public class EZGradient1DTextureGenerator : EZTextureGenerator
    {
        public Gradient gradient = EZTextureProcessorUtility.GradientBlackToWhite();
        [EZCurveRect]
        public AnimationCurve gradientCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public override ScaleMode previewScaleMode { get { return ScaleMode.StretchToFill; } }
        public override TextureWrapMode defaultWrapMode { get { return TextureWrapMode.Clamp; } }

        private void Reset()
        {
            m_OutputResolution = new Vector2Int(256, 4);
        }

        public override void SetTexturePixels(Texture2D texture)
        {
            int maxX = texture.width - 1;
            for (int x = 0; x < texture.width; x++)
            {
                float coordinateU = gradientCurve.Evaluate((float)x / maxX);
                Color color = gradient.Evaluate(coordinateU);
                for (int y = 0; y < texture.height; y++)
                {
                    texture.SetPixel(x, y, color);
                }
            }
        }
    }
}
