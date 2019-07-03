/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-11-29 17:17:10
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [Serializable]
    public class EZDictionary<TKey, TValue> : ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<TKey> m_Keys = new List<TKey>();
        [SerializeField]
        private List<TValue> m_Values = new List<TValue>();

        private Dictionary<TKey, TValue> m_Dictionary = new Dictionary<TKey, TValue>();
        public Dictionary<TKey, TValue> dictionary { get { return m_Dictionary; } }

        public void OnBeforeSerialize()
        {
            m_Keys.Clear();
            m_Values.Clear();
            foreach (var pair in m_Dictionary)
            {
                m_Keys.Add(pair.Key);
                m_Values.Add(pair.Value);
            }
        }
        public void OnAfterDeserialize()
        {
            m_Dictionary.Clear();
            for (int i = 0; i < m_Keys.Count; i++)
            {
                m_Dictionary.Add(m_Keys[i], m_Values[i]);
            }
        }

        public TValue this[TKey key]
        {
            get { return m_Dictionary[key]; }
            set { m_Dictionary[key] = value; }
        }
    }
}
