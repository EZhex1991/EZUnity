/* Author:          ezhex1991@outlook.com
 * CreateTime:      2016-08-08 10:49:31
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections.Generic;
using System.IO;

namespace EZUnity
{
    public class EZScriptSettings : _EZProjectSettingsSingleton<EZScriptSettings>
    {
        public override string assetPath { get { return "ProjectSettings/EZScriptSettings.asset"; } }

        public string timeFormat = "yyyy-MM-dd HH:mm:ss";
        public List<string> extensionList = new List<string> { ".cs", ".lua", ".txt", ".shader", ".cginc", "uxml", "uss", "asmdef" };

        static EZScriptSettings()
        {
            string oldPath = "ProjectSettings/EZScriptTemplate.asset";
            string newPath = "ProjectSettings/EZScriptSettings.asset";
            if (File.Exists(oldPath) && !File.Exists(newPath)) File.Move(oldPath, newPath);
        }

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