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
        [SerializeField]
        private bool hideFacade = false;
        [SerializeField]
        private bool useDefaultLogHandler = true;
        [SerializeField, Tooltip("Don't use 'Develop Mode' here.")]
        private EZFrameworkSettings.RunMode runModeInApp = EZFrameworkSettings.RunMode.Local;

        private ILogHandler defaultLogHandler = Debug.logger.logHandler;

        public delegate void OnApplicationStatusAction(bool status);
        public event OnApplicationStatusAction onApplicationPauseEvent; // 暂停时先触发pause后触发focus
        public event OnApplicationStatusAction onApplicationFocusEvent; // 唤醒时先触发focus后触发pause
        public delegate void OnApplicationQuitAction();
        public event OnApplicationQuitAction onApplicationQuitEvent;

        void Start()
        {
#if !UNITY_EDITOR
            if (runModeInApp == EZFrameworkSettings.RunMode.Develop) runModeInApp = EZFrameworkSettings.RunMode.Local;
            EZFrameworkSettings.Instance.runMode = runModeInApp;
#endif
            gameObject.hideFlags = hideFacade ? HideFlags.HideInHierarchy : HideFlags.None;
            if (useDefaultLogHandler)
            {
                Debug.logger.logHandler = defaultLogHandler;
            }
            else
            {
                string logPath = EZFacade.persistentDirPath + "EZLog/";
                Debug.logger.logHandler = new EZLogHandler(logPath);
            }

            Screen.sleepTimeout = (int)EZFrameworkSettings.Instance.sleepTimeout;
            Application.runInBackground = EZFrameworkSettings.Instance.runInBackground;
            Application.targetFrameRate = EZFrameworkSettings.Instance.targetFrameRate;

            EZNetwork.Instance.Init();
            EZUpdate.Instance.Init();
            EZUpdate.Instance.StartUpdate(delegate ()
            {
                EZDatabase.Instance.Init();
                EZResource.Instance.Init();
                EZUI.Instance.Init();
                EZSound.Instance.Init();
                EZLua.Instance.Init();
            });
        }
        IEnumerator OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                if (onApplicationPauseEvent != null) onApplicationPauseEvent(true);
                // 暂停时存档（iOS一般不会退出）
                EZDatabase.Instance.SaveData();
                yield return null;
            }
            else
            {
                yield return null;
                if (onApplicationPauseEvent != null) onApplicationPauseEvent(false);
            }
        }
        IEnumerator OnApplicationFocus(bool focusStatus)
        {
            if (focusStatus)
            {
                if (onApplicationFocusEvent != null) onApplicationFocusEvent(true);
                yield return null;
            }
            else
            {
                yield return null;
                if (onApplicationFocusEvent != null) onApplicationFocusEvent(false);
            }
        }
        void OnApplicationQuit()
        {
            if (onApplicationQuitEvent != null) onApplicationQuitEvent();
            EZLua.Instance.Exit();
            EZSound.Instance.Exit();
            EZUI.Instance.Exit();
            EZResource.Instance.Exit();
            EZDatabase.Instance.Exit();
            EZUpdate.Instance.Exit();
            EZNetwork.Instance.Exit();
            Debug.logger.logHandler = defaultLogHandler;
        }

        public static string dataDirPath
        {
            get { return Application.dataPath + "/"; }
        }
        public static string streamingDirPath
        {
            get { return Application.streamingAssetsPath + "/"; }
        }
        public static string persistentDirPath
        {
            get
            {
#if UNITY_EDITOR
                return Application.dataPath + "/EZData/";
#else
                return Application.persistentDataPath + "/Data/";   // persistent没有删除权限，建立子文件夹读写更方便
#endif
            }
        }

        public static bool IsNetAvailable
        {
            get { return Application.internetReachability != NetworkReachability.NotReachable; }
        }
        public static bool IsNetLocal
        {
            get { return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork; }
        }
    }
}