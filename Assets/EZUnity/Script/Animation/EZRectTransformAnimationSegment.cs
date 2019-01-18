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
    public struct RectTransformInfo
    {
        [SerializeField]
        private Vector2 m_AnchoredPosition;
        public Vector2 anchoredPosition { get { return m_AnchoredPosition; } set { m_AnchoredPosition = value; } }
        [SerializeField]
        private Vector2 m_SizeDelta;
        public Vector2 sizeDelta { get { return m_SizeDelta; } set { m_SizeDelta = value; } }
        [SerializeField]
        private Vector3 m_Rotation;
        public Vector3 rotation { get { return m_Rotation; } set { m_Rotation = value; } }
        [SerializeField]
        private Vector3 m_Scale;
        public Vector3 scale { get { return m_Scale; } set { m_Scale = value; } }
    }

    [Serializable]
    public class EZRectTransformAnimationSegment : EZAnimationSegment<RectTransformInfo>
    {

    }
}