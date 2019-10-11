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

        public static void KeywordsGUI(this MaterialEditor materialEditor)
        {
            if (materialEditor.targets.Length != 1) return;
            Material material = materialEditor.target as Material;
            SerializedProperty m_ShaderKeywords = materialEditor.serializedObject.FindProperty("m_ShaderKeywords");
            m_ShaderKeywords.isExpanded = EditorGUILayout.Foldout(m_ShaderKeywords.isExpanded, "Keywords", new GUIStyle(EditorStyles.foldout) { fontStyle = FontStyle.Bold });
            if (m_ShaderKeywords.isExpanded)
            {
                KeywordsGUI(material, m_ShaderKeywords);
            }
        }
        public static void ExtraPropertiesGUI(this MaterialEditor materialEditor)
        {
            if (materialEditor.targets.Length != 1)
            {
                if (GUILayout.Button("Optimize Properties"))
                {
                    for (int i = 0; i < materialEditor.targets.Length; i++)
                    {
                        Material material = materialEditor.targets[i] as Material;
                        SerializedObject serializedObject = new SerializedObject(material);
                        SerializedProperty m_SavedProperties = serializedObject.FindProperty("m_SavedProperties");
                        SerializedProperty m_TexEnvs = m_SavedProperties.FindPropertyRelative("m_TexEnvs");
                        SerializedProperty m_Floats = m_SavedProperties.FindPropertyRelative("m_Floats");
                        SerializedProperty m_Colors = m_SavedProperties.FindPropertyRelative("m_Colors");
                        OptimizeProperties(material, m_TexEnvs, m_Floats, m_Colors);
                        serializedObject.ApplyModifiedProperties();
                    }
                }
            }
            else
            {
                Material material = materialEditor.target as Material;
                SerializedObject serializedObject = materialEditor.serializedObject;
                SerializedProperty m_SavedProperties = serializedObject.FindProperty("m_SavedProperties");
                SerializedProperty m_TexEnvs = m_SavedProperties.FindPropertyRelative("m_TexEnvs");
                SerializedProperty m_Floats = m_SavedProperties.FindPropertyRelative("m_Floats");
                SerializedProperty m_Colors = m_SavedProperties.FindPropertyRelative("m_Colors");

                EditorGUILayout.BeginHorizontal();
                m_SavedProperties.isExpanded = EditorGUILayout.Foldout(m_SavedProperties.isExpanded, "Extra Properties", new GUIStyle(EditorStyles.foldout) { fontStyle = FontStyle.Bold });
                if (GUILayout.Button("Optimize"))
                {
                    OptimizeProperties(material, m_TexEnvs, m_Floats, m_Colors);
                }
                EditorGUILayout.EndHorizontal();
                if (m_SavedProperties.isExpanded)
                {
                    ExtraPropertiesGUI(material, m_TexEnvs, m_Floats, m_Colors);
                }
                serializedObject.ApplyModifiedProperties();
            }
        }

        public static void KeywordsGUI(Material material, SerializedProperty m_ShaderKeywords)
        {
            EditorGUI.indentLevel++;
            foreach (string keyword in material.shaderKeywords)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.SelectableLabel(keyword, GUILayout.Height(EditorGUIUtility.singleLineHeight));
                if (GUILayout.Button("Delete", GUILayout.Width(80)))
                {
                    string keywords = m_ShaderKeywords.stringValue;
                    m_ShaderKeywords.stringValue = keywords.Replace(keyword, "");
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel--;
        }

        public static void ExtraPropertiesGUI(Material material, SerializedProperty m_TexEnvs, SerializedProperty m_Floats, SerializedProperty m_Colors)
        {
            EditorGUI.indentLevel++;
            ExtraPropertiesGUI(material, m_TexEnvs);
            ExtraPropertiesGUI(material, m_Floats);
            ExtraPropertiesGUI(material, m_Colors);
            EditorGUI.indentLevel--;
        }
        private static void ExtraPropertiesGUI(Material material, SerializedProperty property)
        {
            for (int j = property.arraySize - 1; j >= 0; j--)
            {
                SerializedProperty propertyName = property.GetArrayElementAtIndex(j).FindPropertyRelative("first");
                if (!material.HasProperty(propertyName.stringValue))
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(propertyName.stringValue);
                    if (GUILayout.Button("Delete", GUILayout.Width(80)))
                    {
                        Debug.LogFormat(material, "Remove Property: {0} from {1}", propertyName.stringValue, material.name);
                        property.DeleteArrayElementAtIndex(j);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        public static void OptimizeProperties(Material material, SerializedProperty m_TexEnvs, SerializedProperty m_Floats, SerializedProperty m_Colors)
        {
            OptimizeProperties(material, m_TexEnvs);
            OptimizeProperties(material, m_Floats);
            OptimizeProperties(material, m_Colors);
        }
        private static void OptimizeProperties(Material material, SerializedProperty property)
        {
            for (int j = property.arraySize - 1; j >= 0; j--)
            {
                SerializedProperty propertyName = property.GetArrayElementAtIndex(j).FindPropertyRelative("first");
                if (!material.HasProperty(propertyName.stringValue))
                {
                    Debug.LogFormat(material, "Remove Property: {0} from {1}", propertyName.stringValue, material.name);
                    property.DeleteArrayElementAtIndex(j);
                }
            }
        }
    }
}
