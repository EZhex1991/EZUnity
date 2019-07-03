/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-09-26 17:58:43
 * Organization:    #ORGANIZATION#
 * Description:     
 */
#if XLUA
using UnityEngine;

namespace EZhex1991.EZUnity.XLuaExtension
{
    [DisallowMultipleComponent]
    public class ApplicationMessage : _Message<ApplicationMessage>
    {
        public class ApplicationEvent : OnMessageEvent<bool> { }

        public ApplicationEvent onApplicationFocus = new ApplicationEvent();
        public ApplicationEvent onApplicationPause = new ApplicationEvent();

        void OnApplicationFocus(bool focusStatus)
        {
            onApplicationFocus.Invoke(focusStatus);
        }
        void OnApplicationPause(bool pauseStatus)
        {
            onApplicationPause.Invoke(pauseStatus);
        }
    }
}
#endif
