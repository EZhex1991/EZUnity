/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-07-08 11:55:28
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CreateAssetMenu(fileName = "EZStringDictionary", menuName = "EZUnity/EZStringDictionary", order = (int)EZAssetMenuOrder.EZStringDictionary)]
    public class EZStringDictionaryAsset : ScriptableObject, ISerializationCallbackReceiver, IEnumerable<KeyValuePair<string, string>>
    {
        [System.Serializable]
        public struct Pair
        {
            [SerializeField]
            private string m_Key;
            public string key { get { return m_Key; } set { m_Key = value; } }
            [SerializeField]
            private string m_Value;
            public string value { get { return m_Value; } set { m_Value = value; } }
        }

        [SerializeField]
        private List<Pair> m_Pairs = new List<Pair>();

        private Dictionary<string, string> m_Dictionary = new Dictionary<string, string>();
        private Dictionary<string, int> m_KeyCount = new Dictionary<string, int>();

        public string this[string key] { get { return m_Dictionary[key]; } set { m_Dictionary[key] = value; } }
        public Pair this[int index] { get { return m_Pairs[index]; } }
        public int Count { get { return m_Dictionary.Count; } }
        public Dictionary<string, string>.KeyCollection Keys { get { return m_Dictionary.Keys; } }
        public Dictionary<string, string>.ValueCollection Values { get { return m_Dictionary.Values; } }

        public bool IsKeyDuplicate(string key)
        {
            return m_KeyCount.ContainsKey(key) && m_KeyCount[key] > 1;
        }
        public void AddKey(string key)
        {
            m_Pairs.Add(new Pair { key = key });
        }
        public void AddPair(string key, string value)
        {
            m_Pairs.Add(new Pair { key = key, value = value });
        }

        public void OnBeforeSerialize()
        {
        }
        public void OnAfterDeserialize()
        {
            m_Dictionary.Clear();
            m_KeyCount.Clear();
            foreach (var pair in m_Pairs)
            {
                if (m_KeyCount.ContainsKey(pair.key))
                {
                    m_KeyCount[pair.key]++;
                }
                else
                {
                    m_KeyCount[pair.key] = 1;
                    m_Dictionary[pair.key] = pair.value;
                }
            }
        }

        public bool ContainsKey(string key)
        {
            return m_Dictionary.ContainsKey(key);
        }
        public bool ContainsValue(string value)
        {
            return m_Dictionary.ContainsValue(value);
        }
        public bool TryGetValue(string key, out string value)
        {
            return m_Dictionary.TryGetValue(key, out value);
        }

        IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
        {
            return m_Dictionary.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_Dictionary.GetEnumerator();
        }
    }
}
