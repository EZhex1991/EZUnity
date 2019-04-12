/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-04-12 15:22:32
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZUnity
{
    [CreateAssetMenu(fileName = "EZTextureChannelModifier", menuName = "EZUnity/EZTextureChannelModifier", order = EZUnityMenuOrder.EZTextureChannelModifier)]
    public class EZTextureChannelModifier : EZTextureGenerator
    {
        public enum Channel { R, G, B, A }

        public Texture2D referenceTexture;

        public Channel channelR = Channel.R;
        [EZCurveRange(0, 0, 1, 1)]
        public AnimationCurve curveR = AnimationCurve.Linear(0, 0, 1, 1);
        public Channel channelG = Channel.G;
        [EZCurveRange(0, 0, 1, 1)]
        public AnimationCurve curveG = AnimationCurve.Linear(0, 0, 1, 1);
        public Channel channelB = Channel.B;
        [EZCurveRange(0, 0, 1, 1)]
        public AnimationCurve curveB = AnimationCurve.Linear(0, 0, 1, 1);
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
                        int coordX = Mathf.RoundToInt((float)x / (texture.width - 1) * (referenceTexture.width - 1));
                        int coordY = Mathf.RoundToInt((float)y / (texture.height - 1) * (referenceTexture.height - 1));
                        Color color = referenceTexture.GetPixel(coordX, coordY);
                        Color newColor = new Color(
                            FloatFromChannel(color, channelR, curveR),
                            FloatFromChannel(color, channelG, curveG),
                            FloatFromChannel(color, channelB, curveB),
                            FloatFromChannel(color, channelA, curveA)
                        );
                        texture.SetPixel(x, y, newColor);
                    }
                }
            }
        }

        public float FloatFromChannel(Color color, Channel channel, AnimationCurve curve)
        {
            float value = 1;
            switch (channel)
            {
                case Channel.R:
                    value = color.r;
                    break;
                case Channel.G:
                    value = color.g;
                    break;
                case Channel.B:
                    value = color.b;
                    break;
                case Channel.A:
                    value = color.a;
                    break;
            }
            return curve.Evaluate(value);
        }
    }
}
