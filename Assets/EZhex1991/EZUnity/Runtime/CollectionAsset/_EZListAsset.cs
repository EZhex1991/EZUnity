/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-07-09 14:44:44
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZhex1991.EZUnity.EZCollectionAsset
{
    /// <summary>
    /// Base class for EZListAssetEditor
    /// </summary>
    public class EZListAsset : ScriptableObject
    {
    }

    public class EZListAsset<TKey> : EZListAsset, ISerializationCallbackReceiver
    {
        [SerializeField]
        protected List<TKey> m_Keys = new List<TKey>();
        public List<TKey> keys { get { return m_Keys; } }

        protected Dictionary<TKey, int> m_KeyIndexMap = new Dictionary<TKey, int>();
        protected Dictionary<TKey, int> m_KeyCountMap = new Dictionary<TKey, int>();
        public int Count { get { return m_Keys.Count; } }

        public virtual void OnBeforeSerialize()
        {
        }
        public virtual void OnAfterDeserialize()
        {
            m_KeyIndexMap.Clear();
            m_KeyCountMap.Clear();
            for (int i = 0; i < m_Keys.Count; i++)
            {
                TKey key = m_Keys[i];
                if (key == null) continue;
                if (m_KeyCountMap.ContainsKey(key))
                {
                    m_KeyCountMap[key]++;
                }
                else
                {
                    m_KeyCountMap[key] = 1;
                    m_KeyIndexMap[key] = i;
                }
            }
        }

        public virtual void Add(TKey key)
        {
            m_Keys.Add(key);
            m_KeyIndexMap[key] = m_Keys.Count - 1;
            if (m_KeyCountMap.ContainsKey(key))
            {
                m_KeyCountMap[key]++;
                Debug.LogWarning("Duplicate Key Detected: " + key);
            }
            else
            {
                m_KeyCountMap[key] = 1;
            }
        }
        public virtual bool Insert(int index, TKey key)
        {
            try
            {
                m_Keys.Insert(index, key);
                m_KeyIndexMap[key] = index;
                if (m_KeyCountMap.ContainsKey(key))
                {
                    m_KeyCountMap[key]++;
                    Debug.LogWarning("Duplicate Key Detected: " + key);
                }
                else
                {
                    m_KeyCountMap[key] = 1;
                }
                for (int i = index + 1; i < m_Keys.Count; i++)
                {
                    m_KeyIndexMap[m_Keys[i]]++;
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }

        public bool IsKeyDuplicate(TKey key)
        {
            return m_KeyCountMap.ContainsKey(key) && m_KeyCountMap[key] > 1;
        }
        public bool ContainsKey(TKey key)
        {
            return m_KeyIndexMap.ContainsKey(key);
        }
    }
}
