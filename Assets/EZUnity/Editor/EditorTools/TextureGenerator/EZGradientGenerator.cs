/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-03-19 15:53:46
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZUnity
{
    [CreateAssetMenu(fileName = "EZGradientGenerator", menuName = "EZUnity/EZGradientGenerator", order = EZUtility.AssetOrder)]
    public class EZGradientGenerator : EZTextureGenerator
    {
        public static readonly float iSqrt2 = 1 / Mathf.Sqrt(2);

        public enum CoordinateMode
        {
            LinearX,
            LinearY,
            Diagonal,
            Radial,
            Angle,
        }
        public delegate float Sampler(float x, float y);

        public Gradient gradient = new Gradient();
        public CoordinateMode coordinateMode = CoordinateMode.LinearX;
        [EZCurveRange(0, 0, 1, 1)]
        public AnimationCurve coordinateX = AnimationCurve.Linear(0, 0, 1, 1);
        [EZCurveRange(0, 0, 1, 1)]
        public AnimationCurve coordinateY = AnimationCurve.Linear(0, 0, 1, 1);

        public override void SetTexture(Texture2D texture)
        {
            switch (coordinateMode)
            {
                case CoordinateMode.LinearX:
                    SetPixels(texture, (x, y) => x);
                    break;
                case CoordinateMode.LinearY:
                    SetPixels(texture, (x, y) => y);
                    break;
                case CoordinateMode.Diagonal:
                    SetPixels(texture, SamplerDiagonal);
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
                    float coordX = coordinateX.Evaluate((float)x / texture.width);
                    float coordY = coordinateY.Evaluate((float)y / texture.height);
                    float time = sampler(coordX, coordY);
                    texture.SetPixel(x, y, gradient.Evaluate(time));
                }
            }
        }
        private float SamplerDiagonal(float x, float y)
        {
            return (x + y) * 0.5f;
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
