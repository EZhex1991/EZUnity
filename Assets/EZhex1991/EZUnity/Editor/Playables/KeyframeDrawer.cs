/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-04-12 12:55:43
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity.Playables
{
    public class KeyframeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            // base.OnGUI(position, property, label);
            EditorGUI.BeginProperty(rect, label, property);
            rect = EditorGUI.PrefixLabel(rect, GUIUtility.GetControlID(FocusType.Passive), label);

            float width = rect.width / 2; float margin = 5;
            float labelWidth = EditorGUIUtility.labelWidth; EditorGUIUtility.labelWidth = width / 2;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width - margin, rect.height), property.FindPropertyRelative("m_Time"));
            rect.x += width;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width - margin, rect.height), property.FindPropertyRelative("m_Value"));
            EditorGUIUtility.labelWidth = labelWidth;

            EditorGUI.EndProperty();
        }
    }

    [CustomPropertyDrawer(typeof(FloatKeyframe))]
    public class FloatKeyframeDrawer : KeyframeDrawer
    {

    }

    [CustomPropertyDrawer(typeof(Vector3Keyframe))]
    public class Vector3KeyframeDrawer : KeyframeDrawer
    {

    }
}
