/*
 * Author:      амем
 * CreateTime:  3/7/2017 6:44:14 PM
 * Description:
 * 
*/
using System.IO;

namespace EZUnityTools.EZEditor
{
    public class EZScriptTemplateProcessor : UnityEditor.AssetModificationProcessor
    {
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
            if (!IsEZScriptAsset(filePath, ezScriptTemplate)) return;
            string content = File.ReadAllText(filePath);
            content = content.Replace("#SCRIPTNAME", Path.GetFileNameWithoutExtension(filePath));
            content = content.Replace("#CREATETIME#", System.DateTime.Now.ToString());
            foreach (EZScriptTemplateObject.Pattern pattern in ezScriptTemplate.patternList)
            {
                content = content.Replace(pattern.Key, pattern.Value);
            }
            File.WriteAllText(filePath, content);
        }

        private static bool IsEZScriptAsset(string filePath, EZScriptTemplateObject ezScriptTemplate)
        {
            string lowerName = filePath.ToLower();
            foreach (string ext in ezScriptTemplate.extensionList)
            {
                if (lowerName.EndsWith(ext + ".txt")) return false;
            }
            foreach (string ext in ezScriptTemplate.extensionList)
            {
                if (lowerName.EndsWith(ext)) return true;
            }
            return false;
        }
    }
}