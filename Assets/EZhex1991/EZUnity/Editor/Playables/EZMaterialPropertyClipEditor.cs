/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-16 11:56:41
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZhex1991.EZUnity.Playables
{
    [CustomEditor(typeof(EZMaterialPropertyClip))]
    public class EZMaterialPropertyClipEditor : Editor
    {
        protected SerializedProperty m_Template;
        protected SerializedProperty m_FloatProperties;
        protected SerializedProperty m_ColorProperties;
        protected SerializedProperty m_VectorProperties;
        protected ReorderableList floatPropertyList;
        protected ReorderableList colorPropertyList;
        protected ReorderableList vectorPropertyList;

        protected static void DrawPropertyListHeader(Rect rect, ReorderableList list)
        {
            rect = EZEditorGUIUtility.CalcReorderableListHeaderRect(rect, list);
            float margin = 2;
            float width = (rect.width - margin) / 2;
            rect.width = width;
            EditorGUI.LabelField(rect, "PropertyName");
            rect.x += width + margin;
            EditorGUI.LabelField(rect, "Value");
        }
        protected static void DrawPropertyListElement(Rect rect, int index, ReorderableList list)
        {
            SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty propertyName = element.FindPropertyRelative("propertyName");
            SerializedProperty value = element.FindPropertyRelative("value");

            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, index, list);
            float margin = 2;
            float width = (rect.width - margin) / 2;
            rect.width = width;
            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect, propertyName, GUIContent.none);
            rect.x += width + margin;
            EditorGUI.PropertyField(rect, value, GUIContent.none);
        }

        protected virtual void OnEnable()
        {
            m_Template = serializedObject.FindProperty("template");
            m_FloatProperties = m_Template.FindPropertyRelative("floatProperties");
            m_ColorProperties = m_Template.FindPropertyRelative("colorProperties");
            m_VectorProperties = m_Template.FindPropertyRelative("vectorProperties");
            floatPropertyList = new ReorderableList(serializedObject, m_FloatProperties)
            {
                drawHeaderCallback = (rect) => DrawPropertyListHeader(rect, floatPropertyList),
                drawElementCallback = (rect, index, isActive, isFocused) => DrawPropertyListElement(rect, index, floatPropertyList),
            };
            colorPropertyList = new ReorderableList(serializedObject, m_ColorProperties)
            {
                drawHeaderCallback = (rect) => DrawPropertyListHeader(rect, colorPropertyList),
                drawElementCallback = (rect, index, isActive, isFocused) => DrawPropertyListElement(rect, index, colorPropertyList),
            };
            vectorPropertyList = new ReorderableList(serializedObject, m_VectorProperties)
            {
                drawHeaderCallback = (rect) => DrawPropertyListHeader(rect, vectorPropertyList),
                drawElementCallback = (rect, index, isActive, isFocused) => DrawPropertyListElement(rect, index, vectorPropertyList),
            };
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EZEditorGUIUtility.ScriptableObjectTitle(target as ScriptableObject, false);

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
