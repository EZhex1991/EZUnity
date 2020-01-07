/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-03-19 15:53:46
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CreateAssetMenu(
        fileName = nameof(EZGradient2DTextureGenerator),
        menuName = MenuName_TextureGenerator + nameof(EZGradient2DTextureGenerator))]
    public class EZGradient2DTextureGenerator : EZTextureGenerator
    {
        public enum CoordinateMode
        {
            X,
            Y,
            AdditiveXY,
            MultiplyXY,
            DifferenceXY,
            Radial,
            Angle,
        }
        private delegate float Sampler(float x, float y);

        public Gradient gradient = GradientExt.BlackToWhite();
        [EZCurveRect]
        public AnimationCurve gradientCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [Space]
        public CoordinateMode coordinateMode = CoordinateMode.AdditiveXY;
        [EZCurveRect]
        public AnimationCurve coordinateCurveU = AnimationCurve.Linear(0, 0, 1, 1);
        [EZCurveRect]
        public AnimationCurve coordinateCurveV = AnimationCurve.Linear(0, 0, 1, 1);
        public float rotation = 0f;

        public override TextureWrapMode defaultWrapMode { get { return TextureWrapMode.Clamp; } }

        public override void SetTexturePixels(Texture2D texture)
        {
            switch (coordinateMode)
            {
                case CoordinateMode.X:
                    SetPixels(texture, SamplerX);
                    break;
                case CoordinateMode.Y:
                    SetPixels(texture, SamplerY);
                    break;
                case CoordinateMode.AdditiveXY:
                    SetPixels(texture, SamplerAdditiveXY);
                    break;
                case CoordinateMode.MultiplyXY:
                    SetPixels(texture, SamplerMultiplyXY);
                    break;
                case CoordinateMode.DifferenceXY:
                    SetPixels(texture, SamplerDifferenceXY);
                    break;
                case CoordinateMode.Radial:
                    SetPixels(texture, SamplerRadial);
                    break;
                case CoordinateMode.Angle:
                    SetPixels(texture, SamplerAngle);
                    break;
            }
        }
        private void SetPixels(Texture2D texture, Sampler sampler)
        {
            int maxX = texture.width - 1;
            int maxY = texture.height - 1;
            for (int x = 0; x < texture.width; x++)
            {
                float u = coordinateCurveU.Evaluate((float)x / maxX);
                for (int y = 0; y < texture.height; y++)
                {
                    float v = coordinateCurveV.Evaluate((float)y / maxY);
                    float time = sampler(u, v);
                    texture.SetPixel(x, y, gradient.Evaluate(gradientCurve.Evaluate(time)));
                }
            }
        }

        private float SamplerX(float x, float y)
        {
            return x;
        }
        private float SamplerY(float x, float y)
        {
            return y;
        }
        private float SamplerAdditiveXY(float x, float y)
        {
            return (x + y) / 2f;
        }
        private float SamplerMultiplyXY(float x, float y)
        {
            return x * y;
        }
        private float SamplerDifferenceXY(float x, float y)
        {
            return Mathf.Abs(x - y);
        }
        private float SamplerRadial(float x, float y)
        {
            x -= 0.5f; y -= 0.5f;
            return Mathf.Sqrt(x * x + y * y) * 2;
        }
        private float SamplerAngle(float x, float y)
        {
            x -= 0.5f; y -= 0.5f;
            return Mathf.Repeat((Mathf.Atan2(y, x) + (Mathf.Deg2Rad * rotation)) / Mathf.PI + 1, 2) * 0.5f;
        }
    }
}
