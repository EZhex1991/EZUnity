/* Author:          ezhex1991@outlook.com
 * CreateTime:      2020-03-31 11:05:28
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZTypeReflectionHelper : EditorWindow
    {
        private static string typeName = "UnityEditor.GameView,UnityEditor";
        private static Type type;

        private static FieldInfo[] fields;
        private static PropertyInfo[] properties;
        private static MethodInfo[] methods;
        private static string[] enumNames;

        private static Vector2 scrollPosition;
        private static bool[] foldouts = new bool[10];

        private static void GetTypeInfo()
        {
            type = Type.GetType(typeName);
            for (int i = 0; i < foldouts.Length; i++)
            {
                foldouts[i] = true;
            }
            if (type != null)
            {
                if (type.IsClass)
                {
                    fields = type.GetFields();
                    properties = type.GetProperties();
                    methods = type.GetMethods();
                    enumNames = null;
                }
                else if (type.IsEnum)
                {
                    fields = null;
                    properties = null;
                    methods = null;
                    enumNames = type.GetEnumNames();
                }
            }
        }

        private void OnGUI()
        {
            EZEditorGUIUtility.WindowTitle(this);

            typeName = EditorGUILayout.TextField("Type Name", typeName);
            if (GUILayout.Button("Get Type Info"))
            {
                GetTypeInfo();
            }

            if (type != null)
            {
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                DrawInfo(ref foldouts[0], "Fields", fields, FieldInfoDrawer);
                DrawInfo(ref foldouts[1], "Methods", methods, MethodInfoDrawer);
                DrawInfo(ref foldouts[2], "Properties", properties, PropertyInfoDrawer);
                DrawInfo(ref foldouts[3], "Enum Names", enumNames);
                EditorGUILayout.EndScrollView();
            }
            else
            {
                EditorGUILayout.HelpBox("Not a valid type", MessageType.Info);
            }
        }
        private void DrawInfo(ref bool foldout, string label, MemberInfo[] info, Action<MemberInfo> drawer)
        {
            if (info == null) return;
            EditorStyles.foldout.fontStyle = FontStyle.Bold;
            foldout = EditorGUILayout.Foldout(foldout, string.Format("{0} ({1})", label, info.Length), true);
            EditorStyles.foldout.fontStyle = FontStyle.Normal;
            if (foldout)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < info.Length; i++)
                {
                    drawer(info[i]);
                }
                EditorGUI.indentLevel--;
            }
        }
        private void FieldInfoDrawer(MemberInfo info)
        {
            FieldInfo fieldInfo = info as FieldInfo;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField(info.DeclaringType.Name, EditorStyles.textField);
            EditorGUILayout.TextField(fieldInfo.FieldType.Name, EditorStyles.textField);
            EditorGUILayout.TextField(info.Name, EditorStyles.textField);
            EditorGUILayout.TextField(fieldInfo.IsStatic.ToString(), EditorStyles.textField);
            EditorGUILayout.EndHorizontal();
        }
        private void PropertyInfoDrawer(MemberInfo info)
        {
            PropertyInfo propertyInfo = info as PropertyInfo;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField(info.DeclaringType.Name, EditorStyles.textField);
            EditorGUILayout.TextField(propertyInfo.PropertyType.Name, EditorStyles.textField);
            EditorGUILayout.TextField(info.Name, EditorStyles.textField);
            EditorGUILayout.EndHorizontal();
        }
        private void MethodInfoDrawer(MemberInfo info)
        {
            MethodInfo methodInfo = info as MethodInfo;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField(info.DeclaringType.Name, EditorStyles.textField);
            EditorGUILayout.TextField(methodInfo.ReturnType.Name, EditorStyles.textField);
            EditorGUILayout.TextField(info.Name, EditorStyles.textField);
            EditorGUILayout.TextField(methodInfo.IsStatic.ToString(), EditorStyles.textField);
            EditorGUILayout.EndHorizontal();
        }

        private void DrawInfo(ref bool foldout, string label, string[] info)
        {
            if (info == null) return;
            EditorStyles.foldout.fontStyle = FontStyle.Bold;
            foldout = EditorGUILayout.Foldout(foldout, string.Format("{0} ({1})", label, info.Length), true);
            EditorStyles.foldout.fontStyle = FontStyle.Normal;
            if (foldout)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < info.Length; i++)
                {
                    EditorGUILayout.TextField(info[i], EditorStyles.textField);
                }
                EditorGUI.indentLevel--;
            }
        }
    }
}
