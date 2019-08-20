/* Author:          ezhex1991@outlook.com
 * CreateTime:      2016-08-08 10:49:31
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections.Generic;

namespace EZhex1991.EZUnity
{
    public class EZScriptSettings : EZProjectSettingsSingleton<EZScriptSettings>
    {
        public override string assetPath { get { return "ProjectSettings/EZScriptSettings.asset"; } }

        public string timeFormat = "yyyy-MM-dd HH:mm:ss";
        public List<string> extensionList = new List<string> { ".cs", ".lua", ".txt", ".shader", ".cginc", ".uxml", ".uss", ".asmdef" };

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
            new Pattern("#NAMESPACE#", "Namespace"),
        };
    }
}