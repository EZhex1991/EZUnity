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
