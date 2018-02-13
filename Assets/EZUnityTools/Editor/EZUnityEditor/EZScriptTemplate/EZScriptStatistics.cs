/* Author:          熊哲
 * CreateTime:      2018-02-13 11:46:59
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
    public class EZScriptStatistics : EZEditorWindow
    {
        public int maxLines = 5;
        public List<ScriptInfo> csInfoList = new List<ScriptInfo>();
        public List<ScriptInfo> luaInfoList = new List<ScriptInfo>();
        public List<Contributor> contributorList = new List<Contributor>();

        private bool showTextAsset;
        private Vector2 scrollPosition;

        public class ScriptInfo
        {
            public string filePath;
            public TextAsset fileObject;
            public string author;
            public string createTime;
            public string orgnization;
            public int lineCount;
            public int validLineCount;
            public ScriptInfo(string filePath)
            {
                this.filePath = filePath;
            }
        }
        public class Contributor
        {
            public string author;
            public int lineCount;
            public int validLineCount;
            public List<ScriptInfo> scriptList = new List<ScriptInfo>();
            public bool foldout;
            public Contributor(string author)
            {
                this.author = author;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Refresh();
        }

        public void Refresh()
        {
            csInfoList.Clear();
            luaInfoList.Clear();
            contributorList.Clear();
            string[] csScripts = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);
            string[] csTxtScripts = Directory.GetFiles(Application.dataPath, "*.cs.txt", SearchOption.AllDirectories);
            csInfoList = (from filePath in csScripts.Concat(csTxtScripts)
                          where true
                          select GetScriptInfo(filePath)).ToList();
            string[] luaScripts = Directory.GetFiles(Application.dataPath, "*.lua", SearchOption.AllDirectories);
            string[] luaTxtScripts = Directory.GetFiles(Application.dataPath, "*.lua.txt", SearchOption.AllDirectories);
            luaInfoList = (from filePath in luaScripts.Concat(luaTxtScripts)
                           where true
                           select GetScriptInfo(filePath)).ToList();
            var contributions = csInfoList.Concat(luaInfoList).GroupBy(info => info.author);
            foreach (var contribution in contributions)
            {
                Contributor contributor = new Contributor(contribution.Key);
                foreach (ScriptInfo info in contribution.Cast<ScriptInfo>())
                {
                    contributor.scriptList.Add(info);
                    contributor.lineCount += info.lineCount;
                    contributor.validLineCount += info.validLineCount;
                }
                contributorList.Add(contributor);
            }
        }
        public void SaveAsText(string fileName)
        {
            string filePath = EZScriptableObject.AssetsDirPath + fileName;
            StreamWriter writer = new StreamWriter(File.Open(filePath, FileMode.Create));
            foreach (Contributor contributor in contributorList)
            {
                writer.WriteLine("{0}\t{1}\t{2}", contributor.author, contributor.lineCount, contributor.validLineCount);
                foreach (ScriptInfo script in contributor.scriptList)
                {
                    writer.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", contributor.author, script.filePath, script.createTime, script.lineCount, script.validLineCount);
                }
            }
            writer.Flush(); writer.Close();
            AssetDatabase.Refresh();
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<TextAsset>(filePath);
        }

        public ScriptInfo GetScriptInfo(string filePath)
        {
            return GetScriptInfo(filePath, Encoding.UTF8);
        }
        public ScriptInfo GetScriptInfo(string filePath, Encoding encoding)
        {
            string relativePath = filePath.Replace(Application.dataPath, "");
            ScriptInfo info = new ScriptInfo(relativePath);
            info.fileObject = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets" + relativePath);
            string[] lines = File.ReadAllLines(filePath, encoding);
            int invalidLineCount = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (i < maxLines)
                {
                    Match match;
                    match = Regex.Match(lines[i], @"^\W*Author:\s*(\S[\s\S]*)$");
                    if (match.Success)
                    {
                        info.author = match.Groups[1].Value;
                    }
                    match = Regex.Match(lines[i], @"^\W*CreateTime:\s*(\S[\s\S]*)$");
                    if (match.Success)
                    {
                        info.createTime = match.Groups[1].Value;
                    }
                }
                if (Regex.IsMatch(lines[i], @"^\s*$")) invalidLineCount++;
            }
            info.lineCount = lines.Length;
            info.validLineCount = lines.Length - invalidLineCount;
            return info;
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            showTextAsset = EditorGUILayout.Toggle("Show File Object", showTextAsset);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Refresh"))
            {
                Refresh();
            }
            if (GUILayout.Button("Save Text"))
            {
                SaveAsText("ScriptStatistics_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt");
            }
            EditorGUILayout.EndHorizontal();
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            foreach (Contributor contributor in contributorList)
            {
                EditorGUILayout.BeginHorizontal();
                contributor.foldout = EditorGUILayout.Foldout(contributor.foldout, contributor.author);
                EditorGUILayout.TextField(contributor.lineCount.ToString(), new GUILayoutOption[] { GUILayout.Width(60) });
                EditorGUILayout.TextField(contributor.validLineCount.ToString(), new GUILayoutOption[] { GUILayout.Width(60) });
                EditorGUILayout.EndHorizontal();
                if (contributor.foldout)
                {
                    foreach (ScriptInfo script in contributor.scriptList)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUI.indentLevel++;
                        if (showTextAsset) EditorGUILayout.ObjectField(script.fileObject, typeof(TextAsset), true);
                        else EditorGUILayout.TextField(script.filePath);
                        EditorGUI.indentLevel--;
                        EditorGUILayout.TextField(script.createTime, new GUILayoutOption[] { GUILayout.Width(180) });
                        EditorGUILayout.TextField(script.lineCount.ToString(), new GUILayoutOption[] { GUILayout.Width(60) });
                        EditorGUILayout.TextField(script.validLineCount.ToString(), new GUILayoutOption[] { GUILayout.Width(60) });
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
            EditorGUILayout.EndScrollView();
        }
    }
}