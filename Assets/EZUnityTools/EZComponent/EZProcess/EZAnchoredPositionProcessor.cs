/*
 * Author:      熊哲
 * CreateTime:  9/12/2017 2:22:50 PM
 * Description:
 * 
*/
using System;
using UnityEngine;

namespace EZComponent.EZProcess
{
    [RequireComponent(typeof(RectTransform)), DisallowMultipleComponent]
    public class EZAnchoredPositionProcessor : EZVector2Process
    {
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

        protected override void StartPhase(int index)
        {
            startValue = rectTransform.anchoredPosition;
            base.StartPhase(index);
        }
        protected override void UpdatePhase(float lerp)
        {
            base.UpdatePhase(lerp);
            rectTransform.anchoredPosition = value;
        }
    }
}