/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-08-31 19:21:33
 * Organization:    #ORGANIZATION#
 * Description:     自定义Selectable的Transition（依赖Selectable而不是继承Selectable）
 */
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EZhex1991.EZUnity
{
    [RequireComponent(typeof(Selectable))]
    public class EZTransition : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
    {
        public enum TransitionType { None, Scale, Size, OutlineDistance, OutlineColor }

        public enum SelectionState { Normal, Highlighted, Pressed, Disabled }

        [SerializeField]
        private TransitionType m_TransitionType = TransitionType.Scale;
        public TransitionType transitionType
        {
            get { return m_TransitionType; }
            set
            {
                if (m_TransitionType != value)
                {
                    m_TransitionType = value;
                    DoTransition();
                }
            }
        }

        [SerializeField]
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
            set
            {
                m_RectTransform = value;
            }
        }
        [SerializeField]
        private Outline m_Outline;
        public Outline outline
        {
            get
            {
                if (m_Outline == null)
                {
                    m_Outline = GetComponent<Outline>();
                }
                return m_Outline;
            }
            set
            {
                m_Outline = value;
            }
        }

        [SerializeField]
        private ScaleState m_ScaleState = ScaleState.standard;
        public ScaleState scaleState { get { return m_ScaleState; } set { m_ScaleState = value; } }
        [SerializeField]
        private SizeState m_SizeState = SizeState.standard;
        public SizeState sizeState { get { return m_SizeState; } set { m_SizeState = value; } }
        [SerializeField]
        private OutlineDistanceState m_OutlineDistanceState = OutlineDistanceState.standard;
        public OutlineDistanceState outlineDistanceState { get { return m_OutlineDistanceState; } set { m_OutlineDistanceState = value; } }
        [SerializeField]
        private OutlineColorState m_OutlineColorState = OutlineColorState.standard;
        public OutlineColorState outlineColorState { get { return m_OutlineColorState; } set { m_OutlineColorState = value; } }

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

        public void OnPointerEnter(PointerEventData eventData)
        {
            isPointerInside = true;
            UpdateSelectionState(eventData);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            isPointerInside = false;
            UpdateSelectionState(eventData);
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                isPointerDown = true;
                UpdateSelectionState(eventData);
            }
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                isPointerDown = false;
                UpdateSelectionState(eventData);
            }
        }
        public void OnSelect(BaseEventData eventData)
        {
            hasSelection = true;
            UpdateSelectionState(eventData);
        }
        public void OnDeselect(BaseEventData eventData)
        {
            hasSelection = false;
            UpdateSelectionState(eventData);
        }

        protected bool IsDisabled()
        {
            return !(selectable.IsActive() && selectable.interactable);
        }
        protected bool IsPressed()
        {
            return selectable.IsActive() && isPointerInside && isPointerDown;
        }
        protected bool IsHighlighted(BaseEventData eventData)
        {
            bool result;
            if (!selectable.IsActive() || IsPressed())
            {
                result = false;
            }
            else
            {
                result = isPointerInside;
                // 以下为Selectable的源码，被点击的按钮状态hasSelection为true会同时出现两个Highlighted
                //result = hasSelection;
                //if (eventData is PointerEventData)
                //{
                //    PointerEventData pointerEventData = eventData as PointerEventData;
                //    result |= ((this.isPointerDown && !this.isPointerInside && pointerEventData.pointerPress == base.gameObject)
                //        || (!this.isPointerDown && this.isPointerInside && pointerEventData.pointerPress == base.gameObject)
                //        || (!this.isPointerDown && this.isPointerInside && pointerEventData.pointerPress == null));
                //}
                //else
                //{
                //    result |= this.isPointerInside;
                //}

            }
            return result;
        }

        protected void UpdateSelectionState(BaseEventData eventData)
        {
            if (IsDisabled())
            {
                currentSelectionState = SelectionState.Disabled;
            }
            else if (IsPressed())
            {
                currentSelectionState = SelectionState.Pressed;
            }
            else if (IsHighlighted(eventData))
            {
                currentSelectionState = SelectionState.Highlighted;
            }
            else
            {
                currentSelectionState = SelectionState.Normal;
            }
            DoTransition();
        }
        protected void DoTransition()
        {
            switch (transitionType)
            {
                case TransitionType.Scale:
                    DoScaleTransition();
                    break;
                case TransitionType.Size:
                    DoSizeTransition();
                    break;
                case TransitionType.OutlineDistance:
                    DoOutlineDistanceTransition();
                    break;
                case TransitionType.OutlineColor:
                    DoOutlineColorTransition();
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
        protected void DoOutlineDistanceTransition()
        {
            if (outline == null) return;
            switch (currentSelectionState)
            {
                case SelectionState.Normal:
                    outline.effectDistance = outlineDistanceState.normalDistance;
                    break;
                case SelectionState.Highlighted:
                    outline.effectDistance = outlineDistanceState.highlightedDistance;
                    break;
                case SelectionState.Pressed:
                    outline.effectDistance = outlineDistanceState.pressedDistance;
                    break;
                case SelectionState.Disabled:
                    outline.effectDistance = outlineDistanceState.disabledDistance;
                    break;
            }
        }
        protected void DoOutlineColorTransition()
        {
            if (outline == null) return;
            switch (currentSelectionState)
            {
                case SelectionState.Normal:
                    outline.effectColor = outlineColorState.normalColor;
                    break;
                case SelectionState.Highlighted:
                    outline.effectColor = outlineColorState.highlightedColor;
                    break;
                case SelectionState.Pressed:
                    outline.effectColor = outlineColorState.pressedColor;
                    break;
                case SelectionState.Disabled:
                    outline.effectColor = outlineColorState.disabledColor;
                    break;
            }
        }

        [Serializable]
        public struct ScaleState
        {
            [SerializeField]
            private Vector2 m_NormalScale;
            public Vector2 normalScale { get { return m_NormalScale; } set { m_NormalScale = value; } }
            [SerializeField]
            private Vector2 m_HighlightedScale;
            public Vector2 highlightedScale { get { return m_HighlightedScale; } set { m_HighlightedScale = value; } }
            [SerializeField]
            private Vector2 m_PressedScale;
            public Vector2 pressedScale { get { return m_PressedScale; } set { m_PressedScale = value; } }
            [SerializeField]
            private Vector2 m_DisabledScale;
            public Vector2 disabledScale { get { return m_DisabledScale; } set { m_DisabledScale = value; } }

            public ScaleState(Vector2 normalScale, Vector2 highlightedScale, Vector2 pressedScale, Vector2 disabledScale)
            {
                m_NormalScale = normalScale;
                m_HighlightedScale = highlightedScale;
                m_PressedScale = pressedScale;
                m_DisabledScale = disabledScale;
            }

            public static ScaleState standard = new ScaleState(Vector2.one, Vector2.one * 1.05f, Vector2.one * 0.95f, Vector2.one);
            public static ScaleState visibility = new ScaleState(Vector2.one, Vector2.one * 1.05f, Vector2.one * 0.95f, Vector2.zero);
        }

        [Serializable]
        public struct SizeState
        {
            [SerializeField]
            private Vector2 m_NormalSize;
            public Vector2 normalSize { get { return m_NormalSize; } set { m_NormalSize = value; } }
            [SerializeField]
            private Vector2 m_HighlightedSize;
            public Vector2 highlightedSize { get { return m_HighlightedSize; } set { m_HighlightedSize = value; } }
            [SerializeField]
            private Vector2 m_PressedSize;
            public Vector2 pressedSize { get { return m_PressedSize; } set { m_PressedSize = value; } }
            [SerializeField]
            private Vector2 m_DisabledSize;
            public Vector2 disabledSize { get { return m_DisabledSize; } set { m_DisabledSize = value; } }

            public SizeState(Vector2 normalSize, Vector2 highlightedSize, Vector2 pressedSize, Vector2 disabledSize)
            {
                m_NormalSize = normalSize;
                m_HighlightedSize = highlightedSize;
                m_PressedSize = pressedSize;
                m_DisabledSize = disabledSize;
            }

            public static SizeState standard = new SizeState(Vector2.one * 100, Vector2.one * 105, Vector2.one * 95, Vector2.one * 100);
        }

        [Serializable]
        public struct OutlineDistanceState
        {
            [SerializeField]
            private Vector2 m_NormalDistance;
            public Vector2 normalDistance { get { return m_NormalDistance; } set { m_NormalDistance = value; } }
            [SerializeField]
            private Vector2 m_HighlightedDistance;
            public Vector2 highlightedDistance { get { return m_HighlightedDistance; } set { m_HighlightedDistance = value; } }
            [SerializeField]
            private Vector2 m_PressedDistance;
            public Vector2 pressedDistance { get { return m_PressedDistance; } set { m_PressedDistance = value; } }
            [SerializeField]
            private Vector2 m_DisabledDistance;
            public Vector2 disabledDistance { get { return m_DisabledDistance; } set { m_DisabledDistance = value; } }

            public OutlineDistanceState(Vector2 normalDistance, Vector2 highlightedDistance, Vector2 pressedDistance, Vector2 disabledDistance)
            {
                m_NormalDistance = normalDistance;
                m_HighlightedDistance = highlightedDistance;
                m_PressedDistance = pressedDistance;
                m_DisabledDistance = disabledDistance;
            }

            public static OutlineDistanceState standard = new OutlineDistanceState(Vector2.one * 1, Vector2.one * 3, Vector2.zero, Vector2.zero);
        }

        [Serializable]
        public struct OutlineColorState
        {
            [SerializeField]
            private Color m_NormalColor;
            public Color normalColor { get { return m_NormalColor; } set { m_NormalColor = value; } }
            [SerializeField]
            private Color m_HighlightedColor;
            public Color highlightedColor { get { return m_HighlightedColor; } set { m_HighlightedColor = value; } }
            [SerializeField]
            private Color m_PressedColor;
            public Color pressedColor { get { return m_PressedColor; } set { m_PressedColor = value; } }
            [SerializeField]
            private Color m_DisabledColor;
            public Color disabledColor { get { return m_DisabledColor; } set { m_DisabledColor = value; } }

            public OutlineColorState(Color normalColor, Color highlightedColor, Color pressedColor, Color disabledColor)
            {
                m_NormalColor = normalColor;
                m_HighlightedColor = highlightedColor;
                m_PressedColor = pressedColor;
                m_DisabledColor = disabledColor;
            }

            public static OutlineColorState standard = new OutlineColorState(Color.green, Color.white, Color.yellow, Color.red);
        }
    }
}