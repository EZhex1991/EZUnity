/*
 * Author:      амем
 * CreateTime:  2/9/2017 6:23:42 PM
 * Description:
 * 
*/
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace EZUnityTools.EZEditor
{
    public class EZRenameEditorWindow : EZEditorWindow
    {
        public enum SelectionMode
        {
            Asset = UnityEditor.SelectionMode.Assets,
            Hierarchy = UnityEditor.SelectionMode.TopLevel | UnityEditor.SelectionMode.OnlyUserModifiable,
        }

        private SelectionMode m_SelectionMode = SelectionMode.Asset;
        private SelectionMode selectionMode
        {
            get
            {
                return m_SelectionMode;
            }
            set
            {
                m_SelectionMode = value;
                GetObjects();
            }
        }
        private bool toLower, toUpper;
        private string oldValue, newValue;
        private string prefix, suffix;

        private bool showLog;

        private bool foldout = true;
        private bool collapse = false;
        private Vector2 scrollView;

        private List<Object> objList = new List<Object>();
        private List<string> nameList = new List<string>();
        private Dictionary<string, int> nameDict = new Dictionary<string, int>();

        private void Reset()
        {
            GUI.FocusControl(null);
            toLower = false; toUpper = false;
            oldValue = ""; newValue = "";
            prefix = ""; suffix = "";
        }
        private void GetObjects()
        {
            objList.Clear();
            nameList.Clear();
            nameDict.Clear();
            foreach (Object obj in Selection.GetFiltered(typeof(object), (UnityEditor.SelectionMode)selectionMode))
            {
                objList.Add(obj);
                nameList.Add(GetOldName(obj));
            }
            nameList.Sort();
            foreach (string name in nameList)
            {
                if (nameDict.ContainsKey(name)) nameDict[name]++;
                else nameDict[name] = 1;
            }
            Repaint();
        }
        private string GetNewName(string name)
        {
            if (toLower) name = name.ToLower();
            if (toUpper) name = name.ToUpper();
            if (oldValue != "")
            {
                try
                {
                    Regex reg = new Regex(oldValue);
                    name = reg.Replace(name, newValue);
                }
                catch
                {
                    name = name.Replace(oldValue, newValue);
                }
            }
            name = prefix + name + suffix;
            return name;
        }
        private string GetNewName(Object obj)
        {
            return GetNewName(GetOldName(obj));
        }
        private string GetOldName(Object obj)
        {
            switch (selectionMode)
            {
                case SelectionMode.Asset:
                    string oldPath = AssetDatabase.GetAssetPath(obj);
                    return Path.GetFileNameWithoutExtension(oldPath);
                case SelectionMode.Hierarchy:
                    return obj.name;
                default:
                    return null;
            }
        }

        private void RenameAsset()
        {
            EditorUtility.DisplayProgressBar("Reimporting", "", 0);
            int process = 0;
            foreach (Object obj in objList)
            {
                if (showLog) Debug.Log("rename->\t" + obj.name + "\nto->\t" + GetNewName(obj));
                EditorUtility.DisplayProgressBar("Reimporting", obj.name, process++ / objList.Count);
                string error = AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(obj), GetNewName(obj));
                if (error != string.Empty)
                {
                    Debug.Log(error);
                }
            }
            EditorUtility.ClearProgressBar();
        }
        private void RenameHierarchy()
        {
            EditorUtility.DisplayProgressBar("Renaming", "", 0);
            int process = 0;
            foreach (Object obj in objList)
            {
                if (showLog) Debug.Log("rename->\t" + obj.name + "\nto->\t" + GetNewName(obj));
                EditorUtility.DisplayProgressBar("Renaming", obj.name, process++ / objList.Count);
                obj.name = GetNewName(obj);
            }
            EditorUtility.ClearProgressBar();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Reset();
            GetObjects();
        }
        protected override void OnSelectionChange()
        {
            foldout = true;
            GetObjects();
        }
        protected override void OnGUI()
        {
            base.OnGUI();
            selectionMode = (SelectionMode)EditorGUILayout.EnumPopup("Selection Mode", selectionMode);
            {
                EditorGUILayout.BeginHorizontal();
                if (toUpper) GUI.enabled = false;
                toLower = EditorGUILayout.Toggle("ToLower", toLower);
                GUI.enabled = true;
                if (toLower) GUI.enabled = false;
                toUpper = EditorGUILayout.Toggle("ToUpper", toUpper);
                GUI.enabled = true;
                EditorGUILayout.EndHorizontal();
            }
            {
                EditorGUILayout.BeginHorizontal();
                oldValue = EditorGUILayout.TextField("Old Value", oldValue);
                newValue = EditorGUILayout.TextField("New Value", newValue);
                EditorGUILayout.EndHorizontal();
            }
            {
                EditorGUILayout.BeginHorizontal();
                prefix = EditorGUILayout.TextField("Prefix", prefix);
                suffix = EditorGUILayout.TextField("Suffix", suffix);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();
            showLog = EditorGUILayout.Toggle("Show Log", showLog);
            if (GUILayout.Button("Reset"))
            {
                objList.Clear();
                Reset();
            }
            if (GUILayout.Button("Confirm"))
            {
                switch (selectionMode)
                {
                    case SelectionMode.Asset:
                        RenameAsset();
                        break;
                    case SelectionMode.Hierarchy:
                        RenameHierarchy();
                        break;
                }
                Reset();
            }
            EditorGUILayout.Space();
            {
                EditorGUILayout.BeginHorizontal();
                foldout = EditorGUILayout.Foldout(foldout, "Name List (" + nameList.Count + ")");
                collapse = EditorGUILayout.ToggleLeft("Collapse", collapse);
                EditorGUILayout.EndHorizontal();
            }
            if (foldout)
            {
                EditorGUILayout.BeginHorizontal();
                if (collapse) GUILayout.Label("" + nameDict.Count, new GUILayoutOption[] { GUILayout.Width(20), });
                EditorGUILayout.LabelField("Old Name ");
                EditorGUILayout.LabelField("New Name");
                EditorGUILayout.EndHorizontal();

                scrollView = EditorGUILayout.BeginScrollView(scrollView);
                if (collapse)
                {
                    foreach (var nameInfo in nameDict)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label("" + nameInfo.Value, new GUILayoutOption[] { GUILayout.Width(20), });
                        EditorGUILayout.TextField(nameInfo.Key);
                        EditorGUILayout.TextField(GetNewName(nameInfo.Key));
                        EditorGUILayout.EndHorizontal();
                    }
                }
                else
                {
                    foreach (string name in nameList)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.TextField(name);
                        EditorGUILayout.TextField(GetNewName(name));
                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUILayout.EndScrollView();
            }
        }
    }
}