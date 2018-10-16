/*
 * Author:      熊哲
 * CreateTime:  11/2/2017 5:15:40 PM
 * Description:
 * 
*/
using System;
using UnityEngine;
using UnityEngine.UI;

namespace EZUnity.Animation
{
    [RequireComponent(typeof(Graphic)), DisallowMultipleComponent]
    public class EZGraphicColorAnimation : EZAnimation<Color, EZColorAnimationSegment>
    {
        [NonSerialized]
        private Graphic m_Graphic;
        private Graphic graphic
        {
            get
            {
                if (m_Graphic == null)
                {
                    m_Graphic = GetComponent<Graphic>();
                }
                return m_Graphic;
            }
        }

        protected override void OnSegmentUpdate()
        {
            graphic.color = Color.Lerp(segment.startValue, segment.endValue, value);
        }
    }
}