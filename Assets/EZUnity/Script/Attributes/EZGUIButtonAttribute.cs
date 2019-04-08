/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-03-18 13:27:52
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif

namespace EZUnity
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EZInspectorButtonAttribute : PropertyAttribute
    {
        public enum ButtonLayout
        {
            Above,
            Replace,
            Below
        }
        public readonly string buttonLabel;
        public readonly string methodName;
        public ButtonLayout layout;
        public EZInspectorButtonAttribute(string methodName, ButtonLayout layout = ButtonLayout.Above)
        {
            this.buttonLabel = methodName;
            this.methodName = methodName.Replace(" ", "");
            this.layout = layout;
        }
        public EZInspectorButtonAttribute(string buttonLabel, string methodName, ButtonLayout layout = ButtonLayout.Above)
        {
            this.buttonLabel = buttonLabel;
            this.methodName = methodName;
            this.layout = layout;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(EZInspectorButtonAttribute))]
    public class EZInspectorButtonPropertyDrawer : PropertyDrawer
    {
        private EZInspectorButtonAttribute buttonAttribute;
        private MethodInfo methodInfo;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (buttonAttribute == null) buttonAttribute = attribute as EZInspectorButtonAttribute;
            switch (buttonAttribute.layout)
            {
                case EZInspectorButtonAttribute.ButtonLayout.Above:
                    return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + base.GetPropertyHeight(property, label);
                case EZInspectorButtonAttribute.ButtonLayout.Replace:
                    return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                case EZInspectorButtonAttribute.ButtonLayout.Below:
                    return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + base.GetPropertyHeight(property, label);
                default:
                    return base.GetPropertyHeight(property, label);
            }
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (buttonAttribute == null) buttonAttribute = attribute as EZInspectorButtonAttribute;

            EditorGUI.BeginProperty(position, label, property);
            switch (buttonAttribute.layout)
            {
                case EZInspectorButtonAttribute.ButtonLayout.Above:
                    position.height = EditorGUIUtility.singleLineHeight;
                    DrawButton(position, property.serializedObject.targetObject);
                    position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
                    position.height = base.GetPropertyHeight(property, label);
                    EditorGUI.PropertyField(position, property);
                    break;
                case EZInspectorButtonAttribute.ButtonLayout.Replace:
                    DrawButton(position, property.serializedObject.targetObject);
                    break;
                case EZInspectorButtonAttribute.ButtonLayout.Below:
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
#endif
}
