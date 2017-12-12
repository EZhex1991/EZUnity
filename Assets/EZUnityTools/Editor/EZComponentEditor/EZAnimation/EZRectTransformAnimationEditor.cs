/*
 * Author:      熊哲
 * CreateTime:  11/2/2017 5:30:41 PM
 * Description:
 * 
*/
using EZComponent.EZAnimation;
using UnityEditor;
using UnityEngine;

namespace EZComponentEditor.EZAnimation
{
    [CustomEditor(typeof(EZRectTransformAnimation), true), CanEditMultipleObjects]
    public class EZRectTransformAnimationEditor : EZAnimationEditor
    {
        private SerializedProperty m_PositionDriver;
        private SerializedProperty m_SizeDriver;
        private SerializedProperty m_RotationDriver;
        private SerializedProperty m_ScaleDriver;

        private bool drivePosition;
        private bool driveSize;
        private bool driveRotation;
        private bool driveScale;

        private bool originFoldout = true;

        private Color positionColor = Color.red;
        private Color sizeColor = Color.yellow;
        private Color rotationColor = Color.green;
        private Color scaleColor = Color.blue;

        protected override void OnEnable()
        {
            m_PositionDriver = serializedObject.FindProperty("m_PositionDriver");
            m_SizeDriver = serializedObject.FindProperty("m_SizeDriver");
            m_RotationDriver = serializedObject.FindProperty("m_RotationDriver");
            m_ScaleDriver = serializedObject.FindProperty("m_ScaleDriver");
            base.OnEnable();
            phaseList.elementHeightCallback = GetPhaseListElementHeight;
        }

        protected override void DrawPhaseListHeader(Rect rect)
        {
            rect.x += headerIndent; rect.y += 1; rect.width -= headerIndent;
            float width = rect.width / 2;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width - space, lineHeight), "Start Value");
            rect.x += width;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width - space, lineHeight), "End Value");
        }

        private void GetDriver()
        {
            drivePosition = m_PositionDriver.FindPropertyRelative("x").boolValue
                || m_PositionDriver.FindPropertyRelative("y").boolValue;
            driveSize = m_SizeDriver.FindPropertyRelative("x").boolValue
                || m_SizeDriver.FindPropertyRelative("y").boolValue;
            driveRotation = m_RotationDriver.FindPropertyRelative("x").boolValue
                || m_RotationDriver.FindPropertyRelative("y").boolValue
                || m_RotationDriver.FindPropertyRelative("z").boolValue;
            driveScale = m_ScaleDriver.FindPropertyRelative("x").boolValue
                || m_ScaleDriver.FindPropertyRelative("y").boolValue
                || m_ScaleDriver.FindPropertyRelative("z").boolValue;
        }
        private int GetLineCount()
        {
            int line = 1;
            if (drivePosition) line++;
            if (driveSize) line++;
            if (driveRotation) line++;
            if (driveScale) line++;
            return line;
        }
        private float GetPhaseListElementHeight(int index)
        {
            GetDriver();
            return GetLineCount() * phaseList.elementHeight;
        }
        protected override void DrawPhaseListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            Color curveColor = anim.currentIndex == index ? Color.red : Color.green;

            SerializedProperty phase = phaseList.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty startValue = phase.FindPropertyRelative("m_StartValue");
            SerializedProperty endValue = phase.FindPropertyRelative("m_EndValue");
            SerializedProperty duration = phase.FindPropertyRelative("m_Duration");
            SerializedProperty curve = phase.FindPropertyRelative("m_Curve");

            rect.y += 1; float width = rect.width / 2;

            float x1 = rect.x, x2 = rect.x + width, space = 10;
            Color bgColor = GUI.backgroundColor;
            if (drivePosition)
            {
                GUI.backgroundColor = positionColor;
                EditorGUI.PropertyField(new Rect(x1, rect.y, width - space, lineHeight), startValue.FindPropertyRelative("m_AnchoredPosition"), GUIContent.none);
                EditorGUI.PropertyField(new Rect(x2, rect.y, width - space, lineHeight), endValue.FindPropertyRelative("m_AnchoredPosition"), GUIContent.none);
                rect.y += rect.height;
            }
            if (driveSize)
            {
                GUI.backgroundColor = sizeColor;
                EditorGUI.PropertyField(new Rect(x1, rect.y, width - space, lineHeight), startValue.FindPropertyRelative("m_SizeDelta"), GUIContent.none);
                EditorGUI.PropertyField(new Rect(x2, rect.y, width - space, lineHeight), endValue.FindPropertyRelative("m_SizeDelta"), GUIContent.none);
                rect.y += rect.height;
            }
            if (driveRotation)
            {
                GUI.backgroundColor = rotationColor;
                EditorGUI.PropertyField(new Rect(x1, rect.y, width - space, lineHeight), startValue.FindPropertyRelative("m_Rotation"), GUIContent.none);
                EditorGUI.PropertyField(new Rect(x2, rect.y, width - space, lineHeight), endValue.FindPropertyRelative("m_Rotation"), GUIContent.none);
                rect.y += rect.height;
            }
            if (driveScale)
            {
                GUI.backgroundColor = scaleColor;
                EditorGUI.PropertyField(new Rect(x1, rect.y, width - space, lineHeight), startValue.FindPropertyRelative("m_Scale"), GUIContent.none);
                EditorGUI.PropertyField(new Rect(x2, rect.y, width - space, lineHeight), endValue.FindPropertyRelative("m_Scale"), GUIContent.none);
                rect.y += rect.height;
            }
            GUI.backgroundColor = bgColor;

            float labelWidth = 60; float propertyWidth = width - labelWidth - space;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, labelWidth, lineHeight), "Duration");
            EditorGUI.PropertyField(new Rect(rect.x + labelWidth, rect.y, propertyWidth, lineHeight), duration, GUIContent.none);
            rect.x += width;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, labelWidth, lineHeight), "Curve");
            curve.animationCurveValue = EditorGUI.CurveField(new Rect(rect.x + labelWidth, rect.y, propertyWidth, lineHeight), curve.animationCurveValue, curveColor, new Rect(0, 0, duration.floatValue, 1));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EZUnityEditor.EZEditorGUIUtility.ScriptTitle(target);
            EditorGUILayout.PropertyField(m_Loop);
            EditorGUILayout.PropertyField(m_RestartOnEnable);
            originFoldout = EditorGUILayout.Foldout(originFoldout, "Driver");
            if (originFoldout)
            {
                EditorGUI.indentLevel++; Color bgColor = GUI.color;
                GUI.color = positionColor;
                EditorGUILayout.PropertyField(m_PositionDriver);
                GUI.color = sizeColor;
                EditorGUILayout.PropertyField(m_SizeDriver);
                GUI.color = rotationColor;
                EditorGUILayout.PropertyField(m_RotationDriver);
                GUI.color = scaleColor;
                EditorGUILayout.PropertyField(m_ScaleDriver);
                EditorGUI.indentLevel--; GUI.color = bgColor;
            }
            phaseList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}