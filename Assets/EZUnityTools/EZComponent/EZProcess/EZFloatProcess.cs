/*
 * Author:      熊哲
 * CreateTime:  9/20/2017 10:29:29 AM
 * Description:
 * 
*/
using UnityEngine;

namespace EZComponent.EZProcess
{
    public class EZFloatProcess : _EZProcess<float, FloatPhase>
    {
        protected override void UpdatePhase(float lerp)
        {
            currentValue = Mathf.Lerp(startValue, endValue, lerp);
        }
    }
}