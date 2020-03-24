/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-01-16 14:06:29
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public abstract class EZMonoBehaviourSingleton<T> : MonoBehaviour
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
                        m_Instance = new GameObject(LogTag).AddComponent<T>();
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
                Debug.LogWarningFormat("Duplicate Singleton Instance: typeof {0}", typeof(T));
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

        protected virtual void Init() { }
        protected virtual void Dispose() { }

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