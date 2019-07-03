/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-09-26 18:07:04
 * Organization:    #ORGANIZATION#
 * Description:     
 */
#if XLUA
using UnityEngine;

namespace EZhex1991.EZUnity.XLuaExtension
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
#endif
