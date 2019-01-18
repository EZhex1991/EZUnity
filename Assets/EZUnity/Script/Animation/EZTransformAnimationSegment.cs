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
    public struct TransformInfo
    {
        [SerializeField]
        private Vector3 m_Position;
        public Vector3 position { get { return m_Position; } set { m_Position = value; } }

        [SerializeField]
        private Vector3 m_Rotation;
        public Vector3 rotation { get { return m_Rotation; } set { m_Rotation = value; } }

        [SerializeField]
        private Vector3 m_Scale;
        public Vector3 scale { get { return m_Scale; } set { m_Scale = value; } }

        public TransformInfo(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            m_Position = position;
            m_Rotation = rotation;
            m_Scale = scale;
        }
    }

    [Serializable]
    public class EZTransformAnimationSegment : EZAnimationSegment<TransformInfo>
    {

    }
}