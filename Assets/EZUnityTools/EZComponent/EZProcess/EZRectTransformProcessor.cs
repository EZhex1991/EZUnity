/*
 * Author:      熊哲
 * CreateTime:  9/20/2017 2:36:28 PM
 * Description:
 * 
*/
using System;
using UnityEngine;

namespace EZComponent.EZProcess
{
    [RequireComponent(typeof(RectTransform))]
    public class EZRectTransformProcessor : _EZProcess<EZRectTransformProcessor.RectTransformInfo, EZRectTransformProcessor.RectTransformPhase>
    {
        [Serializable]
        public struct RectTransformInfo
        {
            [SerializeField]
            private Vector2 m_AnchoredPosition;
            public Vector2 anchoredPosition { get { return m_AnchoredPosition; } set { m_AnchoredPosition = value; } }
            [SerializeField]
            private Vector2 m_SizeDelta;
            public Vector2 sizeDelta { get { return m_SizeDelta; } set { m_SizeDelta = value; } }
            [SerializeField]
            private Vector3 m_Rotation;
            public Vector3 rotation { get { return m_Rotation; } set { m_Rotation = value; } }
            public RectTransformInfo(Vector2 anchoredPosition, Vector2 sizeDelta, Vector3 rotation)
            {
                m_AnchoredPosition = anchoredPosition;
                m_SizeDelta = sizeDelta;
                m_Rotation = rotation;
            }
        }
        [Serializable]
        public class RectTransformPhase : Phase<RectTransformInfo>
        {
            public RectTransformPhase(RectTransformInfo endValue, float duration, float interval = 0, LerpMode lerpMode = LerpMode.Linear) : base(endValue, duration, interval, lerpMode)
            {
            }
        }

        [NonSerialized]
        private RectTransform m_RectTransform;
        private RectTransform rectTransform
        {
            get
            {
                if (m_RectTransform == null)
                {
                    m_RectTransform = GetComponent<RectTransform>();
                }
                return m_RectTransform;
            }
        }

        [SerializeField]
        private bool m_DrivePosition;
        public bool drivePosition { get { return m_DrivePosition; } set { m_DrivePosition = value; } }
        [SerializeField]
        private bool m_DriveSize;
        public bool driveSize { get { return m_DriveSize; } set { m_DriveSize = value; } }
        [SerializeField]
        private bool m_DriveRotation;
        public bool driveRotation { get { return m_DriveRotation; } set { m_DriveRotation = value; } }

        protected override void StartPhase(int index = 0)
        {
            startValue.anchoredPosition = rectTransform.anchoredPosition;
            startValue.sizeDelta = rectTransform.sizeDelta;
            // Rotation比较特别，负数自动变成正数会造成非预期旋转，所以不支持从“当前位置开始旋转”
            base.StartPhase(index);
        }
        protected override void UpdatePhase(float lerp)
        {
            currentValue.anchoredPosition = Vector2.Lerp(startValue.anchoredPosition, endValue.anchoredPosition, lerp);
            currentValue.sizeDelta = Vector2.Lerp(startValue.sizeDelta, endValue.sizeDelta, lerp);
            currentValue.rotation = Vector3.Lerp(startValue.rotation, endValue.rotation, lerp);
            if (drivePosition) rectTransform.anchoredPosition = currentValue.anchoredPosition;
            if (driveSize) rectTransform.sizeDelta = currentValue.sizeDelta;
            if (driveRotation) rectTransform.localRotation = Quaternion.Euler(currentValue.rotation);
        }
    }
}