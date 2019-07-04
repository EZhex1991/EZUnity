/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-07-02 17:09:55
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public class EZPath : MonoBehaviour
    {
        public enum PathMode
        {
            Linear,
            Bezier,
        }

        [SerializeField]
        private PathMode m_PathMode = PathMode.Linear;
        public PathMode pathMode { get { return m_PathMode; } set { m_PathMode = value; } }

        [SerializeField]
        private bool m_ClosedPath = true;
        public bool closedPath { get { return m_ClosedPath; } set { m_ClosedPath = value; } }

        [SerializeField]
        private List<EZPathPoint> m_PathPoints = new List<EZPathPoint>();
        public List<EZPathPoint> pathPoints { get { return m_PathPoints; } }

        public bool GetPoint(ref Vector3 position, ref Quaternion rotation, ref Vector3 scale, int section, float progress, bool loop)
        {
            if (pathPoints.Count < 2)
            {
                Debug.LogError("Invalid Path, a path should be consist of at least two points");
                return false;
            }
            if (section < 0)
            {
                Debug.LogException(new ArgumentOutOfRangeException("section"));
                return false;
            }
            int totalSection = closedPath ? pathPoints.Count : (pathPoints.Count - 1);
            if (section >= totalSection && !loop)
            {
                EZPathPoint pathPoint = pathPoints[pathPoints.Count - 1];
                position = pathPoint.position;
                rotation = pathPoint.transform.rotation;
                scale = pathPoint.transform.localScale;
                return true;
            }
            else
            {
                EZPathPoint pathPoint1 = pathPoints[section % (totalSection)];
                EZPathPoint pathPoint2 = pathPoints[(section + 1) % (pathPoints.Count)];
                if (pathMode == PathMode.Bezier)
                {
                    position = CalcBezierPoint(pathPoint1, pathPoint2, progress);
                }
                else
                {
                    position = Vector3.Lerp(pathPoint1.position, pathPoint2.position, progress);
                }
                rotation = Quaternion.Lerp(pathPoint1.transform.rotation, pathPoint2.transform.rotation, progress);
                scale = Vector3.Lerp(pathPoint1.transform.localScale, pathPoint2.transform.localScale, progress);
                return true;
            }
        }
        public static Vector3 CalcBezierPoint(EZPathPoint p1, EZPathPoint p2, float progress)
        {
            return CalcBezierPoint(p1.position, p1.startTangentPosition, p2.endTangentPosition, p2.position, progress);
        }
        public static Vector3 CalcBezierPoint(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float progress)
        {
            float t1 = progress;
            float t2 = 1 - progress;
            return p1 * t2 * t2 * t2
                + p2 * t2 * t2 * t1 * 3
                + p3 * t2 * t1 * t1 * 3
                + p4 * t1 * t1 * t1;
        }

        private void OnTransformChildrenChanged()
        {
            RebuildPath();
        }
        private void Reset()
        {
            RebuildPath();
        }
        private void RebuildPath()
        {
            m_PathPoints.Clear();
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                child.name = string.Format("PathPoint-{0:D2}", i);
                EZPathPoint point = child.GetComponent<EZPathPoint>();
                if (point == null) point = child.gameObject.AddComponent<EZPathPoint>();
                m_PathPoints.Add(point);
            }
        }

#if UNITY_EDITOR
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
        private void DrawGizmos()
        {
            switch (pathMode)
            {
                case PathMode.Linear:
                    DrawGizmos(DrawLinearGizmos);
                    break;
                case PathMode.Bezier:
                    DrawGizmos(DrawBezierGizmos);
                    break;
            }
        }
        private void DrawGizmos(Action<EZPathPoint, EZPathPoint> drawer)
        {
            for (int i = 0; i < m_PathPoints.Count - 1; i++)
            {
                drawer(m_PathPoints[i], m_PathPoints[i + 1]);
            }
            if (closedPath && m_PathPoints.Count >= 2)
            {
                drawer(m_PathPoints[m_PathPoints.Count - 1], m_PathPoints[0]);
            }
        }
        private void DrawLinearGizmos(EZPathPoint p1, EZPathPoint p2)
        {
            UnityEditor.Handles.DrawLine(p1.position, p2.position);
        }
        private void DrawBezierGizmos(EZPathPoint p1, EZPathPoint p2)
        {
            UnityEditor.Handles.DrawBezier(p1.position, p2.position, p1.startTangentPosition, p2.endTangentPosition, Gizmos.color, null, 1f);
        }
#endif
    }
}
