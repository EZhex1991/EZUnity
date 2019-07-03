/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-01-12 17:28:48
 * Organization:    #ORGANIZATION#
 * Description:     根据偏移量突出显示某个控件，用于ScrollRect的滚动缩放
 */
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EZhex1991.EZUnity
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    public class EZOutstand : UIBehaviour
    {
        [SerializeField]
        protected Vector2 m_FocusPoint = Vector2.zero;
        public Vector2 focusPoint { get { return m_FocusPoint; } set { SetProperty(ref m_FocusPoint, value); } }

        [SerializeField]
        protected float m_FocusRange = 100;
        public float focusRange { get { return m_FocusRange; } set { SetProperty(ref m_FocusRange, value); } }

        [SerializeField]
        protected float m_TiltAngle = 0;
        public float tiltAngle { get { return m_TiltAngle; } set { SetProperty(ref m_TiltAngle, value); } }

        [SerializeField]
        protected Vector2 m_SizeRange = new Vector2(0.5f, 1);
        public Vector2 sizeRange { get { return m_SizeRange; } set { SetProperty(ref m_SizeRange, value); } }

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

        protected virtual void GetChildren()
        {
            rectChildren.Clear();
            m_Tracker.Clear();
            for (int i = 0; i < transform.childCount; i++)
            {
                RectTransform child = transform.GetChild(i).GetComponent<RectTransform>();
                if (child == null || !child.gameObject.activeInHierarchy) continue;
                rectChildren.Add(child);
                m_Tracker.Add(this, child, DrivenTransformProperties.Scale);
            }
        }

        protected override void Awake()
        {
            GetChildren();
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            GetChildren();
        }
        protected override void OnDisable()
        {
            m_Tracker.Clear();
            base.OnDisable();
        }

        protected virtual void OnTransformChildrenChanged()
        {
            GetChildren();
        }

        protected virtual void Update()
        {
            for (int i = 0; i < rectChildren.Count; i++)
            {
                RectTransform child = rectChildren[i];
                Vector2 vec = child.anchoredPosition + rectTransform.anchoredPosition - focusPoint;
                float tiltAngleX = Mathf.Clamp(vec.y / focusRange, -1, 1) * -tiltAngle;
                float tiltAngleY = Mathf.Clamp(vec.x / focusRange, -1, 1) * tiltAngle;
                child.localRotation = Quaternion.Euler(tiltAngleX, tiltAngleY, 0);
                float lerp = Mathf.Lerp(sizeRange.y, sizeRange.x, vec.magnitude / focusRange);
                child.localScale = new Vector3(lerp, lerp, 1);
            }
        }

        protected void SetProperty<T>(ref T currentValue, T newValue)
        {
            if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
                return;
            currentValue = newValue;
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            GetChildren();
        }
#endif
    }
}