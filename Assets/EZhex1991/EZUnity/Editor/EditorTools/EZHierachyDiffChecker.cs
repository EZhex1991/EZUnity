/* Author:          ezhex1991@outlook.com
 * CreateTime:      2020-03-19 17:45:42
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZHierachyDiffChecker : EditorWindow
    {
        public static Transform rootTransform1;
        public static Transform rootTransform2;

        public List<string> pathList = new List<string>();
        public Dictionary<Transform, string> pathMap1 = new Dictionary<Transform, string>();
        public Dictionary<Transform, string> pathMap2 = new Dictionary<Transform, string>();

        private Vector2 scrollPostion;
        private Color colorNormal;
        private Color colorDiff = Color.red;

        private void OnEnable()
        {
        }

        private void OnGUI()
        {
            EZEditorGUIUtility.WindowTitle(this);

            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            rootTransform1 = (Transform)EditorGUILayout.ObjectField("Transform 1", rootTransform1, typeof(Transform), true);
            if (EditorGUI.EndChangeCheck())
            {
                pathMap1.Clear();
                if (rootTransform1 != null)
                {
                    GetChildPath("", rootTransform1, pathMap1);
                }
            }
            EditorGUI.BeginChangeCheck();
            rootTransform2 = (Transform)EditorGUILayout.ObjectField("Transform 2", rootTransform2, typeof(Transform), true);
            if (EditorGUI.EndChangeCheck())
            {
                pathMap2.Clear();
                if (rootTransform2 != null)
                {
                    GetChildPath("", rootTransform2, pathMap2);
                }
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Copy TRS From 2"))
            {
                rootTransform1.CopyTRSFrom(rootTransform2, true);
            }

            EditorGUILayout.Space();
            colorNormal = GUI.backgroundColor;
            scrollPostion = EditorGUILayout.BeginScrollView(scrollPostion);
            EditorGUILayout.BeginHorizontal();
            if (rootTransform1 != null)
            {
                EditorGUILayout.BeginVertical();
                DrawTransformHierachy(rootTransform1, pathMap1, pathMap2);
                EditorGUILayout.EndVertical();
            }
            if (rootTransform2 != null)
            {
                EditorGUILayout.BeginVertical();
                DrawTransformHierachy(rootTransform2, pathMap2, pathMap1);
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }

        private void DrawTransformHierachy(Transform t, Dictionary<Transform, string> pathMap, Dictionary<Transform, string> checker)
        {
            for (int i = 0; i < t.childCount; i++)
            {
                Transform child = t.GetChild(i);
                if (child == null) continue;
                GUI.backgroundColor = checker.ContainsValue(pathMap[child]) ? colorNormal : colorDiff;
                EditorGUILayout.ObjectField(child, typeof(Transform), true);
                EditorGUI.indentLevel++;
                DrawTransformHierachy(child, pathMap, checker);
                EditorGUI.indentLevel--;
            }
        }

        private void GetChildPath(string prefix, Transform t, Dictionary<Transform, string> dict)
        {
            for (int i = 0; i < t.childCount; i++)
            {
                Transform child = t.GetChild(i);
                string path = prefix + child.name;
                dict.Add(child, path);
                path = path + "/";
                GetChildPath(path, t.GetChild(i), dict);
            }
        }
    }
}
