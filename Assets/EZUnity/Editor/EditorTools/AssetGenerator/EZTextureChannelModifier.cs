/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-04-12 15:22:32
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZUnity
{
    [CreateAssetMenu(fileName = "EZTextureChannelModifier", menuName = "EZUnity/EZTextureChannelModifier", order = EZAssetMenuOrder.EZTextureChannelModifier)]
    public class EZTextureChannelModifier : EZTextureGenerator
    {
        public Texture2D referenceTexture;

        public Texture2D overrideTextureR;
        public Channel channelR = Channel.R;
        [EZCurveRange(0, 0, 1, 1)]
        public AnimationCurve curveR = AnimationCurve.Linear(0, 0, 1, 1);

        public Texture2D overrideTextureG;
        public Channel channelG = Channel.G;
        [EZCurveRange(0, 0, 1, 1)]
        public AnimationCurve curveG = AnimationCurve.Linear(0, 0, 1, 1);

        public Texture2D overrideTextureB;
        public Channel channelB = Channel.B;
        [EZCurveRange(0, 0, 1, 1)]
        public AnimationCurve curveB = AnimationCurve.Linear(0, 0, 1, 1);

        public Texture2D overrideTextureA;
        public Channel channelA = Channel.A;
        [EZCurveRange(0, 0, 1, 1)]
        public AnimationCurve curveA = AnimationCurve.Linear(0, 0, 1, 1);

        protected override void SetPixels(Texture2D texture)
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
                            curveR.Evaluate(GetChannel(overrideTextureR == null ? color : overrideTextureR.GetPixelBilinear(coordX, coordY), channelR)),
                            curveG.Evaluate(GetChannel(overrideTextureG == null ? color : overrideTextureG.GetPixelBilinear(coordX, coordY), channelG)),
                            curveB.Evaluate(GetChannel(overrideTextureB == null ? color : overrideTextureB.GetPixelBilinear(coordX, coordY), channelB)),
                            curveA.Evaluate(GetChannel(overrideTextureA == null ? color : overrideTextureA.GetPixelBilinear(coordX, coordY), channelA))
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
                        curveR.Evaluate(GetChannel(color, channelR)),
                        curveG.Evaluate(GetChannel(color, channelG)),
                        curveB.Evaluate(GetChannel(color, channelB)),
                        curveA.Evaluate(GetChannel(color, channelA))
                    );
                    newTexture.SetPixel(x, y, newColor);
                }
            }
            newTexture.Apply();
            return newTexture;
        }
    }
}
