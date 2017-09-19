/*
 * Author:      熊哲
 * CreateTime:  9/13/2017 7:13:02 PM
 * Description:
 * 
*/
using UnityEngine;

namespace EZComponent.EZProcess
{
    [DisallowMultipleComponent]
    public class EZScaleProcessor : EZVector3Process
    {
        protected override void StartPhase(int index)
        {
            startValue = transform.localScale;
            base.StartPhase(index);
        }
        protected override void UpdatePhase(float lerp)
        {
            base.UpdatePhase(lerp);
            transform.localScale = value;
        }
    }
}