/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-06-28 19:19:43
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CustomPropertyDrawer(typeof(EZMinMaxSliderAttribute))]
    public class EZMinMaxSliderDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EZMinMaxSliderAttribute range = attribute as EZMinMaxSliderAttribute;

            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            Vector2 value = property.vector2Value;

            float width = position.width / 4;
            position.width = width - 5f;
            value.x = EditorGUI.FloatField(position, value.x);
            position.x += width;
            position.width = width * 2;
            EditorGUI.MinMaxSlider(position, ref value.x, ref value.y, range.min, range.max);
            position.x += position.width + 5f;
            position.width = width;
            value.y = EditorGUI.FloatField(position, value.y);

            value.x = Mathf.Clamp(value.x, range.min, range.max);
            value.y = Mathf.Clamp(value.y, value.x, range.max);
            property.vector2Value = value;
            EditorGUI.EndProperty();
        }
    }
}
