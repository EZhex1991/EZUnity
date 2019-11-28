/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-11-28 15:39:16
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public static class MaterialExt
    {
        public static string FormatKeyword(string prefix, Enum selection)
        {
            return string.Format("{0}_{1}", prefix, selection).ToUpper();
        }

        public static void DisableKeywords(this Material mat, string prefix, Type enumType)
        {
            foreach (Enum value in Enum.GetValues(enumType))
            {
                mat.DisableKeyword(FormatKeyword(prefix, value));
            }
        }
        public static void SetKeyword(this Material mat, string prefix, Enum selection)
        {
            foreach (Enum value in Enum.GetValues(selection.GetType()))
            {
                mat.DisableKeyword(FormatKeyword(prefix, value));
            }
            mat.EnableKeyword(FormatKeyword(prefix, selection));
        }
        public static void SetKeyword(this Material mat, string keyword, bool value)
        {
            if (value) { mat.EnableKeyword(keyword); }
            else mat.DisableKeyword(keyword);
        }
        public static bool IsKeywordEnabled(this Material mat, string prefix, Enum selection)
        {
            return mat.IsKeywordEnabled(FormatKeyword(prefix, selection));
        }
    }
}
