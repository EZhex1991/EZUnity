/* Author:          熊哲
 * CreateTime:      2018-06-07 20:36:37
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using UnityEngine.UI;

namespace EZUnity
{
    public class EZStringManager : MonoBehaviour
    {
        [System.Serializable]
        public class Subscriber
        {
            public Text text;
            public string key;
        }

        public EZStringAsset stringAsset;
        public Subscriber[] subscribers;

        public void Start()
        {
            Refresh();
        }
        public void Refresh()
        {
            for (int i = 0; i < subscribers.Length; i++)
            {
                subscribers[i].text.text = stringAsset.GetString(subscribers[i].key);
            }
        }

        public static void RefreshAll()
        {
            EZStringManager[] managers = FindObjectsOfType<EZStringManager>();
            for (int i = 0; i < managers.Length; i++)
            {
                managers[i].Refresh();
            }
        }
    }
}
