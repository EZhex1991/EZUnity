/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-12-25 12:55:52
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UObject = UnityEngine.Object;

namespace EZUnity
{
    public class EZGraphicsSettings : SettingsProvider
    {
        public static string assetPath = "ProjectSettings/GraphicsSettings.asset";
        private static List<Shader> shadersInProject;
        private static Dictionary<Shader, int> includedShaders = new Dictionary<Shader, int>();

        public UObject target;
        public SerializedObject serializedObject;
        public SerializedProperty m_AlwaysIncludedShaders;
        public ReorderableList alwaysIncludedShaderList;

        public EZGraphicsSettings(string path, SettingsScope scope) : base(path, scope) { }

        private void RefreshShaders()
        {
            shadersInProject = EZEditorUtility.GetAllShaders(true);
            shadersInProject.Sort((s1, s2) => string.Compare(s1.name, s2.name));
        }
        private void RefreshIncludedShaders()
        {
            includedShaders.Clear();
            for (int i = 0; i < m_AlwaysIncludedShaders.arraySize; i++)
            {
                Shader shader = m_AlwaysIncludedShaders.GetArrayElementAtIndex(i).objectReferenceValue as Shader;
                if (shader == null) continue;
                includedShaders[shader] = i;
            }
        }
        private void OnRemoveShader(ReorderableList list)
        {
            if (list.serializedProperty.GetArrayElementAtIndex(list.index).objectReferenceValue == null)
            {
                list.serializedProperty.DeleteArrayElementAtIndex(list.index);
            }
            else
            {
                list.serializedProperty.DeleteArrayElementAtIndex(list.index);
                list.serializedProperty.DeleteArrayElementAtIndex(list.index);
            }
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            base.OnActivate(searchContext, rootElement);
            target = AssetDatabase.LoadAllAssetsAtPath(assetPath)[0];
            serializedObject = new SerializedObject(target);
            m_AlwaysIncludedShaders = serializedObject.FindProperty("m_AlwaysIncludedShaders");
            alwaysIncludedShaderList = new ReorderableList(serializedObject, m_AlwaysIncludedShaders, true, true, true, true)
            {
                drawHeaderCallback = DrawAlwaysIncludedShaderListHeader,
                drawElementCallback = DrawAlwaysIncludedShaderListElement,
                onChangedCallback = (list) => RefreshIncludedShaders(),
                onRemoveCallback = OnRemoveShader,
            };
            RefreshShaders();
            RefreshIncludedShaders();
        }

        private void DrawAlwaysIncludedShaderListHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Always Included Shaders");
        }
        private void DrawAlwaysIncludedShaderListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty shaderProperty = m_AlwaysIncludedShaders.GetArrayElementAtIndex(index);
            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, m_AlwaysIncludedShaders, index);
            rect.y += 1;
            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect, shaderProperty, GUIContent.none);
        }

        public override void OnGUI(string searchContext)
        {
            serializedObject.Update();

            alwaysIncludedShaderList.DoLayoutList();

            EditorGUILayout.LabelField("Shaders In Project (Supported Only)");
            for (int i = 0; i < shadersInProject.Count; i++)
            {
                Shader shader = shadersInProject[i];
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.ObjectField(shader, typeof(Shader), true);
                if (includedShaders.ContainsKey(shader))
                {
                    if (GUILayout.Button("Remove From Included"))
                    {
                        m_AlwaysIncludedShaders.DeleteArrayElementAtIndex(includedShaders[shader]);
                        m_AlwaysIncludedShaders.DeleteArrayElementAtIndex(includedShaders[shader]);
                        RefreshIncludedShaders();
                    }
                }
                else if (GUILayout.Button("Include"))
                {
                    m_AlwaysIncludedShaders.InsertArrayElementAtIndex(0);
                    m_AlwaysIncludedShaders.GetArrayElementAtIndex(0).objectReferenceValue = shader;
                    RefreshIncludedShaders();
                }
                GUI.enabled = true;
                EditorGUILayout.EndHorizontal();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}