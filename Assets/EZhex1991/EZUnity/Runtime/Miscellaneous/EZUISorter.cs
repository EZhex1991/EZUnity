/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-06-19 17:16:39
 * Organization:    #ORGANIZATION#
 * Description:     
 * 
 * 很多UI会同时用到Particle和RectTransform，必须通过sortingOrder进行预排序，而不同UI之间的排序通常会使用SortingLayer（改动Z轴会导致DrawCall增加）
 * 但是SortingLayer必须在打包前设置，无法动态更改和添加，也就意味着不能热更新
 * 在避免使用SortingLayer的情况下，定义了ORDER_STEP这个值用来在不同UI间进行排序（动态更改UI上的Canvas和Renderer的order）
 */
using System.Collections.Generic;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZUISorter : MonoBehaviour
    {
        private const int ORDER_STEP = 100; // 用来对UI进行排序的Order，要求设置sortingOrder必须在(-ORDER_STEP/2, ORDER_STEP/2)范围内
        private const int ORDER_MAX_ABS = 2000; // 绝对值超过该数值的Order不动态改变

        private Dictionary<string, GameObject> panels = new Dictionary<string, GameObject>();

        protected void Awake()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                panels.Add(child.name, child.gameObject);
            }
        }

        private void RefreshOrder()
        {
            foreach (GameObject panel in panels.Values)
            {
                int offset = panel.transform.GetSiblingIndex() * ORDER_STEP;
                foreach (Canvas canvas in panel.GetComponentsInChildren<Canvas>(true))
                {
                    canvas.overrideSorting = true;
                    canvas.sortingOrder = GetOrder(canvas.sortingOrder) + offset;
                }
                foreach (Renderer renderer in panel.GetComponentsInChildren<Renderer>(true))
                {
                    renderer.sortingOrder = GetOrder(renderer.sortingOrder) + offset;
                }
            }
        }
        private int GetOrder(int order)
        {
            if (Mathf.Abs(order) < ORDER_MAX_ABS)
            {
                return order - Mathf.RoundToInt((float)order / ORDER_STEP) * ORDER_STEP;
            }
            else
            {
                return order;
            }
        }

        public GameObject ShowPanel(string panelName, GameObject prefab)
        {
            GameObject panel;
            if (panels.TryGetValue(panelName, out panel))
            {
                return ShowPanel(panelName);
            }
            else
            {
                panel = Instantiate(prefab, transform);
                panel.transform.SetAsLastSibling();
                panel.name = panelName;
                panels.Add(panelName, panel);
                RefreshOrder();
                return panel;
            }
        }
        public GameObject ShowPanel(string panelName)
        {
            GameObject panel = panels[panelName];
            panel.transform.SetAsFirstSibling();
            panel.SetActive(true);
            RefreshOrder();
            return panel;
        }
        public void HidePanel(string panelName)
        {
            GameObject panel;
            if (panels.TryGetValue(panelName, out panel))
            {
                panel.SetActive(false);
            }
        }
        public void ClosePanel(string panelName)
        {
            GameObject panel;
            if (panels.TryGetValue(panelName, out panel))
            {
                Destroy(panel);
                panels.Remove(panelName);
            }
        }
    }
}
