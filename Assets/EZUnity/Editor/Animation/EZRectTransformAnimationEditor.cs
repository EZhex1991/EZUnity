/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-11-02 17:30:41
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZUnity.Animation
{
    [CustomEditor(typeof(EZRectTransformAnimation), true), CanEditMultipleObjects]
    public class EZRectTransformAnimationEditor : EZAnimationEditor
    {
        protected override string animationTargetPropertyName => "m_RectTransform";

        protected override void DrawSegmentListHeader(Rect rect)
        {
            rect.x += headerIndent; rect.width -= headerIndent;
            float width = rect.width / 2; rect.width -= horizontalSpace;
            EditorGUI.LabelField(rect, "Start Rect");
            rect.x += width;
            EditorGUI.LabelField(rect, "End Rect");
        }
        protected override float GetSegmentListElementHeight(int index)
        {
            return base.GetSegmentListElementHeight(index) * 2;
        }
        protected override Rect OnSegmentProperty(Rect rect, SerializedProperty segment)
        {
            SerializedProperty startRect = segment.FindPropertyRelative("m_StartRect");
            SerializedProperty endRect = segment.FindPropertyRelative("m_EndRect");
            float width = rect.width / 2;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width - horizontalSpace, singleLineHeight), startRect, GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + width, rect.y, width - horizontalSpace, singleLineHeight), endRect, GUIContent.none);
            rect.y += singleLineHeight + verticalSpace;
            rect.height /= 2;
            return rect;
        }
    }
}