/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-02-22 16:48:41
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CreateAssetMenu(fileName = "EZScriptStatistics", menuName = "EZUnity/EZScriptStatistics", order = (int)EZAssetMenuOrder.EZScriptStatistics)]
    public class EZScriptStatistics : ScriptableObject
    {
        public const string AssetName = "EZScriptStatistics";

        public string[] filePatterns = new string[] { "*.cs", "*.lua", "*.lua.txt", "*.shader", "*.cginc" };
        public string[] includePaths = new string[] { "Assets" };
        public string[] excludePaths = new string[] { "Assets/Plugins" };

        public int infoLineCount = 5;
        public string authorRegex = @"^\W*Author:\s*(\S[\s\S]*)$";
        public string createTimeRegex = @"^\W*CreateTime:\s*(\S[\s\S]*)$";
        public string validLineRegex = @"^\W*(\S+)[\S\s]*$";

        public string resultTime;
        public List<Contributor> result = new List<Contributor>();

        public bool showAsset;
    }
    [Serializable]
    public class ScriptInfo
    {
        public string filePath;
        public UnityEngine.Object fileObject;
        public string author;
        public string createTime;
        public string orgnization;
        public int lineCount;
        public int validLineCount;
        public ScriptInfo(string filePath)
        {
            this.filePath = filePath;
        }
    }
    [Serializable]
    public class Contributor
    {
        public string author;
        public int lineCount;
        public int validLineCount;
        public float proportion;
        public List<ScriptInfo> scriptList = new List<ScriptInfo>();
        public bool foldout;
        public Contributor(string author)
        {
            this.author = author;
        }
    }
}