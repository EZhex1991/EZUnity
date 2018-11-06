/* Author:          熊哲
 * CreateTime:      2018-05-07 16:37:12
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZUnity
{
    [CustomPropertyDrawer(typeof(EZDictionary.Element))]
    public class EZDictionaryElementDrawer : PropertyDrawer
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
        static EZDictionaryElementDrawer()
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

        float lineHeight = EditorGUIUtility.singleLineHeight;

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            SerializedProperty typeName = property.FindPropertyRelative("m_TypeName");
            SerializedProperty key = property.FindPropertyRelative("m_Key");
            SerializedProperty objectValue = property.FindPropertyRelative("m_ObjectValue");
            SerializedProperty intValue = property.FindPropertyRelative("m_IntValue");
            SerializedProperty floatValue = property.FindPropertyRelative("m_FloatValue");
            SerializedProperty boolValue = property.FindPropertyRelative("m_BoolValue");
            SerializedProperty stringValue = property.FindPropertyRelative("m_StringValue");
            SerializedProperty vector2Value = property.FindPropertyRelative("m_Vector2Value");
            SerializedProperty vector3Value = property.FindPropertyRelative("m_Vector3Value");
            SerializedProperty vector4Value = property.FindPropertyRelative("m_Vector4Value");
            SerializedProperty animationCurveValue = property.FindPropertyRelative("m_AnimationCurveValue");

            float typeButtonWidth = 40;
            if (GUI.Button(new Rect(rect.x, rect.y, typeButtonWidth, rect.height), "Type", EditorStyles.miniButton))
            {
                DrawTypeMenu(delegate (object name)
                {
                    typeName.stringValue = (string)name;
                    property.serializedObject.ApplyModifiedProperties();
                });
            }
            float width = (rect.width - typeButtonWidth) / 3, space = 5;
            rect.x += typeButtonWidth + space; rect.width = width - space; rect.height = lineHeight;
            EditorGUI.DelayedTextField(new Rect(rect.x, rect.y, rect.width, rect.height), typeName, GUIContent.none);
            rect.x += width;
            EditorGUI.PropertyField(rect, key, GUIContent.none);
            rect.x += width;
            Type type = GetType(typeName.stringValue);
            if (type == null)
            {
                type = typeof(UnityEngine.Object);
                typeName.stringValue = type.FullName;
            }
            if (type == typeof(int)) EditorGUI.PropertyField(rect, intValue, GUIContent.none);
            else if (type == typeof(float)) EditorGUI.PropertyField(rect, floatValue, GUIContent.none);
            else if (type == typeof(bool)) EditorGUI.PropertyField(rect, boolValue, GUIContent.none);
            else if (type == typeof(string)) EditorGUI.PropertyField(rect, stringValue, GUIContent.none);
            else if (type == typeof(Vector2)) EditorGUI.PropertyField(rect, vector2Value, GUIContent.none);
            else if (type == typeof(Vector3)) EditorGUI.PropertyField(rect, vector3Value, GUIContent.none);
            else if (type == typeof(Vector4)) EditorGUI.PropertyField(rect, vector4Value, GUIContent.none);
            else if (type == typeof(AnimationCurve)) EditorGUI.PropertyField(rect, animationCurveValue, GUIContent.none);
            else objectValue.objectReferenceValue = EditorGUI.ObjectField(rect, GUIContent.none, objectValue.objectReferenceValue, type, true);
            EditorGUI.EndProperty();
            property.serializedObject.ApplyModifiedProperties();
        }
        protected void DrawTypeMenu(GenericMenu.MenuFunction2 callback)
        {
            GenericMenu menu = new GenericMenu();
            for (int i = 0; i < typeList.Count; i++)
            {
                string space = typeList[i].Namespace;
                string name = typeList[i].Name;
                string fullName = typeList[i].FullName;
                if (!typeList[i].IsSubclassOf(typeof(UnityEngine.Object)))
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
                    space += "/" + name.Substring(0, 1);
                }
                menu.AddItem(new GUIContent(space + "/" + name), false, callback, fullName);
            }
            menu.ShowAsContext();
        }
    }

    [CustomEditor(typeof(EZDictionary))]
    public class EZDictionaryEditor : Editor
    {
        protected SerializedProperty m_Elements;
        protected ReorderableList elementList;

        protected virtual void OnEnable()
        {
            m_Elements = serializedObject.FindProperty("m_Elements");
            elementList = new ReorderableList(serializedObject, m_Elements, true, true, true, true)
            {
                drawHeaderCallback = DrawElementListHeader,
                drawElementCallback = DrawElementListElement
            };
        }

        public override void OnInspectorGUI()
        {
            EZEditorGUIUtility.ScriptTitle(target);
            serializedObject.Update();
            elementList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }

        protected void DrawElementListHeader(Rect rect)
        {

        }
        protected void DrawElementListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect.y += 1;
            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, m_Elements, index);
            SerializedProperty element = elementList.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, element);
        }
    }
}
