/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-02-25 20:16:03
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public static class EZShaderGUIUtility
    {
        public static void ShaderProperty(this MaterialEditor materialEditor, MaterialProperty property)
        {
            materialEditor.ShaderProperty(property, property.displayName);
        }
        public static void ShaderProperty(this MaterialEditor materialEditor, MaterialProperty property1, MaterialProperty property2)
        {
            Rect rect = EditorGUILayout.GetControlRect(true);

            EditorGUI.PrefixLabel(rect, new GUIContent(property1.displayName));
            materialEditor.ShaderProperty(MaterialEditor.GetLeftAlignedFieldRect(rect), property1, GUIContent.none);

            int oldIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            materialEditor.ShaderProperty(MaterialEditor.GetFlexibleRectBetweenFieldAndRightEdge(rect), property2, GUIContent.none);
            EditorGUI.indentLevel = oldIndentLevel;
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

        public static void TexturePropertyFeatured(this MaterialEditor materialEditor, MaterialProperty texture, string keyword, bool setupRequired = false)
        {
            EditorGUI.BeginChangeCheck();
            materialEditor.TexturePropertySingleLine(texture);
            if (EditorGUI.EndChangeCheck() || setupRequired)
            {
                if (!texture.hasMixedValue)
                {
                    foreach (Material mat in materialEditor.targets)
                    {
                        mat.SetKeyword(keyword, texture.textureValue != null);
                    }
                }
            }
        }
        public static void TexturePropertyFeatured(this MaterialEditor materialEditor, MaterialProperty texture, MaterialProperty extraProperty1, string keyword, bool setupRequired = false)
        {
            EditorGUI.BeginChangeCheck();
            materialEditor.TexturePropertySingleLine(texture, extraProperty1);
            if (EditorGUI.EndChangeCheck() || setupRequired)
            {
                if (!texture.hasMixedValue)
                {
                    foreach (Material mat in materialEditor.targets)
                    {
                        mat.SetKeyword(keyword, texture.textureValue != null);
                    }
                }
            }
        }
        public static void TexturePropertyFeatured(this MaterialEditor materialEditor, MaterialProperty texture, MaterialProperty extraProperty1, MaterialProperty extraProperty2, string keyword, bool setupRequired = false)
        {
            EditorGUI.BeginChangeCheck();
            materialEditor.TexturePropertySingleLine(texture, extraProperty1, extraProperty2);
            if (EditorGUI.EndChangeCheck() || setupRequired)
            {
                if (!texture.hasMixedValue)
                {
                    foreach (Material mat in materialEditor.targets)
                    {
                        mat.SetKeyword(keyword, texture.textureValue != null);
                    }
                }
            }
        }
    }
}
