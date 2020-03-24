/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-10-10 14:51:50
 * Organization:    #ORGANIZATION#
 * Description:     
 */
#pragma warning disable 0414
using UnityEngine;

namespace EZhex1991.EZUnity.Framework
{
    public enum PackageMode
    {
        Develop = 0,
        Local = 1,
        Remote = 2
    }

    public class EZApplicationSettings : EZScriptableObjectSingleton<EZApplicationSettings>
    {
        [SerializeField]
        private PackageMode m_PackageModeInEditor = PackageMode.Develop;
        [SerializeField, Tooltip("Don't use 'Develop Mode' here.")]
        private PackageMode m_PackageModeInApp = PackageMode.Local;
        public PackageMode packageMode
        {
#if UNITY_EDITOR
            get { return m_PackageModeInEditor; }
            set { m_PackageModeInEditor = value; }
#else
            get { return m_PackageModeInApp; }
            set { m_PackageModeInApp = value; }
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
    }
}
