/*
 * Author:      熊哲
 * CreateTime:  1/16/2017 2:06:29 PM
 * Description:
 * 
*/
using UnityEngine;

namespace EZUnity
{
    public abstract class _EZMonoBehaviourSingleton<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        private static readonly string LogTag = typeof(T).Name;

        private static T m_Instance;
        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = FindObjectOfType<T>();
                    if (m_Instance == null)
                    {
                        m_Instance = new GameObject(typeof(T).Name).AddComponent<T>();
                    }
                }
                return m_Instance;
            }
        }
        public static T GetInstance() { return Instance; }

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

        protected void Awake()
        {
            if (m_Instance == null)
            {
                m_Instance = this as T;
            }
            else if (m_Instance != this)
            {
                LogWarning("Duplicate Singleton Instance!");
                Destroy(this);
                return;
            }

            if (transform.parent == null)
            {
                DontDestroyOnLoad(gameObject);
            }
            Log("Init Singleton");
            Init();
        }
        protected void OnDestroy()
        {
            Log("Dispose Singleton");
            Dispose();
        }

        protected abstract void Init();
        protected abstract void Dispose();

        public void Log(string message)
        {
            if (showLog) Debug.LogFormat(this, "{0}\t{1}", LogTag, message);
        }
        public void LogFormat(string format, params object[] args)
        {
            if (showLog) Log(string.Format(format, args));
        }

        public void LogWarning(string message)
        {
            if (showLog) Debug.LogWarningFormat(this, "{0}\t{1}", LogTag, message);
        }
        public void LogWarningFormat(string format, params object[] args)
        {
            if (showLog) LogWarning(string.Format(format, args));
        }

        public void LogError(string message)
        {
            if (showLog) Debug.LogErrorFormat(this, "{0}\t{1}", LogTag, message);
        }
        public void LogErrorFormat(string format, params object[] args)
        {
            if (showLog) LogError(string.Format(format, args));
        }
    }
}