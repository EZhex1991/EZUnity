/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-03-18 10:38:24
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EZUnity.AssetGenerator
{
    public abstract class _EZTextureGenerator : ScriptableObject
    {
        public Vector2Int resolution = new Vector2Int(256, 256);
        public TextureFormat textureFormat = TextureFormat.RGBA32;
        public TextureEncoding textureEncoding = TextureEncoding.PNG;
        [UnityEngine.Serialization.FormerlySerializedAs("textureReference")]
        public Texture2D targetTexture;

        // Don't forget to call texture.Apply()
        public abstract void ApplyToTexture(Texture2D texture);

        public byte[] GetTextureData(Vector2Int resolution, TextureFormat textureFormat)
        {
            Texture2D texture = new Texture2D(resolution.x, resolution.y, textureFormat, false);
            ApplyToTexture(texture);
            texture.Apply();
            byte[] bytes = texture.Encode(textureEncoding);
            DestroyImmediate(texture);
            return bytes;
        }

        public virtual void GenerateTexture()
        {
            if (targetTexture == null)
            {
                string path = AssetDatabase.GetAssetPath(this);
                string prefix = path.Substring(0, path.Length - 6);
                int index = 0;
                do
                {
                    path = string.Format("{0}_{1:D2}.{2}", prefix, index, textureEncoding.ToString().ToLower());
                    index++;
                } while (File.Exists(path));
                File.WriteAllBytes(path, GetTextureData(resolution, textureFormat));
                AssetDatabase.Refresh();
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
                OnTextureCreated(importer);
                importer.SaveAndReimport();
                targetTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            }
            else
            {
                string path = AssetDatabase.GetAssetPath(targetTexture);
                File.WriteAllBytes(path, GetTextureData(resolution, textureFormat));
                string extension = "." + textureEncoding.ToString().ToLower();
                if (Path.GetExtension(path) != extension)
                {
                    string newPath = Path.ChangeExtension(path, extension);
                    AssetDatabase.MoveAsset(path, newPath);
                }
                AssetDatabase.Refresh();
            }
        }
        protected virtual void OnTextureCreated(TextureImporter importer)
        {
            importer.mipmapEnabled = false;
            importer.isReadable = true;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
        }
    }
}
