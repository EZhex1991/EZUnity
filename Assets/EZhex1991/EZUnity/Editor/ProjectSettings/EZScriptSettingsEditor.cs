/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-03-07 18:22:12
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CustomEditor(typeof(EZScriptSettings))]
    public class EZScriptSettingsEditor : Editor
    {
        private string UnityScriptTemplatesDirPath { get { return EditorApplication.applicationContentsPath + "/Resources/ScriptTemplates/"; } }
        private TextAsset[] newTemplates;
        private string[] allTemplates;

        private Vector2 scrollRect;

        private EZScriptSettings settings;
        private SerializedProperty m_TimeFormat;
        private SerializedProperty m_ExtensionList;
        private SerializedProperty m_PatternList;
        private ReorderableList patternList;
        private ReorderableList extensionList;

        public void OnEnable()
        {
            settings = target as EZScriptSettings;
            GetUnityTemplates();
            m_TimeFormat = serializedObject.FindProperty("timeFormat");
            m_ExtensionList = serializedObject.FindProperty("extensionList");
            m_PatternList = serializedObject.FindProperty("patternList");
            patternList = new ReorderableList(serializedObject, m_PatternList, true, true, true, true)
            {
                drawHeaderCallback = DrawPatternListHeader,
                drawElementCallback = DrawPatternListElement
            };
            extensionList = new ReorderableList(serializedObject, m_ExtensionList, true, true, true, true)
            {
                drawHeaderCallback = DrawExtensionListHeader,
                drawElementCallback = DrawExtensionListElement
            };
            GetSelectedTemplates();
            Selection.selectionChanged += GetSelectedTemplates;
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            scrollRect = EditorGUILayout.BeginScrollView(scrollRect);
            DrawAddTemplates();
            DrawTemplateList();
            EditorGUILayout.Space();
            DrawPatternList();
            EditorGUILayout.EndScrollView();

            serializedObject.ApplyModifiedProperties();
        }
        public void OnDisable()
        {
            settings.Save();
            Selection.selectionChanged -= GetSelectedTemplates;
        }

        protected void DrawTemplateList()
        {
            EditorGUILayout.LabelField("Template List", EditorStyles.boldLabel);
            for (int i = 0; i < allTemplates.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(" # " + i.ToString("00"), new GUILayoutOption[] { GUILayout.Width(40), });
                EditorGUILayout.TextField(allTemplates[i]);
                DrawDeleteTemplateButton(allTemplates[i]);
                EditorGUILayout.EndHorizontal();
            }
        }
        protected void DrawPatternList()
        {
            EditorGUILayout.LabelField("Patterns", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_TimeFormat);
            if (GUILayout.Button("Handle patterns in selected file"))
            {
                string filePath = AssetDatabase.GetAssetPath(Selection.activeObject);
                EZScriptProcessor.Replace(filePath);
            }
            EditorGUILayout.LabelField("00 #SCRIPTNAME#", "System.IO.Path.GetFileNameWithoutExtension(filePath)");
            EditorGUILayout.LabelField("01 #CREATETIME#", "System.DateTime.Now.ToString()");
            patternList.DoLayoutList();
            extensionList.DoLayoutList();
        }

        protected void DrawExtensionListHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Associations");
        }
        protected void DrawExtensionListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, index, extensionList);
            rect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty extension = extensionList.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, extension, GUIContent.none);
        }

        protected void DrawPatternListHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Custom Pattern");
        }
        protected void DrawPatternListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, index, patternList);
            SerializedProperty pattern = patternList.serializedProperty.GetArrayElementAtIndex(index);
            float width = rect.width / 2; float margin = 5;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width - margin, EditorGUIUtility.singleLineHeight), pattern.FindPropertyRelative("Key"), GUIContent.none);
            rect.x += width;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width - margin, EditorGUIUtility.singleLineHeight), pattern.FindPropertyRelative("Value"), GUIContent.none);
        }

        protected void DrawAddTemplates()
        {
            EditorGUILayout.LabelField("Add Templates", EditorStyles.boldLabel);
            if (newTemplates.Length == 0)
            {
                EditorGUILayout.HelpBox("No Template File Selected\n\nFile Name Format: [priority]-[menu text]__[submenu text]-[default name].[ext].txt", MessageType.Info);
            }
            else
            {
                EditorGUI.indentLevel++;
                Color originalColor = GUI.backgroundColor;
                for (int i = 0; i < newTemplates.Length; i++)
                {
                    GUI.backgroundColor = allTemplates.Contains(newTemplates[i].name + ".txt") ? Color.red : originalColor;
                    EditorGUILayout.ObjectField(newTemplates[i], typeof(TextAsset), false);
                }
                GUI.backgroundColor = originalColor;
                EditorGUI.indentLevel--;
            }
            if (GUILayout.Button("Add selected files to templates (red indicate replacement)"))
            {
                foreach (TextAsset template in newTemplates)
                {
                    string tmplPath = AssetDatabase.GetAssetPath(template);
                    try
                    {
                        File.Copy(tmplPath, UnityScriptTemplatesDirPath + template.name + ".txt", true);
                        GetUnityTemplates();
                        Debug.Log("Copy template success: " + template.name + ", you may need to restart unity to apply changes.");
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Copy template failed: " + template.name + "\n" + ex.Message);
                    }
                }
            }
            if (GUILayout.Button("Open Template Folder"))
            {
                try
                {
                    Application.OpenURL(UnityScriptTemplatesDirPath);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
        protected void DrawDeleteTemplateButton(string tplName)
        {
            if (GUILayout.Button("Delete", new GUILayoutOption[] { GUILayout.Width(80), }))
            {
                try
                {
                    File.Delete(UnityScriptTemplatesDirPath + tplName);
                    GetUnityTemplates();
                    Debug.Log("Delete template complete: " + tplName + ", restart unity to apply changes.");
                }
                catch (Exception ex)
                {
                    Debug.LogError("Delete template failed: " + tplName + "\n" + ex.Message);
                }
            }
        }

        private void GetSelectedTemplates()
        {
            newTemplates = (from template in Selection.GetFiltered<TextAsset>(SelectionMode.Assets)
                            where EZScriptProcessor.CheckTemplate(template.name + ".txt") == EZScriptProcessor.CheckResult.Template
                            select template).ToArray();
            Repaint();
        }
        private void GetUnityTemplates()
        {
            allTemplates = (from template in Directory.GetFiles(UnityScriptTemplatesDirPath, "*.txt", SearchOption.TopDirectoryOnly)
                            where EZScriptProcessor.CheckTemplate(template) == EZScriptProcessor.CheckResult.Template
                            select Path.GetFileName(template))
                            .OrderBy(fileName => int.Parse(fileName.Split('-')[0]))
                            .ToArray();
        }
    }
}