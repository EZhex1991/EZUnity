/*
 * Author:      熊哲
 * CreateTime:  8/31/2017 7:21:33 PM
 * Description:
 * 
*/
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EZComponent.UI
{
    [RequireComponent(typeof(Selectable))]
    public class EZTransition : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
    {
        public enum TransitionType { Scale = 1, }

        public enum SelectionState { Normal, Highlighted, Pressed, Disabled }

        [SerializeField]
        private TransitionType m_TransitionType = TransitionType.Scale;
        public TransitionType transitionType { get { return m_TransitionType; } set { m_TransitionType = value; } }

        [SerializeField]
        private Vector3 m_NormalScale = Vector3.one;
        public Vector3 normalScale { get { return m_NormalScale; } set { m_NormalScale = value; } }

        [SerializeField]
        private Vector3 m_HighlightedScale = Vector3.one * 1.05f;
        public Vector3 highlightedScale { get { return m_HighlightedScale; } set { m_HighlightedScale = value; } }

        [SerializeField]
        private Vector3 m_PressedScale = Vector3.one * 0.9f;
        public Vector3 pressedScale { get { return m_PressedScale; } set { m_PressedScale = value; } }

        [SerializeField]
        private Vector3 m_DisabledScale = Vector3.one;
        public Vector3 disabledScale { get { return m_DisabledScale; } set { m_DisabledScale = value; } }

        private Selectable m_Selectable;
        protected Selectable selectable
        {
            get
            {
                if (m_Selectable == null)
                {
                    m_Selectable = GetComponent<Selectable>();
                }
                return m_Selectable;
            }
        }
        protected SelectionState currentSelectionState { get; private set; }
        private bool isPointerInside { get; set; }
        private bool isPointerDown { get; set; }
        private bool hasSelection { get; set; }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isPointerInside = true;
            UpdateSelectionState();
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            isPointerInside = false;
            UpdateSelectionState();
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                isPointerDown = true;
                UpdateSelectionState();
            }
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                isPointerDown = false;
                UpdateSelectionState();
            }
        }
        public void OnSelect(BaseEventData eventData)
        {
            hasSelection = true;
            UpdateSelectionState();
        }
        public void OnDeselect(BaseEventData eventData)
        {
            hasSelection = false;
            UpdateSelectionState();
        }

        protected bool IsDisabled()
        {
            return !(selectable.IsActive() && selectable.interactable);
        }
        protected bool IsPressed()
        {
            return selectable.IsActive() && isPointerInside && isPointerDown;
        }
        protected bool IsHighlighted()
        {
            bool result;
            if (!selectable.IsActive() || IsPressed())
            {
                result = false;
            }
            else
            {
                result = isPointerInside;
            }
            return result;
        }

        protected void UpdateSelectionState()
        {
            if (IsDisabled())
            {
                currentSelectionState = SelectionState.Disabled;
            }
            else if (IsPressed())
            {
                currentSelectionState = SelectionState.Pressed;
            }
            else if (IsHighlighted())
            {
                currentSelectionState = SelectionState.Highlighted;
            }
            else
            {
                currentSelectionState = SelectionState.Normal;
            }
            DoStateTransition();
        }
        protected void DoStateTransition()
        {
            switch (currentSelectionState)
            {
                case SelectionState.Normal:
                    transform.localScale = normalScale;
                    break;
                case SelectionState.Highlighted:
                    transform.localScale = highlightedScale;
                    break;
                case SelectionState.Pressed:
                    transform.localScale = pressedScale;
                    break;
                case SelectionState.Disabled:
                    transform.localScale = disabledScale;
                    break;
            }
        }
    }
}