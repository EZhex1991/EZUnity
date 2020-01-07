/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-09 11:20:58
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CreateAssetMenu(fileName = nameof(EZWaveTextureGenerator),
        menuName = MenuName_TextureGenerator + nameof(EZWaveTextureGenerator))]
    public class EZWaveTextureGenerator : EZTextureGenerator
    {
        public enum Antialiasing { None, X2, X4 }

        [EZCurveRect(0, 0, 1, 1)]
        public AnimationCurve waveShape = AnimationCurve.Linear(0, 0.5f, 1, 0.5f);
        public Antialiasing antialiasing = Antialiasing.None;

        public Color color0 = Color.black;
        public Color color1 = Color.white;

        public override void SetTexturePixels(Texture2D texture)
        {
            int maxX = texture.width - 1;
            int maxY = texture.height - 1;
            if (antialiasing == Antialiasing.X2)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    float u1 = (x - 0.25f) / maxX;
                    float u2 = (x + 0.25f) / maxX;
                    for (int y = 0; y < texture.height; y++)
                    {
                        int s1 = 0, s2 = 0;
                        float v1 = (y - 0.25f) / maxY;
                        float v2 = (y + 0.25f) / maxY;
                        if (waveShape.Evaluate(u1) > v1)
                        {
                            s1 = 1;
                        }
                        if (waveShape.Evaluate(u2) > v2)
                        {
                            s2 = 1;
                        }
                        float average = (s1 + s2) / 2f;
                        Color color = Color.Lerp(color0, color1, average);
                        texture.SetPixel(x, y, color);
                    }
                }
            }
            else if (antialiasing == Antialiasing.X4)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    float u1 = (x - 0.25f) / maxX;
                    float u2 = (x + 0.25f) / maxX;
                    for (int y = 0; y < texture.height; y++)
                    {
                        int s1 = 0, s2 = 0, s3 = 0, s4 = 0;
                        float v1 = (y - 0.25f) / maxY;
                        float v2 = (y + 0.25f) / maxY;
                        if (waveShape.Evaluate(u1) > v1)
                        {
                            s1 = 1;
                            if (waveShape.Evaluate(u1) > v2)
                            {
                                s2 = 1;
                            }
                        }
                        if (waveShape.Evaluate(u2) > v1)
                        {
                            s3 = 1;
                            if (waveShape.Evaluate(u2) > v2)
                            {
                                s4 = 1;
                            }
                        }
                        float average = (s1 + s2 + s3 + s4) / 4f;
                        Color color = Color.Lerp(color0, color1, average);
                        texture.SetPixel(x, y, color);
                    }
                }
            }
            else
            {
                for (int x = 0; x < texture.width; x++)
                {
                    float u = (float)x / maxX;
                    float value = waveShape.Evaluate(u);
                    for (int y = 0; y < texture.height; y++)
                    {
                        float v = (float)y / maxY;
                        texture.SetPixel(x, y, v >= value ? color1 : color0);
                    }
                }
            }
        }
    }
}
