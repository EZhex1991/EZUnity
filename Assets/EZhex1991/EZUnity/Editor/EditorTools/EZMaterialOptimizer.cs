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
        private Vector2 scrollPosition;

        private void GetMaterials()
        {
            materials = Selection.GetFiltered<Material>(SelectionMode.Editable | SelectionMode.Assets);
            serializedMaterials = new SerializedObject[materials.Length];
            for (int i = 0; i < materials.Length; i++)
            {
                serializedMaterials[i] = new SerializedObject(materials[i]);
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
                    Material material = materials[i];
                    SerializedObject serializedObject = serializedMaterials[i];
                    SerializedProperty m_SavedProperties = serializedObject.FindProperty("m_SavedProperties");
                    SerializedProperty m_TexEnvs = m_SavedProperties.FindPropertyRelative("m_TexEnvs");
                    SerializedProperty m_Floats = m_SavedProperties.FindPropertyRelative("m_Floats");
                    SerializedProperty m_Colors = m_SavedProperties.FindPropertyRelative("m_Colors");
                    EZShaderGUIUtility.OptimizeProperties(material, m_TexEnvs, m_Floats, m_Colors);
                    serializedObject.ApplyModifiedProperties();
                }
            }

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            for (int i = 0; i < materials.Length; i++)
            {
                Material material = materials[i];
                SerializedObject serializedObject = serializedMaterials[i];
                EditorGUI.indentLevel++;
                serializedObject.Update();

                GUI.enabled = false;
                EditorGUILayout.ObjectField(material, typeof(Material), true);
                GUI.enabled = true;

                SerializedProperty m_SavedProperties = serializedObject.FindProperty("m_SavedProperties");
                SerializedProperty m_TexEnvs = m_SavedProperties.FindPropertyRelative("m_TexEnvs");
                SerializedProperty m_Floats = m_SavedProperties.FindPropertyRelative("m_Floats");
                SerializedProperty m_Colors = m_SavedProperties.FindPropertyRelative("m_Colors");
                SerializedProperty m_ShaderKeywords = serializedObject.FindProperty("m_ShaderKeywords");

                EZShaderGUIUtility.PropertiesGUI(material, m_TexEnvs, m_Floats, m_Colors, MaterialPropertyFilter.All);
                EZShaderGUIUtility.KeywordsGUI(material, m_ShaderKeywords);

                serializedObject.ApplyModifiedProperties();
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
