/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-07-09 14:44:44
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZDictionaryAsset<TKey, TValue> : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        protected List<TKey> m_Keys = new List<TKey>();
        public List<TKey> keys { get { return m_Keys; } }
        [SerializeField]
        protected List<TValue> m_Values = new List<TValue>();
        public List<TValue> values { get { return m_Values; } }

        protected Dictionary<TKey, int> m_Dictionary = new Dictionary<TKey, int>();
        protected Dictionary<TKey, int> m_KeyRecord = new Dictionary<TKey, int>();

        public TValue this[TKey key] { get { return m_Values[m_Dictionary[key]]; } set { m_Values[m_Dictionary[key]] = value; } }
        public TValue this[int index] { get { return m_Values[index]; } set { m_Values[index] = value; } }
        public int Count { get { return m_Keys.Count; } }

        public void OnBeforeSerialize()
        {
        }
        public void OnAfterDeserialize()
        {
            m_Dictionary.Clear();
            m_KeyRecord.Clear();
            for (int i = 0; i < m_Keys.Count; i++)
            {
                TKey key = m_Keys[i];
                if (m_KeyRecord.ContainsKey(key))
                {
                    m_KeyRecord[key]++;
                }
                else
                {
                    m_KeyRecord[key] = 1;
                    m_Dictionary[key] = i;
                }
            }
        }

        public void AddKey(TKey key)
        {
            AddItem(key, default(TValue));
        }
        public void AddItem(TKey key, TValue value)
        {
            if (m_KeyRecord.ContainsKey(key))
            {
                m_KeyRecord[key]++;
                Debug.LogWarning("Duplicate Key Detected: " + key);
            }
            else
            {
                m_KeyRecord[key] = 1;
            }
            m_Dictionary[key] = m_Keys.Count;
            m_Keys.Add(key);
            m_Values.Add(value);
        }

        public bool IsKeyDuplicate(TKey key)
        {
            return m_KeyRecord.ContainsKey(key) && m_KeyRecord[key] > 1;
        }

        public bool ContainsKey(TKey key)
        {
            return m_Dictionary.ContainsKey(key);
        }
        public bool TryGetValue(TKey key, out TValue value)
        {
            int index;
            if (m_Dictionary.TryGetValue(key, out index))
            {
                value = m_Values[index];
                return true;
            }
            else
            {
                value = default(TValue);
                return false;
            }
        }
    }
}
