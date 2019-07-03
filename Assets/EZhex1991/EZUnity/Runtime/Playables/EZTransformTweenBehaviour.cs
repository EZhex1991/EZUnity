/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-06-22 21:35:32
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEngine;
using UnityEngine.Playables;

namespace EZhex1991.EZUnity.Playables
{
    [Serializable]
    public class EZTransformTweenBehaviour : PlayableBehaviour
    {
        [NonSerialized]
        public Transform startPoint;
        [NonSerialized]
        public Transform endPoint;

        public bool tweenPosition = true;
        public bool tweenRotation = true;
        public bool tweenScale = false;
        public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        public Vector3 startEulerAngles;
        public Vector3 endEulerAngles;

        [NonSerialized]
        public Vector3 startPosition;
        [NonSerialized]
        public Quaternion startRotation = Quaternion.identity;
        [NonSerialized]
        public Vector3 startScale;

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            if (startPoint != null)
            {
                startPosition = startPoint.position;
                startRotation = startPoint.rotation;
                startScale = startPoint.localScale;
            }
        }
    }
}
