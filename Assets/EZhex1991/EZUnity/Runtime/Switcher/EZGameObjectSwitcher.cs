/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-03-21 19:11:54
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZGameObjectSwitcher : EZSwitcher<GameObject>
    {
        [SerializeField]
        private Transform m_Parent;
        public Transform parent { get { return m_Parent; } }

        private GameObject[] instances;

        protected override void Start()
        {
            instances = new GameObject[options.Length];
            base.Start();
        }

        public override void Switch(int index)
        {
            if (index >= 0 && index < instances.Length && instances[index] == null)
            {
                instances[index] = Instantiate(options[index], parent);
            }
            for (int i = 0; i < instances.Length; i++)
            {
                if (instances[i] != null) instances[i].SetActive(i == index);
            }
        }
    }
}
