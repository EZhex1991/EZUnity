/* Author:          熊哲
 * CreateTime:      2017-07-03 18:44:14
 * Orgnization:     #ORGNIZATION#
 * Description:     
 */
using System.IO;
using System.Text.RegularExpressions;

namespace EZUnityEditor
{
    public class EZScriptTemplateProcessor : UnityEditor.AssetModificationProcessor
    {
        public enum CheckResult
        {
            Unknow = 0,
            Template = 1,
            Script = 2,
        }

        private static void OnWillCreateAsset(string metaPath)
        {
            string filePath = metaPath.Replace(".meta", "");
            EZScriptTemplateObject ezScriptTemplate = EZScriptableObject.Load<EZScriptTemplateObject>(EZScriptTemplateObject.AssetName, false);
            if (ezScriptTemplate != null)
            {
                Replace(filePath, ezScriptTemplate);
            }
        }

        public static void Replace(string filePath, EZScriptTemplateObject ezScriptTemplate)
        {
            if (CheckTemplate(filePath, ezScriptTemplate) == CheckResult.Script)
            {
                string content = File.ReadAllText(filePath);
                content = content.Replace("#SCRIPTNAME", Path.GetFileNameWithoutExtension(filePath));
                content = content.Replace("#CREATETIME#", System.DateTime.Now.ToString(ezScriptTemplate.timeFormat));
                foreach (EZScriptTemplateObject.Pattern pattern in ezScriptTemplate.patternList)
                {
                    if (!string.IsNullOrEmpty(pattern.Value)) content = content.Replace(pattern.Key, pattern.Value);
                }
                File.WriteAllText(filePath, content);
            }
        }

        public static CheckResult CheckTemplate(string filePath, EZScriptTemplateObject ezScriptTemplate)
        {
            string fileName = Path.GetFileName(filePath.ToLower());
            string[] info = fileName.Split('-');
            if (info.Length == 3 && Regex.IsMatch(info[0], @"^[0-9]{1,2}$"))
            {
                foreach (string ext in ezScriptTemplate.extensionList)
                {
                    if (info[2].EndsWith(ext + ".txt")) return CheckResult.Template;
                }
            }
            foreach (string ext in ezScriptTemplate.extensionList)
            {
                if (fileName.EndsWith(ext)) return CheckResult.Script;
            }
            return CheckResult.Unknow;
        }
    }
}