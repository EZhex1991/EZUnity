/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-11-02 17:30:26
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZUnity.Animation
{
    [CustomEditor(typeof(EZTransformAnimation), true), CanEditMultipleObjects]
    public class EZTransformAnimationEditor : EZAnimationEditor
    {
        protected override string animationTargetPropertyName => "m_TargetTransform";
        protected SerializedProperty m_PathMode;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_PathMode = serializedObject.FindProperty("m_PathMode");
        }
        public override void DrawOtherProperties()
        {
            EditorGUILayout.PropertyField(m_PathMode);
        }

        protected override void DrawSegmentListHeader(Rect rect)
        {
            rect.x += headerIndent; rect.width -= headerIndent;
            float width = rect.width / 2; rect.width -= horizontalSpace;
            EditorGUI.LabelField(rect, "Start Point");
            rect.x += width;
            EditorGUI.LabelField(rect, "End Point");
        }
        protected override float GetSegmentListElementHeight(int index)
        {
            return base.GetSegmentListElementHeight(index) * 2;
        }
        protected override Rect OnSegmentProperty(Rect rect, SerializedProperty segment)
        {
            SerializedProperty startPoint = segment.FindPropertyRelative("m_StartPoint");
            SerializedProperty endPoint = segment.FindPropertyRelative("m_EndPoint");
            float width = rect.width / 2;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width - horizontalSpace, singleLineHeight), startPoint, GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + width, rect.y, width - horizontalSpace, singleLineHeight), endPoint, GUIContent.none);
            rect.y += singleLineHeight + verticalSpace;
            rect.height /= 2;
            return rect;
        }

        private void OnSceneGUI()
        {
            EZTransformAnimation animation = target as EZTransformAnimation;
            Handles.color = Color.gray;
            if (animation.pathMode == EZTransformAnimation.PathMode.Bezier)
            {
                for (int i = 0; i < animation.segments.Count; i++)
                {
                    EZTransformAnimationSegment seg = animation.segments[i];
                    if (seg.startPoint != null && seg.endPoint != null)
                    {
                        Handles.color = Color.green;
                        Handles.matrix = seg.startPoint.localToWorldMatrix;
                        Vector3 startTangentPosition = Handles.FreeMoveHandle(seg.startTangent, Quaternion.identity, HandleUtility.GetHandleSize(seg.startTangent) * 0.15f, Vector3.zero, Handles.SphereHandleCap);
                        if (startTangentPosition != seg.startTangent)
                        {
                            Undo.RegisterCompleteObjectUndo(target, "Path Modify");
                            seg.startTangent = startTangentPosition;
                        }
                        Handles.DrawDottedLine(Vector3.zero, seg.startTangent, 1);

                        Handles.color = Color.red;
                        Handles.matrix = seg.endPoint.localToWorldMatrix;
                        Vector3 endTangentPosition = Handles.FreeMoveHandle(seg.endTangent, Quaternion.identity, HandleUtility.GetHandleSize(seg.endTangent) * 0.15f, Vector3.zero, Handles.SphereHandleCap);
                        if (endTangentPosition != seg.endTangent)
                        {
                            Undo.RegisterCompleteObjectUndo(target, "Path Modify");
                            seg.endTangent = endTangentPosition;
                        }
                        Handles.DrawDottedLine(Vector3.zero, seg.endTangent, 1);
                    }
                }
            }
        }
    }
}