/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-04-15 16:22:00
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public abstract class EZProjectSettingsSingleton<T> : ScriptableObject
        where T : EZProjectSettingsSingleton<T>
    {
        public static readonly string assetPath = $"ProjectSettings/EZhex1991/{typeof(T).Name}.asset";

        private static T m_Instance;
        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = CreateInstance<T>();
                    m_Instance.hideFlags = HideFlags.DontSave;
                    m_Instance.Load();
                }
                return m_Instance;
            }
        }

        public void Load()
        {
            try
            {
                string dataString = File.ReadAllText(assetPath);
                EditorJsonUtility.FromJsonOverwrite(dataString, m_Instance);
            }
            catch (Exception ex)
            {
                Debug.Log("Creating new asset file on " + assetPath + "\n" + ex.Message);
                Save();
            }
        }
        public void Save()
        {
            if (m_Instance == null)
            {
                Debug.Log(typeof(T) + " instance not exist");
                return;
            }
            File.WriteAllText(assetPath, EditorJsonUtility.ToJson(m_Instance, true));
        }
    }
}
