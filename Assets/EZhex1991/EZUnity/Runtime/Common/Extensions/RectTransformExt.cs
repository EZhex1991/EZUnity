/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-11-28 15:40:54
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public static class RectTransformExt
    {
        public static Vector2 GetSize(this RectTransform rt)
        {
            Vector2 anchorDistance = rt.anchorMax - rt.anchorMin;
            Vector2 rectSize = new Vector2(rt.rect.size.x * anchorDistance.x, rt.rect.size.y * anchorDistance.y);
            return rectSize + rt.sizeDelta;
        }

        public static void SetAnchoredPositionX(this RectTransform rt, float x)
        {
            Vector2 anchoredPosition = rt.anchoredPosition;
            anchoredPosition.x = x;
            rt.anchoredPosition = anchoredPosition;
        }
        public static void SetAnchoredPositionY(this RectTransform rt, float y)
        {
            Vector2 anchoredPosition = rt.anchoredPosition;
            anchoredPosition.y = y;
            rt.anchoredPosition = anchoredPosition;
        }
    }
}
