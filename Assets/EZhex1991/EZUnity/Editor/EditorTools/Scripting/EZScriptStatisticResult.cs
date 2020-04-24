/* Author:          ezhex1991@outlook.com
 * CreateTime:      2020-04-24 15:48:50
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZScriptStatisticResult : ScriptableObject
    {
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

        public string time;
        public List<Contributor> contributors = new List<Contributor>();
    }
}
