/*
 * Author:      熊哲
 * CreateTime:  8/31/2017 7:21:33 PM
 * Description:
 * 自定义Selectable的Transition，我只写了一个很常用的Scale，自己扩展也不难（依赖Selectable而不是继承Selectable）
*/
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EZComponent.UI
{
    [RequireComponent(typeof(Selectable)), DisallowMultipleComponent]
    public class EZTransition2 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
    {
        public enum TransitionType { Scale = 1, Size = 2 }

        public enum SelectionState { Normal, Highlighted, Pressed, Disabled }

        [SerializeField]
        private TransitionType m_TransitionType = TransitionType.Scale;
        public TransitionType transitionType { get { return m_TransitionType; } set { m_TransitionType = value; } }

        [SerializeField]
        private ScaleState m_ScaleState = ScaleState.standard;
        public ScaleState scaleState { get { return m_ScaleState; } set { m_ScaleState = value; } }
        [SerializeField]
        private SizeState m_SizeState = new SizeState();
        public SizeState sizeState { get { return m_SizeState; } set { m_SizeState = value; } }

        private RectTransform m_RectTransform;
        public RectTransform rectTransform
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
        private Selectable m_Selectable;
        public Selectable selectable
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

        private Vector2 originalSize;

        public void Start()
        {
            originalSize = rectTransform.sizeDelta;
        }

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
            switch (transitionType)
            {
                case TransitionType.Scale:
                    DoScaleTransition();
                    break;
                case TransitionType.Size:
                    DoSizeTransition();
                    break;
            }
        }

        protected void DoScaleTransition()
        {
            switch (currentSelectionState)
            {
                case SelectionState.Normal:
                    rectTransform.localScale = scaleState.normalScale;
                    break;
                case SelectionState.Highlighted:
                    rectTransform.localScale = scaleState.highlightedScale;
                    break;
                case SelectionState.Pressed:
                    rectTransform.localScale = scaleState.pressedScale;
                    break;
                case SelectionState.Disabled:
                    rectTransform.localScale = scaleState.disabledScale;
                    break;
            }
        }
        protected void DoSizeTransition()
        {
            switch (currentSelectionState)
            {
                case SelectionState.Normal:
                    rectTransform.sizeDelta = sizeState.normalSize;
                    break;
                case SelectionState.Highlighted:
                    rectTransform.sizeDelta = sizeState.highlightedSize;
                    break;
                case SelectionState.Pressed:
                    rectTransform.sizeDelta = sizeState.pressedSize;
                    break;
                case SelectionState.Disabled:
                    rectTransform.sizeDelta = sizeState.disabledSize;
                    break;
            }
        }

        public struct ScaleState
        {
            private Vector2 m_NormalScale;
            public Vector2 normalScale { get { return m_NormalScale; } set { m_NormalScale = value; } }
            private Vector2 m_HighlightedScale;
            public Vector2 highlightedScale { get { return m_HighlightedScale; } set { m_HighlightedScale = value; } }
            private Vector2 m_PressedScale;
            public Vector2 pressedScale { get { return m_PressedScale; } set { m_PressedScale = value; } }
            private Vector2 m_DisabledScale;
            public Vector2 disabledScale { get { return m_DisabledScale; } set { m_DisabledScale = value; } }

            public ScaleState(Vector2 normalScale, Vector2 highlightedScale, Vector2 pressedScale, Vector2 disabledScale)
            {
                m_NormalScale = normalScale;
                m_HighlightedScale = highlightedScale;
                m_PressedScale = pressedScale;
                m_DisabledScale = disabledScale;
            }

            public static ScaleState identity = new ScaleState(Vector2.one, Vector2.one, Vector2.one, Vector2.one);
            public static ScaleState standard = new ScaleState(Vector2.one, Vector2.one * 1.05f, Vector2.one * 0.95f, Vector2.one);
            public static ScaleState visibility = new ScaleState(Vector2.one, Vector2.one * 1.05f, Vector2.one * 0.95f, Vector2.zero);
        }

        public struct SizeState
        {
            private Vector2 m_NormalSize;
            public Vector2 normalSize { get { return m_NormalSize; } set { m_NormalSize = value; } }
            private Vector2 m_HighlightedSize;
            public Vector2 highlightedSize { get { return m_HighlightedSize; } set { m_HighlightedSize = value; } }
            private Vector2 m_PressedSize;
            public Vector2 pressedSize { get { return m_PressedSize; } set { m_PressedSize = value; } }
            private Vector2 m_DisabledSize;
            public Vector2 disabledSize { get { return m_DisabledSize; } set { m_DisabledSize = value; } }

            public SizeState(Vector2 normalSize, Vector2 highlightedSize, Vector2 pressedSize, Vector2 disabledSize)
            {
                m_NormalSize = normalSize;
                m_HighlightedSize = highlightedSize;
                m_PressedSize = pressedSize;
                m_DisabledSize = disabledSize;
            }
        }
    }
}