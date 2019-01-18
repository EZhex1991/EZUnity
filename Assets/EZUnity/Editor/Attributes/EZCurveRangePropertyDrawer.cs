/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-12-24 14:28:13
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZUnity
{
    [CustomPropertyDrawer(typeof(EZCurveRangeAttribute))]
    public class EZCurveRangePropertyDrawer : PropertyDrawer
    {
        private EZCurveRangeAttribute curveRangeAttribute { get { return attribute as EZCurveRangeAttribute; } }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.CurveField(position, property, curveRangeAttribute.color, curveRangeAttribute.range, label);
            EditorGUI.EndProperty();
        }
    }
}
