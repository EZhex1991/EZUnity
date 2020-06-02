/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-02-25 20:16:03
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public enum MaterialPropertyFilter
    {
        All,
        Exposed,
        Extra,
    }

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
            KeywordsGUI(material, m_ShaderKeywords);
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

                TexturePropertiesGUI(material, m_TexEnvs, MaterialPropertyFilter.Extra);
                PropertiesGUI(material, m_Floats, MaterialPropertyFilter.Extra);
                PropertiesGUI(material, m_Colors, MaterialPropertyFilter.Extra);

                serializedObject.ApplyModifiedProperties();
            }
        }

        public static void KeywordsGUI(Material material, SerializedProperty m_ShaderKeywords)
        {
            m_ShaderKeywords.isExpanded = EditorGUILayout.Foldout(m_ShaderKeywords.isExpanded, m_ShaderKeywords.displayName, true, new GUIStyle(EditorStyles.foldout) { fontStyle = FontStyle.Bold });
            if (m_ShaderKeywords.isExpanded)
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
        }

        private static bool FilterProperty(bool hasProperty, MaterialPropertyFilter filter)
        {
            if (filter == MaterialPropertyFilter.All) return true;
            else if (filter == MaterialPropertyFilter.Exposed) return hasProperty;
            else if (filter == MaterialPropertyFilter.Extra) return !hasProperty;
            return true;
        }
        public static void PropertiesGUI(Material material, SerializedProperty m_TexEnvs, SerializedProperty m_Floats, SerializedProperty m_Colors, MaterialPropertyFilter filter)
        {
            TexturePropertiesGUI(material, m_TexEnvs, filter);
            PropertiesGUI(material, m_Floats, filter);
            PropertiesGUI(material, m_Colors, filter);
        }
        private static void PropertiesGUI(Material material, SerializedProperty property, MaterialPropertyFilter filter)
        {
            EditorGUILayout.BeginHorizontal();
            property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, filter + " " + property.displayName, true, new GUIStyle(EditorStyles.foldout) { fontStyle = FontStyle.Bold });
            if (GUILayout.Button("Optimize"))
            {
                OptimizeProperties(material, property);
            }
            EditorGUILayout.EndHorizontal();

            if (property.isExpanded)
            {
                EditorGUIUtility.labelWidth = 0;
                EditorGUI.indentLevel++;
                for (int i = property.arraySize - 1; i >= 0; i--)
                {
                    SerializedProperty child = property.GetArrayElementAtIndex(i);
                    SerializedProperty propertyName = child.FindPropertyRelative("first");

                    bool hasProperty = material.HasProperty(propertyName.stringValue);
                    if (FilterProperty(hasProperty, filter))
                    {
                        SerializedProperty propertyValue = child.FindPropertyRelative("second");
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(propertyValue, new GUIContent(propertyName.stringValue));
                        GUI.enabled = !hasProperty;
                        if (GUILayout.Button("Delete", GUILayout.Width(80)))
                        {
                            Debug.LogFormat(material, "Remove Property: {0} from {1}", propertyName.stringValue, material.name);
                            property.DeleteArrayElementAtIndex(i);
                        }
                        GUI.enabled = true;
                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUI.indentLevel--;
            }
        }
        private static void TexturePropertiesGUI(Material material, SerializedProperty property, MaterialPropertyFilter filter)
        {
            EditorGUILayout.BeginHorizontal();
            property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, filter + " " + property.displayName, true, new GUIStyle(EditorStyles.foldout) { fontStyle = FontStyle.Bold });
            if (GUILayout.Button("Optimize"))
            {
                OptimizeProperties(material, property);
            }
            EditorGUILayout.EndHorizontal();

            if (property.isExpanded)
            {
                EditorGUIUtility.labelWidth = 0;
                EditorGUI.indentLevel++;
                for (int i = property.arraySize - 1; i >= 0; i--)
                {
                    SerializedProperty child = property.GetArrayElementAtIndex(i);
                    SerializedProperty propertyName = child.FindPropertyRelative("first");

                    bool hasProperty = material.HasProperty(propertyName.stringValue);
                    if (FilterProperty(hasProperty, filter))
                    {
                        SerializedProperty texEnv = child.FindPropertyRelative("second");
                        SerializedProperty texture = texEnv.FindPropertyRelative("m_Texture");
                        SerializedProperty scale = texEnv.FindPropertyRelative("m_Scale");
                        SerializedProperty offset = texEnv.FindPropertyRelative("m_Offset");

                        EditorGUILayout.BeginHorizontal();
                        texEnv.isExpanded = EditorGUILayout.Foldout(texEnv.isExpanded, propertyName.stringValue, true);
                        if (!texEnv.isExpanded)
                        {
                            EditorGUILayout.PropertyField(texture, GUIContent.none);
                        }
                        GUI.enabled = !hasProperty;
                        if (GUILayout.Button("Delete", GUILayout.Width(80)))
                        {
                            Debug.LogFormat(material, "Remove Property: {0} from {1}", propertyName.stringValue, material.name);
                            property.DeleteArrayElementAtIndex(i);
                        }
                        GUI.enabled = true;
                        EditorGUILayout.EndHorizontal();

                        if (texEnv.isExpanded)
                        {
                            EditorGUIUtility.wideMode = true;
                            EditorGUI.indentLevel++;
                            EditorGUILayout.PropertyField(texture);
                            EditorGUILayout.PropertyField(scale);
                            EditorGUILayout.PropertyField(offset);
                            EditorGUI.indentLevel--;
                        }
                    }
                }
                EditorGUI.indentLevel--;
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
