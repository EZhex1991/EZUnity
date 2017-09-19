/*
 * Author:      熊哲
 * CreateTime:  9/8/2017 10:28:28 AM
 * Description:
 * 用来批量调整大小的组件
 * 大部分时候一个Label由背景容器Box和文字内容Text组成，由于层级和渲染的关系我们不能将Text作为Box父级来通过Text自适应去改变Box大小
 * 该组件可由一个RectTransform(Driver)去控制关联的RectTransform(Slave)，也可以实现父容器随子容器大小变动的“控制反转”
*/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EZComponent.UI
{
    [ExecuteInEditMode, RequireComponent(typeof(RectTransform))]
    public class EZSizeDriver : UIBehaviour
    {
        [Serializable]
        public class Slave
        {
            public RectTransform rectTransform;
            public Vector2 sizeOffset = Vector2.zero;
        }

        [SerializeField]
        private bool m_Horizontal = true;
        public bool horizontal { get { return m_Horizontal; } set { m_Horizontal = value; } }

        [SerializeField]
        private bool m_Vertical = true;
        public bool vertical { get { return m_Vertical; } set { m_Vertical = value; } }

        [SerializeField]
        private List<Slave> m_SlaveList;
        public List<Slave> slaveList { get { return m_SlaveList; } set { m_SlaveList = value; } }

        [NonSerialized]
        private RectTransform m_RectTransform;
        private RectTransform rectTransform
        {
            get
            {
                if (m_RectTransform == null)
                {
                    m_RectTransform = GetComponent<RectTransform>();
                }
                return m_RectTransform;
            }
        }

        private DrivenRectTransformTracker tracker;

        protected virtual void HandleSlaves()
        {
            tracker.Clear();
            for (int i = 0; i < slaveList.Count; i++)
            {
                Slave slave = slaveList[i];
                if (slave.rectTransform == null) continue;
                if (horizontal)
                {
                    tracker.Add(this, slave.rectTransform, DrivenTransformProperties.SizeDeltaX);
                    slave.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransform.sizeDelta.x + slave.sizeOffset.x);
                }
                if (vertical)
                {
                    tracker.Add(this, slave.rectTransform, DrivenTransformProperties.SizeDeltaY);
                    slave.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectTransform.sizeDelta.y + slave.sizeOffset.y);
                }
            }
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            HandleSlaves();
        }

        protected override void OnDisable()
        {
            tracker.Clear();
            base.OnDisable();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            HandleSlaves();
        }
#endif
    }
}