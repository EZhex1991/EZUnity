/* Author:          熊哲
 * CreateTime:      2018-11-14 16:17:52
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EZUnity
{
    [CustomPropertyDrawer(typeof(EZSerializableProperty))]
    public class EZSerializablePropertyDrawer : PropertyDrawer
    {
        public static List<Type> typeList = new List<Type>()
        {
            typeof(int),
            typeof(long),
            typeof(bool),
            typeof(float),
            typeof(double),
            typeof(string),
            typeof(Color),
            typeof(AnimationCurve),
            typeof(Vector2),
            typeof(Vector3),
            typeof(Vector2Int),
            typeof(Vector3Int),
        };
        public static bool IsSupportedType(Type type)
        {
            if (type.IsAbstract || type.IsGenericType) return false;
            return type.IsSubclassOf(typeof(UnityEngine.Object));
        }
        public static Type GetType(string typeName)
        {
            foreach (Type type in typeList)
            {
                if (type.FullName == typeName) return type;
            }
            return null;
        }
        static EZSerializablePropertyDrawer()
        {
            List<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where((assembly) => assembly.FullName.StartsWith("UnityEngine") || assembly.GetName().Name == "Assembly-CSharp")
                .ToList();
            typeList.AddRange(from assembly in assemblies
                              from type in assembly.GetExportedTypes()
                              where IsSupportedType(type)
                              select type);
            typeList.Sort((t1, t2) => (string.Compare(t1.FullName, t2.FullName)));
        }

        float lineHeight = EditorGUIUtility.singleLineHeight;

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            SerializedProperty typeName = property.FindPropertyRelative("m_TypeName");
            SerializedProperty key = property.FindPropertyRelative("m_Key");

            float typeButtonWidth = 40;
            if (GUI.Button(new Rect(rect.x, rect.y, typeButtonWidth, EditorGUIUtility.singleLineHeight), "Type", EditorStyles.miniButton))
            {
                DrawTypeMenu(delegate (object selection)
                {
                    typeName.stringValue = (selection as Type).FullName;
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
            SerializedProperty targetProperty = property.FindPropertyRelative("m_ObjectValue");
            if (type.IsSubclassOf(typeof(UnityEngine.Object)))
            {
                targetProperty.objectReferenceValue = EditorGUI.ObjectField(rect, GUIContent.none, targetProperty.objectReferenceValue, type, true);
            }
            else
            {
                if (type == typeof(int)) { targetProperty = property.FindPropertyRelative("m_IntValue"); }
                else if (type == typeof(long)) { targetProperty = property.FindPropertyRelative("m_LongValue"); }
                else if (type == typeof(bool)) { targetProperty = property.FindPropertyRelative("m_BoolValue"); }
                else if (type == typeof(float)) { targetProperty = property.FindPropertyRelative("m_FloatValue"); }
                else if (type == typeof(double)) { targetProperty = property.FindPropertyRelative("m_DoubleValue"); }
                else if (type == typeof(string)) { targetProperty = property.FindPropertyRelative("m_StringValue"); }
                else if (type == typeof(Color)) { targetProperty = property.FindPropertyRelative("m_ColorValue"); }
                else if (type == typeof(AnimationCurve)) { targetProperty = property.FindPropertyRelative("m_AnimationCurveValue"); }
                else if (type == typeof(Vector2)) { targetProperty = property.FindPropertyRelative("m_Vector2Value"); }
                else if (type == typeof(Vector3)) { targetProperty = property.FindPropertyRelative("m_Vector3Value"); }
                else if (type == typeof(Vector2Int)) { targetProperty = property.FindPropertyRelative("m_Vector2IntValue"); }
                else if (type == typeof(Vector3Int)) { targetProperty = property.FindPropertyRelative("m_Vector3IntValue"); }
                EditorGUI.PropertyField(rect, targetProperty, GUIContent.none);
            }
            EditorGUI.EndProperty();
            property.serializedObject.ApplyModifiedProperties();
        }
        protected void DrawTypeMenu(GenericMenu.MenuFunction2 callback)
        {
            GenericMenu menu = new GenericMenu();
            for (int i = 0; i < typeList.Count; i++)
            {
                string space = typeList[i].Namespace;
                string fullName = typeList[i].FullName;
                string menuContent;
                if (string.IsNullOrEmpty(space))
                {
                    menuContent = "No Namespace/" + fullName.Substring(0, 1) + "/" + fullName.Replace('.', '/');
                }
                else if (space == "UnityEngine")
                {
                    string shortName = fullName.Substring(space.Length + 1);
                    menuContent = space + "/" + shortName.Substring(0, 1) + "/" + shortName.Replace('.', '/');
                }
                else
                {
                    string shortName = fullName.Substring(space.Length + 1);
                    menuContent = space + "/" + shortName.Replace('.', '/');
                }
                menu.AddItem(new GUIContent(menuContent), false, callback, typeList[i]);
            }
            menu.ShowAsContext();
        }
    }
}
