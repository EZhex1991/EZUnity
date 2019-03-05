/* Author:          ezhex1991@outlook.com
 * CreateTime:      2016-08-08 10:49:31
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EZUnity
{
    public class EZScriptTemplateObject : ScriptableObject
    {
        public const string AssetPath = "ProjectSettings/EZScriptTemplate.asset";
        private static EZScriptTemplateObject m_Instance;
        public static EZScriptTemplateObject Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = CreateInstance<EZScriptTemplateObject>();
                    m_Instance.Load();
                }
                return m_Instance;
            }
        }
        public void Load()
        {
            try
            {
                string data = File.ReadAllText(AssetPath);
                EditorJsonUtility.FromJsonOverwrite(data, this);
            }
            catch (Exception ex) { Debug.Log(ex.Message); }
        }
        public void Save()
        {
            File.WriteAllText(AssetPath, EditorJsonUtility.ToJson(this));
        }

        public string timeFormat = "yyyy-MM-dd HH:mm:ss";
        public List<string> extensionList = new List<string> { ".cs", ".lua", ".txt", ".shader", ".cginc" };

        [Serializable]
        public class Pattern
        {
            public string Key = "";
            public string Value = "";
            public Pattern(string key = "", string value = "")
            {
                this.Key = key;
                this.Value = value;
            }
        }
        public List<Pattern> patternList = new List<Pattern>
        {
            new Pattern("#ORGANIZATION#", ""),
            new Pattern("#AUTHORNAME#", ""),
        };
    }
}