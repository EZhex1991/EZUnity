
/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-13 16:10:35
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CustomPropertyDrawer(typeof(EZLockedFoldoutAttribute))]
    public class EZLockedFoldoutPropertyDrawer : PropertyDrawer
    {
        public static float lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            label.text += " (Foldout Locked)";
            EditorGUI.PropertyField(position, property, label, property.isExpanded);
            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var foldoutAttribute = (attribute as EZLockedFoldoutAttribute);
            property.isExpanded = foldoutAttribute.foldout;
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}
