/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-03-13 19:11:36
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZShaderKeywordManager : EditorWindow
    {
        public static List<Material> materials = new List<Material>();
        public static Dictionary<string, List<Material>> keywordReference = new Dictionary<string, List<Material>>();
        public static Dictionary<string, bool> keywordFoldout = new Dictionary<string, bool>();

        private void GetAllMaterials()
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
        private void GetAllKeywordRenderence()
        {
            keywordReference.Clear();
            foreach (Material material in materials)
            {
                foreach (string keyword in material.shaderKeywords)
                {
                    if (keywordReference.ContainsKey(keyword))
                    {
                        keywordReference[keyword].Add(material);
                    }
                    else
                    {
                        keywordReference[keyword] = new List<Material>() { material };
                    }
                    if (!keywordFoldout.ContainsKey(keyword))
                    {
                        keywordFoldout[keyword] = false;
                    }
                }
            }
            keywordReference = keywordReference.OrderBy((item) => item.Key).ToDictionary((item) => item.Key, (item) => item.Value);
        }

        private Vector2 scrollPosition;

        private void OnSelectionChange()
        {
            Repaint();
        }

        protected void OnGUI()
        {
            EZEditorGUIUtility.WindowTitle(this);

            Material selection = Selection.activeObject as Material;

            EditorGUILayout.Space();
            if (selection != null)
            {
                EditorGUILayout.ObjectField("Selection", selection, typeof(Material), true);
                EditorGUI.indentLevel++;
                for (int i = 0; i < selection.shaderKeywords.Length; i++)
                {
                    string keyword = selection.shaderKeywords[i];
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(keyword);
                    if (GUILayout.Button("Delete"))
                    {
                        selection.DisableKeyword(keyword);
                        EditorUtility.SetDirty(selection);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }

            if (GUILayout.Button("Get All Keywords"))
            {
                GetAllMaterials();
                GetAllKeywordRenderence();
            }
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            bool changed = false;
            int index = 0;
            foreach (var pair in keywordReference)
            {
                EditorGUILayout.BeginHorizontal();
                keywordFoldout[pair.Key] = EditorGUILayout.Foldout(keywordFoldout[pair.Key], index++.ToString("00 ") + pair.Key);
                if (GUILayout.Button("Delete"))
                {
                    foreach (Material mat in pair.Value)
                    {
                        mat.DisableKeyword(pair.Key);
                        EditorUtility.SetDirty(mat);
                    }
                    changed = true;
                }
                EditorGUILayout.EndHorizontal();
                if (keywordFoldout[pair.Key])
                {
                    EditorGUI.indentLevel++;
                    foreach (Material mat in pair.Value)
                    {
                        EditorGUILayout.ObjectField(mat, typeof(Material), true);
                    }
                    EditorGUI.indentLevel--;
                }
            }
            if (changed)
            {
                GetAllKeywordRenderence();
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
