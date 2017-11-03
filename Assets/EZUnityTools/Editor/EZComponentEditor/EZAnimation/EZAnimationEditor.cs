/*
 * Author:      熊哲
 * CreateTime:  10/31/2017 5:30:50 PM
 * Description:
 * 
*/
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZComponentEditor.EZAnimation
{
    public class EZAnimationEditor : Editor
    {
        protected SerializedProperty m_Loop;
        protected SerializedProperty m_RestartOnEnable;
        protected SerializedProperty m_PhaseList;
        protected ReorderableList phaseList;

        protected float height = EditorGUIUtility.singleLineHeight;

        protected virtual void OnEnable()
        {
            m_Loop = serializedObject.FindProperty("m_Loop");
            m_RestartOnEnable = serializedObject.FindProperty("m_RestartOnEnable");
            m_PhaseList = serializedObject.FindProperty("m_PhaseList");
            phaseList = new ReorderableList(serializedObject, m_PhaseList, true, true, true, true);
            phaseList.drawHeaderCallback = DrawPhaseListHeader;
            phaseList.drawElementCallback = DrawPhaseListElement;
        }

        protected virtual void DrawPhaseListHeader(Rect rect)
        {
            rect.x += 15; rect.y += 1; rect.width -= 15;
            float width = rect.width / 6;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width * 2 - 5, height), "Start Value");
            rect.x += width * 2;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width * 2 - 5, height), "End Value");
            rect.x += width * 2;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width - 5, height), "Duration");
            rect.x += width;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width - 5, height), "Curve");
        }

        protected virtual void DrawPhaseListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty phase = phaseList.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty startValue = phase.FindPropertyRelative("m_StartValue");
            SerializedProperty endValue = phase.FindPropertyRelative("m_EndValue");
            SerializedProperty duration = phase.FindPropertyRelative("m_Duration");
            SerializedProperty curve = phase.FindPropertyRelative("m_Curve");

            rect.y += 1; float width = rect.width / 6;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width * 2 - 5, height), startValue, GUIContent.none);
            rect.x += width * 2;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width * 2 - 5, height), endValue, GUIContent.none);
            rect.x += width * 2;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width - 5, height), duration, GUIContent.none);
            if (duration.floatValue <= 0) duration.floatValue = 0;
            rect.x += width;
            curve.animationCurveValue = EditorGUI.CurveField(new Rect(rect.x, rect.y, width - 5, height), curve.animationCurveValue, Color.green, new Rect(0, 0, duration.floatValue, 1));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(target as MonoBehaviour), typeof(MonoScript), false);
            GUI.enabled = true;
            EditorGUILayout.PropertyField(m_Loop);
            EditorGUILayout.PropertyField(m_RestartOnEnable);
            phaseList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}