/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-10-11 10:53:48
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZMaterialOptimizer : EditorWindow
    {
        private Material[] materials;
        private SerializedObject[] serializedMaterials;
        private SerializedProperty[] shaderKeywords;
        private SerializedProperty[] texEnvs;
        private SerializedProperty[] floats;
        private SerializedProperty[] colors;
        private Vector2 scrollPosition;

        private string propertyName1;
        private string propertyName2;

        private void GetMaterials()
        {
            materials = Selection.GetFiltered<Material>(SelectionMode.Editable | SelectionMode.Assets);
            int count = materials.Length;
            serializedMaterials = new SerializedObject[count];
            texEnvs = new SerializedProperty[count];
            floats = new SerializedProperty[count];
            colors = new SerializedProperty[count];
            shaderKeywords = new SerializedProperty[count];
            for (int i = 0; i < count; i++)
            {
                serializedMaterials[i] = new SerializedObject(materials[i]);
                shaderKeywords[i] = serializedMaterials[i].FindProperty("m_ShaderKeywords");
                SerializedProperty m_SavedProperties = serializedMaterials[i].FindProperty("m_SavedProperties");
                texEnvs[i] = m_SavedProperties.FindPropertyRelative("m_TexEnvs");
                floats[i] = m_SavedProperties.FindPropertyRelative("m_Floats");
                colors[i] = m_SavedProperties.FindPropertyRelative("m_Colors");
            }
        }

        private void OnEnable()
        {
            GetMaterials();
            Undo.undoRedoPerformed += Repaint;
        }
        private void OnDisable()
        {
            Undo.undoRedoPerformed -= Repaint;
        }

        private void OnSelectionChange()
        {
            GetMaterials();
            Repaint();
        }

        protected void OnGUI()
        {
            EZEditorGUIUtility.WindowTitle(this);

            if (materials.Length == 0)
            {
                EditorGUILayout.HelpBox("No Material Selected", MessageType.Info);
                return;
            }

            if (GUILayout.Button("Optimize Properties"))
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    EZShaderGUIUtility.OptimizeProperties(materials[i], texEnvs[i], floats[i], colors[i]);
                    serializedMaterials[i].ApplyModifiedProperties();
                }
            }

            propertyName1 = EditorGUILayout.TextField("Copy From", propertyName1);
            propertyName2 = EditorGUILayout.TextField("To", propertyName2);
            GUI.enabled = !(string.IsNullOrEmpty(propertyName1) || string.IsNullOrEmpty(propertyName2) || propertyName1 == propertyName2);
            if (GUILayout.Button("Copy Property"))
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    Material material = materials[i];
                    for (int j = 0; j < texEnvs[i].arraySize; j++)
                    {
                        SerializedProperty property1 = texEnvs[i].GetArrayElementAtIndex(j);
                        if (property1.FindPropertyRelative("first").stringValue == propertyName1)
                        {
                            Undo.RecordObject(material, "Copy Material Properties");
                            SerializedProperty texture = property1.FindPropertyRelative("second.m_Texture");
                            SerializedProperty scale = property1.FindPropertyRelative("second.m_Scale");
                            SerializedProperty offset = property1.FindPropertyRelative("second.m_Offset");
                            material.SetTexture(propertyName2, texture.objectReferenceValue as Texture);
                            material.SetTextureScale(propertyName2, scale.vector2Value);
                            material.SetTextureOffset(propertyName2, offset.vector2Value);
                            serializedMaterials[i].Update();
                        }
                    }
                    for (int j = 0; j < floats[i].arraySize; j++)
                    {
                        Undo.RecordObject(material, "Copy Material Properties");
                        SerializedProperty property1 = floats[i].GetArrayElementAtIndex(j);
                        if (property1.FindPropertyRelative("first").stringValue == propertyName1)
                        {
                            SerializedProperty second = property1.FindPropertyRelative("second");
                            material.SetFloat(propertyName2, second.floatValue);
                        }
                    }
                    for (int j = 0; j < colors[i].arraySize; j++)
                    {
                        Undo.RecordObject(material, "Copy Material Properties");
                        SerializedProperty property1 = colors[i].GetArrayElementAtIndex(j);
                        if (property1.FindPropertyRelative("first").stringValue == propertyName1)
                        {
                            SerializedProperty second = property1.FindPropertyRelative("second");
                            material.SetVector(propertyName2, second.colorValue);
                        }
                    }
                    serializedMaterials[i].Update();
                }
            }
            GUI.enabled = true;

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            for (int i = 0; i < materials.Length; i++)
            {
                EditorGUI.indentLevel++;
                serializedMaterials[i].Update();

                GUI.enabled = false;
                EditorGUILayout.ObjectField(materials[i], typeof(Material), true);
                GUI.enabled = true;

                EZShaderGUIUtility.PropertiesGUI(materials[i], texEnvs[i], floats[i], colors[i], MaterialPropertyFilter.All);
                EZShaderGUIUtility.KeywordsGUI(materials[i], shaderKeywords[i]);

                serializedMaterials[i].ApplyModifiedProperties();
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
