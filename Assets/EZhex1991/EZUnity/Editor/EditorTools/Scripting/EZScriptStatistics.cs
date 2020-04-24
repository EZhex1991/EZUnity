/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-02-22 16:48:41
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CreateAssetMenu(fileName = "EZScriptStatistics", menuName = "EZUnity/EZScriptStatistics", order = (int)EZAssetMenuOrder.EZScriptStatistics)]
    public class EZScriptStatistics : ScriptableObject
    {
        public string[] filePatterns = new string[] { "*.cs", "*.lua", "*.lua.txt", "*.shader", "*.cginc" };
        public string[] includePaths = new string[] { "Assets" };
        public string[] excludePaths = new string[] { "Assets/Plugins" };

        public int infoLineCount = 5;
        public string authorRegex = @"^\W*Author:\s*(\S[\s\S]*)$";
        public string createTimeRegex = @"^\W*CreateTime:\s*(\S[\s\S]*)$";
        public string validLineRegex = @"^\W*(\S+)[\S\s]*$";
    }
}