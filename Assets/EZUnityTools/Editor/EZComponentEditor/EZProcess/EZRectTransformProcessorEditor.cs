/*
 * Author:      熊哲
 * CreateTime:  9/20/2017 3:17:18 PM
 * Description:
 * 
*/
using EZComponent.EZProcess;
using UnityEditor;
using UnityEngine;

namespace EZComponentEditor.EZProcess
{
    [CustomEditor(typeof(EZRectTransformProcessor)), CanEditMultipleObjects]
    public class EZRectTransformProcessorEditor : _EZProcessEditor
    {
        private SerializedProperty m_DrivePosition;
        private SerializedProperty m_DriveSize;
        private SerializedProperty m_DriveRotation;

        private bool originFoldout = true;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_DrivePosition = serializedObject.FindProperty("m_DrivePosition");
            m_DriveSize = serializedObject.FindProperty("m_DriveSize");
            m_DriveRotation = serializedObject.FindProperty("m_DriveRotation");
            phaseList.elementHeightCallback = GetPhaseListElementHeight;
        }

        private int GetLineCount()
        {
            int line = 0;
            if (m_DrivePosition.boolValue) line++;
            if (m_DriveSize.boolValue) line++;
            if (m_DriveRotation.boolValue) line++;
            return line == 0 ? 1 : line;
        }
        private float GetPhaseListElementHeight(int index)
        {
            return GetLineCount() * phaseList.elementHeight;
        }
        protected override void DrawPhaseListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty phase = phaseList.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty endValue = phase.FindPropertyRelative("m_EndValue");
            SerializedProperty duration = phase.FindPropertyRelative("m_Duration");
            SerializedProperty interval = phase.FindPropertyRelative("m_Interval");
            SerializedProperty lerpMode = phase.FindPropertyRelative("m_LerpMode");

            rect.y += 1; float x = rect.x; float y = rect.y;
            if (m_DrivePosition.boolValue)
            {
                EditorGUI.LabelField(new Rect(x, y, 20, height), "P.");
                x += indent; y += rect.height;
            }
            if (m_DriveSize.boolValue)
            {
                EditorGUI.LabelField(new Rect(x, y, 20, height), "S.");
                x += indent; y += rect.height;
            }
            if (m_DriveRotation.boolValue)
            {
                EditorGUI.LabelField(new Rect(x, y, 20, height), "R.");
                x += indent; y += rect.height;
            }
            rect.x += 25; x = rect.x; y = rect.y;
            float width1 = (rect.width - 20) / 2; float width2 = width1 / 4;
            if (m_DrivePosition.boolValue)
            {
                EditorGUI.PropertyField(new Rect(x, y, width1 - 5, height), endValue.FindPropertyRelative("m_AnchoredPosition"), GUIContent.none);
                x += indent; y += rect.height;
            }
            if (m_DriveSize.boolValue)
            {
                EditorGUI.PropertyField(new Rect(x, y, width1 - 5, height), endValue.FindPropertyRelative("m_SizeDelta"), GUIContent.none);
                x += indent; y += rect.height;
            }
            if (m_DriveRotation.boolValue)
            {
                EditorGUI.PropertyField(new Rect(x, y, width1 - 5, height), endValue.FindPropertyRelative("m_Rotation"), GUIContent.none);
                x += indent; y += rect.height;
            }
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
            originFoldout = EditorGUILayout.Foldout(originFoldout, "Driver and Origin");
            if (originFoldout)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(m_DrivePosition);
                if (m_DrivePosition.boolValue) EditorGUILayout.PropertyField(m_Origin.FindPropertyRelative("m_AnchoredPosition"), GUIContent.none);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(m_DriveSize);
                if (m_DriveSize.boolValue) EditorGUILayout.PropertyField(m_Origin.FindPropertyRelative("m_SizeDelta"), GUIContent.none);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(m_DriveRotation);
                if (m_DriveRotation.boolValue) EditorGUILayout.PropertyField(m_Origin.FindPropertyRelative("m_Rotation"), GUIContent.none);
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }
            phaseList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}