/*
 * Author:      熊哲
 * CreateTime:  11/2/2017 5:02:01 PM
 * Description:
 * 
*/
using System;
using UnityEngine;

namespace EZComponent.EZAnimation
{
    [RequireComponent(typeof(RectTransform))]
    public class EZRectTransformAnimation : EZAnimation<EZRectTransformAnimation.RectTransformInfo, EZRectTransformAnimation.Phase>
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
            [SerializeField]
            private Vector3 m_Scale;
            public Vector3 scale { get { return m_Scale; } set { m_Scale = value; } }
        }
        [Serializable]
        public class Phase : Phase<RectTransformInfo>
        {

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
        [SerializeField]
        private bool m_DriveScale;
        public bool driveScale { get { return m_DriveScale; } set { m_DriveScale = value; } }

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

        protected override void UpdatePhase()
        {
            if (drivePosition) rectTransform.anchoredPosition = Vector2.Lerp(currentPhase.startValue.anchoredPosition, currentPhase.endValue.anchoredPosition, frameValue);
            if (driveSize) rectTransform.sizeDelta = Vector2.Lerp(currentPhase.startValue.sizeDelta, currentPhase.endValue.sizeDelta, frameValue);
            if (driveRotation) rectTransform.localEulerAngles = Vector3.Lerp(currentPhase.startValue.rotation, currentPhase.endValue.rotation, frameValue);
            if (driveScale) transform.localScale = Vector3.Lerp(currentPhase.startValue.scale, currentPhase.endValue.scale, frameValue);
        }
    }
}