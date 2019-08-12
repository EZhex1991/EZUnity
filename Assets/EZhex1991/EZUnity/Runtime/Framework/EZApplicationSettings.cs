/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-10-10 14:51:50
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity.Framework
{
    public class EZApplicationSettings : EZScriptableObjectSingleton<EZApplicationSettings>
    {
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

        #region Quality
        [SerializeField]
        private int m_SleepTimeout = SleepTimeout.NeverSleep;
        public int sleepTimeout { get { return m_SleepTimeout; } set { m_SleepTimeout = value; } }
        [SerializeField]
        private int m_TargetFrameRate = 30;
        public int targetFrameRate { get { return m_TargetFrameRate; } set { m_TargetFrameRate = value; } }
        #endregion

        #region Update
        [SerializeField]
        private string m_AndroidServer = "";
        [SerializeField]
        private string m_IOSServer = "";
        [SerializeField]
        private string m_DefaultServer = "";
#if UNITY_ANDROID
        public string updateServer { get { return m_AndroidServer; } set { m_AndroidServer = value; } }
#elif UNITY_IOS
        public string updateServer { get { return m_IOSServer; } set { m_IOSServer = value; } }
#else
        public string updateServer { get { return m_DefaultServer; } set { m_DefaultServer = value; } }
#endif
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
