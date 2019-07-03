/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-03-19 09:49:34
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity.AssetGenerator
{
    [CustomEditor(typeof(_EZTextureGenerator), true)]
    public class _EZTextureGeneratorEditor : Editor
    {
        protected _EZTextureGenerator generator;

        protected SerializedProperty resolution;
        protected SerializedProperty textureFormat;
        protected SerializedProperty textureEncoding;
        protected SerializedProperty targetTexture;

        protected virtual Vector2Int previewResolution { get { return new Vector2Int(128, 128); } }
        private Texture2D m_PreviewTexture;
        protected Texture2D previewTexture
        {
            get
            {
                if (m_PreviewTexture == null)
                    m_PreviewTexture = new Texture2D(previewResolution.x, previewResolution.y, TextureFormat.RGBA32, false);
                return m_PreviewTexture;
            }
        }

        protected void OnEnable()
        {
            generator = target as _EZTextureGenerator;
            resolution = serializedObject.FindProperty("resolution");
            textureFormat = serializedObject.FindProperty("textureFormat");
            textureEncoding = serializedObject.FindProperty("textureEncoding");
            targetTexture = serializedObject.FindProperty("targetTexture");
            GetProperties();
            RefreshPreview();
            Undo.undoRedoPerformed += RefreshPreview;
        }
        protected virtual void GetProperties()
        {
        }
        protected void OnDisable()
        {
            Undo.undoRedoPerformed -= RefreshPreview;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EZEditorGUIUtility.ScriptableObjectTitle(target as ScriptableObject, !serializedObject.isEditingMultipleObjects);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("File Settings", EditorStyles.boldLabel);
            DrawFileSettings();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Texture Settings", EditorStyles.boldLabel);
            DrawTextureSettings();

            EditorGUILayout.Space();
            DrawGenerateButton();

            serializedObject.ApplyModifiedProperties();
            if (GUI.changed) RefreshPreview();
        }
        protected virtual void DrawFileSettings()
        {
            EditorGUILayout.PropertyField(resolution);
            EditorGUILayout.PropertyField(textureFormat);
            EditorGUILayout.PropertyField(textureEncoding);
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.PropertyField(targetTexture);
                if (GUILayout.Button("Clear", new GUILayoutOption[] { GUILayout.Width(50) }))
                {
                    targetTexture.objectReferenceValue = null;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        protected virtual void DrawTextureSettings()
        {
            SerializedProperty iterator = targetTexture.Copy();
            while (iterator.Next(false))
            {
                EditorGUILayout.PropertyField(iterator);
            }
        }
        protected virtual void DrawGenerateButton()
        {
            if (GUILayout.Button("Generate"))
            {
                generator.GenerateTexture();
            }
        }

        public sealed override bool HasPreviewGUI()
        {
            return true;
        }
        public sealed override void DrawPreview(Rect previewArea)
        {
            EditorGUI.DrawPreviewTexture(previewArea, previewTexture);
        }
        public void RefreshPreview()
        {
            generator.ApplyToTexture(previewTexture);
            previewTexture.Apply();
            Repaint();
        }
    }
}
