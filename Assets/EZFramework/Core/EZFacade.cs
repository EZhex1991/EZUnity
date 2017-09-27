/*
 * Author:      熊哲
 * CreateTime:  1/16/2017 2:10:50 PM
 * Description:
 * 
*/
using System;
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

        public delegate void OnApplicationFocusAction(bool focusStatus);
        public event OnApplicationFocusAction onApplicationFocusEvent;
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
        void OnApplicationFocus(bool focusStatus)
        {
            if (onApplicationFocusEvent != null) onApplicationFocusEvent(focusStatus);
            if (!focusStatus)
            {
                // 暂停时存档（iOS一般不会退出）
                EZDatabase.Instance.SaveData();
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