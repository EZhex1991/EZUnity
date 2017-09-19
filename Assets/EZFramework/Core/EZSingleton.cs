/*
 * Author:      熊哲
 * CreateTime:  1/16/2017 2:06:29 PM
 * Description:
 * 
*/
using UnityEngine;

namespace EZFramework
{
    public class EZSingleton<T> : MonoBehaviour
        where T : MonoBehaviour
    {
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
    }
}