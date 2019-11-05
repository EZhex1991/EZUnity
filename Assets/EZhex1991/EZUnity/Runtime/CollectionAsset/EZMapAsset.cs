/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-08-05 11:37:14
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZhex1991.EZUnity.EZCollectionAsset
{
    /// <summary>
    /// Base class for EZMapAssetEditor
    /// </summary>
    public abstract class EZMapAsset : ScriptableObject
    {
        public virtual float keyRectWidth { get { return 0.5f; } }
        public abstract bool IsKeyDuplicate(int index);
    }

    public class EZMapAsset<TKey, TValue> : EZMapAsset, ISerializationCallbackReceiver
    {
        [SerializeField]
        protected List<TKey> m_Keys = new List<TKey>();
        public List<TKey> keys { get { return m_Keys; } }
        [SerializeField]
        protected List<TValue> m_Values = new List<TValue>();
        public List<TValue> values { get { return m_Values; } }

        protected Dictionary<TKey, int> m_KeyIndexMap = new Dictionary<TKey, int>();
        protected Dictionary<TKey, int> m_KeyCountMap = new Dictionary<TKey, int>();

        public int Count { get { return m_Keys.Count; } }

        public TValue this[TKey key] { get { return m_Values[m_KeyIndexMap[key]]; } set { m_Values[m_KeyIndexMap[key]] = value; } }
        protected TValue this[int index] { get { return m_Values[index]; } set { m_Values[index] = value; } }

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

        public void Add(TKey key)
        {
            Add(key, default(TValue));
        }
        public void Add(TKey key, TValue value)
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
            m_Values.Add(value);
        }

        public bool Insert(int index, TKey key)
        {
            return Insert(index, key, default(TValue));
        }
        public bool Insert(int index, TKey key, TValue value)
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
                m_Values.Insert(index, value);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }

        public bool TryGetIndex(TKey key, out int index)
        {
            return m_KeyIndexMap.TryGetValue(key, out index);
        }
        public bool TryGetValue(TKey key, out TValue value)
        {
            int index;
            if (m_KeyIndexMap.TryGetValue(key, out index))
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

        public override bool IsKeyDuplicate(int index)
        {
            TKey key = m_Keys[index];
            return m_KeyCountMap.ContainsKey(key) && m_KeyCountMap[key] > 1;
        }
        public bool ContainsKey(TKey key)
        {
            return m_KeyIndexMap.ContainsKey(key);
        }
        public void Clear()
        {
            m_Keys.Clear();
            m_Values.Clear();
            m_KeyIndexMap.Clear();
            m_KeyCountMap.Clear();
        }
    }
}
