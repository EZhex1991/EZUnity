/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-10-31 17:30:50
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZUnity.Animation
{
    public abstract class EZAnimationEditor : Editor
    {
        protected IEZAnimation animation;

        protected abstract string animationTargetPropertyName { get; }
        protected SerializedProperty m_Target;
        protected SerializedProperty m_Loop;
        protected SerializedProperty m_PlayOnAwake;
        protected SerializedProperty m_RestartOnEnable;
        protected SerializedProperty m_UpdateMode;
        protected SerializedProperty m_Status;
        protected SerializedProperty m_Time;
        protected SerializedProperty m_Segments;
        protected ReorderableList segments;

        protected float horizontalSpace = EZEditorGUIUtility.space;
        protected float headerIndent = EZEditorGUIUtility.reorderableListHeaderIndent;
        protected float singleLineHeight = EditorGUIUtility.singleLineHeight;
        protected float verticalSpace = EditorGUIUtility.standardVerticalSpacing;

        protected virtual void OnEnable()
        {
            animation = target as IEZAnimation;

            m_Target = serializedObject.FindProperty(animationTargetPropertyName);
            m_Loop = serializedObject.FindProperty("m_Loop");
            m_PlayOnAwake = serializedObject.FindProperty("m_PlayOnAwake");
            m_RestartOnEnable = serializedObject.FindProperty("m_RestartOnEnable");
            m_UpdateMode = serializedObject.FindProperty("m_UpdateMode");
            m_Status = serializedObject.FindProperty("m_Status");
            m_Time = serializedObject.FindProperty("m_Time");
            m_Segments = serializedObject.FindProperty("m_Segments");
            segments = new ReorderableList(serializedObject, m_Segments, true, true, true, true)
            {
                drawHeaderCallback = DrawSegmentListHeader,
                elementHeightCallback = GetSegmentListElementHeight,
                drawElementCallback = DrawSegmentListElement,
            };
        }

        protected virtual void DrawSegmentListHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Segments");
        }
        protected virtual float GetSegmentListElementHeight(int index)
        {
            return singleLineHeight + verticalSpace;
        }
        protected virtual void DrawSegmentListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty segment = segments.serializedProperty.GetArrayElementAtIndex(index);
            rect = OnSegmentProperty(rect, segment);

            SerializedProperty duration = segment.FindPropertyRelative("m_Duration");
            SerializedProperty curve = segment.FindPropertyRelative("m_Curve");
            rect.y += 1;
            rect.height = singleLineHeight;
            float width = rect.width / 4;
            rect.width = width - horizontalSpace;
            EditorGUI.LabelField(rect, "Duration");
            rect.x += width; rect.width = width - horizontalSpace;
            EditorGUI.PropertyField(rect, duration, GUIContent.none);
            rect.x += width; rect.width = width * 2 - horizontalSpace;
            Color curveColor = animation.segmentIndex == index ? Color.red : Color.green;
            Rect curveRect = new Rect(0, 0, duration.floatValue, 1);
            curve.animationCurveValue = EditorGUI.CurveField(rect, curve.animationCurveValue, curveColor, curveRect);
        }
        protected virtual Rect OnSegmentProperty(Rect rect, SerializedProperty segment)
        {
            return rect;
        }

        public sealed override void OnInspectorGUI()
        {
            serializedObject.Update();
            EZEditorGUIUtility.MonoBehaviourTitle(target as MonoBehaviour);
            DrawController();
            EditorGUILayout.PropertyField(m_Target);
            EditorGUILayout.PropertyField(m_Loop);
            EditorGUILayout.PropertyField(m_PlayOnAwake);
            EditorGUILayout.PropertyField(m_RestartOnEnable);
            EditorGUILayout.PropertyField(m_UpdateMode);
            EditorGUILayout.PropertyField(m_Time);
            DrawOtherProperties();
            segments.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
        public virtual void DrawController()
        {
            GUI.enabled = Application.isPlaying;
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Play"))
                {
                    if (animation.status == Status.Stopped)
                        animation.Play();
                    else if (animation.status == Status.Paused)
                        animation.Resume();
                }
                if (GUILayout.Button("Pause"))
                {
                    animation.Pause();
                }
                if (GUILayout.Button("Stop"))
                {
                    animation.Stop();
                }
                GUILayout.EndHorizontal();
            }
            EditorGUILayout.PropertyField(m_Status);
            GUI.enabled = true;
        }
        public virtual void DrawOtherProperties()
        {

        }
    }
}