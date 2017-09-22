/*
 * Author:      熊哲
 * CreateTime:  9/20/2017 10:18:18 AM
 * Description:
 * 
*/
using System;
using UnityEngine;

namespace EZComponent.EZProcess
{
    public class EZTransformProcessor : _EZProcess<EZTransformProcessor.TransformInfo, EZTransformProcessor.TransformPhase>
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
        public class TransformPhase : Phase<TransformInfo>
        {
            public TransformPhase(TransformInfo endValue, float duration, float interval = 0, LerpMode lerpMode = LerpMode.Linear) : base(endValue, duration, interval, lerpMode)
            {
            }
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

        protected override void StartPhase(int index = 0)
        {
            startValue.position = transform.localPosition;
            startValue.scale = transform.localScale;
            // Rotation比较特别，负数自动变成正数会造成非预期旋转，所以不支持从“当前位置开始旋转”
            base.StartPhase(index);
        }
        protected override void UpdatePhase(float lerp)
        {
            currentValue.position = Vector3.Lerp(startValue.position, endValue.position, lerp);
            currentValue.scale = Vector3.Lerp(startValue.scale, endValue.scale, lerp);
            currentValue.rotation = Vector3.Lerp(startValue.rotation, endValue.rotation, lerp);
            if (drivePosition) transform.localPosition = currentValue.position;
            if (driveScale) transform.localScale = currentValue.scale;
            if (driveRotation) transform.localRotation = Quaternion.Euler(currentValue.rotation);
        }
    }
}