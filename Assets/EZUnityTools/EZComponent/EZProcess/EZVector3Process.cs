/*
 * Author:      熊哲
 * CreateTime:  9/12/2017 4:45:05 PM
 * Description:
 * 
*/
using UnityEngine;

namespace EZComponent.EZProcess
{
    public class EZVector3Process : _EZProcess<Vector3, Vector3Phase>
    {
        protected override void UpdatePhase(float lerp)
        {
            currentValue = Vector3.Lerp(startValue, endValue, lerp);
        }
    }
}