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
    [CustomEditor(typeof(EZTextureGenerator), true)]
    public class EZTextureGeneratorEditor : Editor
    {
        protected EZTextureGenerator generator;

        protected virtual Vector2Int previewResolution { get { return new Vector2Int(128, 128); } }
        private Texture2D m_PreviewTexture;
        protected Texture2D previewTexture
        {
            get
            {
                if (m_PreviewTexture == null)
                    m_PreviewTexture = new Texture2D(previewResolution.x, previewResolution.y, TextureFormat.RGB24, false);
                return m_PreviewTexture;
            }
        }

        protected virtual void OnEnable()
        {
            generator = target as EZTextureGenerator;
            generator.SetPixels(previewTexture);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawGenerateButton();
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

        protected void DrawGenerateButton()
        {
            if (GUILayout.Button("Generate"))
            {
                GenerateTexture();
            }
        }

        protected virtual void GenerateTexture()
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
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
                OnTextureCreated(importer);
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
        protected virtual void OnTextureCreated(TextureImporter importer)
        {
            importer.mipmapEnabled = false;
            importer.isReadable = true;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
        }
    }

    public abstract class EZTextureGeneratorEditorLayout : EZTextureGeneratorEditor
    {
        protected SerializedProperty resolution;
        protected SerializedProperty textureFormat;
        protected SerializedProperty textureReference;

        protected sealed override void OnEnable()
        {
            base.OnEnable();
            GetFileSettings();
            GetTextureSettings();
        }
        protected virtual void GetFileSettings()
        {
            resolution = serializedObject.FindProperty("resolution");
            textureFormat = serializedObject.FindProperty("textureFormat");
            textureReference = serializedObject.FindProperty("textureReference");
        }
        protected abstract void GetTextureSettings();

        public sealed override void OnInspectorGUI()
        {
            EZEditorGUIUtility.ScriptableObjectTitle(target as EZTextureGenerator, true);
            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("File Settings", EditorStyles.boldLabel);
            DrawFileSettings();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Texture Settings", EditorStyles.boldLabel);
            DrawTextureSettings();

            EditorGUILayout.Space();
            DrawGenerateButton();

            if (GUI.changed) generator.SetPixels(previewTexture);
            serializedObject.ApplyModifiedProperties();
        }
        protected virtual void DrawFileSettings()
        {
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
        }
        protected abstract void DrawTextureSettings();
    }
}
