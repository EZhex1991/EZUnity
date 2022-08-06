/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-06-28 19:19:43
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CustomPropertyDrawer(typeof(EZMinMaxAttribute))]
    public class EZMinMaxDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EZMinMaxAttribute minMaxAttribute = attribute as EZMinMaxAttribute;
            EditorGUI.BeginProperty(position, label, property);

            if (property.propertyType == SerializedPropertyType.Float)
            {
                property.floatValue = EditorGUILayout.Slider(label, property.floatValue, minMaxAttribute.limitMin, minMaxAttribute.limitMax);
            }
            else if (property.propertyType == SerializedPropertyType.Integer)
            {
                property.intValue = EditorGUILayout.IntSlider(label, property.intValue, (int)minMaxAttribute.limitMin, (int)minMaxAttribute.limitMax);
            }
            else if (property.propertyType == SerializedPropertyType.Vector2 || property.propertyType == SerializedPropertyType.Vector4)
            {
                position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

                if (property.propertyType == SerializedPropertyType.Vector2)
                {
                    property.vector2Value = EZEditorGUIUtility.MinMaxSliderV2(position, property.vector2Value, minMaxAttribute.limitMin, minMaxAttribute.limitMax);
                }
                else if (property.propertyType == SerializedPropertyType.Vector4)
                {
                    if (minMaxAttribute.fixedLimit)
                    {
                        property.vector4Value = EZEditorGUIUtility.MinMaxSliderV4(position, property.vector4Value, minMaxAttribute.limitMin, minMaxAttribute.limitMax);
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
                            property.vector4Value = EZEditorGUIUtility.MinMaxSliderV4(position, property.vector4Value);
                        }
                    }
                }
            }
            else
            {
                EditorGUI.HelpBox(position, string.Format("EZMinMaxAttribute not suitable for {0}: {1}", property.type, property.name), MessageType.Warning);
            }

            EditorGUI.EndProperty();
        }
    }
}
