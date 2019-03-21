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
            generator.SetPixels(previewTexture);
        }

        public override void OnInspectorGUI()
        {
            EZEditorGUIUtility.ScriptableObjectTitle(target as EZTextureGenerator, true);
            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("File Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(resolution);
            EditorGUILayout.PropertyField(textureFormat);
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.PropertyField(textureReference);
                if (GUILayout.Button("Clear", new GUILayoutOption[] { GUILayout.Width(50) }))
                {
                    textureReference.objectReferenceValue = null;
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Texture Settings", EditorStyles.boldLabel);
            DrawTextureSettings();

            EditorGUILayout.Space();
            if (GUILayout.Button("Generate"))
            {
                GenerateTexture();
            }

            serializedObject.ApplyModifiedProperties();
            if (GUI.changed) generator.SetPixels(previewTexture);
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
                string prefix = path.Substring(0, path.Length - 6);
                int index = 0;
                do
                {
                    path = string.Format("{0}_{1:D2}.{2}", prefix, index, "png");
                    index++;
                } while (File.Exists(path));
                File.WriteAllBytes(path, generator.GetTextureData());
                AssetDatabase.Refresh();
                OnTextureCreated(path);
                generator.textureReference = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            }
            else
            {
                string path = AssetDatabase.GetAssetPath(generator.textureReference);
                File.WriteAllBytes(path, generator.GetTextureData());
                AssetDatabase.Refresh();
            }
        }
        public virtual void OnTextureCreated(string path)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            importer.mipmapEnabled = false;
            importer.isReadable = true;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.SaveAndReimport();
        }
    }
}
