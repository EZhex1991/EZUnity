/*
 * Author:      熊哲
 * CreateTime:  9/26/2017 6:07:04 PM
 * Description:
 * 
*/
using UnityEngine;

namespace EZFramework.XLuaExtension
{
    [DisallowMultipleComponent]
    public class ActivityMessage : _Message<ActivityMessage>
    {
        public class ActivityEvent : OnMessageEvent { }

        public ActivityEvent start = new ActivityEvent();
        public ActivityEvent onEnable = new ActivityEvent();
        public ActivityEvent onDisable = new ActivityEvent();
        public ActivityEvent onDestroy = new ActivityEvent();

        void Start()
        {
            start.Invoke();
        }
        void OnEnable()
        {
            onEnable.Invoke();
        }
        void OnDisable()
        {
            onDisable.Invoke();
        }
        void OnDestroy()
        {
            onDestroy.Invoke();
        }
    }
}