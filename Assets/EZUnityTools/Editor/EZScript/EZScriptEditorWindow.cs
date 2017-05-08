/*
 * Author:      熊哲
 * CreateTime:  3/7/2017 6:22:12 PM
 * Description:
 * 
*/
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EZUnityTools.EZEditor
{
    public class EZScriptEditorWindow : EZEditorWindow
    {
        private string UnityScriptTemplatesDirPath { get { return EditorApplication.applicationContentsPath + "/Resources/ScriptTemplates/"; } }

        private EZScriptObject ezScript;
        private int templateListSize;
        private int extensionListSize;
        private int patternListSize;

        protected override void OnFocus()
        {
            base.OnFocus();
            ezScript = EZScriptableObject.Load<EZScriptObject>(EZScriptObject.AssetName);
            templateListSize = ezScript.templateList.Count;
            extensionListSize = ezScript.extensionList.Count;
            patternListSize = ezScript.patternList.Count;
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            EditorGUILayout.Space();
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Templates", subtitleStyle, new GUILayoutOption[] { GUILayout.Width(80), });
                EditorGUILayout.LabelField("File Name Format: [priority]-[menu text]-[default name]");
                EditorGUILayout.EndHorizontal();
            }
            OnGUI_TemplateList();
            OnGUI_TemplateControl();
            EditorGUILayout.Space();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Pattern", subtitleStyle);
            OnGUI_ExtensionList();
            OnGUI_ConstPatternList();
            OnGUI_CustomPatternList();
            OnGUI_PatternControl();
            EditorGUILayout.Space();

            if (GUI.changed) EditorUtility.SetDirty(ezScript);
        }
        protected void OnGUI_TemplateList()
        {
            EditorGUILayout.Space();
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Templates", new GUILayoutOption[] { GUILayout.Width(100), });
                EditorGUILayout.LabelField("Size", new GUILayoutOption[] { GUILayout.Width(40), });
                templateListSize = EditorGUILayout.DelayedIntField(templateListSize); templateListSize = templateListSize < 0 ? 0 : templateListSize;
                EditorGUILayout.EndHorizontal();
            }
            while (templateListSize != ezScript.templateList.Count)
            {
                if (templateListSize < ezScript.templateList.Count) ezScript.templateList.RemoveAt(templateListSize);
                else ezScript.templateList.Add(new TextAsset());
            }
            for (int i = 0; i < ezScript.templateList.Count; i++)
            {
                ezScript.templateList[i] = EditorGUILayout.ObjectField(ezScript.templateList[i], typeof(TextAsset), false) as TextAsset;
            }
        }
        protected void OnGUI_ExtensionList()
        {
            EditorGUILayout.Space();
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Extensions", new GUILayoutOption[] { GUILayout.Width(100), });
                EditorGUILayout.LabelField("Size", new GUILayoutOption[] { GUILayout.Width(40), });
                extensionListSize = EditorGUILayout.DelayedIntField(extensionListSize); extensionListSize = extensionListSize < 0 ? 0 : extensionListSize;
                EditorGUILayout.EndHorizontal();
            }
            while (extensionListSize != ezScript.extensionList.Count)
            {
                if (extensionListSize < ezScript.extensionList.Count) ezScript.extensionList.RemoveAt(extensionListSize);
                else ezScript.extensionList.Add("");
            }
            {
                EditorGUILayout.BeginHorizontal();
                for (int i = 0; i < ezScript.extensionList.Count; i++)
                {
                    ezScript.extensionList[i] = GUILayout.TextField(ezScript.extensionList[i], new GUILayoutOption[] { GUILayout.Width(40), });
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        protected void OnGUI_ConstPatternList()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Const");
            EditorGUILayout.LabelField("#SCRIPTNAME#", "System.IO.Path.GetFileNameWithoutExtension(filePath)");
            EditorGUILayout.LabelField("#CREATETIME#", "System.DateTime.Now.ToString()");
        }
        protected void OnGUI_CustomPatternList()
        {
            EditorGUILayout.Space();
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Custom", new GUILayoutOption[] { GUILayout.Width(100), });
                EditorGUILayout.LabelField("Size", new GUILayoutOption[] { GUILayout.Width(40), });
                patternListSize = EditorGUILayout.DelayedIntField(patternListSize); patternListSize = patternListSize < 0 ? 0 : patternListSize;
                EditorGUILayout.EndHorizontal();
            }
            while (patternListSize != ezScript.patternList.Count)
            {
                if (patternListSize < ezScript.patternList.Count) ezScript.patternList.RemoveAt(patternListSize);
                else ezScript.patternList.Add(new EZScriptObject.Pattern());
            }
            for (int i = 0; i < ezScript.patternList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(i.ToString(), new GUILayoutOption[] { GUILayout.Width(20), });
                ezScript.patternList[i].Key = EditorGUILayout.TextField(ezScript.patternList[i].Key);
                ezScript.patternList[i].Value = EditorGUILayout.TextField(ezScript.patternList[i].Value);
                EditorGUILayout.EndHorizontal();
            }
        }
        protected void OnGUI_TemplateControl()
        {
            EditorGUILayout.Space();
            if (GUILayout.Button("Copy Templates To UnityEditor Path"))
            {
                for (int i = 0; i < ezScript.templateList.Count; i++)
                {
                    TextAsset tpl = ezScript.templateList[i];
                    string tplPath = AssetDatabase.GetAssetPath(tpl);
                    try
                    {
                        File.Copy(tplPath, UnityScriptTemplatesDirPath + tpl.name + ".txt", true);
                        Debug.Log("Copy Template Success: " + tpl.name);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Copy Template Failed: " + tpl.name + "\n" + ex.Message);
                    }
                }
                Debug.Log("Complete, restart unity to apply changes.");
            }
        }
        protected void OnGUI_PatternControl()
        {
            EditorGUILayout.Space();
            if (GUILayout.Button("Replace Selected File"))
            {
                string filePath = AssetDatabase.GetAssetPath(Selection.activeObject);
                EZScriptProcessor.Replace(filePath, ezScript);
            }
        }
    }
}