/*
 * Author:      熊哲
 * CreateTime:  1/16/2017 2:27:06 PM
 * Description:
 * 管理器的模板，初始化和退出不使用MonoBehaviour的Message是为了更好地控制加载和退出顺序。
*/
using UnityEngine;

namespace EZFramework
{
    public abstract class _EZManager<T> : EZSingleton<T>
        where T : MonoBehaviour
    {
        // 该管理器的初始化，可以根据需要重写
        public virtual void Init()
        {
            transform.SetParent(EZFacade.Instance.transform);
            gameObject.hideFlags = EZFacade.Instance.gameObject.hideFlags;
            Log("Activated");
        }
        // 程序退出时管理器需要执行的逻辑，单例一般不建议手动销毁
        public virtual void Exit()
        {
            Log("Exit");
        }

        // 是否显示该管理器的日志， 由于程序中管理器可能会很多，所以做了一个这样的开关
        public bool showLog = true;
#if UNITY_EDITOR
        protected void Log(string log) { if (showLog) Debug.logger.Log(typeof(T).Name, log); }
        protected void LogWarning(string log) { if (showLog) Debug.logger.LogWarning(typeof(T).Name, log); }
        protected void LogError(string log) { if (showLog) Debug.logger.LogError(typeof(T).Name, log); }
#else
        protected void Log(string log) { Debug.logger.Log(typeof(T).Name, log); }
        protected void LogWarning(string log) { Debug.logger.LogWarning(typeof(T).Name, log); }
        protected void LogError(string log) { Debug.logger.LogError(typeof(T).Name, log); }
#endif
    }
}