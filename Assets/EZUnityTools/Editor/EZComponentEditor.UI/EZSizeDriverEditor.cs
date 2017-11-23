/*
 * Author:      熊哲
 * CreateTime:  9/8/2017 10:48:54 AM
 * Description:
 * 
*/
using EZComponent.UI;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZComponentEditor.UI
{
    [CustomEditor(typeof(EZSizeDriver)), CanEditMultipleObjects]
    public class EZSizeDriverEditor : Editor
    {
        protected SerializedProperty m_Horizontal;
        protected SerializedProperty m_Vertical;
        protected SerializedProperty m_SlaveList;
        protected ReorderableList slaveList;

        void OnEnable()
        {
            m_Horizontal = serializedObject.FindProperty("m_Horizontal");
            m_Vertical = serializedObject.FindProperty("m_Vertical");
            m_SlaveList = serializedObject.FindProperty("m_SlaveList");
            slaveList = new ReorderableList(serializedObject, m_SlaveList, true, true, true, true);
            slaveList.drawHeaderCallback = DrawSlaveListHeader;
            slaveList.drawElementCallback = DrawSlaveListElement;
        }

        private void DrawSlaveListHeader(Rect rect)
        {
            rect.x += 15; rect.width -= 15;
            rect.y += 1; float height = EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, 20, height), "NO.");
            rect.x += 25;
            float width = (rect.width - 20) / 2;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width - 5, height), "Slave List");
            rect.x += width;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width - 5, height), "Size Offset");
        }

        protected void DrawSlaveListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty rt = slaveList.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty rectTransform = rt.FindPropertyRelative("rectTransform");
            SerializedProperty sizeOffset = rt.FindPropertyRelative("sizeOffset");

            rect.y += 1; float height = EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, 20, height), index.ToString("00"));
            rect.x += 25;
            float width = (rect.width - 20) / 2;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width - 5, height), rectTransform, GUIContent.none);
            rect.x += width;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width - 5, height), sizeOffset, GUIContent.none);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EZUnityEditor.EZEditorGUIUtility.ScriptTitle(target);
            EditorGUILayout.PropertyField(m_Horizontal);
            EditorGUILayout.PropertyField(m_Vertical);
            slaveList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}