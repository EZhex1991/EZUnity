/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-01-12 17:28:48
 * Organization:    #ORGANIZATION#
 * Description:     2D的中心点距布局（UGUI原生GridLayout是边距布局）
 */
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EZhex1991.EZUnity
{
    [RequireComponent(typeof(RectTransform)), ExecuteInEditMode, DisallowMultipleComponent]
    public class EZGridLayout2D : UIBehaviour, ILayoutElement, ILayoutGroup
    {
        public enum Corner { UpperLeft = 0, UpperRight = 1, LowerLeft = 2, LowerRight = 3 }

        [SerializeField]
        private Corner m_StartCorner = Corner.UpperLeft;
        public Corner startCorner { get { return m_StartCorner; } set { SetProperty(ref m_StartCorner, value); } }

        [SerializeField]
        private RectTransform.Axis m_StartAxis = RectTransform.Axis.Horizontal;
        public RectTransform.Axis startAxis { get { return m_StartAxis; } set { SetProperty(ref m_StartAxis, value); } }

        [SerializeField]
        private int m_AxisConstraint = 5;
        public int axisConstraint { get { return m_AxisConstraint; } set { SetProperty(ref m_AxisConstraint, Mathf.Max(1, value)); } }

        [SerializeField]
        private RectOffset m_Padding = new RectOffset();
        public RectOffset padding { get { return m_Padding; } set { SetProperty(ref m_Padding, value); } }

        [SerializeField]
        private Vector2 m_Spacing = new Vector2(100, 100);
        public Vector2 spacing { get { return m_Spacing; } set { SetProperty(ref m_Spacing, new Vector2(Mathf.Abs(value.x), Mathf.Abs(value.y))); } }

        [SerializeField]
        private Vector2 m_Pivot = new Vector2(0.5f, 0.5f);
        public Vector2 pivot { get { return m_Pivot; } set { m_Pivot = value; } }

        [NonSerialized]
        private RectTransform m_RectTransform;
        protected RectTransform rectTransform
        {
            get
            {
                if (m_RectTransform == null) m_RectTransform = GetComponent<RectTransform>();
                return m_RectTransform;
            }
        }
        [NonSerialized]
        private List<RectTransform> m_RectChildren = new List<RectTransform>();
        protected List<RectTransform> rectChildren { get { return m_RectChildren; } }

        protected DrivenRectTransformTracker m_Tracker;
        private List<ILayoutIgnorer> ignoreList = new List<ILayoutIgnorer>();
        private Vector2 m_MinSize = Vector2.zero;
        private Vector2 m_PreferredSized = Vector2.zero;
        private Vector2 offset = Vector2.zero;

        public virtual void CalculateLayoutInputHorizontal()
        {
            m_RectChildren.Clear();
            for (int i = 0; i < rectTransform.childCount; i++)
            {
                RectTransform child = rectTransform.GetChild(i).GetComponent<RectTransform>();
                if (child == null || !child.gameObject.activeInHierarchy) continue;
                child.GetComponents(ignoreList);
                if (ignoreList.Count == 0)
                {
                    m_RectChildren.Add(child);
                    continue;
                }
                for (int j = 0; j < ignoreList.Count; j++)
                {
                    if (!ignoreList[j].ignoreLayout)
                    {
                        m_RectChildren.Add(child);
                        break;
                    }
                }
                ignoreList.Clear();
                m_Tracker.Clear();
            }
            int count = rectChildren.Count;
            int length1, length2;
            if (count <= axisConstraint)
            {
                length1 = count - 1;
                length2 = 0;
            }
            else
            {
                length1 = axisConstraint - 1;
                length2 = (count - 1) / axisConstraint;
            }
            if (startAxis == RectTransform.Axis.Horizontal)
            {
                m_MinSize = new Vector2(padding.horizontal + length1 * spacing.x, padding.vertical + length2 * spacing.y);
                m_PreferredSized = new Vector2(padding.horizontal + (axisConstraint - 1) * spacing.x, padding.vertical + length2 * spacing.y);
            }
            else
            {
                m_MinSize = new Vector2(padding.horizontal + length2 * spacing.x, padding.vertical + length1 * spacing.y);
                m_PreferredSized = new Vector2(padding.horizontal + length2 * spacing.x, padding.vertical + (axisConstraint - 1) * spacing.y);
            }
            offset.x = m_MinSize.x * (((int)startCorner % 2 == 0 ? 0 : 1) - pivot.x);
            offset.y = m_MinSize.y * (((int)startCorner / 2 == 0 ? 1 : 0) - pivot.y);
        }
        public virtual void CalculateLayoutInputVertical()
        {

        }
        public virtual float minWidth { get { return m_MinSize[0]; } }
        public virtual float minHeight { get { return m_MinSize[1]; } }
        public virtual float preferredWidth { get { return m_PreferredSized[0]; } }
        public virtual float preferredHeight { get { return m_PreferredSized[1]; } }
        public virtual float flexibleWidth { get { return 1; } }
        public virtual float flexibleHeight { get { return 1; } }
        public virtual int layoutPriority { get { return 0; } }

        public virtual void SetLayoutHorizontal()
        {
            SetChildren();
        }
        public virtual void SetLayoutVertical()
        {

        }
        protected void SetChildren()
        {
            for (int i = 0; i < rectChildren.Count; i++)
            {
                RectTransform child = rectChildren[i];
                int row, column;
                if (startAxis == RectTransform.Axis.Horizontal)
                {
                    row = i / axisConstraint;
                    column = i % axisConstraint;
                }
                else
                {
                    row = i % axisConstraint;
                    column = i / axisConstraint;
                }
                Vector2 pos = new Vector2();
                pos.x = (int)startCorner % 2 == 0 ? padding.left + spacing.x * column : -padding.right - spacing.x * column;
                pos.y = (int)startCorner / 2 == 0 ? -padding.top - spacing.y * row : padding.bottom + spacing.y * row;
                SetChild(child, pos);
            }
        }
        protected void SetChild(RectTransform rect, Vector2 pos)
        {
            if (rect == null) return;
            m_Tracker.Add(this, rect, DrivenTransformProperties.AnchoredPosition);
            rect.anchoredPosition = pos + offset;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SetDirty();
        }
        protected override void OnDisable()
        {
            m_Tracker.Clear();
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
            base.OnDisable();
        }
        protected override void OnDidApplyAnimationProperties()
        {
            base.OnDidApplyAnimationProperties();
            SetDirty();
        }
        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            if (transform.parent != null && transform.parent.GetComponent<ILayoutGroup>() != null)
                return;
            else
                SetDirty();
        }
        protected virtual void OnTransformChildrenChanged()
        {
            SetDirty();
        }

        protected void SetProperty<T>(ref T currentValue, T newValue)
        {
            if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
                return;
            currentValue = newValue;
            SetDirty();
        }

        protected void SetDirty()
        {
            if (!IsActive()) return;
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            SetDirty();
        }
#endif
    }
}