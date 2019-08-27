/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-04-12 15:22:32
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZhex1991.EZUnity;
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CreateAssetMenu(fileName = "EZTextureChannelModifier",
        menuName = EZTextureProcessorUtility.MenuName_TextureProcessor + "EZTextureChannelModifier",
        order = (int)EZAssetMenuOrder.EZTextureChannelModifier)]
    public class EZTextureChannelModifier : EZTextureGenerator
    {
        public Texture2D inputTexture;
        [EZCurveRect]
        public AnimationCurve outputCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public Texture2D overrideTextureR;
        public ColorChannel overrideChannelR = ColorChannel.R;
        [EZCurveRect]
        public AnimationCurve overrideCurveR = AnimationCurve.Linear(0, 0, 1, 1);

        public Texture2D overrideTextureG;
        public ColorChannel overrideChannelG = ColorChannel.G;
        [EZCurveRect]
        public AnimationCurve overrideCurveG = AnimationCurve.Linear(0, 0, 1, 1);

        public Texture2D overrideTextureB;
        public ColorChannel overrideChannelB = ColorChannel.B;
        [EZCurveRect]
        public AnimationCurve overrideCurveB = AnimationCurve.Linear(0, 0, 1, 1);

        public Texture2D overrideTextureA;
        public ColorChannel overrideChannelA = ColorChannel.A;
        [EZCurveRect]
        public AnimationCurve overrideCurveA = AnimationCurve.Linear(0, 0, 1, 1);

        public override void SetTexturePixels(Texture2D texture)
        {
            int maxX = texture.width - 1;
            int maxY = texture.height - 1;
            for (int x = 0; x < texture.width; x++)
            {
                float coordX = (float)x / maxX;
                for (int y = 0; y < texture.height; y++)
                {
                    float coordY = (float)y / maxY;
                    Color color = inputTexture == null ?
                        Color.white * outputCurve.Evaluate(1) :
                        inputTexture.GetPixelBilinear(coordX, coordY);
                    color.r = overrideTextureR == null ?
                        outputCurve.Evaluate(color.r) :
                        overrideCurveR.Evaluate(overrideTextureR.GetPixelBilinear(coordX, coordY).GetChannel(overrideChannelR));
                    color.g = overrideTextureG == null ?
                        outputCurve.Evaluate(color.g) :
                        overrideCurveG.Evaluate(overrideTextureG.GetPixelBilinear(coordX, coordY).GetChannel(overrideChannelG));
                    color.b = overrideTextureB == null ?
                        outputCurve.Evaluate(color.b) :
                        overrideCurveB.Evaluate(overrideTextureB.GetPixelBilinear(coordX, coordY).GetChannel(overrideChannelB));
                    color.a = overrideTextureA == null ?
                        outputCurve.Evaluate(color.a) :
                        overrideCurveA.Evaluate(overrideTextureA.GetPixelBilinear(coordX, coordY).GetChannel(overrideChannelA));
                    texture.SetPixel(x, y, color);
                }
            }
        }

        public Texture2D ResampleTexture(Texture2D texture)
        {
            Texture2D newTexture = new Texture2D(texture.width, texture.height, outputFormat, false);
            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    Color color = texture.GetPixel(x, y);
                    color = new Color(
                        outputCurve.Evaluate(color.r),
                        outputCurve.Evaluate(color.g),
                        outputCurve.Evaluate(color.b),
                        outputCurve.Evaluate(color.a)
                    );
                    newTexture.SetPixel(x, y, color);
                }
            }
            newTexture.Apply();
            return newTexture;
        }
    }
}
