/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-11-02 17:15:40
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZUnity.Animation
{
    public class EZGraphicColorAnimation : EZAnimation<EZColorAnimationSegment>
    {
        [SerializeField]
        private Graphic m_TargetGraphic;
        public Graphic targetGraphic
        {
            get
            {
                if (m_TargetGraphic == null)
                {
                    m_TargetGraphic = GetComponent<Graphic>();
                }
                return m_TargetGraphic;
            }
        }

        protected override void OnSegmentUpdate()
        {
            targetGraphic.color = segment.gradient.Evaluate(process);
        }

        private void Reset()
        {
            m_TargetGraphic = GetComponent<Graphic>();
            m_Segments = new List<EZColorAnimationSegment>()
            {
                new EZColorAnimationSegment(),
            };
        }
    }
}