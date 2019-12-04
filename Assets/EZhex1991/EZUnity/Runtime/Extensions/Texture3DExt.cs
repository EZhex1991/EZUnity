/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-12-04 17:39:36
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public static class Texture3DExt
    {
        // Textures
        private static Texture3D m_White;
        private static Texture3D m_Black;
        private static Texture3D m_Gray;
        private static Texture3D m_Transparent;
        public static Texture3D SingleColor(Color color, TextureFormat format = TextureFormat.ARGB32, string name = "")
        {
            if (string.IsNullOrEmpty(name)) name = "Texture-" + ColorUtility.ToHtmlStringRGBA(color);
            Texture3D texture = new Texture3D(1, 1, 1, format, false) { name = name };
            texture.SetPixels(new Color[] { color });
            texture.Apply();
            return texture;
        }
        public static Texture3D white
        {
            get
            {
                if (m_White == null)
                {
                    m_White = SingleColor(Color.white, TextureFormat.ARGB32, "Texture3D-White");
                }
                return m_White;
            }
        }
        public static Texture3D black
        {
            get
            {
                if (m_Black == null)
                {
                    m_Black = SingleColor(Color.black, TextureFormat.ARGB32, "Texture3D-Black");
                }
                return m_Black;
            }
        }
        public static Texture3D gray
        {
            get
            {
                if (m_Gray == null)
                {
                    m_Gray = SingleColor(Color.gray, TextureFormat.ARGB32, "Texture3D-Gray");
                }
                return m_Gray;
            }
        }
        public static Texture3D transparent
        {
            get
            {
                if (m_Transparent == null)
                {
                    m_Transparent = SingleColor(Color.clear, TextureFormat.ARGB32, "Texture3D-Transparent");
                }
                return m_Transparent;
            }
        }
    }
}
