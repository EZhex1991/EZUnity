/*
 * Author:      熊哲
 * CreateTime:  9/11/2017 12:23:16 PM
 * Description:
 * 
*/
using UnityEngine;

namespace EZComponent.EZProcess
{
    [DisallowMultipleComponent]
    public class EZPositionProcessor : EZVector3Process
    {
        protected override void StartPhase(int index)
        {
            startValue = transform.localPosition;
            base.StartPhase(index);
        }
        protected override void UpdatePhase(float lerp)
        {
            base.UpdatePhase(lerp);
            transform.localPosition = value;
        }
    }
}