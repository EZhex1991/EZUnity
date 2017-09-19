/*
 * Author:      熊哲
 * CreateTime:  3/16/2017 10:14:09 AM
 * Description:
 * 
*/
namespace EZUnityEditor
{
    public class EZKeystoreObject : EZScriptableObject
    {
        public const string AssetName = "EZKeystore";

        public string keystoreFilePath = "";
        public string keystorePassword = "";
        public string keyAliasName = "";
        public string keyAliasPassword = "";
    }
}