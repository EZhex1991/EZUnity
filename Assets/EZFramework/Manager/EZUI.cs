/*
 * Author:      熊哲
 * CreateTime:  11/28/2016 2:25:36 PM
 * Description:
 * 
*/
using System.Collections.Generic;
using UnityEngine;

namespace EZFramework
{
    public class EZUI : TEZManager<EZUI>
    {
        [SerializeField]
        private GameObject m_UICanvas;
        public GameObject UICanvas { get { return m_UICanvas; } set { m_UICanvas = value; } }

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
            GameObject panel = Instantiate(panelPrefab);
            panel.transform.SetParent(UICanvas.transform, false);
            panel.transform.SetAsLastSibling();
            panel.name = panelName;
            panel.layer = LayerMask.NameToLayer("UI");
            panel.transform.localScale = Vector3.one;
            panel.transform.localPosition = Vector3.zero;
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
            RefreshOrder();
        }
        public void ClosePanel(string panelName)
        {
            if (panelList.Contains(panelName))
            {
                Destroy(panelDict[panelName]);
                panelDict.Remove(panelName);
                panelList.Remove(panelName);
            }
            RefreshOrder();
        }
        
        public void RefreshOrder()
        {
            foreach (var panel in panelDict.Values)
            {
                int order = panel.transform.GetSiblingIndex();
                foreach (var canvas in panel.GetComponentsInChildren<Canvas>())
                {
                    canvas.overrideSorting = true;
                    canvas.sortingOrder = order;
                }
                foreach (var renderer in panel.GetComponentsInChildren<Renderer>())
                {
                    renderer.sortingOrder = order;
                }
            }
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

    public static class EZUIExtensions
    {
        public static Vector2 GetSize(this RectTransform recttransform)
        {
            Vector2 anchorDistance = recttransform.anchorMax - recttransform.anchorMin;
            Vector2 rectSize = new Vector2(recttransform.rect.size.x * anchorDistance.x, recttransform.rect.size.y * anchorDistance.y);
            return rectSize + recttransform.sizeDelta;
        }
    }
}