/*
 * Author:      熊哲
 * CreateTime:  12/14/2016 6:18:53 PM
 * Description:
 * 
*/
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EZUnityTools
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(ScrollRect))]
    public class EZScrollRect : MonoBehaviour, IEventSystemHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private float m_TiltAngle = 0;
        public float tiltAngle { get { return m_TiltAngle; } set { m_TiltAngle = value; } }

        [SerializeField]
        private float m_ScaleCenter = 1;
        public float scaleCenter { get { return m_ScaleCenter; } set { m_ScaleCenter = value; } }
        [SerializeField]
        private float m_ScaleOther = 0.4f;
        public float scaleOther { get { return m_ScaleOther; } set { m_ScaleOther = value; } }

        [SerializeField]
        private float m_SpeedThreshold = 500;
        public float speedThreshold { get { return m_SpeedThreshold; } set { m_SpeedThreshold = value; } }

        [SerializeField]
        private float m_RelocateSpeed = 500;
        public float relocateSpeed { get { return m_RelocateSpeed; } set { m_RelocateSpeed = value; } }

        Vector2 UnnormalizedPosition
        {
            get
            {
                float x = scrollRect.normalizedPosition.x * scrollRange.x;
                float y = scrollRect.normalizedPosition.y * scrollRange.y;
                return new Vector2(x, y) + GetSize(scrollRect.viewport) / 2;
            }
        }

        public enum Status
        {
            Dragging,
            Coasting,
            Relocating,
            Relocated,
        }
        public Status status { get; private set; }
        public bool isDragging { get { return status == Status.Dragging; } }
        public bool isCoasting { get { return status == Status.Coasting; } }
        public bool isRelocating { get { return status == Status.Relocating; } }
        public bool isRelocated { get { return status == Status.Relocated; } }
        public Vector2 relocation { get; private set; }
        public GameObject focusObject { get; private set; }

        public Action onMoveEvent = delegate { };
        public Action<GameObject> onStopEvent = delegate { };

        private ScrollRect scrollRect;
        private GridLayoutGroup gridLayoutGroup;
        private Vector2 gridSize;
        private Vector2 scrollRange;

        protected void Awake()
        {
            scrollRect = GetComponent<ScrollRect>();
            gridLayoutGroup = scrollRect.content.GetComponent<GridLayoutGroup>();
            if (gridLayoutGroup == null) gridLayoutGroup = scrollRect.content.gameObject.AddComponent<GridLayoutGroup>();
            status = Status.Coasting;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            status = Status.Dragging;
            onMoveEvent();
        }
        public void OnDrag(PointerEventData eventData)
        {
            status = Status.Dragging;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            status = Status.Coasting;
        }

        protected void Update()
        {
            Vector2 padding = (GetSize(scrollRect.viewport) - gridLayoutGroup.cellSize) / 2;
            gridLayoutGroup.padding.left = gridLayoutGroup.padding.right = (int)padding.x;
            gridSize = gridLayoutGroup.cellSize + gridLayoutGroup.spacing;
            scrollRange = GetSize(scrollRect.content) - GetSize(scrollRect.viewport);
            for (int i = 0; i < scrollRect.content.childCount; i++)
            {
                RectTransform item = scrollRect.content.GetChild(i).GetComponent<RectTransform>();
                float offsetX = Mathf.Clamp((UnnormalizedPosition.x - item.anchoredPosition.x) / gridSize.x, -1, 1);
                //float offsetY = Mathf.Clamp((UnnormalizedPosition.y - item.anchoredPosition.y) / gridSize.y, -1, 1);
                item.localRotation = Quaternion.AngleAxis(tiltAngle * offsetX, Vector3.up);
                item.localScale = Vector2.one * Mathf.Lerp(scaleCenter, scaleOther, Mathf.Abs(offsetX));
            }

            switch (status)
            {
                case Status.Dragging:
                    break;
                case Status.Coasting:
                    if (scrollRect.velocity.magnitude < speedThreshold)
                    {
                        relocation = Mathf.Infinity * Vector2.one;
                        for (int i = 0; i < scrollRect.content.childCount; i++)
                        {
                            Vector2 position = scrollRect.content.GetChild(i).GetComponent<RectTransform>().anchoredPosition;
                            if ((UnnormalizedPosition - relocation).sqrMagnitude > (UnnormalizedPosition - position).sqrMagnitude)
                            {
                                relocation = position;
                                focusObject = scrollRect.content.GetChild(i).gameObject;
                            }
                        }
                        status = Status.Relocating;
                    }
                    break;
                case Status.Relocating:
                    scrollRect.normalizedPosition = Vector2.MoveTowards(scrollRect.normalizedPosition, Normalize(relocation), relocateSpeed / scrollRange.magnitude * Time.deltaTime);
                    if ((scrollRect.normalizedPosition - Normalize(relocation)).sqrMagnitude <= Vector2.kEpsilon)
                    {
                        status = Status.Relocated;
                        onStopEvent(focusObject);
                    }
                    break;
                case Status.Relocated:
                    break;
            }
    }

        protected Vector2 Normalize(Vector2 position)
        {
            position = position - GetSize(scrollRect.viewport) / 2;
            float x = scrollRange.x == 0 ? 0 : Mathf.Clamp01(position.x / scrollRange.x);
            float y = scrollRange.y == 0 ? 0 : Mathf.Clamp01(position.y / scrollRange.y);
            return new Vector2(x, y);
        }
        protected Vector2 GetSize(RectTransform recttransform)
        {
            Vector2 anchorSize = recttransform.anchorMax - recttransform.anchorMin;
            Vector2 rectSize = new Vector2(recttransform.rect.size.x * anchorSize.x, recttransform.rect.size.y * anchorSize.y);
            return rectSize + recttransform.sizeDelta;
        }
    }
}