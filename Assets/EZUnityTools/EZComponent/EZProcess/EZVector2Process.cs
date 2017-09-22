/*
 * Author:      熊哲
 * CreateTime:  9/12/2017 3:45:25 PM
 * Description:
 * 
*/
using UnityEngine;

namespace EZComponent.EZProcess
{
    public class EZVector2Process : _EZProcess<Vector2, Vector2Phase>
    {
        protected override void UpdatePhase(float lerp)
        {
            currentValue = Vector2.Lerp(startValue, endValue, lerp);
        }
    }
}