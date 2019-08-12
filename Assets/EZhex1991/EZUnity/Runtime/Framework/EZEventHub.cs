/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-11-26 14:35:53
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEngine.UI;

namespace EZhex1991.EZUnity.Framework.Events
{
    public class EZEventHub : EZMonoBehaviourSingleton<EZEventHub>
    {
        public Dictionary<int, EZEvent> m_Events = new Dictionary<int, EZEvent>();

        public void SendEvent(object eventKey, object sender, object value)
        {
            LogFormat("Event {0} From {1}", eventKey, sender);
            int intKey = eventKey.GetHashCode();
            EZEvent e;
            if (m_Events.TryGetValue(intKey, out e))
            {
                e.SendEvent(sender, value);
            }
            else
            {
                e = new EZEvent(value);
            }
        }
        public void AddListener(object eventKey, EZEventHandler eventHandler)
        {
            int intKey = eventKey.GetHashCode();
            if (m_Events.ContainsKey(intKey))
            {
                m_Events[intKey].handlers += eventHandler;
            }
            else
            {
                m_Events.Add(intKey, new EZEvent(eventHandler));
            }
        }
        public void RemoveListener(object eventKey, EZEventHandler eventHandler)
        {
            int intKey = eventKey.GetHashCode();
            if (m_Events.ContainsKey(intKey))
            {
                m_Events[intKey].handlers -= eventHandler;
            }
        }

        public void BindComponent(object eventKey, Button button, object arg = null)
        {
            button.onClick.AddListener(() =>
            {
                SendEvent(eventKey, button, arg);
            });
        }
        public void BindComponent(object eventKey, Toggle toggle)
        {
            toggle.onValueChanged.AddListener((isOn) =>
            {
                SendEvent(eventKey, toggle, isOn);
            });
            AddListener(eventKey, (sender, arg) =>
            {
                bool isOn = (bool)arg;
                toggle.isOn = isOn;
            });
        }
        public void BindComponent(object eventKey, Dropdown dropdown)
        {
            dropdown.onValueChanged.AddListener((selection) =>
            {
                SendEvent(eventKey, dropdown, selection);
            });
            AddListener(eventKey, (sender, arg) =>
            {
                int selection = (int)arg;
                dropdown.value = selection;
            });
        }
        public void BindComponent(object eventKey, Text text)
        {
            AddListener(eventKey, (sender, arg) =>
            {
                string value = (string)arg;
                text.text = value;
            });
        }
    }
}
