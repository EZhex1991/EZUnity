/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-16 10:32:11
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CustomPropertyDrawer(typeof(EZSingleLineVector4Attribute))]
    public class EZSingleLineVector4PropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            if (property.propertyType == SerializedPropertyType.Vector4)
            {
                property.vector4Value = EditorGUI.Vector4Field(position, label, property.vector4Value);
            }
            else
            {
                EditorGUI.HelpBox(position, typeof(EZSingleLineVector4Attribute).Name + " used on a non-vector4 property", MessageType.Warning);
            }
            EditorGUI.EndProperty();
        }
    }
}
