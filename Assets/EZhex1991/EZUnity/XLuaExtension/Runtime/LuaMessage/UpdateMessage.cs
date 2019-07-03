/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-09-26 17:56:03
 * Organization:    #ORGANIZATION#
 * Description:     
 */
#if XLUA
using UnityEngine;

namespace EZhex1991.EZUnity.XLuaExtension
{
    [DisallowMultipleComponent]
    public class UpdateMessage : _Message<UpdateMessage>
    {
        public class UpdateEvent : OnMessageEvent { }

        public UpdateEvent update = new UpdateEvent();
        public UpdateEvent fixedUpdate = new UpdateEvent();
        public UpdateEvent lateUpdate = new UpdateEvent();

        void Update()
        {
            update.Invoke();
        }
        void FixedUpdate()
        {
            fixedUpdate.Invoke();
        }
        void LateUpdate()
        {
            lateUpdate.Invoke();
        }
    }
}
#endif
