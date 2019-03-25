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
            rectTransform.anchoredPosition = Vector2.Lerp(segment.startRect.anchoredPosition, segment.endRect.anchoredPosition, process);
            rectTransform.anchorMin = Vector2.Lerp(segment.startRect.anchorMin, segment.endRect.anchorMin, process);
            rectTransform.anchorMax = Vector2.Lerp(segment.startRect.anchorMax, segment.endRect.anchorMax, process);
            rectTransform.sizeDelta = Vector2.Lerp(segment.startRect.sizeDelta, segment.endRect.sizeDelta, process);
            rectTransform.rotation = Quaternion.Lerp(segment.startRect.rotation, segment.endRect.rotation, process);
            rectTransform.localScale = Vector3.Lerp(segment.startRect.localScale, segment.endRect.localScale, process);
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