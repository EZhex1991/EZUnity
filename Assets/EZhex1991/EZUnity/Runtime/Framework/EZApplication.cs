/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-06-19 14:53:50
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections;
using UnityEngine;

namespace EZhex1991.EZUnity.Framework
{
    public class EZApplication : EZMonoBehaviourSingleton<EZApplication>
    {
        public EZApplicationSettings settings { get { return EZApplicationSettings.Instance; } }
        public PackageMode packageMode { get { return settings.packageMode; } }

        public string persistentDataPath
        {
            get
            {
#if UNITY_EDITOR
                return Application.dataPath.Substring(0, Application.dataPath.Length - 6) + "EZPersistent/";
#else
                return Application.persistentDataPath + "/EZPersistent/"; // 部分设备persistent没有删除权限，建立子文件夹读写更方便
#endif
            }
        }

        public delegate void OnApplicationAction();
        public event OnApplicationAction onApplicationStartEvent;
        public event OnApplicationAction onApplicationQuitEvent;
        public delegate void OnApplicationStatusAction(bool status);
        public event OnApplicationStatusAction onApplicationPauseEvent;
        public event OnApplicationStatusAction onApplicationFocusEvent;

        protected override void Init()
        {
            Screen.sleepTimeout = settings.sleepTimeout;
            Application.targetFrameRate = settings.targetFrameRate;
            System.IO.Directory.CreateDirectory(persistentDataPath);
            if (onApplicationStartEvent != null) onApplicationStartEvent();
        }

        private IEnumerator OnApplicationQuit()
        {
            yield return null;
            if (onApplicationQuitEvent != null) onApplicationQuitEvent();
        }

        private IEnumerator OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                if (onApplicationPauseEvent != null) onApplicationPauseEvent(true);
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
        private IEnumerator OnApplicationFocus(bool focusStatus)
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
