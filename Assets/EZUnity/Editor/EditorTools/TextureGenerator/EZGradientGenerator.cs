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
            X,
            Y,
            Diagonal,
            Radial,
        }
        public delegate Color Sampler(float x, float y);

        public Gradient gradient = new Gradient();
        public CoordinateMode coordinateMode = CoordinateMode.X;
        [EZCurveRange(0, 0, 1, 1)]
        public AnimationCurve coordinateX = AnimationCurve.Linear(0, 0, 1, 1);
        [EZCurveRange(0, 0, 1, 1)]
        public AnimationCurve coordinateY = AnimationCurve.Linear(0, 0, 1, 1);

        public override void SetTexture(Texture2D texture)
        {
            switch (coordinateMode)
            {
                case CoordinateMode.X:
                    SetPixels(texture, SamplerX);
                    break;
                case CoordinateMode.Y:
                    SetPixels(texture, SamplerY);
                    break;
                case CoordinateMode.Diagonal:
                    SetPixels(texture, SamplerDiagonal);
                    break;
                case CoordinateMode.Radial:
                    SetPixels(texture, SamplerRadial);
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
                    texture.SetPixel(x, y, sampler(coordX, coordY));
                }
            }
        }
        private Color SamplerX(float x, float y)
        {
            return gradient.Evaluate(x);
        }
        private Color SamplerY(float x, float y)
        {
            return gradient.Evaluate(y);
        }
        private Color SamplerDiagonal(float x, float y)
        {
            return gradient.Evaluate((x + y) * 0.5f);
        }
        private Color SamplerRadial(float x, float y)
        {
            x -= 0.5f; y -= 0.5f;
            float time = Mathf.Sqrt(x * x + y * y) * 2;
            return gradient.Evaluate(time);
        }
    }
}
