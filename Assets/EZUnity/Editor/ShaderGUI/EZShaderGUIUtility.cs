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
        public static void ShaderProperty(this MaterialEditor materialEditor, MaterialProperty property)
        {
            materialEditor.ShaderProperty(property, property.displayName);
        }
        public static void TextureProperty(this MaterialEditor materialEditor, MaterialProperty property)
        {
            materialEditor.TextureProperty(property, property.displayName);
        }
        public static void TexturePropertySingleLine(this MaterialEditor materialEditor, MaterialProperty property)
        {
            materialEditor.TexturePropertySingleLine(new GUIContent(property.displayName), property);
        }
        public static void TexturePropertySingleLine(this MaterialEditor materialEditor, MaterialProperty property, MaterialProperty extraProperty1)
        {
            materialEditor.TexturePropertySingleLine(new GUIContent(property.displayName), property, extraProperty1);
        }
        public static void TexturePropertySingleLine(this MaterialEditor materialEditor, MaterialProperty property, MaterialProperty extraProperty1, MaterialProperty extraProperty2)
        {
            materialEditor.TexturePropertySingleLine(new GUIContent(property.displayName), property, extraProperty1, extraProperty2);
        }
        public static void TexturePropertyTwoLines(this MaterialEditor materialEditor, MaterialProperty property, MaterialProperty extraProperty1, MaterialProperty extraProperty2)
        {
            materialEditor.TexturePropertyTwoLines(new GUIContent(property.displayName), property, extraProperty1, new GUIContent(extraProperty2.displayName), extraProperty2);
        }
        public static void ColorProperty(this MaterialEditor materialEditor, MaterialProperty property)
        {
            materialEditor.ColorProperty(property, property.displayName);
        }

        public static void TexturePropertyFeatured(this MaterialEditor materialEditor, MaterialProperty texture, string keyword, bool setupRequired = false)
        {
            EditorGUI.BeginChangeCheck();
            materialEditor.TexturePropertySingleLine(texture);
            if (setupRequired || EditorGUI.EndChangeCheck())
            {
                (materialEditor.target as Material).SetKeyword(keyword, texture.textureValue != null);
            }
        }
        public static void TexturePropertyFeatured(this MaterialEditor materialEditor, MaterialProperty texture, MaterialProperty extraProperty1, string keyword, bool setupRequired = false)
        {
            EditorGUI.BeginChangeCheck();
            materialEditor.TexturePropertySingleLine(texture, extraProperty1);
            if (setupRequired || EditorGUI.EndChangeCheck())
            {
                (materialEditor.target as Material).SetKeyword(keyword, texture.textureValue != null);
            }
        }
        public static void TexturePropertyFeatured(this MaterialEditor materialEditor, MaterialProperty texture, MaterialProperty extraProperty1, MaterialProperty extraProperty2, string keyword, bool setupRequired = false)
        {
            EditorGUI.BeginChangeCheck();
            materialEditor.TexturePropertySingleLine(texture, extraProperty1, extraProperty2);
            if (setupRequired || EditorGUI.EndChangeCheck())
            {
                (materialEditor.target as Material).SetKeyword(keyword, texture.textureValue != null);
            }
        }

        public static void KeywordEnum<T>(this MaterialEditor materialEditor, MaterialProperty property, params GUILayoutOption[] options)
#if CSHARP_7_3_OR_NEWER
            where T : Enum
#endif
        {
            materialEditor.EnumPopup<T>(property, (mat, selection) =>
            {
                mat.SetKeyword(selection);
            }, options);
        }
        public static void KeywordEnum<T>(this MaterialEditor materialEditor, MaterialProperty property, string label, params GUILayoutOption[] options)
#if CSHARP_7_3_OR_NEWER
            where T : Enum
#endif
        {
            materialEditor.EnumPopup<T>(property, label, (mat, selection) =>
            {
                mat.SetKeyword(selection);
            }, options);
        }

        public static void EnumPopup<T>(this MaterialEditor materialEditor, MaterialProperty property, Action<Material, Enum> callback = null, params GUILayoutOption[] options)
#if CSHARP_7_3_OR_NEWER
            where T : Enum
#endif
        {
            materialEditor.EnumPopup<T>(property, property.displayName, callback, options);
        }
        public static void EnumPopup<T>(this MaterialEditor materialEditor, MaterialProperty property, string label, Action<Material, Enum> callback = null, params GUILayoutOption[] options)
#if CSHARP_7_3_OR_NEWER
            where T : Enum
#endif
        {
            EditorGUI.showMixedValue = property.hasMixedValue;
            float value = property.floatValue;

            EditorGUI.BeginChangeCheck();
            value = EditorGUILayout.Popup(label, (int)value, Enum.GetNames(typeof(T)), options);
            if (EditorGUI.EndChangeCheck())
            {
                materialEditor.RegisterPropertyChangeUndo(label);
                property.floatValue = value;
                if (callback != null)
                {
                    foreach (Material mat in property.targets)
                    {
                        callback(mat, (Enum)Enum.ToObject(typeof(T), (int)value));
                    }
                }
            }
            EditorGUI.showMixedValue = false;
        }

        public static void Toggle(this MaterialEditor materialEditor, MaterialProperty property, Action<Material, bool> callback = null, params GUILayoutOption[] options)
        {
            materialEditor.Toggle(property, property.displayName, callback, options);
        }
        public static void Toggle(this MaterialEditor materialEditor, MaterialProperty property, string label, Action<Material, bool> callback = null, params GUILayoutOption[] options)
        {
            EditorGUI.showMixedValue = property.hasMixedValue;
            bool value = property.floatValue == 1;
            EditorGUI.BeginChangeCheck();
            value = EditorGUILayout.Toggle(label, value, options);
            if (EditorGUI.EndChangeCheck())
            {
                materialEditor.RegisterPropertyChangeUndo(label);
                property.floatValue = value ? 1 : 0;
                if (callback != null)
                {
                    foreach (Material mat in property.targets)
                    {
                        callback(mat, value);
                    }
                }
            }
            EditorGUI.showMixedValue = false;
        }

        public static void MinMaxSliderPropertyTwoLines(this MaterialEditor materialEditor, MaterialProperty property, float minValue = 0, float maxValue = 1)
        {
            MinMaxSliderPropertyTwoLines(materialEditor, property, property.displayName, minValue, maxValue);
        }
        public static void MinMaxSliderPropertyTwoLines(this MaterialEditor materialEditor, MaterialProperty property, string label, float minValue = 0, float maxValue = 1)
        {
            Vector2 range = property.vectorValue;
            property.vectorValue = EditorGUILayout.Vector2Field(label, range);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.MinMaxSlider("", ref range.x, ref range.y, minValue, maxValue);
            if (EditorGUI.EndChangeCheck())
            {
                property.vectorValue = range;
            }
        }

        public static void SetKeyword(this MaterialEditor materialEditor, Enum selection)
        {
            (materialEditor.target as Material).SetKeyword(selection);
        }
        public static void SetKeyword(this MaterialEditor materialEditor, string keyword, bool value)
        {
            (materialEditor.target as Material).SetKeyword(keyword, value);
        }

        public static void SetKeyword(this Material mat, Enum selection)
        {
            foreach (Enum value in Enum.GetValues(selection.GetType()))
            {
                mat.DisableKeyword(FormatKeyword(value));
            }
            mat.EnableKeyword(FormatKeyword(selection));
        }
        public static void SetKeyword(this Material mat, string keyword, bool value)
        {
            if (value) { mat.EnableKeyword(keyword); }
            else mat.DisableKeyword(keyword);
        }
        public static bool IsKeywordEnabled(this Material mat, Enum selection)
        {
            return mat.IsKeywordEnabled(FormatKeyword(selection));
        }

        public static string FormatKeyword(Enum value)
        {
            return string.Format("_{0}_{1}", value.GetType().Name, value).ToUpper();
        }
    }
}
