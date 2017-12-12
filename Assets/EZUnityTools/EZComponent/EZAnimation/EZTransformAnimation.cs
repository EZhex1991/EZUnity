/*
 * Author:      熊哲
 * CreateTime:  10/31/2017 5:21:48 PM
 * Description:
 * 
*/
using UnityEngine;

namespace EZComponent.EZAnimation
{
    public class EZTransformAnimation : EZAnimation<TransformInfo, TransformPhase>
    {
        [SerializeField]
        private V3Driver m_PositionDriver;
        public V3Driver positionDriver { get { return m_PositionDriver; } set { m_PositionDriver = value; } }
        [SerializeField]
        private V3Driver m_RotationDriver;
        public V3Driver rotationDriver { get { return m_RotationDriver; } set { m_RotationDriver = value; } }
        [SerializeField]
        private V3Driver m_ScaleDriver;
        public V3Driver scaleDriver { get { return m_ScaleDriver; } set { m_ScaleDriver = value; } }

        protected override void OnPhaseUpdate()
        {
            if (positionDriver)
            {
                Vector3 position = Vector3.Lerp(currentPhase.startValue.position, currentPhase.endValue.position, frameValue);
                Vector3 targetPosition = transform.localPosition;
                if (positionDriver.x) targetPosition.x = position.x;
                if (positionDriver.y) targetPosition.y = position.y;
                if (positionDriver.z) targetPosition.z = position.z;
                transform.localPosition = targetPosition;
            }

            if (rotationDriver)
            {
                Vector3 rotation = Vector3.Lerp(currentPhase.startValue.rotation, currentPhase.endValue.rotation, frameValue);
                Vector3 targetRotation = transform.localEulerAngles;
                if (rotationDriver.x) targetRotation.x = rotation.x;
                if (rotationDriver.y) targetRotation.y = rotation.y;
                if (rotationDriver.z) targetRotation.z = rotation.z;
                transform.localEulerAngles = targetRotation;
            }

            if (scaleDriver)
            {
                Vector3 scale = Vector3.Lerp(currentPhase.startValue.scale, currentPhase.endValue.scale, frameValue);
                Vector3 targetScale = transform.localScale;
                if (scaleDriver.x) targetScale.x = scale.x;
                if (scaleDriver.y) targetScale.y = scale.y;
                if (scaleDriver.z) targetScale.z = scale.z;
                transform.localScale = targetScale;
            }
        }
    }
}