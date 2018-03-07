/*
 * Author:      熊哲
 * CreateTime:  1/16/2017 2:06:29 PM
 * Description:
 * 
*/
using UnityEngine;

namespace EZFramework
{
    public abstract class EZSingleton<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        private static string LogTag = typeof(T).Name;

        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        instance = new GameObject(typeof(T).Name).AddComponent<T>();
                    }
                }
                return instance;
            }
        }

        [SerializeField]
        private bool m_ShowLog = true;
        public bool showLog
        {
            get
            {
#if UNITY_EDITOR
                return m_ShowLog;
#else
                return true;
#endif
            }
        }

        // Awake时对该单例进行初始化，防止出现多实例
        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
            }
            else if (instance != this)
            {
                Destroy(this);
                return;
            }

            if (transform.parent == null)
            {
                DontDestroyOnLoad(gameObject);  // 单例在游戏中不要销毁
            }
        }
        protected virtual void OnDestroy()
        {
            instance = null;
        }

#if UNITY_5
        public void Log(string log) { if (showLog) Debug.logger.Log(LogTag, log, Instance); }
        public void LogWarning(string log) { if (showLog) Debug.logger.LogWarning(LogTag, log, Instance); }
        public void LogError(string log) { if (showLog) Debug.logger.LogError(LogTag, log, Instance); }
#else
        public void Log(string log) { if (showLog) Debug.unityLogger.Log(LogTag, log, Instance); }
        public void LogWarning(string log) { if (showLog) Debug.unityLogger.LogWarning(LogTag, log, Instance); }
        public void LogError(string log) { if (showLog) Debug.unityLogger.LogError(LogTag, log, Instance); }
#endif
    }
}