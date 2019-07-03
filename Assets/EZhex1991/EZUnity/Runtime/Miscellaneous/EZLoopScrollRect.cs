/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-11-08 17:33:45
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using UnityEngine.UI;

namespace EZhex1991.EZUnity
{
    [RequireComponent(typeof(ScrollRect))]
    public class EZLoopScrollRect : MonoBehaviour
    {
        private ScrollRect m_ScrollRect;
        public ScrollRect scrollRect
        {
            get
            {
                if (m_ScrollRect == null)
                    m_ScrollRect = GetComponent<ScrollRect>();
                return m_ScrollRect;
            }
        }

        private RectTransform content { get { return scrollRect.content; } }
        private RectTransform content1;
        private RectTransform content2;

        private void Start()
        {
            scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
            scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
            content1 = Instantiate(content, content.parent);
            content2 = Instantiate(content, content.parent);
            if (scrollRect.horizontal)
            {
                content1.pivot = content.pivot + new Vector2(-1, 0);
                content2.pivot = content.pivot + new Vector2(1, 0);
            }
            else
            {
                content1.pivot = content.pivot + new Vector2(0, -1);
                content2.pivot = content.pivot + new Vector2(0, 1);
            }
            SyncContents();
        }

        private void OnScrollValueChanged(Vector2 position)
        {
            Vector2 anchoredPosition = content.anchoredPosition;
            Vector2 size = content.GetSize();
            if (anchoredPosition.x < -size.x)
            {
                anchoredPosition.x += size.x;
            }
            else if (anchoredPosition.x > 0)
            {
                anchoredPosition.x -= size.x;
            }
            if (anchoredPosition.y < -size.y)
            {
                anchoredPosition.y += size.y;
            }
            else if (anchoredPosition.y > 0)
            {
                anchoredPosition.y -= size.y;
            }
            content.anchoredPosition = anchoredPosition;
            SyncContents();
        }

        private void SyncContents()
        {
            content1.anchoredPosition = content.anchoredPosition;
            content2.anchoredPosition = content.anchoredPosition;
        }
    }
}
