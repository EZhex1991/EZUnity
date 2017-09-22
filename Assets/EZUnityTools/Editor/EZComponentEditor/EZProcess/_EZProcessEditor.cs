/*
 * Author:      熊哲
 * CreateTime:  9/12/2017 2:21:43 PM
 * Description:
 * 
*/
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZComponentEditor.EZProcess
{
    public abstract class _EZProcessEditor : Editor
    {
        protected SerializedProperty m_Loop;
        protected SerializedProperty m_RestartOnEnable;
        protected SerializedProperty m_StartFromOrigin;
        protected SerializedProperty m_Origin;
        protected SerializedProperty m_PhaseList;
        protected ReorderableList phaseList;

        protected float indent = 20;
        protected float height = EditorGUIUtility.singleLineHeight;

        protected virtual void OnEnable()
        {
            m_Loop = serializedObject.FindProperty("m_Loop");
            m_RestartOnEnable = serializedObject.FindProperty("m_RestartOnEnable");
            m_StartFromOrigin = serializedObject.FindProperty("m_StartFromOrigin");
            m_Origin = serializedObject.FindProperty("m_Origin");
            m_PhaseList = serializedObject.FindProperty("m_PhaseList");
            phaseList = new ReorderableList(serializedObject, m_PhaseList, true, true, true, true);
            phaseList.drawHeaderCallback = DrawPhaseListHeader;
            phaseList.drawElementCallback = DrawPhaseListElement;
        }

        protected virtual void DrawPhaseListHeader(Rect rect)
        {
            rect.y += 1;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, 20, height), "NO.");
            rect.x += 40; rect.width -= 15;
            float width1 = (rect.width - 20) / 2; float width2 = width1 / 4;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width1 - 5, height), "End Value");
            rect.x += width1;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width2 - 5, height), "Duration");
            rect.x += width2;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width2 - 5, height), "interval");
            rect.x += width2;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width2 * 2 - 5, height), "Lerp Mode");
        }

        protected virtual void DrawPhaseListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty phase = phaseList.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty endValue = phase.FindPropertyRelative("m_EndValue");
            SerializedProperty duration = phase.FindPropertyRelative("m_Duration");
            SerializedProperty interval = phase.FindPropertyRelative("m_Interval");
            SerializedProperty lerpMode = phase.FindPropertyRelative("m_LerpMode");

            rect.y += 1;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, 20, height), index.ToString("00"));
            rect.x += 25;
            float width1 = (rect.width - 20) / 2; float width2 = width1 / 4;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width1 - 5, height), endValue, GUIContent.none);
            rect.x += width1;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width2 - 5, height), duration, GUIContent.none);
            rect.x += width2;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width2 - 5, height), interval, GUIContent.none);
            rect.x += width2;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width2 * 2 - 5, height), lerpMode, GUIContent.none);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(target as MonoBehaviour), typeof(MonoScript), false);
            GUI.enabled = true;
            EditorGUILayout.PropertyField(m_Loop);
            EditorGUILayout.PropertyField(m_RestartOnEnable);
            EditorGUILayout.PropertyField(m_StartFromOrigin);
            EditorGUILayout.PropertyField(m_Origin);
            phaseList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}