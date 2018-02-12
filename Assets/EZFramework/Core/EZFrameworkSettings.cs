/*
 * Author:      熊哲
 * CreateTime:  1/16/2017 2:13:01 PM
 * Description:
 * 
*/
using System.Collections.Generic;
using UnityEngine;

namespace EZFramework
{
    public class EZFrameworkSettings : ScriptableObject
    {
        // 必须放置在Resources目录下！
        public const string AssetDirPath = "Assets/Resources/";
        public const string AssetName = "EZFrameworkSettings";
        private static EZFrameworkSettings instance;
        public static EZFrameworkSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<EZFrameworkSettings>(AssetName);
                    if (instance == null) instance = CreateInstance<EZFrameworkSettings>();
                }
                return instance;
            }
        }

        public enum RunMode
        {
            Develop,
            Local,
            Update,
        }
        public enum SleepTimeout
        {
            NeverSleep = UnityEngine.SleepTimeout.NeverSleep,
            SystemSetting = UnityEngine.SleepTimeout.SystemSetting,
        }

        [SerializeField]
        private RunMode m_RunMode = RunMode.Develop;
        public RunMode runMode { get { return m_RunMode; } set { m_RunMode = value; } }

        [SerializeField]
        private SleepTimeout m_SleepTimeout = SleepTimeout.SystemSetting;
        public SleepTimeout sleepTimeout { get { return m_SleepTimeout; } set { m_SleepTimeout = value; } }

        [SerializeField]
        private bool m_RunInBackground = false;
        public bool runInBackground { get { return m_RunInBackground; } set { m_RunInBackground = value; } }

        [SerializeField, Range(15, 120)]
        private int m_TargetFrameRate = 30;
        public int targetFrameRate { get { return m_TargetFrameRate; } set { m_TargetFrameRate = value; } }

        [SerializeField]
        private string m_UpdateServer = "";
        public string updateServer { get { return m_UpdateServer; } set { m_UpdateServer = value; } }

        [SerializeField]
        private string m_BundleExtension = "";
        public string bundleExtension { get { return m_BundleExtension; } set { m_BundleExtension = value; } }

        [SerializeField]
        private List<string> m_LuaDirList = new List<string> { "Script_Lua" };
        public List<string> luaDirList { get { return m_LuaDirList; } set { m_LuaDirList = value; } }

        [SerializeField]
        private List<string> m_LuaBundleList = new List<string> { "script_lua" };
        public List<string> luaBundleList { get { return m_LuaBundleList; } set { m_LuaBundleList = value; } }
    }
}