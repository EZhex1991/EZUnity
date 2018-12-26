/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-12-24 14:28:13
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZUnity
{
    [CustomPropertyDrawer(typeof(EZCurveAttribute))]
    public class EZCurveRangePropertyDrawer : PropertyDrawer
    {
        private EZCurveAttribute curveAttribute { get { return attribute as EZCurveAttribute; } }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.CurveField(position, property, curveAttribute.color, curveAttribute.range, label);
            EditorGUI.EndProperty();
        }
    }
}
