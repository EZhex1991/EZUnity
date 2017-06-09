/*
 * Author:      амем
 * CreateTime:  3/7/2017 6:44:14 PM
 * Description:
 * 
*/
using System.IO;

namespace EZUnityTools.EZEditor
{
    public class EZScriptProcessor : UnityEditor.AssetModificationProcessor
    {
        private static void OnWillCreateAsset(string metaPath)
        {
            string filePath = metaPath.Replace(".meta", "");
            EZScriptObject ezScript = EZScriptableObject.Load<EZScriptObject>(EZScriptObject.AssetName, false);
            if (ezScript != null)
            {
                Replace(filePath, ezScript);
            }
        }

        public static void Replace(string filePath, EZScriptObject ezScript)
        {
            if (!IsEZScriptAsset(filePath, ezScript)) return;
            string content = File.ReadAllText(filePath);
            content = content.Replace("#SCRIPTNAME", Path.GetFileNameWithoutExtension(filePath));
            content = content.Replace("#CREATETIME#", System.DateTime.Now.ToString());
            foreach (EZScriptObject.Pattern pattern in ezScript.patternList)
            {
                content = content.Replace(pattern.Key, pattern.Value);
            }
            File.WriteAllText(filePath, content);
        }

        private static bool IsEZScriptAsset(string filePath, EZScriptObject ezScript)
        {
            string lowerName = filePath.ToLower();
            foreach (string ext in ezScript.extensionList)
            {
                if (lowerName.EndsWith(ext + ".txt")) return false;
            }
            foreach (string ext in ezScript.extensionList)
            {
                if (lowerName.EndsWith(ext)) return true;
            }
            return false;
        }
    }
}