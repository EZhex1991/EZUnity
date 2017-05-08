/*
 * Author:      熊哲
 * CreateTime:  1/12/2017 5:28:48 PM
 * Description:
 * 
*/
using UnityEngine;

namespace EZUnityTools
{
    [ExecuteInEditMode]
    public class EZGridLayout : MonoBehaviour
    {
        [SerializeField]
        private RectTransform.Axis m_StartAxis;
        public RectTransform.Axis startAxis { get { return m_StartAxis; } set { m_StartAxis = value; } }

        [SerializeField]
        private int m_Count = 1;
        public int Count { get { return m_Count; } set { m_Count = Mathf.Clamp(value, 1, 100); } }

        [SerializeField]
        private Vector2 m_Padding;
        public Vector2 padding { get { return m_Padding; } set { m_Padding = value; } }

        [SerializeField]
        private Vector2 m_GridSize;
        public Vector2 gridSize { get { return m_GridSize; } set { m_GridSize = value; } }

        void LateUpdate()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                switch (m_StartAxis)
                {
                    case RectTransform.Axis.Horizontal:
                        transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = new Vector2(
                            padding.x + (i % m_Count) * gridSize.x,
                            padding.y + Mathf.FloorToInt(i / m_Count) * gridSize.y
                        );
                        break;
                    case RectTransform.Axis.Vertical:
                        transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = new Vector2(
                            padding.x + (i / m_Count) * gridSize.x,
                            padding.y + Mathf.FloorToInt(i % m_Count) * gridSize.y
                        );
                        break;
                }
            }
        }
    }
}