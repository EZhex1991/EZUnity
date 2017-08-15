/*
 * Author:      熊哲
 * CreateTime:  1/12/2017 5:28:48 PM
 * Description:
 * 
*/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace EZUnityTools.UI
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

        protected virtual void GetChildren()
        {
            rectChildren.Clear();
            for (int i = 0; i < transform.childCount; i++)
            {
                RectTransform child = transform.GetChild(i).GetComponent<RectTransform>();
                if (child == null || !child.gameObject.activeInHierarchy) continue;
                rectChildren.Add(child);
            }
        }

        protected override void Awake()
        {
            GetChildren();
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
                child.localScale = Vector2.one * Mathf.Lerp(sizeRange.y, sizeRange.x, vec.magnitude / focusRange);
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