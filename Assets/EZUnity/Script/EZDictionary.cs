/* Author:          熊哲
 * CreateTime:      2018-05-07 16:19:38
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEngine;

namespace EZUnity
{
    public class EZDictionary : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField]
        private EZSerializableProperty[] m_Elements;
        public Dictionary<string, EZSerializableProperty> dictionary = new Dictionary<string, EZSerializableProperty>();

        public void OnBeforeSerialize()
        {

        }
        public void OnAfterDeserialize()
        {
            dictionary.Clear();
            for (int i = 0; i < m_Elements.Length; i++)
            {
                string key = m_Elements[i].key;
                if (string.IsNullOrEmpty(key)) continue;
                if (dictionary.ContainsKey(key)) continue;
                dictionary.Add(key, m_Elements[i]);
            }
        }

        public EZSerializableProperty this[string key]
        {
            get
            {
                return dictionary[key];
            }
        }

        public bool Contains(string key)
        {
            return dictionary.ContainsKey(key);
        }
        public EZSerializableProperty Get(string key)
        {
            return dictionary[key];
        }
        public T Get<T>(string key) where T : Object
        {
            return dictionary[key].objectValue as T;
        }
    }
}
