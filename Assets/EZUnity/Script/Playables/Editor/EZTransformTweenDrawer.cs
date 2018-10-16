/* Author:          熊哲
 * CreateTime:      2018-08-14 11:02:22
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZUnity.Playables
{
    [CustomPropertyDrawer(typeof(EZTransformTweenBehaviour))]
    public class EZTransformTweenDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            SerializedProperty tweenPosition = property.FindPropertyRelative("tweenPosition");
            SerializedProperty tweenRotation = property.FindPropertyRelative("tweenRotation");
            SerializedProperty tweenScale = property.FindPropertyRelative("tweenScale");
            SerializedProperty curve = property.FindPropertyRelative("curve");
            SerializedProperty startEulerAngles = property.FindPropertyRelative("startEulerAngles");
            SerializedProperty endEulerAngles = property.FindPropertyRelative("endEulerAngles");

            EditorGUI.indentLevel++;
            Rect rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            rect.y += rect.height;
            EditorGUI.PropertyField(rect, tweenPosition);
            rect.y += rect.height;
            EditorGUI.PropertyField(rect, tweenRotation);
            rect.y += rect.height;
            EditorGUI.PropertyField(rect, tweenScale);
            rect.y += rect.height;
            EditorGUI.PropertyField(rect, startEulerAngles);
            rect.y += rect.height;
            EditorGUI.PropertyField(rect, endEulerAngles);
            rect.y += rect.height;
            EditorGUI.CurveField(rect, curve, Color.green, new Rect(0, 0, 1, 1));
            EditorGUI.EndProperty();
            EditorGUI.indentLevel--;
        }
    }
}
