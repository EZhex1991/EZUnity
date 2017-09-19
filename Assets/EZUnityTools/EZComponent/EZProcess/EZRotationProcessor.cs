/*
 * Author:      熊哲
 * CreateTime:  9/13/2017 7:16:14 PM
 * Description:
 * 
*/
using UnityEngine;

namespace EZComponent.EZProcess
{
    [DisallowMultipleComponent]
    public class EZRotationProcessor : EZVector3Process
    {
        protected override void StartPhase(int index)
        {
            // Rotation比较特别，负数自动变成正数会造成非预期旋转，所以不支持从“当前位置开始旋转”
            base.StartPhase(index);
        }
        protected override void UpdatePhase(float lerp)
        {
            base.UpdatePhase(lerp);
            transform.localRotation = Quaternion.Euler(value);
        }
    }
}