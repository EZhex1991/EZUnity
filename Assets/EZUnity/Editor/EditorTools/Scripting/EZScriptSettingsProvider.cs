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
using UnityEngine.Experimental.UIElements;

namespace EZUnity
{
    public class EZScriptSettingsProvider : SettingsProvider
    {
        private string UnityScriptTemplatesDirPath { get { return EditorApplication.applicationContentsPath + "/Resources/ScriptTemplates/"; } }
        private TextAsset[] newTemplates;
        private string[] allTemplates;

        private bool templatesFoldout = true;
        private bool patternFoldout = true;
        private Vector2 scrollRect;

        private EZScriptSettings settings { get { return EZScriptSettings.Instance; } }
        private SerializedObject serializedObject;

        private SerializedProperty m_TimeFormat;
        private SerializedProperty m_ExtensionList;
        private SerializedProperty m_PatternList;
        private ReorderableList patternList;

        public EZScriptSettingsProvider(string path, SettingsScope scope) : base(path, scope) { }

        [SettingsProvider]
        private static SettingsProvider CreateSettingsProvider()
        {
            SettingsProvider provider = new EZScriptSettingsProvider("Project/EZSettings/EZScript", SettingsScope.Project);
            return provider;
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            base.OnActivate(searchContext, rootElement);
            GetUnityTemplates();
            serializedObject = new SerializedObject(settings);
            m_TimeFormat = serializedObject.FindProperty("timeFormat");
            m_ExtensionList = serializedObject.FindProperty("extensionList");
            m_PatternList = serializedObject.FindProperty("patternList");
            patternList = new ReorderableList(serializedObject, m_PatternList, true, true, true, true);
            patternList.drawHeaderCallback = DrawPatternListHeader;
            patternList.drawElementCallback = DrawPatternListElement;
            GetSelectedTemplates();
            Selection.selectionChanged += GetSelectedTemplates;
        }
        public override void OnGUI(string searchContext)
        {
            serializedObject.Update();

            scrollRect = EditorGUILayout.BeginScrollView(scrollRect);
            DrawTemplateList();
            EditorGUILayout.Space();
            DrawPatternList();
            EditorGUILayout.EndScrollView();

            serializedObject.ApplyModifiedProperties();
        }
        public override void OnDeactivate()
        {
            base.OnDeactivate();
            settings.Save();
            Selection.selectionChanged -= GetSelectedTemplates;
        }

        protected void DrawTemplateList()
        {
            EditorGUILayout.LabelField("Template List", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("File Name Format: [priority]-[menu text]__[submenu text]-[default name].[ext].txt");
            DrawAddTemplates();
            templatesFoldout = EditorGUILayout.Foldout(templatesFoldout, "Templates in UnityEditor");
            if (templatesFoldout)
            {
                for (int i = 0; i < allTemplates.Length; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(" # " + i.ToString("00"), new GUILayoutOption[] { GUILayout.Width(40), });
                    EditorGUILayout.TextField(allTemplates[i]);
                    DrawDeleteTemplateButton(allTemplates[i]);
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
        protected void DrawPatternList()
        {
            EditorGUILayout.LabelField("Pattern", EditorStyles.boldLabel);

            if (GUILayout.Button("Handle patterns in selected file"))
            {
                string filePath = AssetDatabase.GetAssetPath(Selection.activeObject);
                EZScriptProcessor.Replace(filePath);
            }
            EditorGUILayout.PropertyField(m_TimeFormat);
            patternFoldout = EditorGUILayout.Foldout(patternFoldout, "Patterns");
            if (patternFoldout)
            {
                DrawConstPatternList();
                DrawCustomPatternList();
            }
            DrawExtensionList();
        }

        protected void DrawExtensionList()
        {
            EditorGUILayout.PropertyField(m_ExtensionList, true);
        }
        protected void DrawConstPatternList()
        {
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("    00", new GUILayoutOption[] { GUILayout.Width(40), });
                EditorGUILayout.LabelField("#SCRIPTNAME#", new GUILayoutOption[] { GUILayout.Width(140), });
                EditorGUILayout.LabelField("System.IO.Path.GetFileNameWithoutExtension(filePath)");
                EditorGUILayout.EndHorizontal();
            }
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("    01", new GUILayoutOption[] { GUILayout.Width(40), });
                EditorGUILayout.LabelField("#CREATETIME#", new GUILayoutOption[] { GUILayout.Width(140), });
                EditorGUILayout.LabelField("System.DateTime.Now.ToString()");
                EditorGUILayout.EndHorizontal();
            }
        }
        protected void DrawCustomPatternList()
        {
            patternList.DoLayoutList();
        }

        protected void DrawPatternListHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Custom Pattern");
        }
        protected void DrawPatternListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect.y += 1;
            SerializedProperty pattern = patternList.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.LabelField(new Rect(rect.x, rect.y, 20, EditorGUIUtility.singleLineHeight), index.ToString("00"));
            EditorGUI.PropertyField(new Rect(rect.x + 25, rect.y, 140, EditorGUIUtility.singleLineHeight), pattern.FindPropertyRelative("Key"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + 170, rect.y, rect.width - 170, EditorGUIUtility.singleLineHeight), pattern.FindPropertyRelative("Value"), GUIContent.none);
        }

        protected void DrawAddTemplates()
        {
            if (newTemplates.Length == 0)
            {
                EditorGUILayout.HelpBox("No Template File Selected", MessageType.Info);
                return;
            }
            EditorGUILayout.LabelField("Add Templates");
            EditorGUI.indentLevel++;
            Color originalColor = GUI.backgroundColor;
            for (int i = 0; i < newTemplates.Length; i++)
            {
                GUI.backgroundColor = allTemplates.Contains(newTemplates[i].name + ".txt") ? Color.red : originalColor;
                EditorGUILayout.ObjectField(newTemplates[i], typeof(TextAsset), false);
            }
            GUI.backgroundColor = originalColor;
            EditorGUI.indentLevel--;
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
                            where true
                            select Path.GetFileName(template)).ToArray();
        }
    }
}