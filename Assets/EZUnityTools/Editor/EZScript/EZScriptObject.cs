/*
 * Author:      熊哲
 * CreateTime:  2016/08/08 10:49
 * Description:
 * 用于初始化脚本文件，使用前请先修改Unity的脚本模板
 * 文件请放置于Editor文件夹下
*/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZUnityTools.EZEditor
{
    public class EZScriptObject : EZScriptableObject
    {
        public const string AssetName = "EZScript";

        public List<TextAsset> templateList = new List<TextAsset>();

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