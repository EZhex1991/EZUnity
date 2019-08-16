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
    [CustomEditor(typeof(EZMaterialPropertyBlockClip))]
    public class EZMaterialPropertyBlockClipEditor : Editor
    {
        private SerializedProperty m_Template;
        private SerializedProperty m_FloatRanges;
        private SerializedProperty m_ColorRanges;
        private SerializedProperty m_VectorRanges;
        private SerializedProperty m_Curve;
        private ReorderableList floatRangeList;
        private ReorderableList colorRangeList;
        private ReorderableList vectorRangeList;

        private void OnEnable()
        {
            m_Template = serializedObject.FindProperty("template");
            m_FloatRanges = m_Template.FindPropertyRelative("floatRanges");
            m_ColorRanges = m_Template.FindPropertyRelative("colorRanges");
            m_VectorRanges = m_Template.FindPropertyRelative("vectorRanges");
            m_Curve = m_Template.FindPropertyRelative("curve");
            floatRangeList = new ReorderableList(serializedObject, m_FloatRanges)
            {
                drawHeaderCallback = (rect) => DrawPropertyListHeader(rect, floatRangeList),
                drawElementCallback = (rect, index, isActive, isFocused) => DrawPropertyListElement(rect, m_FloatRanges, index, isActive, isFocused),
            };
            colorRangeList = new ReorderableList(serializedObject, m_ColorRanges)
            {
                drawHeaderCallback = (rect) => DrawPropertyListHeader(rect, colorRangeList),
                drawElementCallback = (rect, index, isActive, isFocused) => DrawPropertyListElement(rect, m_ColorRanges, index, isActive, isFocused),
            };
            vectorRangeList = new ReorderableList(serializedObject, m_VectorRanges)
            {
                drawHeaderCallback = (rect) => DrawPropertyListHeader(rect, vectorRangeList),
                drawElementCallback = (rect, index, isActive, isFocused) => DrawPropertyListElement(rect, m_VectorRanges, index, isActive, isFocused),
            };
        }

        private void DrawPropertyListHeader(Rect rect, ReorderableList list)
        {
            rect = EZEditorGUIUtility.CalcReorderableListHeaderRect(rect, list);
            float margin = 2;
            float width = (rect.width - margin * 2) / 3;
            rect.width = width;
            EditorGUI.LabelField(rect, "PropertyName");
            rect.x += width + margin;
            EditorGUI.LabelField(rect, "StartValue");
            rect.x += width + margin;
            EditorGUI.LabelField(rect, "EndValue");
        }
        private void DrawPropertyListElement(Rect rect, SerializedProperty listProperty, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = listProperty.GetArrayElementAtIndex(index);
            SerializedProperty propertyName = element.FindPropertyRelative("propertyName");
            SerializedProperty startValue = element.FindPropertyRelative("startValue");
            SerializedProperty endValue = element.FindPropertyRelative("endValue");

            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, listProperty, index);
            float margin = 2;
            float width = (rect.width - margin * 2) / 3;
            rect.width = width;
            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect, propertyName, GUIContent.none);
            rect.x += width + margin;
            EditorGUI.PropertyField(rect, startValue, GUIContent.none);
            rect.x += width + margin;
            EditorGUI.PropertyField(rect, endValue, GUIContent.none);
        }

        public override void OnInspectorGUI()
        {
            EZEditorGUIUtility.ScriptableObjectTitle(target as ScriptableObject, false);
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_Curve);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Float Ranges", EditorStyles.boldLabel);
            floatRangeList.DoLayoutList();
            EditorGUILayout.LabelField("Color Ranges", EditorStyles.boldLabel);
            colorRangeList.DoLayoutList();
            EditorGUILayout.LabelField("Vector Ranges", EditorStyles.boldLabel);
            vectorRangeList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }

    }
}
