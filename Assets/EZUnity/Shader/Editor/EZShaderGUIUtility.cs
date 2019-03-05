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
        public static void ColorProperty(this MaterialEditor materialEditor, MaterialProperty property)
        {
            materialEditor.ColorProperty(property, property.displayName);
        }

        public static void SliderProperty(this MaterialEditor materialEditor, MaterialProperty property, float min, float max, Action<Material, float> callback = null, params GUILayoutOption[] options)
        {
            materialEditor.SliderProperty(property, min, max, property.displayName, callback, options);
        }
        public static void SliderProperty(this MaterialEditor materialEditor, MaterialProperty property, float min, float max, string label, Action<Material, float> callback = null, params GUILayoutOption[] options)
        {
            EditorGUI.showMixedValue = property.hasMixedValue;
            float value = property.floatValue;

            EditorGUI.BeginChangeCheck();
            value = EditorGUILayout.Slider(label, value, min, max);
            if (EditorGUI.EndChangeCheck())
            {
                materialEditor.RegisterPropertyChangeUndo(label);
                property.floatValue = value;
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

        public static void BlendModeProperty(this MaterialEditor materialEditor, MaterialProperty property)
        {
            materialEditor.EnumPopup<UnityEngine.Rendering.BlendMode>(property);
        }
        public static void CullModeProperty(this MaterialEditor materialEditor, MaterialProperty property)
        {
            materialEditor.EnumPopup<UnityEngine.Rendering.CullMode>(property);
        }

        public static void EnumPopup<T>(this MaterialEditor materialEditor, MaterialProperty property, Action<Material, Enum> callback = null, params GUILayoutOption[] options) where T : Enum
        {
            materialEditor.EnumPopup<T>(property, property.displayName, callback, options);
        }
        public static void EnumPopup<T>(this MaterialEditor materialEditor, MaterialProperty property, string label, Action<Material, Enum> callback = null, params GUILayoutOption[] options) where T : Enum
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
                        callback(mat, (T)Enum.ToObject(typeof(T), (int)value));
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

        public static void SetKeyword<T>(this Material mat, T selection) where T : Enum
        {
            foreach (T value in Enum.GetValues(typeof(T)))
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

        public static void FeaturedPropertiesWithTexture(this MaterialEditor materialEditor, MaterialProperty featureOn, MaterialProperty texture, MaterialProperty adjustor, string keyword)
        {
            EditorStyles.label.fontStyle = FontStyle.Bold;
            materialEditor.Toggle(featureOn, (mat, isOn) =>
            {
                mat.SetKeyword(keyword, isOn && texture.textureValue != null);
            });
            EditorStyles.label.fontStyle = FontStyle.Normal;
            if (featureOn.floatValue == 1)
            {
                EditorGUI.BeginChangeCheck();
                materialEditor.ShaderProperty(texture);
                if (EditorGUI.EndChangeCheck())
                {
                    (materialEditor.target as Material).SetKeyword(keyword, texture.textureValue != null);
                }
                materialEditor.ShaderProperty(adjustor);
            }
        }

        public static string FormatKeyword<T>(T value) where T : Enum
        {
            return string.Format("_{0}_{1}", typeof(T).Name, value).ToUpper();
        }
    }
}
