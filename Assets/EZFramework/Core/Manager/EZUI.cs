/*
 * Author:      熊哲
 * CreateTime:  11/28/2016 2:25:36 PM
 * Description:
 * 很多UI会同时用到Particle和RectTransform，必须通过sortingOrder进行预排序，而不同UI之间的排序通常会使用SortingLayer（改动Z轴会导致DrawCall增加）
 * 但是SortingLayer必须在打包前设置，无法动态更改和添加，也就意味着不能热更新
 * 在避免使用SortingLayer的情况下，定义了ORDER_STEP这个值用来在不同UI间进行排序（动态更改UI上的Canvas和Renderer的order）
*/
using System.Collections.Generic;
using UnityEngine;

namespace EZFramework
{
    public class EZUI : _EZManager<EZUI>
    {
        [SerializeField]
        private GameObject m_UICanvas;
        public GameObject UICanvas { get { return m_UICanvas; } set { m_UICanvas = value; } }

        private const int ORDER_STEP = 100; // 用来对UI进行排序的Order，要求设置sortingOrder必须在(-ORDER_STEP/2, ORDER_STEP/2)范围内
        private const int ORDER_MAX_ABS = 2000; // 绝对值超过该数值的Order不动态改变

        protected Dictionary<string, GameObject> panelDict = new Dictionary<string, GameObject>();
        protected LinkedList<string> panelList = new LinkedList<string>();

        public override void Init()
        {
            base.Init();
        }
        public override void Exit()
        {
            base.Exit();
        }

        public GameObject ShowPanel(string panelName, GameObject panelPrefab)
        {
            if (panelList.Contains(panelName))
            {
                return ShowPanel(panelName);
            }
            GameObject panel = Instantiate(panelPrefab, UICanvas.transform);
            panel.transform.localPosition = Vector3.zero;
            panel.transform.SetAsLastSibling();
            panel.name = panelName;
            panel.layer = LayerMask.NameToLayer("UI");
            panelDict.Add(panelName, panel);
            panelList.AddFirst(panelName);
            RefreshOrder();
            return panel;
        }
        public GameObject ShowPanel(string panelName)
        {
            panelList.Remove(panelName);
            panelList.AddFirst(panelName);
            panelDict[panelName].transform.SetAsLastSibling();
            panelDict[panelName].SetActive(true);
            RefreshOrder();
            return panelDict[panelName];
        }
        public void HidePanel(string panelName)
        {
            if (panelList.Contains(panelName))
            {
                panelList.Remove(panelName);
                panelList.AddLast(panelName);
                panelDict[panelName].SetActive(false);
            }
        }
        public void ClosePanel(string panelName)
        {
            if (panelList.Contains(panelName))
            {
                Destroy(panelDict[panelName]);
                panelDict.Remove(panelName);
                panelList.Remove(panelName);
            }
        }

        public void RefreshOrder()
        {
            foreach (var panel in panelDict.Values)
            {
                int offset = panel.transform.GetSiblingIndex() * ORDER_STEP;
                foreach (var canvas in panel.GetComponentsInChildren<Canvas>(true))
                {
                    canvas.overrideSorting = true;
                    canvas.sortingOrder = GetOrder(canvas.sortingOrder) + offset;
                }
                foreach (var renderer in panel.GetComponentsInChildren<Renderer>(true))
                {
                    renderer.sortingOrder = GetOrder(renderer.sortingOrder) + offset;
                }
            }
        }
        private int GetOrder(int order)
        {
            return Mathf.Abs(order) < ORDER_MAX_ABS ? order - Mathf.RoundToInt((float)order / ORDER_STEP) * ORDER_STEP : order;
        }

        public string GetCurrentPanel()
        {
            return panelList.First.Value;
        }
        public bool IsPeak(string panelName)
        {
            return panelList.First.Value == panelName;
        }
        public bool IsShowing(string panelName)
        {
            return panelDict[panelName].activeSelf;
        }
    }
}