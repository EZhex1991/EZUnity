/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-04-16 17:13:26
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.IO;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public abstract class EZScriptableObjectSingleton<T> : ScriptableObject
        where T : EZScriptableObjectSingleton<T>
    {
        public const string AssetFolderPath = "Assets/Resources";

        private static T m_Instance;
        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    var assets = Resources.LoadAll<T>("");
                    if (assets.Length == 0)
                    {
                        m_Instance = CreateInstance<T>();
#if UNITY_EDITOR
                        string assetName = typeof(T).Name + ".asset";
                        if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Resources"))
                        {
                            UnityEditor.AssetDatabase.CreateFolder("Assets", "Resources");
                        }
                        UnityEditor.AssetDatabase.CreateAsset(m_Instance, Path.Combine(AssetFolderPath, assetName));
#endif
                    }
                    else
                    {
                        m_Instance = assets[0];
                    }
                }
                return m_Instance;
            }
        }
    }
}
