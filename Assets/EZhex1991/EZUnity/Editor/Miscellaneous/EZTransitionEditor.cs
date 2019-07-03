/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-28-28 10:55:35
 * Organization:    #ORGANIZATION#
 * Description:     自定义Selectable的Transition（依赖Selectable而不是继承Selectable）
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CustomEditor(typeof(EZTransition)), CanEditMultipleObjects]
    public class EZTransitionEditor : Editor
    {
        SerializedProperty m_TransitionType;
        SerializedProperty m_RectTransform;
        SerializedProperty m_Outline;
        SerializedProperty m_ScaleState;
        SerializedProperty m_SizeState;
        SerializedProperty m_OutlineDistanceState;
        SerializedProperty m_OutlineColorState;

        void OnEnable()
        {
            m_TransitionType = serializedObject.FindProperty("m_TransitionType");
            m_RectTransform = serializedObject.FindProperty("m_RectTransform");
            m_Outline = serializedObject.FindProperty("m_Outline");
            m_ScaleState = serializedObject.FindProperty("m_ScaleState");
            m_SizeState = serializedObject.FindProperty("m_SizeState");
            m_OutlineDistanceState = serializedObject.FindProperty("m_OutlineDistanceState");
            m_OutlineColorState = serializedObject.FindProperty("m_OutlineColorState");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EZEditorGUIUtility.MonoBehaviourTitle(target as MonoBehaviour);

            EditorGUILayout.PropertyField(m_TransitionType);
            EditorGUI.indentLevel++;
            switch (m_TransitionType.enumValueIndex)
            {
                case (int)EZTransition.TransitionType.None:
                    break;
                case (int)EZTransition.TransitionType.Scale:
                    EditorGUILayout.PropertyField(m_RectTransform);
                    EditorGUILayout.PropertyField(m_ScaleState.FindPropertyRelative("m_NormalScale"));
                    EditorGUILayout.PropertyField(m_ScaleState.FindPropertyRelative("m_HighlightedScale"));
                    EditorGUILayout.PropertyField(m_ScaleState.FindPropertyRelative("m_PressedScale"));
                    EditorGUILayout.PropertyField(m_ScaleState.FindPropertyRelative("m_DisabledScale"));
                    break;
                case (int)EZTransition.TransitionType.Size:
                    EditorGUILayout.PropertyField(m_RectTransform);
                    EditorGUILayout.PropertyField(m_SizeState.FindPropertyRelative("m_NormalSize"));
                    EditorGUILayout.PropertyField(m_SizeState.FindPropertyRelative("m_HighlightedSize"));
                    EditorGUILayout.PropertyField(m_SizeState.FindPropertyRelative("m_PressedSize"));
                    EditorGUILayout.PropertyField(m_SizeState.FindPropertyRelative("m_DisabledSize"));
                    break;
                case (int)EZTransition.TransitionType.OutlineDistance:
                    EditorGUILayout.PropertyField(m_Outline);
                    EditorGUILayout.PropertyField(m_OutlineDistanceState.FindPropertyRelative("m_NormalDistance"));
                    EditorGUILayout.PropertyField(m_OutlineDistanceState.FindPropertyRelative("m_HighlightedDistance"));
                    EditorGUILayout.PropertyField(m_OutlineDistanceState.FindPropertyRelative("m_PressedDistance"));
                    EditorGUILayout.PropertyField(m_OutlineDistanceState.FindPropertyRelative("m_DisabledDistance"));
                    break;
                case (int)EZTransition.TransitionType.OutlineColor:
                    EditorGUILayout.PropertyField(m_Outline);
                    EditorGUILayout.PropertyField(m_OutlineColorState.FindPropertyRelative("m_NormalColor"));
                    EditorGUILayout.PropertyField(m_OutlineColorState.FindPropertyRelative("m_HighlightedColor"));
                    EditorGUILayout.PropertyField(m_OutlineColorState.FindPropertyRelative("m_PressedColor"));
                    EditorGUILayout.PropertyField(m_OutlineColorState.FindPropertyRelative("m_DisabledColor"));
                    break;
            }
            EditorGUI.indentLevel--;

            serializedObject.ApplyModifiedProperties();
        }
    }
}