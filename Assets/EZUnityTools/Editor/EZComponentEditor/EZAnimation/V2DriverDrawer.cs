/*
 * Author:      熊哲
 * CreateTime:  12/11/2017 5:33:21 PM
 * Description:
 * 
*/
using EZComponent.EZAnimation;
using UnityEditor;
using UnityEngine;

namespace EZComponentEditor.EZAnimation
{
    [CustomPropertyDrawer(typeof(V2Driver))]
    public class V2DriverDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            int indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float width = position.width / 2; float labelWidth = 20;
            EditorGUI.LabelField(new Rect(position.x, position.y, labelWidth, position.height), "X");
            EditorGUI.PropertyField(new Rect(position.x + labelWidth, position.y, width - labelWidth - 5, position.height), property.FindPropertyRelative("x"), GUIContent.none);
            position.x += width;
            EditorGUI.LabelField(new Rect(position.x, position.y, labelWidth, position.height), "Y");
            EditorGUI.PropertyField(new Rect(position.x + labelWidth, position.y, width - labelWidth - 5, position.height), property.FindPropertyRelative("y"), GUIContent.none);

            EditorGUI.indentLevel = indentLevel;
            EditorGUI.EndProperty();
        }
    }
}