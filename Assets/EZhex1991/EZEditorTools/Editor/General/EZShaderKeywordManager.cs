/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-03-13 19:11:36
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZEditorTools
{
    public class EZShaderKeywordManager : EditorWindow
    {
        public class KeywordInfo
        {
            public string keyword;
            public bool isGlobal;
            public List<Material> materials;
            public bool foldout;

            public KeywordInfo(string keyword, bool isGlobal = false)
            {
                this.keyword = keyword;
                this.isGlobal = isGlobal;
                materials = new List<Material>();
            }
        }

        public static string keywordStringFromWarning;
        public static List<Material> materials = new List<Material>();
        public static Dictionary<string, KeywordInfo> keywordInfoDict = new Dictionary<string, KeywordInfo>();

        private Vector2 scrollPosition;

        private static void GetAllMaterials()
        {
            materials.Clear();
            string[] guids = AssetDatabase.FindAssets("t:Material");
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
                if (mat != null)
                {
                    materials.Add(mat);
                }
            }
        }
        private static void GetKeywordsFromString()
        {
            if (string.IsNullOrEmpty(keywordStringFromWarning)) return;
            string[] keywords = keywordStringFromWarning.Split(' ');
            for (int i = 0; i < keywords.Length; i++)
            {
                string keyword = keywords[i];
                if (keywordInfoDict.ContainsKey(keyword))
                {
                    keywordInfoDict[keyword].isGlobal = Shader.IsKeywordEnabled(keyword);
                }
                else
                {
                    keywordInfoDict.Add(keyword, new KeywordInfo(keyword, Shader.IsKeywordEnabled(keyword)));
                }
            }
        }
        private static void GetKeywordsFromMaterials()
        {
            foreach (Material material in materials)
            {
                foreach (string keyword in material.shaderKeywords)
                {
                    if (keywordInfoDict.ContainsKey(keyword))
                    {
                        keywordInfoDict[keyword].materials.Add(material);
                    }
                    else
                    {
                        KeywordInfo info = new KeywordInfo(keyword);
                        info.materials.Add(material);
                        keywordInfoDict.Add(keyword, info);

                    }
                }
            }
            keywordInfoDict = keywordInfoDict.OrderBy((item) => item.Key).ToDictionary((item) => item.Key, (item) => item.Value);
        }
        private static void GetKeywords()
        {
            GetAllMaterials();
            keywordInfoDict.Clear();
            GetKeywordsFromString();
            GetKeywordsFromMaterials();
        }

        private static void SetAllFoldouts(bool expanded)
        {
            foreach (KeywordInfo keywordInfo in keywordInfoDict.Values)
            {
                keywordInfo.foldout = expanded;
            }
        }

        private void OnSelectionChange()
        {
            Repaint();
        }
        protected void OnGUI()
        {
            EZEditorGUIUtility.WindowTitle(this);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Additional Keywords", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "Paste space-separated keywords from shader warnings or logs. These keywords are added to the material scan results.",
                MessageType.Info);
            keywordStringFromWarning = EditorGUILayout.TextArea(keywordStringFromWarning, GUILayout.Height(80));

            Rect buttonRow = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
            float buttonWidth = (buttonRow.width - 2f) * 0.5f;
            Rect leftButton = new Rect(buttonRow.x, buttonRow.y, buttonWidth, buttonRow.height);
            Rect rightButton = new Rect(buttonRow.x + buttonWidth + 2f, buttonRow.y, buttonWidth, buttonRow.height);
            if (GUI.Button(leftButton, "Get Keywords"))
            {
                GetKeywords();
            }
            if (GUI.Button(rightButton, "Clear Results"))
            {
                keywordInfoDict.Clear();
            }

            buttonRow = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
            leftButton = new Rect(buttonRow.x, buttonRow.y, buttonWidth, buttonRow.height);
            rightButton = new Rect(buttonRow.x + buttonWidth + 2f, buttonRow.y, buttonWidth, buttonRow.height);
            if (GUI.Button(leftButton, "Expand All"))
            {
                SetAllFoldouts(true);
            }
            if (GUI.Button(rightButton, "Collapse All"))
            {
                SetAllFoldouts(false);
            }
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            bool changed = false;
            int index = 0;
            foreach (var pair in keywordInfoDict)
            {
                KeywordInfo keywordInfo = pair.Value;
                EditorGUILayout.BeginHorizontal();
                keywordInfo.foldout = EditorGUILayout.Foldout(keywordInfo.foldout, index++.ToString("000 ") + keywordInfo.keyword);
                EditorGUILayout.LabelField("Count:" + keywordInfo.materials.Count, GUILayout.Width(80));
                EditorGUILayout.LabelField("IsGlobal:" + keywordInfo.isGlobal, GUILayout.Width(120));
                if (GUILayout.Button("Delete", GUILayout.Width(80)))
                {
                    foreach (Material mat in keywordInfo.materials)
                    {
                        mat.DisableKeyword(keywordInfo.keyword);
                        EditorUtility.SetDirty(mat);
                    }
                    Shader.DisableKeyword(keywordInfo.keyword);
                    changed = true;
                }
                EditorGUILayout.EndHorizontal();
                if (keywordInfo.foldout)
                {
                    EditorGUI.indentLevel++;
                    foreach (Material mat in keywordInfo.materials)
                    {
                        EditorGUILayout.ObjectField(mat, typeof(Material), true);
                    }
                    EditorGUI.indentLevel--;
                }
            }
            if (changed)
            {
                GetKeywords();
            }
            EditorGUILayout.EndScrollView();
        }
    }
}



