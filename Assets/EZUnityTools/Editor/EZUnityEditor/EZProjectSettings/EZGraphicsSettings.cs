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
            "Standard",
            "Standard (Specular setup)",
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
            List<Shader> shaders = new List<Shader>();
            foreach (Object asset in AssetDatabase.LoadAllAssetsAtPath("Resources/unity_builtin_extra")
                .Where(obj => obj is Shader)
                .Where(obj => !obj.name.StartsWith("Hidden") && !obj.name.StartsWith("Legacy Shaders") && !obj.name.StartsWith("VR") && !obj.name.StartsWith("Nature"))
                .Where(obj => !blackList.Contains(obj.name)))
            {
                shaders.Add(asset as Shader);
            }
            shaders.Sort((s1, s2) => { return string.Compare(s1.name, s2.name); });
            for (int i = 0; i < m_AlwaysIncludedShaders.arraySize; i++)
            {
                Shader shader = m_AlwaysIncludedShaders.GetArrayElementAtIndex(i).objectReferenceValue as Shader;
                if (shader == null || shaders.Contains(shader)) continue;
                shaders.Add(shader);
            }

            m_AlwaysIncludedShaders.ClearArray();
            m_AlwaysIncludedShaders.arraySize = shaders.Count;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < shaders.Count; i++)
            {
                Shader shader = shaders[i];
                sb.AppendLine("\"" + shader.name + "\",");
                m_AlwaysIncludedShaders.GetArrayElementAtIndex(i).objectReferenceValue = shader;
            }
            serializedObject.ApplyModifiedProperties();
            Debug.Log(shaders.Count + " Shaders included:\n" + sb.ToString());
            Selection.activeObject = target;
        }
    }
}