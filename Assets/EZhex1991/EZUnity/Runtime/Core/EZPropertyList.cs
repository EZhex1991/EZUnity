/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-05-07 16:19:38
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZPropertyList : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField]
        protected bool m_IsList;
        public bool isList { get { return m_IsList; } }

        [SerializeField]
        protected EZProperty[] m_Elements;
        public EZProperty[] elements { get { return m_Elements; } }

        private Dictionary<object, EZProperty> m_Dictionary = new Dictionary<object, EZProperty>();
        public Dictionary<object, EZProperty> dictionary { get { return m_Dictionary; } }

        public void OnBeforeSerialize()
        {

        }
        public void OnAfterDeserialize()
        {
            dictionary.Clear();
            if (isList)
            {
                for (int i = 0; i < elements.Length; i++)
                {
                    dictionary.Add(i, elements[i]);
                }
            }
            else
            {
                for (int i = 0; i < elements.Length; i++)
                {
                    string key = elements[i].key;
                    if (string.IsNullOrEmpty(key)) continue;
                    if (dictionary.ContainsKey(key)) continue;
                    dictionary.Add(key, elements[i]);
                }
            }
        }

        public EZProperty this[int index] { get { return elements[index]; } }
        public EZProperty this[string key] { get { return dictionary[key]; } }

        public EZProperty Get(int index)
        {
            return elements[index];
        }
        public T Get<T>(int index) where T : Object
        {
            return elements[index].objectValue as T;
        }

        public EZProperty Get(string key)
        {
            return dictionary[key];
        }
        public T Get<T>(string key) where T : Object
        {
            return dictionary[key].objectValue as T;
        }

        public bool Contains(string key)
        {
            return dictionary.ContainsKey(key);
        }
    }
}
