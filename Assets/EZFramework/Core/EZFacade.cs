/*
 * Author:      熊哲
 * CreateTime:  1/16/2017 2:10:50 PM
 * Description:
 * 
*/
using System.Collections;
using UnityEngine;

namespace EZFramework
{
    public class EZFacade : EZSingleton<EZFacade>
    {
        [SerializeField, Tooltip("Don't use 'Develop Mode' here.")]
        private EZFrameworkSettings.RunMode runModeInApp = EZFrameworkSettings.RunMode.Local;

        public string dataDirPath;
        public string streamingDirPath;
        public string persistentDirPath;

#if UNITY_5
        private static ILogger unityLogger = Debug.logger;
#else
        private static ILogger unityLogger = Debug.unityLogger;
#endif
        private static ILogHandler unityLogHandler = unityLogger.logHandler;

        public delegate void OnApplicationAction();
        public event OnApplicationAction onApplicationStartEvent;
        public event OnApplicationAction onApplicationQuitEvent;
        public delegate void OnApplicationStatusAction(bool status);
        public event OnApplicationStatusAction onApplicationPauseEvent; // 暂停时先触发pause后触发focus
        public event OnApplicationStatusAction onApplicationFocusEvent; // 唤醒时先触发focus后触发pause

        protected override void Awake()
        {
            base.Awake();

            dataDirPath = Application.dataPath + "/";
            streamingDirPath = Application.streamingAssetsPath + "/";
#if UNITY_EDITOR
            persistentDirPath = Application.dataPath + "/EZData/";
#else
            persistentDirPath = Application.persistentDataPath + "/Data/";   // persistent没有删除权限，建立子文件夹读写更方便
            if (runModeInApp == EZFrameworkSettings.RunMode.Develop) runModeInApp = EZFrameworkSettings.RunMode.Local;
            EZFrameworkSettings.Instance.runMode = runModeInApp;
#endif

            unityLogger.logHandler = new EZLogHandler(persistentDirPath + "EZLog/");

            Screen.sleepTimeout = (int)EZFrameworkSettings.Instance.sleepTimeout;
            Application.runInBackground = EZFrameworkSettings.Instance.runInBackground;
            Application.targetFrameRate = EZFrameworkSettings.Instance.targetFrameRate;
        }
        void Start()
        {
            if (onApplicationStartEvent != null) onApplicationStartEvent();
        }
        void OnApplicationQuit()
        {
            if (onApplicationQuitEvent != null) onApplicationQuitEvent();
            unityLogger.logHandler = unityLogHandler;
        }

        IEnumerator OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                if (onApplicationPauseEvent != null) onApplicationPauseEvent(true);
                // 暂停时存档（iOS一般不会退出）
                EZDatabase.Instance.SaveData();
                yield return null;  // 暂停完等待一帧去刷新暂停时的画面
            }
            else
            {
                yield return null;  // 恢复前等待一帧确保回到UnityActivity并且画面正常
                if (onApplicationPauseEvent != null) onApplicationPauseEvent(false);
            }
        }
        IEnumerator OnApplicationFocus(bool focusStatus)
        {
            if (focusStatus)
            {
                yield return null;
                if (onApplicationFocusEvent != null) onApplicationFocusEvent(true);
            }
            else
            {
                if (onApplicationFocusEvent != null) onApplicationFocusEvent(false);
                yield return null;
            }
        }
    }
}