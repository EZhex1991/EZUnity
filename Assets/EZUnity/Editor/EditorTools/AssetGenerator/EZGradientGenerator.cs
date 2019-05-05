/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-03-19 15:53:46
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZUnity
{
    [CreateAssetMenu(fileName = "EZGradientGenerator", menuName = "EZUnity/EZGradientGenerator", order = (int)EZAssetMenuOrder.EZGradientGenerator)]
    public class EZGradientGenerator : EZTextureGenerator
    {
        public static readonly float iSqrt2 = 1 / Mathf.Sqrt(2);

        public enum CoordinateMode
        {
            AdditiveXY,
            MultiplyXY,
            DifferenceXY,
            Radial,
            Angle,
        }
        public enum Rotation
        {
            None,
            Clockwise90,
            CounterClockwise90,
            _180,
        }
        public delegate float Sampler(float x, float y);

        public Gradient gradient = new Gradient();
        public CoordinateMode coordinateMode = CoordinateMode.AdditiveXY;
        [EZCurveRange(0, 0, 1, 1)]
        public AnimationCurve coordinateX = AnimationCurve.Linear(0, 0, 1, 1);
        [EZCurveRange(0, 0, 1, 1)]
        public AnimationCurve coordinateY = AnimationCurve.Linear(0, 0, 1, 1);
        public Rotation rotation = Rotation.None;

        protected override void SetPixels(Texture2D texture)
        {
            switch (coordinateMode)
            {
                case CoordinateMode.AdditiveXY:
                    SetPixels(texture, SamplerAdditiveXY);
                    break;
                case CoordinateMode.MultiplyXY:
                    SetPixels(texture, SamplerMultiplyXY);
                    break;
                case CoordinateMode.DifferenceXY:
                    SetPixels(texture, SamplerDifference);
                    break;
                case CoordinateMode.Radial:
                    SetPixels(texture, SamplerRadial);
                    break;
                case CoordinateMode.Angle:
                    SetPixels(texture, SamplerAngle);
                    break;
            }
            texture.Apply();
        }

        private void SetPixels(Texture2D texture, Sampler sampler)
        {
            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    float coordX = coordinateX.Evaluate((float)x / (texture.width - 1));
                    float coordY = coordinateY.Evaluate((float)y / (texture.height - 1));
                    float time = 0;
                    switch (rotation)
                    {
                        case Rotation.None:
                            time = sampler(coordX, coordY);
                            break;
                        case Rotation.Clockwise90:
                            time = sampler(coordY, 1 - coordX);
                            break;
                        case Rotation._180:
                            time = sampler(1 - coordX, 1 - coordY);
                            break;
                        case Rotation.CounterClockwise90:
                            time = sampler(1 - coordY, coordX);
                            break;
                    }
                    texture.SetPixel(x, y, gradient.Evaluate(time));
                }
            }
        }
        private float SamplerAdditiveXY(float x, float y)
        {
            return (x + y) * 0.5f;
        }
        private float SamplerMultiplyXY(float x, float y)
        {
            return x * y;
        }
        private float SamplerDifference(float x, float y)
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
            return (Mathf.Atan2(y, x) / Mathf.PI + 1) * 0.5f;
        }
    }
}
