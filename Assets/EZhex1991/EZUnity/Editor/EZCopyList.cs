/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-05-27 16:35:04
 * Organization:    ezhex1991@outlook.com
 * Description:     
 */
using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [Serializable]
    public class EZCopyList
    {
        [Serializable]
        public class PathPair
        {
            [SerializeField]
            private string m_SrcPath;
            public string srcPath { get { return m_SrcPath; } }

            [SerializeField]
            private string m_DstPath;
            public string dstPath { get { return m_DstPath; } }
        }

        [SerializeField]
        private PathPair[] m_CopyList;
        public PathPair[] copyList { get { return m_CopyList; } }

        [SerializeField]
        private string[] m_BlackList = new string[] {
            "\\.meta$",
            "\\.tmp$",
            "^~",
        };
        public string[] blackList { get { return m_BlackList; } }

        public void CopyFiles(string destination)
        {
            for (int i = 0; i < copyList.Length; i++)
            {
                EditorUtility.DisplayProgressBar("Copying Files", "", (float)i / copyList.Length);
                string src = copyList[i].srcPath;
                string dst = Path.Combine(destination, copyList[i].dstPath);
                if (string.IsNullOrEmpty(src) || string.IsNullOrEmpty(dst)) continue;
                if (File.Exists(src))
                {
                    try
                    {
                        File.Copy(src, dst, true);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning(e.Message);
                    }
                }
                else if (Directory.Exists(src))
                {
                    Directory.CreateDirectory(dst);
                    string[] files = Directory.GetFiles(src);
                    foreach (string filePath in files)
                    {
                        try
                        {
                            if (IsInBlackList(Path.GetFileName(filePath)))
                            {
                                Debug.LogFormat("CopyList Ignored: {0}", filePath);
                                continue;
                            }
                            string newPath = dst + filePath.Substring(src.Length);
                            Directory.CreateDirectory(Path.GetDirectoryName(newPath));
                            File.Copy(filePath, newPath, true);
                        }
                        catch (Exception e)
                        {
                            Debug.LogWarning(e.Message);
                        }
                    }
                }
            }
            EditorUtility.ClearProgressBar();
        }

        public bool IsInBlackList(string fileName)
        {
            for (int i = 0; i < blackList.Length; i++)
            {
                if (string.IsNullOrEmpty(blackList[i])) continue;
                if (Regex.IsMatch(fileName, blackList[i])) return true;
            }
            return false;
        }
    }

    [CustomPropertyDrawer(typeof(EZCopyList))]
    public class EZCopyListDrawer : PropertyDrawer
    {
        public SerializedProperty m_CopyList;
        public ReorderableList copyList;
        public SerializedProperty m_BlackList;
        public ReorderableList blackList;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (m_CopyList == null)
            {
                m_CopyList = property.FindPropertyRelative("m_CopyList");
                copyList = new ReorderableList(property.serializedObject, m_CopyList)
                {
                    drawHeaderCallback = DrawCopyListHeader,
                    drawElementCallback = DrawCopyListElement,
                };
            }
            if (m_BlackList == null)
            {
                m_BlackList = property.FindPropertyRelative("m_BlackList");
                blackList = new ReorderableList(property.serializedObject, m_BlackList)
                {
                    drawHeaderCallback = DrawBlackListHeader,
                    drawElementCallback = DrawBlackListElement,
                };
            }
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.LabelField(position, label, EditorStyles.boldLabel);
            copyList.DoLayoutList();
            blackList.DoLayoutList();
            EditorGUI.EndProperty();
        }

        public void DrawCopyListHeader(Rect rect)
        {
            rect = EZEditorGUIUtility.DrawReorderableListCount(rect, copyList);
            if (copyList.count == 0)
            {
                EditorGUI.LabelField(rect, "Copy List");
            }
            else
            {
                float width = rect.width / 2; float margin = 5;
                rect.y += 1; rect.width = width - margin;
                EditorGUI.LabelField(rect, "Src");
                rect.x += width;
                EditorGUI.LabelField(rect, "Dst");
            }
        }
        public void DrawCopyListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty m_PathPair = m_CopyList.GetArrayElementAtIndex(index);
            SerializedProperty m_SrcPath = m_PathPair.FindPropertyRelative("m_SrcPath");
            SerializedProperty m_DstPath = m_PathPair.FindPropertyRelative("m_DstPath");

            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, index, copyList);
            rect.y += 1; rect.height = EditorGUIUtility.singleLineHeight;
            float width = rect.width / 2; float margin = 5; rect.width = width - margin;
            EditorGUI.PropertyField(rect, m_SrcPath, GUIContent.none);
            rect.x += width;
            EditorGUI.PropertyField(rect, m_DstPath, GUIContent.none);
        }

        public void DrawBlackListHeader(Rect rect)
        {
            rect = EZEditorGUIUtility.DrawReorderableListCount(rect, blackList);
            EditorGUI.LabelField(rect, "Black List");
        }
        public void DrawBlackListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, index, blackList);
            rect.y += 1; rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect, m_BlackList.GetArrayElementAtIndex(index), GUIContent.none);
        }
    }
}
