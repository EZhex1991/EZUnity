/* Author:          熊哲
 * CreateTime:      2017-09-02 18:23:42
 * Orgnization:     #ORGNIZATION#
 * Description:     
 */
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace EZUnityEditor
{
    public class EZRenameEditorWindow : EZEditorWindow
    {
        public enum SelectionMode
        {
            Assets = UnityEditor.SelectionMode.Assets,
            SceneObjects = UnityEditor.SelectionMode.TopLevel | UnityEditor.SelectionMode.OnlyUserModifiable,
        }
        public enum CaseConversion
        {
            None = 0,
            ToUpper = 1,
            ToLower = 2,
        }

        private SelectionMode m_SelectionMode = SelectionMode.Assets;
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
        private CaseConversion caseConversion = CaseConversion.None;
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
            caseConversion = CaseConversion.None;
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
            switch (caseConversion)
            {
                case CaseConversion.None:
                    break;
                case CaseConversion.ToUpper:
                    name = name.ToUpper();
                    break;
                case CaseConversion.ToLower:
                    name = name.ToLower();
                    break;
            }
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
                case SelectionMode.Assets:
                    string oldPath = AssetDatabase.GetAssetPath(obj);
                    return Path.GetFileNameWithoutExtension(oldPath);
                case SelectionMode.SceneObjects:
                    return obj.name;
                default:
                    return null;
            }
        }

        private void RenameAssets()
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
        private void RenameSceneObjects()
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
            DrawConfig();
            EditorGUILayout.Space();
            DrawButton();
            EditorGUILayout.Space();
            DrawFilePreview();
        }

        private void DrawConfig()
        {
            selectionMode = (SelectionMode)EditorGUILayout.EnumPopup("Selection Mode", selectionMode);
            caseConversion = (CaseConversion)EditorGUILayout.EnumPopup("Case Conversion", caseConversion);
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
        }

        private void DrawButton()
        {
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
                    case SelectionMode.Assets:
                        RenameAssets();
                        break;
                    case SelectionMode.SceneObjects:
                        RenameSceneObjects();
                        break;
                }
                Reset();
            }
        }

        private void DrawFilePreview()
        {
            {
                EditorGUILayout.BeginHorizontal();
                foldout = EditorGUILayout.Foldout(foldout, "Name List " + nameList.Count.ToString("(00)"));
                collapse = EditorGUILayout.ToggleLeft("Collapse", collapse);
                EditorGUILayout.EndHorizontal();
            }
            if (foldout)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("", new GUILayoutOption[] { GUILayout.Width(20), });
                EditorGUILayout.LabelField("Old Name ");
                EditorGUILayout.LabelField("New Name");
                EditorGUILayout.EndHorizontal();

                scrollView = EditorGUILayout.BeginScrollView(scrollView);
                if (collapse)
                    DrawNameDict();
                else
                    DrawNameList();
                EditorGUILayout.EndScrollView();
            }
        }
        private void DrawNameDict()
        {
            foreach (var nameInfo in nameDict)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(nameInfo.Value.ToString(), new GUILayoutOption[] { GUILayout.Width(20), });
                EditorGUILayout.TextField(nameInfo.Key);
                EditorGUILayout.TextField(GetNewName(nameInfo.Key));
                EditorGUILayout.EndHorizontal();
            }
        }
        private void DrawNameList()
        {
            for (int i = 0; i < nameList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(i.ToString("00"), new GUILayoutOption[] { GUILayout.Width(20), });
                EditorGUILayout.TextField(nameList[i]);
                EditorGUILayout.TextField(GetNewName(nameList[i]));
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}