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
using static EZhex1991.EZUnity.EZScriptStatisticResult;

namespace EZhex1991.EZUnity
{
    [CustomEditor(typeof(EZScriptStatistics))]
    public class EZScriptStatisticsEditor : Editor
    {
        private EZScriptStatistics statistics;

        private SerializedProperty m_FilePatterns;
        private SerializedProperty m_IncludePaths;
        private SerializedProperty m_ExcludePaths;
        private SerializedProperty m_InfoLineCount;
        private SerializedProperty m_AuthorRegex;
        private SerializedProperty m_CreateTimeRegex;
        private SerializedProperty m_ValidLineRegex;

        void OnEnable()
        {
            statistics = target as EZScriptStatistics;
            m_FilePatterns = serializedObject.FindProperty("filePatterns");
            m_IncludePaths = serializedObject.FindProperty("includePaths");
            m_ExcludePaths = serializedObject.FindProperty("excludePaths");
            m_InfoLineCount = serializedObject.FindProperty("infoLineCount");
            m_AuthorRegex = serializedObject.FindProperty("authorRegex");
            m_CreateTimeRegex = serializedObject.FindProperty("createTimeRegex");
            m_ValidLineRegex = serializedObject.FindProperty("validLineRegex");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EZEditorGUIUtility.ScriptableObjectTitle(target as ScriptableObject, !serializedObject.isEditingMultipleObjects);

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

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Results", EditorStyles.boldLabel);
            foreach (var subAsset in AssetDatabase.LoadAllAssetRepresentationsAtPath(AssetDatabase.GetAssetPath(target)))
            {
                if (subAsset == null) continue;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.ObjectField(subAsset.name, subAsset, typeof(EZScriptStatisticResult), false);
                if (GUILayout.Button("Remove", GUILayout.Width(80)))
                {
                    Undo.RecordObject(target, "Remove Result");
                    Undo.DestroyObjectImmediate(subAsset);
                    EditorUtility.SetDirty(target);
                    AssetDatabase.SaveAssets();
                }
                EditorGUILayout.EndHorizontal();
            }

            serializedObject.ApplyModifiedProperties();
        }

        public void RefreshResult()
        {
            PathNormalize(statistics.includePaths);
            PathNormalize(statistics.excludePaths);
            EditorUtility.DisplayProgressBar("EZScriptStatistics", "Analysing", 0);
            List<string> scriptFilePaths = new List<string>();
            for (int i = 0; i < statistics.includePaths.Length; i++)
            {
                string includePath = statistics.includePaths[i];
                if (string.IsNullOrEmpty(includePath)) continue;
                for (int j = 0; j < statistics.filePatterns.Length; j++)
                {
                    string pattern = statistics.filePatterns[j];
                    if (string.IsNullOrEmpty(pattern)) continue;
                    try
                    {
                        scriptFilePaths.AddRange(Directory.GetFiles(includePath, pattern, SearchOption.AllDirectories));
                    }
                    catch { }
                }
            }
            PathNormalize(scriptFilePaths);
            List<ScriptInfo> scriptInfoList = new List<ScriptInfo>();
            for (int i = 0; i < scriptFilePaths.Count; i++)
            {
                var scriptInfo = GetScriptInfo(scriptFilePaths[i]);
                if (scriptInfo != null) scriptInfoList.Add(scriptInfo);
            }
            if (scriptInfoList.Count == 0)
            {
                Debug.Log("No script file found");
                return;
            }

            EZScriptStatisticResult result = CreateInstance<EZScriptStatisticResult>();
            result.time = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            result.name = "Result-" + result.time;
            int totalValidLineCount = 0;
            foreach (var contribution in scriptInfoList.GroupBy(info => info.author))
            {
                Contributor contributor = new Contributor(contribution.Key);
                foreach (ScriptInfo info in contribution.Cast<ScriptInfo>())
                {
                    contributor.scriptList.Add(info);
                    contributor.lineCount += info.lineCount;
                    contributor.validLineCount += info.validLineCount;
                    totalValidLineCount += info.validLineCount;
                }
                result.contributors.Add(contributor);
            }
            foreach (Contributor contributor in result.contributors)
            {
                contributor.proportion = (float)contributor.validLineCount / totalValidLineCount;
            }
            string assetPath = AssetDatabase.GetAssetPath(target);
            AssetDatabase.AddObjectToAsset(result, target);
            EditorUtility.ClearProgressBar();
            AssetDatabase.SaveAssets();
            Selection.activeObject = result;
        }

        private bool IsValidPath(string filePath)
        {
            for (int i = 0; i < statistics.excludePaths.Length; i++)
            {
                string excludePath = statistics.excludePaths[i];
                if (string.IsNullOrEmpty(excludePath)) continue;
                if (filePath.StartsWith(excludePath)) return false;
            }
            return true;
        }
        private ScriptInfo GetScriptInfo(string filePath)
        {
            string relativePath = filePath.Replace(Application.dataPath, "");
            if (!IsValidPath(relativePath)) return null;
            ScriptInfo info = new ScriptInfo(relativePath);
            info.fileObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(relativePath);
            string[] lines = File.ReadAllLines(filePath);
            info.lineCount = lines.Length;
            for (int i = 0; i < lines.Length; i++)
            {
                if (i < statistics.infoLineCount)
                {
                    Match match;
                    match = Regex.Match(lines[i], statistics.authorRegex);
                    if (match.Success)
                    {
                        info.author = match.Groups[1].Value;
                    }
                    match = Regex.Match(lines[i], statistics.createTimeRegex);
                    if (match.Success)
                    {
                        info.createTime = match.Groups[1].Value;
                    }
                }
                if (Regex.IsMatch(lines[i], statistics.validLineRegex)) info.validLineCount++;
            }
            return info;
        }

        private static void PathNormalize(IList<string> stringList)
        {
            for (int i = 0; i < stringList.Count; i++)
            {
                stringList[i] = stringList[i].Replace("\\", "/");
            }
        }
    }
}