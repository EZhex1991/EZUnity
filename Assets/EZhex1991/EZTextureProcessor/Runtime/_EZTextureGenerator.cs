/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-03-18 10:38:24
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using System.IO;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EZhex1991.EZTextureProcessor
{
    public abstract class EZTextureGenerator : ScriptableObject
    {
        public const string MenuName_TextureGenerator = "EZTextureProcessor/TextureGenerator/";
        public const string MenuName_TextureProcessor = "EZTextureProcessor/TextureProcessor/";

        [SerializeField]
        protected Vector2Int m_OutputResolution = new Vector2Int(256, 256);
        public Vector2Int outputResolution { get { return m_OutputResolution; } }
        [SerializeField]
        protected TextureFormat m_OutputFormat = TextureFormat.RGB24;
        public TextureFormat outputFormat { get { return m_OutputFormat; } }
        [SerializeField]
        protected TextureEncoding m_OutputEncoding = TextureEncoding.JPG;
        public TextureEncoding outputEncoding { get { return m_OutputEncoding; } }
        [SerializeField]
        protected Texture2D m_OutputTexture;
        public Texture2D outputTexture { get { return m_OutputTexture; } set { m_OutputTexture = value; } }

        [SerializeField]
        protected EZTextureGenerator m_CorrespondingGenerator;
        public EZTextureGenerator correspondingGenerator { get { return m_CorrespondingGenerator; } }

        public virtual bool previewAutoUpdate { get { return true; } }
        public virtual Vector2Int previewResolution { get { return new Vector2Int(128, 128); } }
        public virtual ScaleMode previewScaleMode { get { return ScaleMode.ScaleToFit; } }
        public virtual TextureWrapMode defaultWrapMode { get { return TextureWrapMode.Repeat; } }
        public virtual bool defaultMipmapSetting { get { return false; } }

        // Don't forget to call texture.Apply()
        public abstract void SetTexturePixels(Texture2D texture);

        public byte[] GetTextureData()
        {
            return GetTextureData(outputResolution, outputFormat);
        }
        public byte[] GetTextureData(Vector2Int resolution, TextureFormat textureFormat)
        {
            Texture2D texture = new Texture2D(resolution.x, resolution.y, textureFormat, false);
            SetTexturePixels(texture);
            texture.Apply();
            byte[] bytes = texture.Encode(outputEncoding);
            DestroyImmediate(texture);
            return bytes;
        }

#if UNITY_EDITOR
        public void GenerateTexture(HashSet<EZTextureGenerator> generators)
        {
            GenerateTexture();
            if (generators != null && correspondingGenerator != null)
            {
                generators.Add(this);
                if (!generators.Add(correspondingGenerator))
                {
                    Debug.LogErrorFormat(this, "Loop corresponding detected on {0}", correspondingGenerator.name);
                    return;
                }
                correspondingGenerator.GenerateTexture(generators);
            }
        }
        public void GenerateTexture()
        {
            if (outputTexture == null)
            {
                string generatorPath = AssetDatabase.GetAssetPath(this);
                string fileExtension = outputEncoding.ToString().ToLowerInvariant();
                string texturePath = Path.ChangeExtension(generatorPath, fileExtension);
                texturePath = AssetDatabase.GenerateUniqueAssetPath(texturePath);
                File.WriteAllBytes(texturePath, GetTextureData());
                AssetDatabase.Refresh();
                TextureImporter importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
                OnTextureCreated(importer);
                importer.SaveAndReimport();
                outputTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
            }
            else
            {
                string texturePath = AssetDatabase.GetAssetPath(outputTexture);
                File.WriteAllBytes(texturePath, GetTextureData());
                string fileExtension = outputEncoding.ToString().ToLowerInvariant();
                if (Path.GetExtension(texturePath) != fileExtension)
                {
                    string newPath = Path.ChangeExtension(texturePath, fileExtension);
                    AssetDatabase.MoveAsset(texturePath, newPath);
                }
                AssetDatabase.Refresh();
            }
        }
        protected virtual void OnTextureCreated(TextureImporter importer)
        {
            importer.wrapMode = defaultWrapMode;
            importer.mipmapEnabled = defaultMipmapSetting;
        }
#endif
    }
}
