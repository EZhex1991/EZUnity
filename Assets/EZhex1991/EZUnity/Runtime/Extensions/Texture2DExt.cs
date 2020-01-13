/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-11-28 15:47:31
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public enum TextureEncoding { PNG, JPG, TGA }

    public static class Texture2DExt
    {
        // Textures
        private static Texture2D m_White;
        private static Texture2D m_Black;
        private static Texture2D m_Gray;
        private static Texture2D m_Transparent;
        public static Texture2D SingleColor(Color color, TextureFormat format = TextureFormat.ARGB32, string name = "")
        {
            if (string.IsNullOrEmpty(name)) name = "Texture-" + ColorUtility.ToHtmlStringRGBA(color);
            Texture2D texture = new Texture2D(1, 1, format, false) { name = name };
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }
        public static Texture2D white
        {
            get
            {
                if (m_White == null)
                {
                    m_White = SingleColor(Color.white, TextureFormat.ARGB32, "Texture-White");
                }
                return m_White;
            }
        }
        public static Texture2D black
        {
            get
            {
                if (m_Black == null)
                {
                    m_Black = SingleColor(Color.black, TextureFormat.ARGB32, "Texture-Black");
                }
                return m_Black;
            }
        }
        public static Texture2D gray
        {
            get
            {
                if (m_Gray == null)
                {
                    m_Gray = SingleColor(Color.gray, TextureFormat.ARGB32, "Texture-Gray");
                }
                return m_Gray;
            }
        }
        public static Texture2D transparent
        {
            get
            {
                if (m_Transparent == null)
                {
                    m_Transparent = SingleColor(Color.clear, TextureFormat.ARGB32, "Texture-Transparent");
                }
                return m_Transparent;
            }
        }

        public static Texture2D GetLut(AnimationCurve curve, int width = 1024, int height = 1, TextureFormat format = TextureFormat.RFloat)
        {
            Texture2D lut = new Texture2D(width, height, format, false, false);
            float maxX = width - 1;
            for (int i = 0; i < width; i++)
            {
                Color color = curve.Evaluate(i / maxX) * Color.white;
                for (int j = 0; j < height; j++)
                {
                    lut.SetPixel(i, j, color);
                }
            }
            lut.Apply();
            lut.wrapMode = TextureWrapMode.Clamp;
            return lut;
        }
        public static Texture2D GetLut(Gradient gradient, int width = 1024, int height = 1, TextureFormat format = TextureFormat.RFloat)
        {
            Texture2D lut = new Texture2D(width, height, format, false, false);
            float maxX = width - 1;
            for (int i = 0; i < width; i++)
            {
                Color color = gradient.Evaluate(i / maxX);
                for (int j = 0; j < height; j++)
                {
                    lut.SetPixel(i, j, color);
                }
            }
            lut.Apply();
            lut.wrapMode = TextureWrapMode.Clamp;
            return lut;
        }

        public static void SetLut(this Texture2D texture, AnimationCurve curve)
        {
            float maxX = texture.width - 1;
            for (int i = 0; i < texture.width; i++)
            {
                Color color = curve.Evaluate(i / maxX) * Color.white;
                for (int j = 0; j < texture.height; j++)
                {
                    texture.SetPixel(i, j, color);
                }
            }
            texture.Apply();
        }
        public static void SetLut(this Texture2D texture, Gradient gradient)
        {
            float maxX = texture.width - 1;
            for (int i = 0; i < texture.width; i++)
            {
                Color color = gradient.Evaluate(i / maxX);
                for (int j = 0; j < texture.height; j++)
                {
                    texture.SetPixel(i, j, color);
                }
            }
            texture.Apply();
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
                    Debug.LogError("TGA encoding is not available on Unity2018.2 or earlier version, PNG encoding will be used");
                    break;
#endif
            }
            return texture.EncodeToPNG();
        }
    }
}
