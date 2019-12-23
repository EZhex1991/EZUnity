/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-11-28 15:46:04
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public enum ColorChannel { R, G, B, A }

    public static class ColorExt
    {
        public static float GetChannel(this Color color, ColorChannel channel)
        {
            switch (channel)
            {
                case ColorChannel.R: return color.r;
                case ColorChannel.G: return color.g;
                case ColorChannel.B: return color.b;
                case ColorChannel.A: return color.a;
            }
            return 0;
        }

        public static float MaxRGB(this Color color)
        {
            return Mathf.Max(Mathf.Max(color.r, color.g), color.b);
        }
        public static float MinRGB(this Color color)
        {
            return Mathf.Min(Mathf.Min(color.r, color.g), color.b);
        }

        public static float MaxRGBA(this Color color)
        {
            return Mathf.Max(Mathf.Max(color.r, color.g), Mathf.Max(color.b, color.a));
        }
        public static float MinRGBA(this Color color)
        {
            return Mathf.Min(Mathf.Min(color.r, color.g), Mathf.Min(color.b, color.a));
        }

        public static float ToGray(this Color color)
        {
            return ToGray(color, new Color(0.299f, 0.587f, 0.114f, 1));
        }
        public static float ToGray(this Color color, Color grayWeight)
        {
            Color gray = color * grayWeight;
            return (gray.r + gray.g + gray.b) * gray.a;
        }
    }
}
