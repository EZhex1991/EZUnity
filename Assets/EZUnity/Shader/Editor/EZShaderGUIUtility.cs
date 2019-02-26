/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-02-25 20:16:03
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEditor;
using UnityEngine;

namespace EZUnity
{
    public static class EZShaderGUIUtility
    {
        public static void DrawEnumPopup<T>(MaterialEditor materialEditor, MaterialProperty property) where T : Enum
        {
            DrawEnumPopup<T>(materialEditor, property, property.name);
        }
        public static void DrawEnumPopup<T>(MaterialEditor materialEditor, MaterialProperty property, string label) where T : Enum
        {
            EditorGUI.showMixedValue = property.hasMixedValue;
            float value = property.floatValue;

            EditorGUI.BeginChangeCheck();
            value = EditorGUILayout.Popup(label, (int)value, Enum.GetNames(typeof(T)));
            if (EditorGUI.EndChangeCheck())
            {
                materialEditor.RegisterPropertyChangeUndo(label);
                property.floatValue = value;
            }
            EditorGUI.showMixedValue = false;
        }

        public static void DrawEnumPopup<T>(MaterialEditor materialEditor, MaterialProperty property, Action<Material, Enum> callback) where T : Enum
        {
            DrawEnumPopup<T>(materialEditor, property, property.name, callback);
        }
        public static void DrawEnumPopup<T>(MaterialEditor materialEditor, MaterialProperty property, string label, Action<Material, Enum> callback) where T : Enum
        {
            EditorGUI.showMixedValue = property.hasMixedValue;
            float value = property.floatValue;

            EditorGUI.BeginChangeCheck();
            value = EditorGUILayout.Popup(label, (int)value, Enum.GetNames(typeof(T)));
            if (EditorGUI.EndChangeCheck())
            {
                materialEditor.RegisterPropertyChangeUndo(label);
                property.floatValue = value;
                foreach (Material mat in property.targets)
                {
                    callback(mat, (T)Enum.ToObject(typeof(T), (int)value));
                }
            }
            EditorGUI.showMixedValue = false;
        }

        public static void SetKeyword<T>(Material mat, T selection) where T : Enum
        {
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                mat.DisableKeyword(FormatKeyword(value));
            }
            mat.EnableKeyword(FormatKeyword(selection));
        }

        public static string FormatKeyword<T>(T value) where T : Enum
        {
            return string.Format("_{0}_{1}", typeof(T).Name, value).ToUpper();
        }
    }
}
