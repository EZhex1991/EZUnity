/* Author:          熊哲
 * CreateTime:      2018-02-13 11:46:59
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EZUnityEditor
{
    public class EZScriptStatisticsWindow : EZEditorWindow
    {
        public string resultTime;
        public List<Contributor> contributorList = new List<Contributor>();

        private bool showAsTextAsset;
        private Vector2 scrollPosition;

        public void Show(EZScriptStatisticsObject resultObject)
        {
            Show();
            this.resultTime = resultObject.resultTime;
            this.contributorList = resultObject.result;
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            EditorGUILayout.LabelField("Result Time: " + resultTime);
            showAsTextAsset = EditorGUILayout.Toggle("Show Script As TextAsset", showAsTextAsset);

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            foreach (Contributor contributor in contributorList)
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
                        if (showAsTextAsset) EditorGUILayout.ObjectField(script.fileObject, typeof(TextAsset), true);
                        else EditorGUILayout.TextField(script.filePath);
                        EditorGUI.indentLevel--;
                        EditorGUILayout.TextField(script.createTime, new GUILayoutOption[] { GUILayout.Width(unitWidth * 3) });
                        EditorGUILayout.TextField(script.lineCount.ToString(), new GUILayoutOption[] { GUILayout.Width(unitWidth) });
                        EditorGUILayout.TextField(script.validLineCount.ToString(), new GUILayoutOption[] { GUILayout.Width(unitWidth) });
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
            EditorGUILayout.EndScrollView();
        }
    }
}