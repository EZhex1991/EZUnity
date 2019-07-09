/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-05-30 21:08:15
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CreateAssetMenu(fileName = "EZStringCollection", menuName = "EZUnity/EZStringCollection", order = (int)EZAssetMenuOrder.EZStringCollection)]
    public class EZStringCollectionAsset : EZDictionaryAsset<string, EZStringCollectionAsset.StringCollection>
    {
        public static Language GlobalLanguageSetting = Language.Chinese;

        public enum Language
        {
            Chinese,
            English,
        }

        [System.Serializable]
        public struct StringCollection
        {
            [SerializeField, TextArea]
            private string m_CH;
            public string CH { get { return m_CH; } }
            [SerializeField, TextArea]
            private string m_EN;
            public string EN { get { return m_EN; } }
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
            return GetString(m_Dictionary[key], language);
        }
        public string GetString(int index, Language language)
        {
            switch (language)
            {
                case Language.Chinese: return m_Values[index].CH;
                case Language.English: return m_Values[index].EN;
                default: return m_Values[index].EN;
            }
        }
    }
}
