/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-05-30 21:08:15
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEngine;

namespace EZUnity
{
    [CreateAssetMenu(fileName = "EZStringAsset", menuName = "EZUnity/EZStringAsset", order = EZUnityMenuOrder.EZStringAsset)]
    public class EZStringAsset : ScriptableObject, ISerializationCallbackReceiver
    {
        public static Language GlobalLanguageSetting = Language.Chinese;

        public enum Language
        {
            Chinese,
            English,
        }

        [System.Serializable]
        public class Values
        {
            public string key;
            [TextArea]
            public string ch;
            [TextArea]
            public string en;
        }

        [SerializeField]
        private Values[] m_Items;

        private Dictionary<string, Values> dict = new Dictionary<string, Values>();

        public void OnBeforeSerialize()
        {

        }
        public void OnAfterDeserialize()
        {
            dict.Clear();
            for (int i = 0; i < m_Items.Length; i++)
            {
                if (!dict.ContainsKey(m_Items[i].key))
                {
                    dict.Add(m_Items[i].key, m_Items[i]);
                }
            }
        }

        public string this[string key]
        {
            get
            {
                return GetString(key);
            }
        }
        public string this[int index]
        {
            get
            {
                return GetString(index);
            }
        }

        public bool Contains(string key)
        {
            return dict.ContainsKey(key);
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
                case Language.Chinese: return dict[key].ch;
                case Language.English: return dict[key].en;
                default: return dict[key].ch;
            }
        }
        public string GetString(int index, Language language)
        {
            switch (language)
            {
                case Language.Chinese: return m_Items[index].ch;
                case Language.English: return m_Items[index].en;
                default: return m_Items[index].ch;
            }
        }

        public Values GetStrings(string key)
        {
            return dict[key];
        }
        public Values GetStrings(int index)
        {
            return m_Items[index];
        }
    }
}
