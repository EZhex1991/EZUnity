/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-03-19 09:49:34
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CustomEditor(typeof(EZTextureGenerator), true)]
    public class EZTextureGeneratorEditor : Editor
    {
        protected EZTextureGenerator generator;
        protected Texture2D previewTexture;

        protected SerializedProperty m_OutputResolution;
        protected SerializedProperty m_OutputFormat;
        protected SerializedProperty m_OutputEncoding;
        protected SerializedProperty m_OutputTexture;

        protected SerializedProperty m_CorrespondingGenerator;

        protected void OnEnable()
        {
            generator = target as EZTextureGenerator;
            previewTexture = new Texture2D(generator.previewResolution.x, generator.previewResolution.y);
            m_OutputResolution = serializedObject.FindProperty("m_OutputResolution");
            m_OutputFormat = serializedObject.FindProperty("m_OutputFormat");
            m_OutputEncoding = serializedObject.FindProperty("m_OutputEncoding");
            m_OutputTexture = serializedObject.FindProperty("m_OutputTexture");
            m_CorrespondingGenerator = serializedObject.FindProperty("m_CorrespondingGenerator");
            GetInputProperties();
            if (generator.previewAutoUpdate)
            {
                RefreshPreview();
                Undo.undoRedoPerformed += RefreshPreview;
            }
        }
        protected virtual void GetInputProperties()
        {
        }
        protected void OnDisable()
        {
            Undo.undoRedoPerformed -= RefreshPreview;
        }

        public sealed override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject(target as ScriptableObject), typeof(MonoScript), false);
            EditorGUILayout.ObjectField("Target", target, typeof(Object), false);
            GUI.enabled = true;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Output Settings", EditorStyles.boldLabel);
            DrawOutputSettings();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Input Settings", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            DrawInputSettings();
            bool inputChanged = EditorGUI.EndChangeCheck();
            if (GUILayout.Button("Refresh Preview"))
            {
                RefreshPreview(true);
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Generate Settings", EditorStyles.boldLabel);
            DrawGenerateSettings();

            serializedObject.ApplyModifiedProperties();
            if (generator.previewAutoUpdate && inputChanged) RefreshPreview();
        }
        protected virtual void DrawOutputSettings()
        {
            EditorGUILayout.PropertyField(m_OutputResolution);
            EditorGUILayout.PropertyField(m_OutputFormat);
            EditorGUILayout.PropertyField(m_OutputEncoding);
#if !UNITY_2018_3_OR_NEWER
            if (m_OutputEncoding.intValue == (int)TextureEncoding.TGA)
            {
                Debug.LogWarning("TGA encoding is not supported on Unity2018.2 or earlier version, PNG encoding will be used");
                m_OutputEncoding.intValue = (int)TextureEncoding.PNG;
            }
#endif

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(m_OutputTexture);
            if (GUILayout.Button("Clear", GUILayout.Width(50)))
            {
                m_OutputTexture.objectReferenceValue = null;
            }
            EditorGUILayout.EndHorizontal();
        }
        protected virtual void DrawInputSettings()
        {
            SerializedProperty iterator = m_CorrespondingGenerator.Copy();
            while (iterator.Next(false))
            {
                EditorGUILayout.PropertyField(iterator, iterator.isExpanded);
            }
        }
        protected virtual void DrawGenerateSettings()
        {
            EditorGUILayout.PropertyField(m_CorrespondingGenerator);
            if (GUILayout.Button("Generate"))
            {
                generator.GenerateTexture(new System.Collections.Generic.HashSet<EZTextureGenerator>());
            }
            if (GUILayout.Button("Generate (No Correspondings)"))
            {
                generator.GenerateTexture();
            }
        }

        public sealed override bool HasPreviewGUI()
        {
            return true;
        }
        public override void DrawPreview(Rect previewArea)
        {
            EditorGUI.DrawPreviewTexture(previewArea, previewTexture, null, generator.previewScaleMode);
        }

        protected virtual void RefreshPreview(bool checkResolution)
        {
            if (checkResolution)
            {
                if (previewTexture.width != generator.previewResolution.x || previewTexture.height != generator.previewResolution.y)
                {
                    previewTexture = new Texture2D(generator.previewResolution.x, generator.previewResolution.y, TextureFormat.RGBA32, false);
                }
            }
            RefreshPreview();
        }
        protected virtual void RefreshPreview()
        {
            generator.SetTexturePixels(previewTexture);
            previewTexture.Apply();
            Repaint();
        }
    }
}
