/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-08-09 18:08:00
 * Organization:    #ORGANIZATION#
 * Description:     对原生ScrollRect的扩展，设置网格大小，滑动停止后自动Focus最近的点，和EZGridLayout2D配合
 */
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EZhex1991.EZUnity
{
    public class EZScrollRect : ScrollRect
    {
        [SerializeField]
        private int m_HorizontalCount = 1;
        public int horizontalCount { get { return m_HorizontalCount; } set { m_HorizontalCount = value; } }

        [SerializeField]
        private int m_VerticalCount = 1;
        public int verticalCount { get { return m_VerticalCount; } set { m_VerticalCount = value; } }

        [SerializeField]
        private float m_InertialSpeedThreshold = 500;
        public float inertialSpeedThreshold { get { return m_InertialSpeedThreshold; } set { m_InertialSpeedThreshold = value; } }

        [SerializeField]
        private float m_RepositionTime = 0.3f;
        public float repositionTime { get { return m_RepositionTime; } set { m_RepositionTime = value; } }

        public enum Status
        {
            Idle,
            Dragging,
            Coasting,
            Repositioning,
        }
        public Status status { get; private set; }
        public int focusingX { get; private set; }
        public int focusingY { get; private set; }

        public delegate void OnBeginScrollAction();
        public event OnBeginScrollAction onBeginScrollEvent;
        public delegate void OnEndScrollAction(int x, int y);
        public event OnEndScrollAction onEndScrollEvent;

        private Vector2 step
        {
            get
            {
                float stepX = horizontalCount > 1 ? 1.0f / (horizontalCount - 1) : 0;
                float stepY = verticalCount > 1 ? 1.0f / (verticalCount - 1) : 0;
                return new Vector2(stepX, stepY);
            }
        }
        private Vector2 sourcePosition;
        private Vector2 reposition;
        private Vector2 distance;
        private float inversedRepositionTime;
        private float lerp;

        protected override void Start()
        {
            base.Start();
            status = Status.Idle;
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            status = Status.Dragging;
            if (onBeginScrollEvent != null) onBeginScrollEvent();
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            status = Status.Coasting;
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            switch (status)
            {
                case Status.Idle:
                    break;
                case Status.Dragging:
                    break;
                case Status.Coasting:
                    if (velocity.magnitude < inertialSpeedThreshold)
                    {
                        sourcePosition = normalizedPosition;
                        focusingX = step.x == 0 ? 0 : Mathf.RoundToInt(normalizedPosition.x / step.x);
                        focusingY = step.y == 0 ? 0 : Mathf.RoundToInt(normalizedPosition.y / step.y);
                        reposition = new Vector2(focusingX * step.x, focusingY * step.y);
                        inversedRepositionTime = 1 / repositionTime;
                        status = Status.Repositioning;
                    }
                    break;
                case Status.Repositioning:
                    // 当行数或列数为1时，normalizedPosition的位置可能在0和1两者间变化，此时MoveTowards会出现异常（以0-0计算的无效速度去做1-0的移动）
                    lerp = lerp + Time.deltaTime;
                    if (step.x != 0)
                    {
                        horizontalNormalizedPosition = Mathf.Lerp(sourcePosition.x, reposition.x, lerp * inversedRepositionTime);
                        distance.x = horizontalNormalizedPosition - reposition.x;
                    }
                    else
                    {
                        distance.x = 0;
                    }
                    if (step.y != 0)
                    {
                        verticalNormalizedPosition = Mathf.Lerp(sourcePosition.y, reposition.y, lerp * inversedRepositionTime);
                        distance.y = verticalNormalizedPosition - reposition.y;
                    }
                    else
                    {
                        distance.y = 0;
                    }
                    if (distance.sqrMagnitude <= 1e-6)
                    {
                        lerp = 0;
                        StopMovement();
                        status = Status.Idle;
                        if (onEndScrollEvent != null) onEndScrollEvent(focusingX, focusingY);
                    }
                    break;
            }
        }
    }
}