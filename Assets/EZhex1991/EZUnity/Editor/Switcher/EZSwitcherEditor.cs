/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-03-21 19:49:17
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZSwitcherEditor : Editor
    {
        private SerializedProperty m_AllowSwitchOff;
        private SerializedProperty m_SwitchOnStart;
        private SerializedProperty m_Next;
        private SerializedProperty m_Options;
        private ReorderableList optionList;

        protected virtual void OnEnable()
        {
            m_AllowSwitchOff = serializedObject.FindProperty("m_AllowSwitchOff");
            m_SwitchOnStart = serializedObject.FindProperty("m_SwitchOnStart");
            m_Next = serializedObject.FindProperty("m_Next");
            m_Options = serializedObject.FindProperty("m_Options");
            optionList = new ReorderableList(serializedObject, m_Options, true, false, true, true);
            optionList.drawElementCallback = DrawPrefabListElement;
        }

        protected virtual void DrawPrefabListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect.y += 1;
            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, index, optionList);
            SerializedProperty prefab = optionList.serializedProperty.GetArrayElementAtIndex(index);
            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect, prefab, GUIContent.none);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EZEditorGUIUtility.MonoBehaviourTitle(target as MonoBehaviour);

            DrawOtherProperties();
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_AllowSwitchOff);
            EditorGUILayout.PropertyField(m_SwitchOnStart);
            EditorGUILayout.PropertyField(m_Next);
            optionList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
        protected virtual void DrawOtherProperties()
        {

        }
    }
}
