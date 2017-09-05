/*
 * Author:      熊哲
 * CreateTime:  8/31/2017 7:21:33 PM
 * Description:
 * 
*/
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EZUnityTools.UI
{
    [RequireComponent(typeof(Selectable))]
    public class EZTransition : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        public enum TransitionType { Scale = 1, }

        [SerializeField]
        private TransitionType m_TransitionType = TransitionType.Scale;
        public TransitionType transitionType { get { return m_TransitionType; } set { m_TransitionType = value; } }

        [SerializeField]
        private bool m_InteractableOnly = true;
        public bool interactableOnly { get { return m_InteractableOnly; } set { m_InteractableOnly = value; } }

        [SerializeField]
        private Vector3 m_NormalScale = Vector3.one;
        public Vector3 normalScale { get { return m_NormalScale; } set { m_NormalScale = value; } }

        [SerializeField]
        private Vector3 m_HighlightedScale = Vector3.one * 1.1f;
        public Vector3 highlightedScale { get { return m_HighlightedScale; } set { m_HighlightedScale = value; } }

        [SerializeField]
        private Vector3 m_PressedScale = Vector3.one * 0.9f;
        public Vector3 pressedScale { get { return m_PressedScale; } set { m_PressedScale = value; } }

        private Selectable m_Selectable;

        void Awake()
        {
            m_Selectable = GetComponent<Selectable>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!m_Selectable.interactable && interactableOnly) return;
            transform.localScale = highlightedScale;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!m_Selectable.interactable && interactableOnly) return;
            transform.localScale = pressedScale;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!m_Selectable.interactable && interactableOnly) return;
            transform.localScale = highlightedScale;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!m_Selectable.interactable && interactableOnly) return;
            transform.localScale = normalScale;
        }
    }
}