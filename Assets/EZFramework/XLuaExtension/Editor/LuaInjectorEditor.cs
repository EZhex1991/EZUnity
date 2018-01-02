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
    [CustomEditor(typeof(LuaInjector))]
    public class LuaInjectorEditor : Editor
    {
        public static List<Type> typeList = new List<Type>();
        public static Type GetType(string typeName, bool deepSearch = true)
        {
            foreach (Type type in typeList)
            {
                if (type.FullName == typeName) return type;
            }
            if (deepSearch)
            {
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    Type type = assembly.GetType(typeName);
                    if (type != null) return type;
                }
            }
            return null;
        }
        static LuaInjectorEditor()
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

        private SerializedProperty m_InjectionList;
        private ReorderableList injectionList;

        private float space = EZEditorGUIUtility.space;
        private float lineHeight = EditorGUIUtility.singleLineHeight;

        void OnEnable()
        {
            m_InjectionList = serializedObject.FindProperty("injections");
            injectionList = new ReorderableList(serializedObject, m_InjectionList, true, true, true, true);
            injectionList.drawHeaderCallback = DrawInjectionListHeader;
            injectionList.drawElementCallback = DrawInjectionListElement;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EZEditorGUIUtility.ScriptTitle(target);
            injectionList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }

        protected void DrawTypeMenu(GenericMenu.MenuFunction2 callback)
        {
            GenericMenu menu = new GenericMenu();
            for (int i = 0; i < typeList.Count; i++)
            {
                string space = typeList[i].Namespace;
                string name = typeList[i].Name;
                string fullName = typeList[i].FullName;
                if (string.IsNullOrEmpty(space))
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

        protected void DrawInjectionListHeader(Rect rect)
        {

        }
        protected void DrawInjectionListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect.y += 1;
            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, m_InjectionList, index);

            SerializedProperty pair = injectionList.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty key = pair.FindPropertyRelative("key");
            SerializedProperty value = pair.FindPropertyRelative("value");
            SerializedProperty typeName = pair.FindPropertyRelative("typeName");
            if (string.IsNullOrEmpty(typeName.stringValue)) typeName.stringValue = "UnityEngine.Object";
            Type type = GetType(typeName.stringValue);

            float width = rect.width / 3;
            if (GUI.Button(new Rect(rect.x, rect.y, width - space, lineHeight), type.Name))
            {
                DrawTypeMenu(delegate (object name) { typeName.stringValue = (string)name; serializedObject.ApplyModifiedProperties(); });
            }
            rect.x += width;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width - space, lineHeight), key, GUIContent.none);
            rect.x += width;
            value.objectReferenceValue = EditorGUI.ObjectField(new Rect(rect.x, rect.y, width - space, lineHeight), value.objectReferenceValue, type, true);
        }
    }
}