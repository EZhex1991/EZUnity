/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-12-11 17:33:21
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZUnity.Animation
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

    [CustomPropertyDrawer(typeof(V3Driver))]
    public class V3DriverDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            int indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float width = position.width / 3; float labelWidth = 20;
            EditorGUI.LabelField(new Rect(position.x, position.y, labelWidth, position.height), "X");
            EditorGUI.PropertyField(new Rect(position.x + labelWidth, position.y, width - labelWidth - 5, position.height), property.FindPropertyRelative("x"), GUIContent.none);
            position.x += width;
            EditorGUI.LabelField(new Rect(position.x, position.y, labelWidth, position.height), "Y");
            EditorGUI.PropertyField(new Rect(position.x + labelWidth, position.y, width - labelWidth - 5, position.height), property.FindPropertyRelative("y"), GUIContent.none);
            position.x += width;
            EditorGUI.LabelField(new Rect(position.x, position.y, labelWidth, position.height), "Z");
            EditorGUI.PropertyField(new Rect(position.x + labelWidth, position.y, width - labelWidth - 5, position.height), property.FindPropertyRelative("z"), GUIContent.none);

            EditorGUI.indentLevel = indentLevel;
            EditorGUI.EndProperty();
        }
    }
}