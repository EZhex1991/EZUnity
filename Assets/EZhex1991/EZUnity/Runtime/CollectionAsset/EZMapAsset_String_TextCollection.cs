/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-05-30 21:08:15
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity.EZCollectionAsset
{
    [CreateAssetMenu(fileName = "EZMapAsset String-TextCollection", menuName = "EZUnity/EZMapAsset/String-TextCollection", order = (int)EZAssetMenuOrder.EZMapAsset_String_TextCollection)]
    public class EZMapAsset_String_TextCollection : EZMapAsset<string, EZMapAsset_String_TextCollection.TextCollection>
    {
        public static Language GlobalLanguageSetting = Language.Chinese;

        public enum Language
        {
            Chinese,
            English,
        }

        [System.Serializable]
        public struct TextCollection
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
            return GetString(m_KeyIndexMap[key], language);
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
