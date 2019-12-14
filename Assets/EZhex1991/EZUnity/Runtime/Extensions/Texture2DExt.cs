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
