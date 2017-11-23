/*
 * Author:      熊哲
 * CreateTime:  10/31/2017 5:30:50 PM
 * Description:
 * 
*/
using EZUnityEditor;
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

        protected float space = EZEditorGUIUtility.space;
        protected float headerIndent = EZEditorGUIUtility.reorderableListHeaderIndent;
        protected float lineHeight = EditorGUIUtility.singleLineHeight;

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
            rect.x += headerIndent; rect.y += 1; rect.width -= headerIndent;
            float width = rect.width / 6;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width * 2 - space, lineHeight), "Start Value");
            rect.x += width * 2;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width * 2 - space, lineHeight), "End Value");
            rect.x += width * 2;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width - space, lineHeight), "Duration");
            rect.x += width;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width - space, lineHeight), "Curve");
        }

        protected virtual void DrawPhaseListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty phase = phaseList.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty startValue = phase.FindPropertyRelative("m_StartValue");
            SerializedProperty endValue = phase.FindPropertyRelative("m_EndValue");
            SerializedProperty duration = phase.FindPropertyRelative("m_Duration");
            SerializedProperty curve = phase.FindPropertyRelative("m_Curve");

            rect.y += 1; float width = rect.width / 6;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width * 2 - space, lineHeight), startValue, GUIContent.none);
            rect.x += width * 2;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width * 2 - space, lineHeight), endValue, GUIContent.none);
            rect.x += width * 2;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width - space, lineHeight), duration, GUIContent.none);
            if (duration.floatValue <= 0) duration.floatValue = 0;
            rect.x += width;
            curve.animationCurveValue = EditorGUI.CurveField(new Rect(rect.x, rect.y, width - space, lineHeight), curve.animationCurveValue, Color.green, new Rect(0, 0, duration.floatValue, 1));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EZEditorGUIUtility.ScriptTitle(target);
            EditorGUILayout.PropertyField(m_Loop);
            EditorGUILayout.PropertyField(m_RestartOnEnable);
            phaseList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}