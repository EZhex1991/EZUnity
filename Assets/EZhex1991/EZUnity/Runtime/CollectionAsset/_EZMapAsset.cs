/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-07-11 11:55:16
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEngine;

namespace EZhex1991.EZUnity.EZCollectionAsset
{
    /// <summary>
    /// Base class for EZMapAssetEditor_String
    /// </summary>
    public class EZMapAsset_String_ : EZListAsset<string>
    {
    }

    public class EZMapAsset_String_<TValue> : EZMapAsset_String_
    {
        [SerializeField]
        protected List<TValue> m_Values = new List<TValue>();
        public List<TValue> values { get { return m_Values; } }

        public TValue this[string key] { get { return m_Values[m_KeyIndexMap[key]]; } set { m_Values[m_KeyIndexMap[key]] = value; } }
        public TValue this[int index] { get { return m_Values[index]; } set { m_Values[index] = value; } }

        public override void Add(string key)
        {
            base.Add(key);
            m_Values.Add(default(TValue));
        }
        public void Add(string key, TValue value)
        {
            base.Add(key);
            m_Values.Add(value);
        }

        public override bool Insert(int index, string key)
        {
            if (base.Insert(index, key))
            {
                m_Values.Insert(index, default(TValue));
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Insert(int index, string key, TValue value)
        {
            if (base.Insert(index, key))
            {
                m_Values.Insert(index, value);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryGetValue(string key, out TValue value)
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
    }
}
