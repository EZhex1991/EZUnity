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
            EZMinMaxSliderAttribute minMaxSliderAttribute = attribute as EZMinMaxSliderAttribute;

            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            if (property.propertyType == SerializedPropertyType.Vector2)
            {
                property.vector2Value = MinMaxSliderV2(position, property.vector2Value, minMaxSliderAttribute.limitMin, minMaxSliderAttribute.limitMax);
            }
            else if (property.propertyType == SerializedPropertyType.Vector4)
            {
                if (minMaxSliderAttribute.fixedLimit)
                {
                    property.vector4Value = MinMaxSliderV4(position, property.vector4Value, minMaxSliderAttribute.limitMin, minMaxSliderAttribute.limitMax);
                }
                else
                {
                    property.isExpanded = EditorGUI.Foldout(new Rect(position) { width = 0 }, property.isExpanded, GUIContent.none, false);
                    if (property.isExpanded)
                    {
                        property.vector4Value = EditorGUI.Vector4Field(position, "", property.vector4Value);
                    }
                    else
                    {
                        property.vector4Value = MinMaxSliderV4(position, property.vector4Value);
                    }
                }
            }
            else
            {
                EditorGUI.HelpBox(position, "EZMinMaxSlider used on a non-vector2/vector4 property: " + property.name, MessageType.Warning);
            }

            EditorGUI.EndProperty();
        }
        private Vector2 MinMaxSliderV2(Rect position, Vector2 value, float limitMin, float limitMax)
        {
            float valueRectWidth = 50f;
            float margin = 5f;
            float sliderRectWidth = position.width - (valueRectWidth + margin) * 2f;

            position.width = valueRectWidth;
            value.x = EditorGUI.FloatField(position, value.x);

            position.x += valueRectWidth + margin;
            position.width = sliderRectWidth;
            EditorGUI.MinMaxSlider(position, ref value.x, ref value.y, limitMin, limitMax);

            position.x += sliderRectWidth + margin;
            position.width = valueRectWidth;
            value.y = EditorGUI.FloatField(position, value.y);

            value.x = Mathf.Clamp(value.x, limitMin, limitMax);
            value.y = Mathf.Clamp(value.y, value.x, limitMax);
            return value;
        }
        private Vector4 MinMaxSliderV4(Rect position, Vector4 value)
        {
            Vector2 valueXY = MinMaxSliderV2(position, value, value.z, value.w);
            value.x = valueXY.x;
            value.y = valueXY.y;
            return value;
        }
        private Vector4 MinMaxSliderV4(Rect position, Vector4 value, float limitMin, float limitMax)
        {
            Vector2 valueXY = MinMaxSliderV2(position, value, limitMin, limitMax);
            value.x = valueXY.x;
            value.y = valueXY.y;
            return value;
        }
    }
}
