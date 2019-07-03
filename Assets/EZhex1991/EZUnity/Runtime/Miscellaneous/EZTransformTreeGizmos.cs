/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-05-22 15:29:49
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [ExecuteInEditMode]
    public class EZTransformTreeGizmos : MonoBehaviour
    {
        private List<List<Transform>> m_Transforms;
        private List<List<Transform>> transforms
        {
            get
            {
                if (m_Transforms == null)
                {
                    m_Transforms = new List<List<Transform>>();
                    GetTransforms(transform);
                }
                return m_Transforms;
            }
        }
        private void GetTransforms(Transform parent)
        {
            List<Transform> group = new List<Transform>();
            m_Transforms.Add(group);
            group.Add(parent);
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                if (child == null) continue;
                group.Add(child);
                if (child.childCount > 0)
                    GetTransforms(child);
            }
        }

        public Color normalColor = Color.gray * Color.green;
        public Color selectedColor = Color.green;

        private void Reset()
        {
            transforms.Clear();
            GetTransforms(transform);
        }
        private void OnTransformChildrenChanged()
        {
            transforms.Clear();
            GetTransforms(transform);
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = normalColor;
            DrawLines();
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = selectedColor;
            DrawLines();
        }
        private void DrawLines()
        {
            for (int i = 0; i < transforms.Count; i++)
            {
                Transform start = transforms[i][0];
                if (start == null) continue;
                for (int j = 1; j < transforms[i].Count; j++)
                {
                    Transform end = transforms[i][j];
                    if (end == null) continue;
                    Gizmos.DrawLine(start.position, end.position);
                }
            }
        }
    }
}
