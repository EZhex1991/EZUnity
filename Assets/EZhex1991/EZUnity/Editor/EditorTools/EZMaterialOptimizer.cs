/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-10-11 10:53:48
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZMaterialOptimizer : EditorWindow
    {
        private Material[] materials;
        private Vector2 scrollPosition;

        private void GetMaterials()
        {
            materials = Selection.GetFiltered<Material>(SelectionMode.Editable | SelectionMode.Assets);
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
                    Material material = materials[i];
                    SerializedObject serializedObject = new SerializedObject(material);
                    SerializedProperty m_SavedProperties = serializedObject.FindProperty("m_SavedProperties");
                    SerializedProperty m_TexEnvs = m_SavedProperties.FindPropertyRelative("m_TexEnvs");
                    SerializedProperty m_Floats = m_SavedProperties.FindPropertyRelative("m_Floats");
                    SerializedProperty m_Colors = m_SavedProperties.FindPropertyRelative("m_Colors");
                    EZShaderGUIUtility.OptimizeProperties(material, m_TexEnvs, m_Floats, m_Colors);
                }
            }

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            for (int i = 0; i < materials.Length; i++)
            {
                Material material = materials[i];
                SerializedObject serializedObject = new SerializedObject(material);

                GUI.enabled = false;
                EditorGUILayout.ObjectField(material, typeof(Material), true);
                GUI.enabled = true;

                EditorGUI.indentLevel++;

                EditorGUILayout.LabelField("Keywords", EditorStyles.boldLabel);
                SerializedProperty m_ShaderKeywords = serializedObject.FindProperty("m_ShaderKeywords");
                EZShaderGUIUtility.KeywordsGUI(material, m_ShaderKeywords);

                SerializedProperty m_SavedProperties = serializedObject.FindProperty("m_SavedProperties");
                SerializedProperty m_TexEnvs = m_SavedProperties.FindPropertyRelative("m_TexEnvs");
                SerializedProperty m_Floats = m_SavedProperties.FindPropertyRelative("m_Floats");
                SerializedProperty m_Colors = m_SavedProperties.FindPropertyRelative("m_Colors");
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Extra Properties", EditorStyles.boldLabel);
                if (GUILayout.Button("Optimize"))
                {
                    EZShaderGUIUtility.OptimizeProperties(material, m_TexEnvs, m_Floats, m_Colors);
                }
                EditorGUILayout.EndHorizontal();
                EZShaderGUIUtility.ExtraPropertiesGUI(material, m_TexEnvs, m_Floats, m_Colors);

                serializedObject.ApplyModifiedProperties();
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
