/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-12-11 17:50:56
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEngine;

namespace EZUnity.Animation
{
    [Serializable]
    public class EZRectTransformAnimationSegment : EZAnimationSegment
    {
        [SerializeField]
        private RectTransform m_StartRect;
        public RectTransform startRect { get { return m_StartRect; } set { m_StartRect = value; } }
        [SerializeField]
        private RectTransform m_EndRect;
        public RectTransform endRect { get { return m_EndRect; } set { m_EndRect = value; } }
    }
}