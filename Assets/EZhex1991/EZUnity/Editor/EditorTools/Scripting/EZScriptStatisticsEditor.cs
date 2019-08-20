/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-02-22 17:44:56
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CustomEditor(typeof(EZScriptStatistics))]
    public class EZScriptStatisticsEditor : Editor
    {
        SerializedProperty m_FilePatterns;
        SerializedProperty m_IncludePaths;
        SerializedProperty m_ExcludePaths;
        SerializedProperty m_InfoLineCount;
        SerializedProperty m_AuthorRegex;
        SerializedProperty m_CreateTimeRegex;
        SerializedProperty m_ValidLineRegex;
        SerializedProperty m_ShowAsset;

        EZScriptStatistics targetObject;
        private Vector2 scrollPosition;

        void OnEnable()
        {
            m_FilePatterns = serializedObject.FindProperty("filePatterns");
            m_IncludePaths = serializedObject.FindProperty("includePaths");
            m_ExcludePaths = serializedObject.FindProperty("excludePaths");
            m_InfoLineCount = serializedObject.FindProperty("infoLineCount");
            m_AuthorRegex = serializedObject.FindProperty("authorRegex");
            m_CreateTimeRegex = serializedObject.FindProperty("createTimeRegex");
            m_ValidLineRegex = serializedObject.FindProperty("validLineRegex");
            m_ShowAsset = serializedObject.FindProperty("showAsset");
            targetObject = target as EZScriptStatistics;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EZEditorGUIUtility.ScriptableObjectTitle(target as ScriptableObject, !serializedObject.isEditingMultipleObjects);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            EditorGUILayout.LabelField("File Association", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_FilePatterns, true);
            EditorGUILayout.PropertyField(m_IncludePaths, true);
            EditorGUILayout.PropertyField(m_ExcludePaths, true);
            EditorGUI.indentLevel--;

            EditorGUILayout.LabelField("Analysis", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_InfoLineCount);
            EditorGUILayout.PropertyField(m_AuthorRegex);
            EditorGUILayout.PropertyField(m_CreateTimeRegex);
            EditorGUILayout.PropertyField(m_ValidLineRegex);
            EditorGUILayout.HelpBox(new StringBuilder()
                .AppendLine(@"'^\W*(\w+)[\S\s]*$': line contains word characters")
                .AppendLine(@"'^\W*(\S+)[\S\s]*$': line contains non-white-space characters")
                .ToString(), MessageType.Info);
            EditorGUI.indentLevel--;

            if (GUILayout.Button("Refresh"))
            {
                RefreshResult();
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Result: " + targetObject.resultTime, EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_ShowAsset);
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel++;
            DrawResult(targetObject);
            EditorGUI.indentLevel--;

            EditorGUILayout.EndScrollView();
            serializedObject.ApplyModifiedProperties();
        }

        public void RefreshResult()
        {
            EditorUtility.DisplayProgressBar("EZScriptStatistics", "Analysing", 0);
            targetObject.resultTime = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            targetObject.result.Clear();
            PathNormalize(m_IncludePaths);
            PathNormalize(m_ExcludePaths);
            List<string> scripts = new List<string>();
            for (int i = 0; i < m_IncludePaths.arraySize; i++)
            {
                string includePath = m_IncludePaths.GetArrayElementAtIndex(i).stringValue;
                if (string.IsNullOrEmpty(includePath)) continue;
                for (int j = 0; j < m_FilePatterns.arraySize; j++)
                {
                    string pattern = m_FilePatterns.GetArrayElementAtIndex(j).stringValue;
                    if (string.IsNullOrEmpty(pattern)) continue;
                    scripts.AddRange(Directory.GetFiles(includePath, pattern, SearchOption.AllDirectories));
                }
            }
            PathNormalize(scripts);
            var scriptInfoList = (from scriptPath in scripts
                                  where IsValidPath(scriptPath)
                                  select GetScriptInfo(scriptPath)).ToList();
            var authorContributions = scriptInfoList.GroupBy(info => info.author);
            int totalValidLineCount = 0;
            foreach (var authorContribution in authorContributions)
            {
                Contributor contributor = new Contributor(authorContribution.Key);
                foreach (ScriptInfo info in authorContribution.Cast<ScriptInfo>())
                {
                    contributor.scriptList.Add(info);
                    contributor.lineCount += info.lineCount;
                    contributor.validLineCount += info.validLineCount;
                    totalValidLineCount += info.validLineCount;
                }
                targetObject.result.Add(contributor);
            }
            foreach (Contributor contributor in targetObject.result)
            {
                contributor.proportion = (float)contributor.validLineCount / totalValidLineCount;
            }
            EditorUtility.ClearProgressBar();
            serializedObject.ApplyModifiedProperties();
            if (targetObject.result.Count != 0)
            {
                string assetPath = AssetDatabase.GetAssetPath(target);
                string fileName = EZScriptStatistics.AssetName + "-" + targetObject.resultTime;
                string filePath = assetPath.Replace(target.name, fileName);
                AssetDatabase.CreateAsset(Instantiate(target as EZScriptStatistics), filePath);
            }
        }
        private bool IsValidPath(string filePath)
        {
            for (int i = 0; i < m_ExcludePaths.arraySize; i++)
            {
                string excludePath = m_ExcludePaths.GetArrayElementAtIndex(i).stringValue;
                if (string.IsNullOrEmpty(excludePath)) continue;
                if (filePath.Contains(excludePath)) return false;
            }
            return true;
        }
        private ScriptInfo GetScriptInfo(string filePath)
        {
            return GetScriptInfo(filePath, Encoding.UTF8);
        }
        private ScriptInfo GetScriptInfo(string filePath, Encoding encoding)
        {
            string relativePath = filePath.Replace(Application.dataPath, "");
            ScriptInfo info = new ScriptInfo(relativePath);
            info.fileObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(relativePath);
            string[] lines = File.ReadAllLines(filePath, encoding);
            info.lineCount = lines.Length;
            for (int i = 0; i < lines.Length; i++)
            {
                if (i < m_InfoLineCount.intValue)
                {
                    Match match;
                    match = Regex.Match(lines[i], m_AuthorRegex.stringValue);
                    if (match.Success)
                    {
                        info.author = match.Groups[1].Value;
                    }
                    match = Regex.Match(lines[i], m_CreateTimeRegex.stringValue);
                    if (match.Success)
                    {
                        info.createTime = match.Groups[1].Value;
                    }
                }
                if (Regex.IsMatch(lines[i], m_ValidLineRegex.stringValue)) info.validLineCount++;
            }
            return info;
        }

        private void PathNormalize(SerializedProperty arrayProperty)
        {
            for (int i = 0; i < arrayProperty.arraySize; i++)
            {
                SerializedProperty element = arrayProperty.GetArrayElementAtIndex(i);
                element.stringValue = element.stringValue.Replace("\\", "/");
            }
        }
        private void PathNormalize(List<string> stringList)
        {
            for (int i = 0; i < stringList.Count; i++)
            {
                stringList[i] = stringList[i].Replace("\\", "/");
            }
        }

        private void DrawResult(EZScriptStatistics targetObject)
        {
            foreach (Contributor contributor in targetObject.result)
            {
                EditorGUILayout.BeginHorizontal();
                float unitWidth = Math.Min(60, EditorGUIUtility.currentViewWidth / 10);
                contributor.foldout = EditorGUILayout.Foldout(contributor.foldout, contributor.author);
                EditorGUILayout.LabelField("proportion: " + contributor.proportion.ToString("00.00%"), new GUILayoutOption[] { GUILayout.Width(unitWidth * 2) });
                EditorGUILayout.LabelField("file count: " + contributor.scriptList.Count.ToString(), new GUILayoutOption[] { GUILayout.Width(unitWidth * 2) });
                EditorGUILayout.TextField(contributor.lineCount.ToString(), new GUILayoutOption[] { GUILayout.Width(unitWidth) });
                EditorGUILayout.TextField(contributor.validLineCount.ToString(), new GUILayoutOption[] { GUILayout.Width(unitWidth) });
                EditorGUILayout.EndHorizontal();
                if (contributor.foldout)
                {
                    foreach (ScriptInfo script in contributor.scriptList)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUI.indentLevel++;
                        if (m_ShowAsset.boolValue) EditorGUILayout.ObjectField(script.fileObject, typeof(UnityEngine.Object), true);
                        else EditorGUILayout.TextField(script.filePath);
                        EditorGUI.indentLevel--;
                        EditorGUILayout.TextField(script.createTime, new GUILayoutOption[] { GUILayout.Width(unitWidth * 3) });
                        EditorGUILayout.TextField(script.lineCount.ToString(), new GUILayoutOption[] { GUILayout.Width(unitWidth) });
                        EditorGUILayout.TextField(script.validLineCount.ToString(), new GUILayoutOption[] { GUILayout.Width(unitWidth) });
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
        }
    }
}