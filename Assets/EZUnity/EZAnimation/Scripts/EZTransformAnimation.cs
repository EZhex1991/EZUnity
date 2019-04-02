/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-10-31 17:21:48
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZUnity.Animation
{
    public class EZTransformAnimation : EZAnimation<EZTransformAnimationSegment>
    {
        public enum PathMode
        {
            Linear,
            Bezier,
        }

        [SerializeField]
        private Transform m_TargetTransform;
        public Transform targetTransform
        {
            get
            {
                if (m_TargetTransform == null)
                    m_TargetTransform = transform;
                return m_TargetTransform;
            }
        }

        [SerializeField]
        private PathMode m_PathMode = PathMode.Linear;
        public PathMode pathMode { get { return m_PathMode; } set { m_PathMode = value; } }

        protected override void OnSegmentUpdate()
        {
            switch (pathMode)
            {
                case PathMode.Linear:
                    OnLinearUpdate();
                    break;
                case PathMode.Bezier:
                    OnBezierUpdate();
                    break;
            }
            targetTransform.rotation = Quaternion.Lerp(activeSegment.startPoint.rotation, activeSegment.endPoint.rotation, segmentProcess);
            targetTransform.localScale = Vector3.Lerp(activeSegment.startPoint.localScale, activeSegment.endPoint.localScale, segmentProcess);
        }
        private void OnLinearUpdate()
        {
            targetTransform.position = Vector3.Lerp(activeSegment.startPoint.position, activeSegment.endPoint.position, segmentProcess);
        }
        private void OnBezierUpdate()
        {
            float t1 = segmentProcess;
            float t2 = 1 - segmentProcess;
            Vector3 p1 = activeSegment.startPoint.position;
            Vector3 p2 = p1 + activeSegment.startTangent;
            Vector3 p3 = activeSegment.endPoint.position + activeSegment.endTangent;
            Vector3 p4 = activeSegment.endPoint.position;
            targetTransform.position = p1 * t2 * t2 * t2
                + 3 * p2 * t2 * t2 * t1
                + 3 * p3 * t2 * t1 * t1
                + p4 * t1 * t1 * t1;
        }

#if UNITY_EDITOR
        private void DrawGizmos()
        {
            switch (pathMode)
            {
                case PathMode.Linear:
                    DrawGizmos(LinearDrawer);
                    break;
                case PathMode.Bezier:
                    DrawGizmos(BezierDrawer);
                    break;
            }
        }
        private void DrawGizmos(Action<EZTransformAnimationSegment> drawer)
        {
            for (int i = 0; i < segments.Count; i++)
            {
                EZTransformAnimationSegment seg = segments[i];
                if (seg.startPoint != null && seg.endPoint != null)
                {
                    drawer(seg);
                }
            }
        }
        private void LinearDrawer(EZTransformAnimationSegment segment)
        {
            UnityEditor.Handles.DrawLine(segment.startPoint.position, segment.endPoint.position);
        }
        private void BezierDrawer(EZTransformAnimationSegment segment)
        {
            Vector3 startPoint = segment.startPoint.position;
            Vector3 endPoint = segment.endPoint.position;
            Vector3 startTangent = startPoint + segment.startTangent;
            Vector3 endTangent = endPoint + segment.endTangent;
            UnityEditor.Handles.DrawBezier(startPoint, endPoint, startTangent, endTangent, Gizmos.color, null, 1f);
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.grey;
            DrawGizmos();
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            DrawGizmos();
        }
#endif

        private void Reset()
        {
            m_TargetTransform = transform;
            m_Segments = new List<EZTransformAnimationSegment>()
            {
                new EZTransformAnimationSegment(),
            };
        }
    }
}