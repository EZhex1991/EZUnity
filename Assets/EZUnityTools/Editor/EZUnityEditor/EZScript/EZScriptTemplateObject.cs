/*
 * Author:      熊哲
 * CreateTime:  2016/08/08 10:49
 * Description:
 * 用于初始化脚本文件，使用前请先修改Unity的脚本模板
 * 文件请放置于Editor文件夹下
*/
using System;
using System.Collections.Generic;

namespace EZUnityEditor
{
    public class EZScriptTemplateObject : EZScriptableObject
    {
        public const string AssetName = "EZScriptTemplate";

        public string timeFormat = "yyyy-MM-dd HH:mm:ss";
        public List<string> extensionList = new List<string> { ".cs", ".lua", ".txt", };

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