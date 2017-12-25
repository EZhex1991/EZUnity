/*
 * Author:      熊哲
 * CreateTime:  12/25/2017 12:55:52 PM
 * Description:
 * 
*/
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace EZUnityEditor
{
    public static class EZGraphicsSettings
    {
        public static string assetPath = "ProjectSettings/GraphicsSettings.asset";

        public static Object target;
        public static SerializedObject serializedObject;
        public static SerializedProperty m_AlwaysIncludedShaders;

        public static List<string> blackList = new List<string>()
        {
            "FX/Flare",
        };

        static EZGraphicsSettings()
        {
            target = AssetDatabase.LoadAllAssetsAtPath(assetPath)[0];
            serializedObject = new SerializedObject(target);
            m_AlwaysIncludedShaders = serializedObject.FindProperty("m_AlwaysIncludedShaders");
        }

        public static void IncludeBuiltinShaders()
        {
            serializedObject.Update();
            List<Shader> shaders = Resources.FindObjectsOfTypeAll<Shader>()
                .Where(shader => AssetDatabase.GetAssetPath(shader).StartsWith("Resources"))
                .Where(shader => !shader.name.StartsWith("Hidden"))
                .Where(shader => !shader.name.StartsWith("Legacy Shaders"))
                .Where(shader => !blackList.Contains(shader.name))
                .ToList();
            shaders.Sort((s1, s2) => { return string.Compare(s1.name, s2.name); });

            for (int i = 0; i < m_AlwaysIncludedShaders.arraySize; i++)
            {
                Shader sh = m_AlwaysIncludedShaders.GetArrayElementAtIndex(i).objectReferenceValue as Shader;
                if (sh == null || shaders.Contains(sh)) continue;
                shaders.Add(sh);
            }

            m_AlwaysIncludedShaders.ClearArray();
            m_AlwaysIncludedShaders.arraySize = shaders.Count;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < shaders.Count; i++)
            {
                Shader sh = shaders[i];
                sb.AppendLine("\"" + sh.name + "\",");
                m_AlwaysIncludedShaders.GetArrayElementAtIndex(i).objectReferenceValue = sh;
            }
            serializedObject.ApplyModifiedProperties();
            Debug.Log("Shaders included:\n" + sb.ToString());
            Selection.activeObject = target;
        }
    }
}