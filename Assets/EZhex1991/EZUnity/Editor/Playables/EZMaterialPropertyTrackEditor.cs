/* Author:          ezhex1991@outlook.com
 * CreateTime:      2020-03-25 18:19:30
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZhex1991.EZUnity.Playables
{
    [CustomEditor(typeof(EZMaterialPropertyTrack))]
    public class EZMaterialPropertyTrackEditor : Editor
    {
        private SerializedProperty m_Template;
        private SerializedProperty m_MaterialIndex;
        private SerializedProperty m_FloatProperties;
        private SerializedProperty m_ColorProperties;
        private SerializedProperty m_VectorProperties;
        private ReorderableList floatPropertyList;
        private ReorderableList colorPropertyList;
        private ReorderableList vectorPropertyList;

        private void OnEnable()
        {
            m_Template = serializedObject.FindProperty("template");
            m_MaterialIndex = m_Template.FindPropertyRelative("materialIndex");
            m_FloatProperties = m_Template.FindPropertyRelative("floatProperties");
            m_ColorProperties = m_Template.FindPropertyRelative("colorProperties");
            m_VectorProperties = m_Template.FindPropertyRelative("vectorProperties");
            floatPropertyList = new ReorderableList(serializedObject, m_FloatProperties)
            {
                drawHeaderCallback = (rect) => DrawPropertyListHeader(rect, floatPropertyList),
                drawElementCallback = (rect, index, isActive, isFocused) => DrawPropertyListElement(rect, m_FloatProperties, index, isActive, isFocused),
            };
            colorPropertyList = new ReorderableList(serializedObject, m_ColorProperties)
            {
                drawHeaderCallback = (rect) => DrawPropertyListHeader(rect, colorPropertyList),
                drawElementCallback = (rect, index, isActive, isFocused) => DrawPropertyListElement(rect, m_ColorProperties, index, isActive, isFocused),
            };
            vectorPropertyList = new ReorderableList(serializedObject, m_VectorProperties)
            {
                drawHeaderCallback = (rect) => DrawPropertyListHeader(rect, vectorPropertyList),
                drawElementCallback = (rect, index, isActive, isFocused) => DrawPropertyListElement(rect, m_VectorProperties, index, isActive, isFocused),
            };
        }

        private void DrawPropertyListHeader(Rect rect, ReorderableList list)
        {
            rect = EZEditorGUIUtility.CalcReorderableListHeaderRect(rect, list);
            float margin = 2;
            float width = (rect.width - margin) / 2;
            rect.width = width;
            EditorGUI.LabelField(rect, "PropertyName");
            rect.x += width + margin;
            EditorGUI.LabelField(rect, "Value");
        }
        private void DrawPropertyListElement(Rect rect, SerializedProperty listProperty, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = listProperty.GetArrayElementAtIndex(index);
            SerializedProperty propertyName = element.FindPropertyRelative("propertyName");
            SerializedProperty value = element.FindPropertyRelative("value");

            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, listProperty, index);
            float margin = 2;
            float width = (rect.width - margin) / 2;
            rect.width = width;
            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect, propertyName, GUIContent.none);
            rect.x += width + margin;
            EditorGUI.PropertyField(rect, value, GUIContent.none);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EZEditorGUIUtility.ScriptableObjectTitle(target as ScriptableObject, false);

            EditorGUILayout.PropertyField(m_MaterialIndex);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Float Properties", EditorStyles.boldLabel);
            floatPropertyList.DoLayoutList();
            EditorGUILayout.LabelField("Color Properties", EditorStyles.boldLabel);
            colorPropertyList.DoLayoutList();
            EditorGUILayout.LabelField("Vector Properties", EditorStyles.boldLabel);
            vectorPropertyList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
