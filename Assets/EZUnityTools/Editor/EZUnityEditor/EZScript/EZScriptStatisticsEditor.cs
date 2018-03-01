/* Author:          熊哲
 * CreateTime:      2018-02-22 17:44:56
 * Orgnization:     #ORGNIZATION#
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

namespace EZUnityEditor
{
    [CustomEditor(typeof(EZScriptStatisticsObject))]
    public class EZScriptStatisticsEditor : Editor
    {
        SerializedProperty m_FilePatterns;
        SerializedProperty m_IncludePaths;
        SerializedProperty m_ExcludePaths;
        SerializedProperty m_InfoLineCount;
        SerializedProperty m_AuthorRegex;
        SerializedProperty m_CreateTimeRegex;
        SerializedProperty m_ValidLineRegex;
        SerializedProperty m_AutoBackup;

        EZScriptStatisticsObject targetObject;

        void OnEnable()
        {
            m_FilePatterns = serializedObject.FindProperty("filePatterns");
            m_IncludePaths = serializedObject.FindProperty("includePaths");
            m_ExcludePaths = serializedObject.FindProperty("excludePaths");
            m_InfoLineCount = serializedObject.FindProperty("infoLineCount");
            m_AuthorRegex = serializedObject.FindProperty("authorRegex");
            m_CreateTimeRegex = serializedObject.FindProperty("createTimeRegex");
            m_ValidLineRegex = serializedObject.FindProperty("validLineRegex");
            m_AutoBackup = serializedObject.FindProperty("autoBackup");
            targetObject = target as EZScriptStatisticsObject;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            if (targetObject.result.Count != 0)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Result Time: " + targetObject.resultTime);
                if (GUILayout.Button("Refresh"))
                {
                    RefreshResult();
                    ShowResult();
                }
                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("Show Result"))
            {
                if (targetObject.result.Count == 0)
                {
                    RefreshResult();
                }
                ShowResult();
            }
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
            EditorGUILayout.PropertyField(m_AutoBackup);
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
            if (m_AutoBackup.boolValue)
            {
                EZScriptableObject.Create(EZScriptStatisticsObject.AssetName + "-" + targetObject.resultTime, Instantiate(target as EZScriptStatisticsObject));
            }
        }
        public bool IsValidPath(string filePath)
        {
            for (int i = 0; i < m_ExcludePaths.arraySize; i++)
            {
                string excludePath = m_ExcludePaths.GetArrayElementAtIndex(i).stringValue;
                if (string.IsNullOrEmpty(excludePath)) continue;
                if (filePath.Contains(excludePath)) return false;
            }
            return true;
        }
        public ScriptInfo GetScriptInfo(string filePath)
        {
            return GetScriptInfo(filePath, Encoding.UTF8);
        }
        public ScriptInfo GetScriptInfo(string filePath, Encoding encoding)
        {
            string relativePath = filePath.Replace(Application.dataPath, "");
            ScriptInfo info = new ScriptInfo(relativePath);
            info.fileObject = AssetDatabase.LoadAssetAtPath<TextAsset>(relativePath);
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

        public void ShowResult()
        {
            EZScriptStatisticsWindow resultWindow = EditorWindow.GetWindow<EZScriptStatisticsWindow>("Script Statistics");
            resultWindow.Show(targetObject);
        }

        public void PathNormalize(SerializedProperty arrayProperty)
        {
            for (int i = 0; i < arrayProperty.arraySize; i++)
            {
                SerializedProperty element = arrayProperty.GetArrayElementAtIndex(i);
                element.stringValue = element.stringValue.Replace("\\", "/");
            }
        }
        public void PathNormalize(List<string> stringList)
        {
            for (int i = 0; i < stringList.Count; i++)
            {
                stringList[i] = stringList[i].Replace("\\", "/");
            }
        }
    }
}