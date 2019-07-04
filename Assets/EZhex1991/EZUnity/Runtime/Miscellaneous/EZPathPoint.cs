/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-07-02 17:15:10
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public class EZPathPoint : MonoBehaviour
    {
        [SerializeField]
        private bool m_BrokenTangent;
        public bool brokenTangent { get { return m_BrokenTangent; } }

        [SerializeField]
        private Vector3 m_StartTangent = Vector3.back;
        public Vector3 startTangent { get { return m_StartTangent; } set { m_StartTangent = value; } }

        [SerializeField]
        private Vector3 m_EndTangent = Vector3.forward;
        public Vector3 endTangent { get { return m_EndTangent; } set { m_EndTangent = value; } }

        public Vector3 position { get { return transform.position; } }
        public Vector3 startTangentPosition { get { return transform.TransformPoint(startTangent); } }
        public Vector3 endTangentPosition { get { return transform.TransformPoint(endTangent); } }

        private void OnValidate()
        {
            if (!m_BrokenTangent)
            {
                m_EndTangent = -m_StartTangent;
            }
        }
    }
}
