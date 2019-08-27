/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-19 11:55:49
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    public enum ColorChannel { R, G, B, A }

    public enum TextureEncoding { PNG, JPG, TGA }

    public static class EZTextureProcessorUtility
    {
        public const string MenuName_TextureProcessor = "EZUnity/TextureProcessor/";
        public const string MenuName_TextureGenerator = "EZUnity/TextureGenerator/";

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

        public static byte[] Encode(this Texture2D texture, TextureEncoding encoding)
        {
            switch (encoding)
            {
                case TextureEncoding.PNG: return texture.EncodeToPNG();
                case TextureEncoding.JPG: return texture.EncodeToJPG();
                case TextureEncoding.TGA:
#if UNITY_2018_3_OR_NEWER
                    return texture.EncodeToTGA();
#else
                    Debug.LogWarning("TGA encoding is not supported on Unity2018.2 or earlier version, PNG encoding will be used");
                    break;
#endif
            }
            return texture.EncodeToPNG();
        }

        public static Gradient GradientFadeOut()
        {
            GradientColorKey[] colorKeys = new GradientColorKey[]
            {
                new GradientColorKey(Color.white, 0),
                new GradientColorKey(Color.black, 1),
            };
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[]
            {
                new GradientAlphaKey(1, 0),
                new GradientAlphaKey(0, 1),
            };

            Gradient gradient = new Gradient();
            gradient.SetKeys(colorKeys, alphaKeys);
            return gradient;
        }
        public static Gradient GradientFadeIn()
        {
            GradientColorKey[] colorKeys = new GradientColorKey[]
            {
                new GradientColorKey(Color.black, 0),
                new GradientColorKey(Color.white, 1),
            };
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[]
            {
                new GradientAlphaKey(0, 0),
                new GradientAlphaKey(1, 1),
            };

            Gradient gradient = new Gradient();
            gradient.SetKeys(colorKeys, alphaKeys);
            return gradient;
        }
        public static Gradient GradientBlackToWhite()
        {
            GradientColorKey[] colorKeys = new GradientColorKey[]
            {
                new GradientColorKey(Color.black, 0),
                new GradientColorKey(Color.white, 1),
            };
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[]
            {
                new GradientAlphaKey(1, 0),
            };

            Gradient gradient = new Gradient();
            gradient.SetKeys(colorKeys, alphaKeys);
            return gradient;
        }
        public static Gradient GradientWhiteToBlack()
        {
            GradientColorKey[] colorKeys = new GradientColorKey[]
            {
                new GradientColorKey(Color.white, 0),
                new GradientColorKey(Color.black, 1),
            };
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[]
            {
                new GradientAlphaKey(1, 0),
            };

            Gradient gradient = new Gradient();
            gradient.SetKeys(colorKeys, alphaKeys);
            return gradient;
        }
    }
}
