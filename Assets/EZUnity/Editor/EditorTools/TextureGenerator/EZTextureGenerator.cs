/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-03-18 10:38:24
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZUnity
{
    public abstract class EZTextureGenerator : ScriptableObject
    {
        public Vector2Int resolution = new Vector2Int(256, 256);
        public TextureFormat textureFormat = TextureFormat.RGB24;
        public Texture2D textureReference;

        public abstract void SetTexture(Texture2D texture);
        public byte[] GetTextureData()
        {
            Texture2D texture = new Texture2D(resolution.x, resolution.y, textureFormat, false);
            SetTexture(texture);
            texture.Apply();
            byte[] bytes = texture.EncodeToPNG();
            DestroyImmediate(texture);
            return bytes;
        }
    }
}
