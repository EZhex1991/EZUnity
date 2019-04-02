/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-11-02 17:02:01
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEngine;

namespace EZUnity.Animation
{
    public class EZRectTransformAnimation : EZAnimation<EZRectTransformAnimationSegment>
    {
        [SerializeField]
        private RectTransform m_RectTransform;
        public RectTransform rectTransform
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
            rectTransform.anchoredPosition = Vector2.Lerp(activeSegment.startRect.anchoredPosition, activeSegment.endRect.anchoredPosition, segmentProcess);
            rectTransform.anchorMin = Vector2.Lerp(activeSegment.startRect.anchorMin, activeSegment.endRect.anchorMin, segmentProcess);
            rectTransform.anchorMax = Vector2.Lerp(activeSegment.startRect.anchorMax, activeSegment.endRect.anchorMax, segmentProcess);
            rectTransform.sizeDelta = Vector2.Lerp(activeSegment.startRect.sizeDelta, activeSegment.endRect.sizeDelta, segmentProcess);
            rectTransform.rotation = Quaternion.Lerp(activeSegment.startRect.rotation, activeSegment.endRect.rotation, segmentProcess);
            rectTransform.localScale = Vector3.Lerp(activeSegment.startRect.localScale, activeSegment.endRect.localScale, segmentProcess);
        }

        private void Reset()
        {
            m_RectTransform = GetComponent<RectTransform>();
            m_Segments = new List<EZRectTransformAnimationSegment>()
            {
                new EZRectTransformAnimationSegment(),
            };
        }
    }
}