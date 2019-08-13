/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-03-19 09:49:34
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity.AssetGenerator
{
    [CustomEditor(typeof(EZTextureGenerator), true)]
    public class EZTextureGeneratorEditor : Editor
    {
        protected EZTextureGenerator generator;
        protected Texture2D previewTexture;

        protected SerializedProperty resolution;
        protected SerializedProperty textureFormat;
        protected SerializedProperty textureEncoding;
        protected SerializedProperty targetTexture;

        protected void OnEnable()
        {
            generator = target as EZTextureGenerator;
            previewTexture = new Texture2D(generator.previewResolution.x, generator.previewResolution.y, TextureFormat.RGBA32, false);
            resolution = serializedObject.FindProperty("resolution");
            textureFormat = serializedObject.FindProperty("textureFormat");
            textureEncoding = serializedObject.FindProperty("textureEncoding");
            targetTexture = serializedObject.FindProperty("targetTexture");
            GetProperties();
            if (generator.previewAutoUpdate)
            {
                RefreshPreview();
                Undo.undoRedoPerformed += RefreshPreview;
            }
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

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Texture Settings", EditorStyles.boldLabel);
            DrawTextureSettings();
            bool refreshPreview = EditorGUI.EndChangeCheck();

            EditorGUILayout.Space();
            DrawGenerateButton();

            serializedObject.ApplyModifiedProperties();
            if (generator.previewAutoUpdate && refreshPreview) RefreshPreview();
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
            if (!generator.previewAutoUpdate)
            {
                if (GUILayout.Button("Refresh Preview"))
                {
                    RefreshPreview();
                }
            }
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
            generator.SetTexturePixels(previewTexture);
            previewTexture.Apply();
            Repaint();
        }
    }
}
