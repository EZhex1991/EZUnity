/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-04-15 16:22:00
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EZUnity
{
    public abstract class EZScriptableSingleton<T> : ScriptableObject
        where T : EZScriptableSingleton<T>
    {
        public abstract string assetPath { get; }

        private static T m_Instance;
        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = CreateInstance<T>();
                    m_Instance.Load();
                }
                return m_Instance;
            }
        }

        internal EZScriptableSingleton()
        {
            if (m_Instance != null)
                Debug.LogError(typeof(T) + " already exists!");
        }

        //[SettingsProvider]
        private static SettingsProvider CreateSettingsProvider()
        {
            Instance.Load();
            AssetSettingsProvider provider = AssetSettingsProvider.CreateProviderFromObject("Project/" + typeof(T).Name, Instance);
            provider.activateHandler += (searchContext, rootElement) =>
            {
                if (provider.settingsEditor != null &&
                    provider.settingsEditor.serializedObject.UpdateIfRequiredOrScript())
                {
                    provider.Repaint();
                }
            };
            return provider;
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
            File.WriteAllText(assetPath, EditorJsonUtility.ToJson(m_Instance));
        }
    }
}
