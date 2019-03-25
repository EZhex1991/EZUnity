/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-12-11 17:47:10
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEngine;

namespace EZUnity.Animation
{
    [Serializable]
    public class EZTransformAnimationSegment : EZAnimationSegment
    {
        [SerializeField]
        private Transform m_StartPoint;
        public Transform startPoint { get { return m_StartPoint; } set { m_StartPoint = value; } }
        [SerializeField]
        private Transform m_EndPoint;
        public Transform endPoint { get { return m_EndPoint; } set { m_EndPoint = value; } }

        [SerializeField]
        private Vector3 m_StartTangent = Vector3.zero;
        public Vector3 startTangent { get { return m_StartTangent; } set { m_StartTangent = value; } }
        [SerializeField]
        private Vector3 m_EndTangent = Vector3.zero;
        public Vector3 endTangent { get { return m_EndTangent; } set { m_EndTangent = value; } }
    }
}