/*
 * Author:      熊哲
 * CreateTime:  11/2/2017 5:02:01 PM
 * Description:
 * 
*/
using System;
using UnityEngine;

namespace EZUnity.Animation
{
    [RequireComponent(typeof(RectTransform))]
    public class EZRectTransformAnimation : EZAnimation<RectTransformInfo, EZRectTransformAnimationSegment>
    {
        [SerializeField]
        private V2Driver m_PositionDriver;
        public V2Driver positionDriver { get { return m_PositionDriver; } set { m_PositionDriver = value; } }
        [SerializeField]
        private V2Driver m_SizeDriver;
        public V2Driver sizeDriver { get { return m_SizeDriver; } set { m_SizeDriver = value; } }
        [SerializeField]
        private V3Driver m_RotationDriver;
        public V3Driver rotationDriver { get { return m_RotationDriver; } set { m_RotationDriver = value; } }
        [SerializeField]
        private V3Driver m_ScaleDriver;
        public V3Driver scaleDriver { get { return m_ScaleDriver; } set { m_ScaleDriver = value; } }

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

        protected override void OnSegmentUpdate()
        {
            if (positionDriver)
            {
                Vector2 position = Vector2.Lerp(segment.startValue.anchoredPosition, segment.endValue.anchoredPosition, value);
                Vector2 targetPosition = rectTransform.anchoredPosition;
                if (positionDriver.x) targetPosition.x = position.x;
                if (positionDriver.y) targetPosition.y = position.y;
                rectTransform.anchoredPosition = targetPosition;
            }

            if (sizeDriver)
            {
                Vector2 size = Vector2.Lerp(segment.startValue.sizeDelta, segment.endValue.sizeDelta, value);
                Vector2 targetSize = rectTransform.sizeDelta;
                if (sizeDriver.x) targetSize.x = size.x;
                if (sizeDriver.y) targetSize.y = size.y;
                rectTransform.sizeDelta = targetSize;
            }

            if (rotationDriver)
            {
                Vector3 rotation = Vector3.Lerp(segment.startValue.rotation, segment.endValue.rotation, value);
                Vector3 targetRotation = transform.localEulerAngles;
                if (rotationDriver.x) targetRotation.x = rotation.x;
                if (rotationDriver.y) targetRotation.y = rotation.y;
                if (rotationDriver.z) targetRotation.z = rotation.z;
                transform.localEulerAngles = targetRotation;
            }

            if (scaleDriver)
            {
                Vector3 scale = Vector3.Lerp(segment.startValue.scale, segment.endValue.scale, value);
                Vector3 targetScale = transform.localScale;
                if (scaleDriver.x) targetScale.x = scale.x;
                if (scaleDriver.y) targetScale.y = scale.y;
                if (scaleDriver.z) targetScale.z = scale.z;
                transform.localScale = targetScale;
            }
        }
    }
}