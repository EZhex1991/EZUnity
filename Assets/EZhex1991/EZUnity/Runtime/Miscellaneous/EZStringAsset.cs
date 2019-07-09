/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-05-30 21:08:15
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CreateAssetMenu(fileName = "EZStringAsset", menuName = "EZUnity/EZStringAsset", order = (int)EZAssetMenuOrder.EZStringAsset)]
    public class EZStringAsset : ScriptableObject, ISerializationCallbackReceiver, IEnumerable<KeyValuePair<string, EZStringAsset.Item>>
    {
        public static Language GlobalLanguageSetting = Language.Chinese;

        public enum Language
        {
            Chinese,
            English,
        }

        [System.Serializable]
        public struct Item
        {
            [SerializeField]
            private string m_Key;
            public string key { get { return m_Key; } set { m_Key = value; } }
            [SerializeField, TextArea]
            private string m_CH;
            public string CH { get { return m_CH; } }
            [SerializeField, TextArea]
            private string m_EN;
            public string EN { get { return m_EN; } }
        }

        [SerializeField]
        private List<Item> m_Items = new List<Item>();

        private Dictionary<string, Item> m_Dictionary = new Dictionary<string, Item>();
        private Dictionary<string, int> m_KeyCount = new Dictionary<string, int>();

        public bool IsKeyDuplicate(string key)
        {
            return m_KeyCount.ContainsKey(key) && m_KeyCount[key] > 1;
        }
        public void AddKey(string key)
        {
            m_Items.Add(new Item { key = key });
        }

        public void OnBeforeSerialize()
        {

        }
        public void OnAfterDeserialize()
        {
            m_Dictionary.Clear();
            m_KeyCount.Clear();
            foreach (var item in m_Items)
            {
                if (m_KeyCount.ContainsKey(item.key))
                {
                    m_KeyCount[item.key]++;
                }
                else
                {
                    m_KeyCount[item.key] = 1;
                    m_Dictionary[item.key] = item;
                }
            }
        }

        public bool ContainsKey(string key)
        {
            return m_Dictionary.ContainsKey(key);
        }
        public bool TryGetValue(string key, out Item item)
        {
            return m_Dictionary.TryGetValue(key, out item);
        }

        IEnumerator<KeyValuePair<string, Item>> IEnumerable<KeyValuePair<string, Item>>.GetEnumerator()
        {
            return m_Dictionary.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_Dictionary.GetEnumerator();
        }

        public string GetString(string key)
        {
            return GetString(key, GlobalLanguageSetting);
        }
        public string GetString(int index)
        {
            return GetString(index, GlobalLanguageSetting);
        }
        public string GetString(string key, Language language)
        {
            switch (language)
            {
                case Language.Chinese: return m_Dictionary[key].CH;
                case Language.English: return m_Dictionary[key].EN;
                default: return m_Dictionary[key].EN;
            }
        }
        public string GetString(int index, Language language)
        {
            switch (language)
            {
                case Language.Chinese: return m_Items[index].CH;
                case Language.English: return m_Items[index].EN;
                default: return m_Items[index].EN;
            }
        }
    }
}
