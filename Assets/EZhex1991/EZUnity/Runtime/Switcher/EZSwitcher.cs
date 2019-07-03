/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-04-17 15:25:24
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public abstract class EZSwitcher<T> : MonoBehaviour
    {
        [SerializeField]
        private bool m_AllowSwitchOff;
        public bool allowSwitchOff { get { return m_AllowSwitchOff; } set { m_AllowSwitchOff = value; } }

        [SerializeField]
        private bool m_SwitchOnStart;
        public bool switchOnStart { get { return m_SwitchOnStart; } }

        [SerializeField]
        private int m_Next;
        public int next { get { return m_Next; } private set { m_Next = value; } }

        [SerializeField]
        private T[] m_Options;
        public T[] options { get { return m_Options; } set { m_Options = value; } }

        protected virtual void Start()
        {
            if (switchOnStart) Switch();
        }

        public void Switch()
        {
            Switch(next++);
            next %= options.Length + 1;
            if (next == options.Length && !allowSwitchOff)
            {
                next = 0;
            }
        }
        public abstract void Switch(int index);
    }
}
