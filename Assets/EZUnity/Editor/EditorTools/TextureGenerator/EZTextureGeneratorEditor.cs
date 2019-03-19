/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-03-19 09:49:34
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EZUnity
{
    public abstract class EZTextureGeneratorEditor : Editor
    {
        protected EZTextureGenerator generator;

        protected SerializedProperty resolution;
        protected SerializedProperty textureFormat;
        protected SerializedProperty textureReference;

        private Texture2D m_PreviewTexture;
        protected Texture2D previewTexture
        {
            get
            {
                if (m_PreviewTexture == null)
                    m_PreviewTexture = new Texture2D(128, 128, TextureFormat.RGB24, false);
                return m_PreviewTexture;
            }
        }

        protected virtual void OnEnable()
        {
            generator = target as EZTextureGenerator;
            resolution = serializedObject.FindProperty("resolution");
            textureFormat = serializedObject.FindProperty("textureFormat");
            textureReference = serializedObject.FindProperty("textureReference");
            generator.SetTexture(previewTexture);
        }

        public override void OnInspectorGUI()
        {
            EZEditorGUIUtility.ScriptableObjectTitle(target as EZTextureGenerator, true);
            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("File Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(resolution);
            EditorGUILayout.PropertyField(textureFormat);
            EditorGUILayout.PropertyField(textureReference);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Texture Settings", EditorStyles.boldLabel);
            DrawTextureSettings();

            EditorGUILayout.Space();
            if (GUILayout.Button("Generate"))
            {
                GenerateTexture();
            }

            serializedObject.ApplyModifiedProperties();
            if (GUI.changed) generator.SetTexture(previewTexture);
        }

        public sealed override bool HasPreviewGUI()
        {
            return previewTexture != null;
        }
        public sealed override void DrawPreview(Rect previewArea)
        {
            EditorGUI.DrawPreviewTexture(previewArea, previewTexture);
        }

        public abstract void DrawTextureSettings();

        public virtual void GenerateTexture()
        {
            if (generator.textureReference == null)
            {
                string path = AssetDatabase.GetAssetPath(generator);
                path = string.Format("{0}.{1}", path.Substring(0, path.Length - 6), "png");
                int index = 0;
                while (File.Exists(path))
                {
                    path = string.Format("{0}_{1:D2}.{2}", path.Substring(0, path.Length - 6), index, "png");
                    index++;
                }
                File.WriteAllBytes(path, generator.GetTextureData());
                AssetDatabase.Refresh();
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
                importer.mipmapEnabled = false;
                importer.isReadable = true;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.wrapMode = TextureWrapMode.Clamp;
                importer.SaveAndReimport();
                generator.textureReference = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            }
            else
            {
                string path = AssetDatabase.GetAssetPath(generator.textureReference);
                File.WriteAllBytes(path, generator.GetTextureData());
                AssetDatabase.Refresh();
            }
        }
    }
}
