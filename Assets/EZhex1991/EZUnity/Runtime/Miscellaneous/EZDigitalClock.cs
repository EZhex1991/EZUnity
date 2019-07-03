/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-05-11 13:05:58
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEngine;
using UnityEngine.UI;

namespace EZhex1991.EZUnity
{
    [RequireComponent(typeof(Text))]
    public class EZDigitalClock : MonoBehaviour
    {
        public enum Mode { UTC, Local }

        public Mode mode;
        public string format = "HH:mm:ss";

        private Text m_UIText;
        public Text uiText
        {
            get
            {
                if (m_UIText == null)
                    m_UIText = GetComponent<Text>();
                return m_UIText;
            }
        }

        public DateTime time
        {
            get
            {
                switch (mode)
                {
                    case Mode.UTC:
                        return DateTime.UtcNow;
                    case Mode.Local:
                        return DateTime.Now;
                    default:
                        return DateTime.Now;
                }
            }
        }
        public string timeText
        {
            get
            {
                try
                {
                    return time.ToString(format);
                }
                catch
                {
                    return time.ToShortTimeString();
                }
            }
        }

        void Update()
        {
            uiText.text = timeText;
        }
    }
}
