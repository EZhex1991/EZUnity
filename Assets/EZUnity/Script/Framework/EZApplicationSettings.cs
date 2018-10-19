/* Author:          熊哲
 * CreateTime:      2018-10-10 14:51:50
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.IO;
using UnityEngine;

namespace EZUnity.Framework
{
    public class EZApplicationSettings : ScriptableObject
    {
        // 必须放置在Resources目录下！
        public const string FolderPath = "Assets/Resources";
        private static EZApplicationSettings m_Instance;
        public static EZApplicationSettings Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    var assets = Resources.LoadAll<EZApplicationSettings>("");
                    if (assets.Length == 0)
                    {
                        m_Instance = CreateInstance<EZApplicationSettings>();
#if UNITY_EDITOR
                        string assetName = typeof(EZApplicationSettings).Name + ".asset";
                        Directory.CreateDirectory(FolderPath);
                        UnityEditor.AssetDatabase.CreateAsset(m_Instance, Path.Combine(FolderPath, assetName));
#endif
                    }
                    else
                    {
                        m_Instance = assets[0];
                    }
                }
                return m_Instance;
            }
        }

        [SerializeField]
        private RunMode m_RunModeInEditor = RunMode.Develop;
        [SerializeField, Tooltip("Don't use 'Develop Mode' here.")]
        private RunMode m_RunModeInApp = RunMode.Package;
        public RunMode runMode
        {
#if UNITY_EDITOR
            get { return m_RunModeInEditor; }
            set { m_RunModeInEditor = value; }
#else
            get { return m_RunModeInApp; }
            set { m_RunModeInApp = value; }
#endif
        }

        [SerializeField]
        private int m_TargetFrameRate = 30;
        public int targetFrameRate { get { return m_TargetFrameRate; } set { m_TargetFrameRate = value; } }

        #region Update
        [SerializeField]
        private string m_UpdateServer = "";
        public string updateServer { get { return m_UpdateServer; } set { m_UpdateServer = value; } }

        [SerializeField]
        private string m_FileListName = "files";
        public string fileListName { get { return m_FileListName; } set { m_FileListName = value; } }

        [SerializeField]
        private string m_IgnorePrefix = "delay";
        public string ignorePrefix { get { return m_IgnorePrefix; } set { m_IgnorePrefix = value; } }

        [SerializeField]
        private string m_IgnoreSuffix = "delay";
        public string ignoreSuffix { get { return m_IgnoreSuffix; } set { m_IgnoreSuffix = value; } }
        #endregion

        #region Lua
        [SerializeField]
        private string[] m_LuaFolders = new string[] { "Script_Lua" };
        public string[] luaFolders { get { return m_LuaFolders; } set { m_LuaFolders = value; } }

        [SerializeField]
        private string[] m_LuaBundles = new string[] { "script_lua" };
        public string[] luaBundles { get { return m_LuaBundles; } set { m_LuaBundles = value; } }

        [SerializeField]
        private string m_LuaBootModule;
        public string luaBootModule;

        [SerializeField]
        private string m_LuaEntrance;
        public string luaEntrance;

        [SerializeField]
        private string m_LuaExit;
        public string luaExit;
        #endregion
    }
}
