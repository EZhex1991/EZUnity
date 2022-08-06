/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-11-28 14:58:13
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public static class GradientExt
    {
        public static GradientColorKey Color0_White = new GradientColorKey(Color.white, 0);
        public static GradientColorKey Color0_Black = new GradientColorKey(Color.black, 0);

        public static GradientColorKey Color1_White = new GradientColorKey(Color.white, 1);
        public static GradientColorKey Color1_Black = new GradientColorKey(Color.black, 1);

        public static GradientAlphaKey Alpha0_0 = new GradientAlphaKey(0, 0);
        public static GradientAlphaKey Alpha0_1 = new GradientAlphaKey(1, 0);

        public static GradientAlphaKey Alpha1_0 = new GradientAlphaKey(0, 1);
        public static GradientAlphaKey Alpha1_1 = new GradientAlphaKey(1, 1);

        public static Gradient New(GradientColorKey[] colorKeys, GradientAlphaKey[] alphaKeys)
        {
            Gradient gradient = new Gradient();
            gradient.SetKeys(colorKeys, alphaKeys);
            return gradient;
        }

        public static Gradient FadeIn()
        {
            GradientColorKey[] colorKeys = new GradientColorKey[] { Color0_Black, Color1_White };
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[] { Alpha0_0, Alpha1_1 };
            return New(colorKeys, alphaKeys);
        }
        public static Gradient FadeOut()
        {
            GradientColorKey[] colorKeys = new GradientColorKey[] { Color0_White, Color1_Black };
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[] { Alpha0_1, Alpha1_0 };
            return New(colorKeys, alphaKeys);
        }

        public static Gradient WhiteFadeIn()
        {
            GradientColorKey[] colorKeys = new GradientColorKey[] { Color0_White };
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[] { Alpha0_0, Alpha1_1 };
            return New(colorKeys, alphaKeys);
        }
        public static Gradient WhiteFadeOut()
        {
            GradientColorKey[] colorKeys = new GradientColorKey[] { Color0_White };
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[] { Alpha0_1, Alpha1_0 };
            return New(colorKeys, alphaKeys);
        }

        public static Gradient BlackFadeIn()
        {
            GradientColorKey[] colorKeys = new GradientColorKey[] { Color0_Black };
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[] { Alpha0_0, Alpha1_1 };
            return New(colorKeys, alphaKeys);
        }
        public static Gradient BlackFadeOut()
        {
            GradientColorKey[] colorKeys = new GradientColorKey[] { Color0_Black };
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[] { Alpha0_1, Alpha1_0 };
            return New(colorKeys, alphaKeys);
        }

        public static Gradient BlackToWhite()
        {
            GradientColorKey[] colorKeys = new GradientColorKey[] { Color0_Black, Color1_White };
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[] { Alpha0_1 };
            return New(colorKeys, alphaKeys);
        }
        public static Gradient WhiteToBlack()
        {
            GradientColorKey[] colorKeys = new GradientColorKey[] { Color0_White, Color1_Black };
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[] { Alpha0_1 };
            return New(colorKeys, alphaKeys);
        }
    }
}
