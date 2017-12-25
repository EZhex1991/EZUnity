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
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZFrameworkEditor.XLuaExtension
{
    [CustomEditor(typeof(LuaInjector))]
    public class LuaInjectorEditor : Editor
    {
        public static List<Type> typeList = new List<Type>
        {
            typeof(EZFramework.XLuaExtension.LuaInjector), // 可嵌套使用

            typeof(EZComponent.EZAnimation.EZGraphicColorAnimation),
            typeof(EZComponent.EZAnimation.EZRectTransformAnimation),
            typeof(EZComponent.EZAnimation.EZTransformAnimation),

            typeof(UnityEngine.Object),
            typeof(UnityEngine.AudioSource),
            typeof(UnityEngine.AudioClip),
            typeof(UnityEngine.Collider),
            typeof(UnityEngine.GameObject),
            typeof(UnityEngine.GUIText),
            typeof(UnityEngine.ParticleSystem),
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
                string typeName = typeList[i].FullName;
                menu.AddItem(new GUIContent(space + "/" + name), false, callback, typeName);
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