/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-06-14 20:14:43
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CustomPropertyDrawer(typeof(EZButtonPropertyAttribute))]
    public class EZButtonPropertyDrawer : PropertyDrawer
    {
        private EZButtonPropertyAttribute buttonAttribute;
        private MethodInfo methodInfo;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (buttonAttribute == null) buttonAttribute = attribute as EZButtonPropertyAttribute;
            switch (buttonAttribute.layout)
            {
                case EZButtonPropertyAttribute.ButtonLayout.Above:
                    return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + base.GetPropertyHeight(property, label);
                case EZButtonPropertyAttribute.ButtonLayout.Replace:
                    return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                case EZButtonPropertyAttribute.ButtonLayout.Below:
                    return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + base.GetPropertyHeight(property, label);
                default:
                    return base.GetPropertyHeight(property, label);
            }
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (buttonAttribute == null) buttonAttribute = attribute as EZButtonPropertyAttribute;

            EditorGUI.BeginProperty(position, label, property);
            switch (buttonAttribute.layout)
            {
                case EZButtonPropertyAttribute.ButtonLayout.Above:
                    position.height = EditorGUIUtility.singleLineHeight;
                    DrawButton(position, property.serializedObject.targetObject);
                    position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
                    position.height = base.GetPropertyHeight(property, label);
                    EditorGUI.PropertyField(position, property);
                    break;
                case EZButtonPropertyAttribute.ButtonLayout.Replace:
                    DrawButton(position, property.serializedObject.targetObject);
                    break;
                case EZButtonPropertyAttribute.ButtonLayout.Below:
                    position.height = base.GetPropertyHeight(property, label);
                    EditorGUI.PropertyField(position, property);
                    position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
                    position.height = EditorGUIUtility.singleLineHeight;
                    DrawButton(position, property.serializedObject.targetObject);
                    break;
                default:
                    EditorGUI.PropertyField(position, property);
                    break;
            }
            EditorGUI.EndProperty();
        }
        public void DrawButton(Rect position, object target)
        {
            if (GUI.Button(position, buttonAttribute.buttonLabel))
            {
                Type type = target.GetType();
                if (methodInfo == null)
                {
                    methodInfo = type.GetMethod(buttonAttribute.methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                }
                if (methodInfo == null)
                {
                    Debug.LogWarningFormat("Method {0} not exist: {1}", buttonAttribute.methodName, type);
                }
                else
                {
                    methodInfo.Invoke(target, null);
                }
            }
        }
    }
}
