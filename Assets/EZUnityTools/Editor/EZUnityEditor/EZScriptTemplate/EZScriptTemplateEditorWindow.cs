/*
 * Author:      熊哲
 * CreateTime:  3/7/2017 6:22:12 PM
 * Description:
 * 
*/
using System;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZUnityEditor
{
    public class EZScriptTemplateEditorWindow : EZEditorWindow
    {
        private string UnityScriptTemplatesDirPath { get { return EditorApplication.applicationContentsPath + "/Resources/ScriptTemplates/"; } }
        private string[] allTemplates;
        private TextAsset newTemplate;

        private bool templatesFoldout = true;
        private bool patternFoldout = true;
        private Vector2 scrollRect;

        private EZScriptTemplateObject ezScriptTemplate;
        private SerializedObject so_EZScriptTemplate;
        private SerializedProperty extensionList;
        private ReorderableList patternList;

        protected override void OnEnable()
        {
            base.OnEnable();
            GetAllTemplates();
        }
        protected override void OnFocus()
        {
            base.OnFocus();
            ezScriptTemplate = EZScriptableObject.Load<EZScriptTemplateObject>(EZScriptTemplateObject.AssetName);
            so_EZScriptTemplate = new SerializedObject(ezScriptTemplate);
            extensionList = so_EZScriptTemplate.FindProperty("extensionList");
            patternList = new ReorderableList(so_EZScriptTemplate, so_EZScriptTemplate.FindProperty("patternList"), true, true, true, true);
            patternList.drawHeaderCallback = DrawPatternListHeader;
            patternList.drawElementCallback = DrawPatternListElement;
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            so_EZScriptTemplate.Update();

            scrollRect = EditorGUILayout.BeginScrollView(scrollRect);
            DrawTemplateList();
            EditorGUILayout.Space();
            DrawPatternList();
            EditorGUILayout.EndScrollView();

            so_EZScriptTemplate.ApplyModifiedProperties();
        }

        protected void DrawTemplateList()
        {
            EditorGUILayout.LabelField("Template List", subtitleStyle);
            DrawAddTemplateButton();
            templatesFoldout = EditorGUILayout.Foldout(templatesFoldout, "Templates in UnityEditor");
            if (templatesFoldout)
            {
                for (int i = 0; i < allTemplates.Length; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(" # " + i.ToString("00"), new GUILayoutOption[] { GUILayout.Width(40), });
                    EditorGUILayout.TextField(Path.GetFileName(allTemplates[i]));
                    DrawDeleteTemplateButton(allTemplates[i]);
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
        protected void DrawPatternList()
        {
            EditorGUILayout.LabelField("Pattern", subtitleStyle);
            DrawHandlePatternButton();
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
            EditorGUILayout.PropertyField(extensionList, true);
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

        protected void DrawAddTemplateButton()
        {
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Add Template", new GUILayoutOption[] { GUILayout.Width(100), });
                newTemplate = EditorGUILayout.ObjectField(newTemplate, typeof(TextAsset), false) as TextAsset;
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.LabelField("File Name Format: [priority]-[menu text]__[submenu text]-[default name].[ext].txt");
            if (GUILayout.Button("Add template to UnityEditor path"))
            {
                if (newTemplate == null) return;
                string tplPath = AssetDatabase.GetAssetPath(newTemplate);
                try
                {
                    File.Copy(tplPath, UnityScriptTemplatesDirPath + newTemplate.name + ".txt", true);
                    GetAllTemplates();
                    Debug.Log("Copy template success: " + newTemplate.name + ", you may need to restart unity to apply changes.");
                }
                catch (Exception ex)
                {
                    Debug.LogError("Copy template failed: " + newTemplate.name + "\n" + ex.Message);
                }
                newTemplate = null;
            }
        }
        protected void DrawDeleteTemplateButton(string tplPath)
        {
            if (GUILayout.Button("Delete", new GUILayoutOption[] { GUILayout.Width(80), }))
            {
                try
                {
                    File.Delete(tplPath);
                    GetAllTemplates();
                    Debug.Log("Delete template complete: " + tplPath + ", restart unity to apply changes.");
                }
                catch (Exception ex)
                {
                    Debug.LogError("Delete template failed: " + tplPath + "\n" + ex.Message);
                }
            }
        }
        protected void DrawHandlePatternButton()
        {
            if (GUILayout.Button("Handle patterns in selected file"))
            {
                string filePath = AssetDatabase.GetAssetPath(Selection.activeObject);
                EZScriptTemplateProcessor.Replace(filePath, ezScriptTemplate);
            }
        }

        private void GetAllTemplates()
        {
            allTemplates = Directory.GetFiles(UnityScriptTemplatesDirPath, "*.txt", SearchOption.TopDirectoryOnly);
        }
    }
}