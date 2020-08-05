/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-12-25 12:55:52
 * Organization:    #ORGANIZATION#
 * Description:     
 */
#if UNITY_2018_3_OR_NEWER
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

#if UNITY_2019_1_OR_NEWER
using UnityEngine.UIElements;
#else
using UnityEngine.Experimental.UIElements;
#endif

using UObject = UnityEngine.Object;

namespace EZhex1991.EZUnity
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
            serializedObject.Update();
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
            list.index--;
            serializedObject.ApplyModifiedProperties();
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
            Undo.undoRedoPerformed += RefreshIncludedShaders;
        }
        public override void OnDeactivate()
        {
            base.OnDeactivate();
            Undo.undoRedoPerformed -= RefreshIncludedShaders;
        }

        private void DrawAlwaysIncludedShaderListHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Always Included Shaders");
        }
        private void DrawAlwaysIncludedShaderListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty shaderProperty = m_AlwaysIncludedShaders.GetArrayElementAtIndex(index);
            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, index, alwaysIncludedShaderList);
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
                        serializedObject.ApplyModifiedProperties();
                        RefreshIncludedShaders();
                    }
                }
                else if (GUILayout.Button("Include"))
                {
                    m_AlwaysIncludedShaders.InsertArrayElementAtIndex(0);
                    m_AlwaysIncludedShaders.GetArrayElementAtIndex(0).objectReferenceValue = shader;
                    serializedObject.ApplyModifiedProperties();
                    RefreshIncludedShaders();
                }
                GUI.enabled = true;
                EditorGUILayout.EndHorizontal();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
