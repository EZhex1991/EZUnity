/*
 * Author:      熊哲
 * CreateTime:  3/16/2017 10:14:09 AM
 * Description:
 * 
*/
using UnityEngine;

namespace EZUnityEditor
{
    public class EZKeystoreObject : EZScriptableObject
    {
        public const string AssetName = "EZKeystore";

        [SerializeField]
        private string m_KeystoreName = "";
        public string keystoreName { get { return m_KeystoreName; } set { m_KeystoreName = value; } }

        [SerializeField]
        private string m_KeystorePass = "";
        public string keystorePass { get { return m_KeystorePass; } set { m_KeystorePass = value; } }

        [SerializeField]
        private string m_KeyAliasName = "";
        public string keyAliasName { get { return m_KeyAliasName; } set { m_KeyAliasName = value; } }

        [SerializeField]
        private string m_KeyAliasPass = "";
        public string keyAliasPass { get { return m_KeyAliasPass; } set { m_KeyAliasPass = value; } }
    }
}