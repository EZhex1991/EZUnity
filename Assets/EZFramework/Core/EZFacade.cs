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
    // 任何初始化放在Awake里的field都需要改ExecuteOrder才能保证其能在其它Manager的Awake里正常调用
    // 所以，要么使用property，要么把field放到EZFrameworkSettings里通过Instance调用
    public class EZFacade : EZSingleton<EZFacade>
    {
        // 不能再static或者constructor里调用Application.dataPath
        public string dataDirPath { get { return Application.dataPath + "/"; } }
        public string streamingDirPath { get { return Application.streamingAssetsPath + "/"; } }
        public string persistentDirPath
        {
            get
            {
#if UNITY_EDITOR
                return Application.dataPath + "/EZData/";
#else
                return Application.persistentDataPath + "/Data/"; // 部分设备persistent没有删除权限，建立子文件夹读写更方便
#endif
            }
        }

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
        // On Android, when the on-screen keyboard is enabled, it causes a OnApplicationFocus( false ) event.
        // Additionally, if you press Home at the moment the keyboard is enabled, the OnApplicationFocus() event is not called, but OnApplicationPause() is called instead.
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