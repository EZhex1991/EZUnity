/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-04-12 15:22:32
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZUnity.AssetGenerator
{
    [CreateAssetMenu(fileName = "EZTextureChannelModifier", menuName = "EZUnity/EZTextureChannelModifier", order = (int)EZAssetMenuOrder.EZTextureChannelModifier)]
    public class EZTextureChannelModifier : EZTextureGenerator
    {
        public Texture2D referenceTexture;

        public Texture2D overrideTextureR;
        public ColorChannel channelR = ColorChannel.R;
        [EZCurveRange(0, 0, 1, 1)]
        public AnimationCurve curveR = AnimationCurve.Linear(0, 0, 1, 1);

        public Texture2D overrideTextureG;
        public ColorChannel channelG = ColorChannel.G;
        [EZCurveRange(0, 0, 1, 1)]
        public AnimationCurve curveG = AnimationCurve.Linear(0, 0, 1, 1);

        public Texture2D overrideTextureB;
        public ColorChannel channelB = ColorChannel.B;
        [EZCurveRange(0, 0, 1, 1)]
        public AnimationCurve curveB = AnimationCurve.Linear(0, 0, 1, 1);

        public Texture2D overrideTextureA;
        public ColorChannel channelA = ColorChannel.A;
        [EZCurveRange(0, 0, 1, 1)]
        public AnimationCurve curveA = AnimationCurve.Linear(0, 0, 1, 1);

        public override void ApplyToTexture(Texture2D texture)
        {
            if (referenceTexture != null)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    for (int y = 0; y < texture.height; y++)
                    {
                        float coordX = (float)x / (texture.width - 1);
                        float coordY = (float)y / (texture.height - 1);
                        Color color = referenceTexture.GetPixelBilinear(coordX, coordY);
                        Color newColor = new Color(
                            curveR.Evaluate((overrideTextureR == null ? color : overrideTextureR.GetPixelBilinear(coordX, coordY)).GetChannel(channelR)),
                            curveG.Evaluate((overrideTextureG == null ? color : overrideTextureG.GetPixelBilinear(coordX, coordY)).GetChannel(channelG)),
                            curveB.Evaluate((overrideTextureB == null ? color : overrideTextureB.GetPixelBilinear(coordX, coordY)).GetChannel(channelB)),
                            curveA.Evaluate((overrideTextureA == null ? color : overrideTextureA.GetPixelBilinear(coordX, coordY)).GetChannel(channelA))
                        );
                        texture.SetPixel(x, y, newColor);
                    }
                }
            }
        }

        public Texture2D ResampleTexture(Texture2D texture)
        {
            Texture2D newTexture = new Texture2D(texture.width, texture.height, textureFormat, false);
            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    Color color = texture.GetPixel(x, y);
                    Color newColor = new Color(
                        curveR.Evaluate(color.GetChannel(channelR)),
                        curveG.Evaluate(color.GetChannel(channelG)),
                        curveB.Evaluate(color.GetChannel(channelB)),
                        curveA.Evaluate(color.GetChannel(channelA))
                    );
                    newTexture.SetPixel(x, y, newColor);
                }
            }
            newTexture.Apply();
            return newTexture;
        }
    }
}
