/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-06-14 20:11:56
 * Organization:    #ORGANIZATION#
 * Description:     
 * Role:             Source file for independent package copies.
 * Sync:             Changes to this file should be synchronized to the package copies.
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CustomPropertyDrawer(typeof(EZCurveRectAttribute))]
    public class EZCurveRectDrawer : PropertyDrawer
    {
        private EZCurveRectAttribute curveRectAttribute { get { return attribute as EZCurveRectAttribute; } }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            if (property.propertyType == SerializedPropertyType.AnimationCurve)
            {
                EditorGUI.CurveField(position, property, curveRectAttribute.color, curveRectAttribute.rect, label);
            }
            else
            {
                EditorGUI.HelpBox(position, typeof(EZCurveRectAttribute).Name + " used on a non-AnimationCurve field", MessageType.Warning);
            }
            EditorGUI.EndProperty();
        }
    }
}
