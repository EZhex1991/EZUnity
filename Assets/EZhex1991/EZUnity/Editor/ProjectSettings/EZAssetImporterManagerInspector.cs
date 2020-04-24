/* Author:          ezhex1991@outlook.com
 * CreateTime:      2020-04-16 14:28:07
 * Organization:    #ORGANIZATION#
 * Description:     
 */
#if UNITY_2018_3_OR_NEWER
using UnityEditor;
using UnityEditor.Presets;
using UnityEditorInternal;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CustomEditor(typeof(EZAssetImporterManager))]
    public class EZAssetImporterManagerInspector : Editor
    {
        private SerializedProperty m_DefaultModelImporterOverrides;
        private SerializedProperty m_ModelImporters;
        private SerializedProperty m_DefaultTextureImporterOverrides;
        private SerializedProperty m_TextureImporters;
        private SerializedProperty m_AudioImporters;

        private ReorderableList modelImporterList;
        private ReorderableList textureImporterList;
        private ReorderableList audioImporterList;

        private static readonly float recursiveToggleWidth = 60f;
        private static readonly float margin = 5f;

        private void OnEnable()
        {
            m_DefaultModelImporterOverrides = serializedObject.FindProperty("m_DefaultModelImporterOverrides");
            m_ModelImporters = serializedObject.FindProperty("m_ModelImporters");
            modelImporterList = new ReorderableList(serializedObject, m_ModelImporters, true, true, true, true)
            {
                drawHeaderCallback = (rect) => DrawImporterListHeader(rect, modelImporterList),
                drawElementCallback = (rect, index, isActive, isFocused) => DrawImporterListElement(rect, index, modelImporterList, "UnityEditor.FBXImporter"),
            };

            m_DefaultTextureImporterOverrides = serializedObject.FindProperty("m_DefaultTextureImporterOverrides");
            m_TextureImporters = serializedObject.FindProperty("m_TextureImporters");
            textureImporterList = new ReorderableList(serializedObject, m_TextureImporters, true, true, true, true)
            {
                drawHeaderCallback = (rect) => DrawImporterListHeader(rect, textureImporterList),
                drawElementCallback = (rect, index, isActive, isFocused) => DrawImporterListElement(rect, index, textureImporterList, typeof(TextureImporter).FullName),
            };
            m_AudioImporters = serializedObject.FindProperty("m_AudioImporters");
            audioImporterList = new ReorderableList(serializedObject, m_AudioImporters, true, true, true, true)
            {
                drawHeaderCallback = (rect) => DrawImporterListHeader(rect, audioImporterList),
                drawElementCallback = (rect, index, isActive, isFocused) => DrawImporterListElement(rect, index, audioImporterList, typeof(AudioImporter).FullName),
            };
        }
        public override void OnInspectorGUI()
        {
            EZEditorGUIUtility.ScriptableObjectTitle(target as ScriptableObject, false);
            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Model Importer Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_DefaultModelImporterOverrides, m_DefaultModelImporterOverrides.isExpanded);
            modelImporterList.DoLayoutList();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Texture Importer Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_DefaultTextureImporterOverrides, m_DefaultTextureImporterOverrides.isExpanded);
            textureImporterList.DoLayoutList();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Audio Importer Settings", EditorStyles.boldLabel);
            audioImporterList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }

        private static void DrawImporterListHeader(Rect rect, ReorderableList list)
        {
            rect.y += 1;
            rect = EZEditorGUIUtility.DrawReorderableListCount(rect, list);
            float width = (rect.width - recursiveToggleWidth) * 0.5f;
            rect.width = width;
            EditorGUI.LabelField(rect, "Preset");
            rect.x += width;
            EditorGUI.LabelField(rect, "Association");
            rect.x += width;
            rect.width = recursiveToggleWidth;
            EditorGUI.LabelField(rect, "Recursive");
        }
        private static void DrawImporterListElement(Rect rect, int index, ReorderableList list, string targetTypeName)
        {
            SerializedProperty importer = list.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty m_Preset = importer.FindPropertyRelative("m_Preset");
            SerializedProperty m_Association = importer.FindPropertyRelative("m_Association");
            SerializedProperty m_Recursive = importer.FindPropertyRelative("m_Recursive");

            rect.y += 1;
            rect.height = EditorGUIUtility.singleLineHeight;
            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, index, list);
            float width = (rect.width - recursiveToggleWidth) * 0.5f;
            rect.width = width - margin;

            Color guiColor = GUI.color;
            if (m_Preset.objectReferenceValue != null && (m_Preset.objectReferenceValue as Preset).GetTargetFullTypeName() != targetTypeName)
            {
                GUI.color = Color.red;
            }
            EditorGUI.PropertyField(rect, m_Preset, GUIContent.none);
            GUI.color = guiColor;

            rect.x += width;
            EditorGUI.PropertyField(rect, m_Association, GUIContent.none);
            rect.x += width;
            rect.width = recursiveToggleWidth;
            EditorGUI.PropertyField(rect, m_Recursive, GUIContent.none);
        }
    }
}
#endif
