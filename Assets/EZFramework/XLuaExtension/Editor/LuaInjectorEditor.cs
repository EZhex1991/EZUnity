/*
 * Author:      熊哲
 * CreateTime:  9/4/2017 6:26:57 PM
 * Description:
 * 
*/
using EZFramework.XLuaExtension;
using EZUnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZFrameworkEditor.XLuaExtension
{
    public static class InjectionType
    {
        public static List<Type> typeList = new List<Type>()
        {
            typeof(int),
            typeof(float),
            typeof(bool),
            typeof(string),
            typeof(Vector2),
            typeof(Vector3),
            typeof(Vector4),
            typeof(AnimationCurve),
        };
        static InjectionType()
        {
            List<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where((assembly) => assembly.FullName.StartsWith("UnityEngine") || assembly.GetName().Name == "Assembly-CSharp")
                .ToList();
            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetExportedTypes())
                {
                    if (type.IsSubclassOf(typeof(UnityEngine.Object)) && !type.IsAbstract && !type.IsGenericType)
                    {
                        typeList.Add(type);
                    }
                }
            }
            typeList.Sort((t1, t2) => (string.Compare(t1.FullName, t2.FullName)));
        }
        public static Type GetType(string typeName)
        {
            foreach (Type type in typeList)
            {
                if (type.FullName == typeName) return type;
            }
            return null;
        }
    }

    [CustomPropertyDrawer(typeof(Injection))]
    public class InjectionDrawer : PropertyDrawer
    {
        float lineHeight = EditorGUIUtility.singleLineHeight;

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            SerializedProperty typeName = property.FindPropertyRelative("typeName");
            SerializedProperty key = property.FindPropertyRelative("key");
            SerializedProperty value = property.FindPropertyRelative("value");
            SerializedProperty nonObjectValue = property.FindPropertyRelative("nonObjectValue");

            Type type = InjectionType.GetType(typeName.stringValue);
            if (type == null)
            {
                type = typeof(UnityEngine.GameObject);
                typeName.stringValue = type.FullName;
            }

            float width = rect.width / 3, space = 5;
            rect.width = width - space; rect.height = lineHeight;
            if (GUI.Button(rect, type.Name))
            {
                DrawTypeMenu(delegate (object name)
                {
                    typeName.stringValue = (string)name;
                    property.serializedObject.ApplyModifiedProperties();
                });
            }
            rect.x += width;
            EditorGUI.PropertyField(rect, key, GUIContent.none);
            rect.x += width;
            if (type.IsSubclassOf(typeof(UnityEngine.Object)))
            {
                value.objectReferenceValue = EditorGUI.ObjectField(rect, GUIContent.none, value.objectReferenceValue, type, true);
            }
            else
            {
                SerializedProperty valueProperty = value;
                if (type == typeof(int)) { valueProperty = nonObjectValue.FindPropertyRelative("intValue"); }
                else if (type == typeof(float)) { valueProperty = nonObjectValue.FindPropertyRelative("floatValue"); }
                else if (type == typeof(bool)) { valueProperty = nonObjectValue.FindPropertyRelative("boolValue"); }
                else if (type == typeof(string)) { valueProperty = nonObjectValue.FindPropertyRelative("stringValue"); }
                else if (type == typeof(Vector2)) { valueProperty = nonObjectValue.FindPropertyRelative("v2Value"); }
                else if (type == typeof(Vector3)) { valueProperty = nonObjectValue.FindPropertyRelative("v3Value"); }
                else if (type == typeof(Vector4)) { valueProperty = nonObjectValue.FindPropertyRelative("v4Value"); }
                else if (type == typeof(AnimationCurve)) { valueProperty = nonObjectValue.FindPropertyRelative("animationCurveValue"); }
                EditorGUI.PropertyField(rect, valueProperty, GUIContent.none);
            }
            EditorGUI.EndProperty();
            property.serializedObject.ApplyModifiedProperties();
        }
        protected void DrawTypeMenu(GenericMenu.MenuFunction2 callback)
        {
            GenericMenu menu = new GenericMenu();
            for (int i = 0; i < InjectionType.typeList.Count; i++)
            {
                string space = InjectionType.typeList[i].Namespace;
                string name = InjectionType.typeList[i].Name;
                string fullName = InjectionType.typeList[i].FullName;
                if (!InjectionType.typeList[i].IsSubclassOf(typeof(UnityEngine.Object)))
                {
                    space = "Non-Object";
                    name = fullName;
                }
                else if (string.IsNullOrEmpty(space))
                {
                    space = "No Namespace";
                }
                else if (space == "UnityEngine")
                {
                    string firstCase = name.Substring(0, 1);
                    if (firstCase.CompareTo("B") <= 0)
                        space = "UnityEngine/A~B";
                    else if (firstCase.CompareTo("H") <= 0)
                        space = "UnityEngine/C~H";
                    else if (firstCase.CompareTo("P") <= 0)
                        space = "UnityEngine/I~P";
                    else
                        space = "UnityEngine/Q~Z";
                }
                menu.AddItem(new GUIContent(space + "/" + name), false, callback, fullName);
            }
            menu.ShowAsContext();
        }
    }

    [CustomEditor(typeof(LuaInjector))]
    public class LuaInjectorEditor : Editor
    {
        protected SerializedProperty m_InjectionList;
        protected ReorderableList injectionList;

        private float space = EZEditorGUIUtility.space;
        private float lineHeight = EditorGUIUtility.singleLineHeight;

        protected virtual void OnEnable()
        {
            m_InjectionList = serializedObject.FindProperty("injections");
            injectionList = new ReorderableList(serializedObject, m_InjectionList, true, true, true, true);
            injectionList.drawHeaderCallback = DrawInjectionListHeader;
            injectionList.drawElementCallback = DrawInjectionListElement;
        }

        public override void OnInspectorGUI()
        {
            EZEditorGUIUtility.ScriptTitle(target);
            serializedObject.Update();
            injectionList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }

        protected void DrawInjectionListHeader(Rect rect)
        {

        }
        protected void DrawInjectionListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect.y += 1;
            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, m_InjectionList, index);

            SerializedProperty pair = injectionList.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, pair);
        }
    }
}