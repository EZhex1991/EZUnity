/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-11-28 16:56:52
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CustomPropertyDrawer(typeof(EZVector3Bool))]
    public class EZVector3BoolDrawer : PropertyDrawer
    {
        private const float labelWidth = 16f;
        private const float toggleWidth = 50f;

        private SerializedProperty m_X;
        private SerializedProperty m_Y;
        private SerializedProperty m_Z;

        private void GetProperties(SerializedProperty property)
        {
            m_X = property.FindPropertyRelative("x");
            m_Y = property.FindPropertyRelative("y");
            m_Z = property.FindPropertyRelative("z");
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            GetProperties(property);
            Rect rect = EditorGUI.PrefixLabel(position, label);

            EditorGUI.BeginChangeCheck();

            int indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            rect.x -= 1;
            rect.width = toggleWidth;
            DrawToggleComponent(rect, m_X);
            rect.x = rect.xMax;
            DrawToggleComponent(rect, m_Y);
            rect.x = rect.xMax;
            DrawToggleComponent(rect, m_Z);
            EditorGUI.indentLevel = indentLevel;

            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }

            EditorGUI.EndProperty();
        }

        private void DrawToggleComponent(Rect rect, SerializedProperty property)
        {
            EditorGUI.LabelField(rect, property.displayName);
            rect.x += labelWidth;
            rect.width -= labelWidth;
            EditorGUI.PropertyField(rect, property, GUIContent.none);
        }
    }
}
