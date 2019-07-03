/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-09-26 17:52:19
 * Organization:    #ORGANIZATION#
 * Description:     
 */
#if XLUA
using UnityEngine;

namespace EZhex1991.EZUnity.XLuaExtension
{
    [DisallowMultipleComponent]
    public class TriggerMessage : _Message<TriggerMessage>
    {
        public class TriggerEvent : OnMessageEvent<Collider> { }

        public TriggerEvent onTriggerEnter = new TriggerEvent();
        public TriggerEvent onTriggerStay = new TriggerEvent();
        public TriggerEvent onTriggerExit = new TriggerEvent();

        void OnTriggerEnter(Collider collider)
        {
            onTriggerEnter.Invoke(collider);
        }
        void OnTriggerStay(Collider collider)
        {
            onTriggerStay.Invoke(collider);
        }
        void OnTriggerExit(Collider collider)
        {
            onTriggerExit.Invoke(collider);
        }
    }
}
#endif
