/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-03-13 19:11:36
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EZUnity
{
    public class EZShaderKeywordsManager : EZEditorWindow
    {
        public static List<Material> materials = new List<Material>();
        public static Dictionary<string, List<Material>> keywordReference = new Dictionary<string, List<Material>>();
        public static Dictionary<string, bool> keywordFoldout = new Dictionary<string, bool>();

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
        private static void GetAllKeywordRenderence()
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
                        keywordFoldout[keyword] = false;
                    }
                }
            }
            keywordReference.OrderBy((item) => item.Key);
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            if (GUILayout.Button("Refresh"))
            {
                GetAllMaterials();
                GetAllKeywordRenderence();
            }
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            bool changed = false;
            int i = 0;
            foreach (var pair in keywordReference)
            {
                EditorGUILayout.BeginHorizontal();
                keywordFoldout[pair.Key] = EditorGUILayout.Foldout(keywordFoldout[pair.Key], i++.ToString("00 ") + pair.Key);
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
                    foreach (Material mat in pair.Value)
                    {
                        EditorGUILayout.ObjectField(mat, typeof(Material), true);
                    }
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
