/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-10-24 14:03:36
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEngine;
using UnityEngine.Playables;

namespace EZhex1991.EZUnity.Playables
{
    [Serializable]
    public class EZMaterialBehaviour : PlayableBehaviour
    {
        public Material startValue;
        public Material endValue;
        public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);

        public float process { get; set; }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            process = curve.Evaluate((float)(playable.GetTime() / playable.GetDuration()));
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            Material material = playerData as Material;
            if (material == null) return;
            if (startValue == null || endValue == null) return;
            material.Lerp(startValue, endValue, process);
        }
    }
}
