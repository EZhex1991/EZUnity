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
        protected override void Awake()
        {
            base.Awake();
            transform.SetParent(EZFacade.Instance.transform);
            Log("Activated");
        }
        protected override void OnDestroy()
        {
            Log("Destroyed");
            base.OnDestroy();
        }
    }
}