/*
 * Author:      熊哲
 * CreateTime:  9/4/2017 6:26:57 PM
 * Description:
 * 
*/
using EZFramework.XLuaExtension;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZFramework.XLuaExtensionEditor
{
    [CustomEditor(typeof(LuaInjector))]
    public class LuaInjectorEditor : Editor
    {
        public static List<Type> typeList = new List<Type>
        {
            typeof(EZFramework.XLuaExtension.LuaInjector), // 可嵌套使用
            typeof(UnityEngine.Object),
            typeof(UnityEngine.AudioSource),
            typeof(UnityEngine.AudioClip),
            typeof(UnityEngine.Collider),
            typeof(UnityEngine.GameObject),
            typeof(UnityEngine.GUIText),
            typeof(UnityEngine.RectTransform),
            typeof(UnityEngine.Sprite),
            typeof(UnityEngine.TextMesh),
            typeof(UnityEngine.Texture),
            typeof(UnityEngine.Transform),
            typeof(UnityEngine.UI.Button),
            typeof(UnityEngine.UI.Image),
            typeof(UnityEngine.UI.ScrollRect),
            typeof(UnityEngine.UI.Slider),
            typeof(UnityEngine.UI.Text),
            typeof(UnityEngine.UI.Toggle),
            typeof(UnityEngine.UI.ToggleGroup),
        };
        public Type GetType(string typeName, bool deepSearch = true)
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

        private ReorderableList injectionList;

        void OnEnable()
        {
            injectionList = new ReorderableList(serializedObject, serializedObject.FindProperty("injections"), true, true, true, true);
            injectionList.drawHeaderCallback = DrawInjectionListHeader;
            injectionList.drawElementCallback = DrawInjectionListElement;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(target as MonoBehaviour), typeof(MonoScript), false);
            GUI.enabled = true;
            injectionList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }

        protected void DrawTypeMenu(GenericMenu.MenuFunction2 callback)
        {
            callback += delegate { serializedObject.ApplyModifiedProperties(); };
            GenericMenu menu = new GenericMenu();
            for (int i = 0; i < typeList.Count; i++)
            {
                string typeName = typeList[i].FullName;
                menu.AddItem(new GUIContent(typeName.Replace(".", "/")), false, callback, typeName);
            }
            menu.ShowAsContext();
        }

        protected void DrawInjectionListHeader(Rect rect)
        {

        }
        protected void DrawInjectionListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect.y += 1;
            SerializedProperty pair = injectionList.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty key = pair.FindPropertyRelative("key");
            SerializedProperty value = pair.FindPropertyRelative("value");
            SerializedProperty typeName = pair.FindPropertyRelative("typeName");
            if (string.IsNullOrEmpty(typeName.stringValue)) typeName.stringValue = "UnityEngine.Object";
            Type type = GetType(typeName.stringValue);
            EditorGUI.LabelField(new Rect(rect.x, rect.y, 20, EditorGUIUtility.singleLineHeight), index.ToString("00"));
            float width = (rect.width - 20) / 3 - 5;
            if (GUI.Button(new Rect(rect.x + 25, rect.y, width, EditorGUIUtility.singleLineHeight), type.Name))
            {
                DrawTypeMenu(delegate (object name) { typeName.stringValue = (string)name; });
            }
            EditorGUI.PropertyField(new Rect(rect.x + 30 + width, rect.y, width, EditorGUIUtility.singleLineHeight), key, GUIContent.none);
            value.objectReferenceValue = EditorGUI.ObjectField(new Rect(rect.x + 35 + width * 2, rect.y, width, EditorGUIUtility.singleLineHeight), value.objectReferenceValue, type, true);
        }
    }
}