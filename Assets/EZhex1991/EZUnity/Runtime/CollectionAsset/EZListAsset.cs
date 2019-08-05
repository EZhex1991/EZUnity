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
    public class EZListAsset : ScriptableObject { }

    public class EZListAsset<TKey> : EZListAsset
    {
        [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("m_Keys")]
        protected List<TKey> m_Elements = new List<TKey>();
        public List<TKey> elements { get { return m_Elements; } }

        public int Count { get { return m_Elements.Count; } }

        public TKey this[int index] { get { return m_Elements[index]; } set { m_Elements[index] = value; } }

        public void Add(TKey key)
        {
            m_Elements.Add(key);
        }
        public bool Insert(int index, TKey key)
        {
            try
            {
                m_Elements.Insert(index, key);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }
    }
}
