/*
 * Author:      熊哲
 * CreateTime:  10/31/2017 5:21:48 PM
 * Description:
 * 
*/
using System;
using UnityEngine;

namespace EZComponent.EZAnimation
{
    public class EZTransformAnimation : EZAnimation<EZTransformAnimation.TransformInfo, EZTransformAnimation.Phase>
    {
        [Serializable]
        public struct TransformInfo
        {
            [SerializeField]
            private Vector3 m_Position;
            public Vector3 position { get { return m_Position; } set { m_Position = value; } }

            [SerializeField]
            private Vector3 m_Rotation;
            public Vector3 rotation { get { return m_Rotation; } set { m_Rotation = value; } }

            [SerializeField]
            private Vector3 m_Scale;
            public Vector3 scale { get { return m_Scale; } set { m_Scale = value; } }

            public TransformInfo(Vector3 position, Vector3 rotation, Vector3 scale)
            {
                m_Position = position;
                m_Rotation = rotation;
                m_Scale = scale;
            }
        }
        [Serializable]
        public class Phase : Phase<TransformInfo>
        {
        }

        [SerializeField]
        private bool m_DrivePosition;
        public bool drivePosition { get { return m_DrivePosition; } set { m_DrivePosition = value; } }
        [SerializeField]
        private bool m_DriveRotation;
        public bool driveRotation { get { return m_DriveRotation; } set { m_DriveRotation = value; } }
        [SerializeField]
        private bool m_DriveScale;
        public bool driveScale { get { return m_DriveScale; } set { m_DriveScale = value; } }

        protected override void UpdatePhase()
        {
            if (drivePosition) transform.localPosition = Vector3.Lerp(currentPhase.startValue.position, currentPhase.endValue.position, frameValue);
            if (driveRotation) transform.localEulerAngles = Vector3.Lerp(currentPhase.startValue.rotation, currentPhase.endValue.rotation, frameValue);
            if (driveScale) transform.localScale = Vector3.Lerp(currentPhase.startValue.scale, currentPhase.endValue.scale, frameValue);
        }
    }
}