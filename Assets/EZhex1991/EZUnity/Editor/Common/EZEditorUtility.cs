/* Author:          ezhex1991@outlook.com
 * CreateTime:      #CREATETIME#
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace EZhex1991.EZUnity
{
    public static partial class EZEditorUtility
    {
        public static IEnumerable<Type> GetAllTypes(bool excludeGeneric = true)
        {
            List<Type> types = new List<Type>();
#if UNITY_2018_1_OR_NEWER && NET_4_6
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where((assembly) => !assembly.IsDynamic);
#else
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
#endif
            foreach (Assembly assembly in assemblies)
            {
                types.AddRange((from type in assembly.GetExportedTypes()
                                where excludeGeneric ? !type.IsGenericType : true
                                select type));
            }
            return types;
        }
        public static IEnumerable<Type> GetStaticTypes()
        {
            return (from type in GetAllTypes()
                    where type.IsAbstract && type.IsSealed
                    select type);
        }
        public static IEnumerable<FieldInfo> GetFields(IEnumerable<Type> types, BindingFlags flags)
        {
            List<FieldInfo> fields = new List<FieldInfo>();
            foreach (Type type in types)
            {
                fields.AddRange(type.GetFields(flags));
            }
            return fields;
        }
        public static IEnumerable<PropertyInfo> GetProperties(IEnumerable<Type> types, BindingFlags flags)
        {
            List<PropertyInfo> properties = new List<PropertyInfo>();
            foreach (Type type in types)
            {
                properties.AddRange(type.GetProperties(flags));
            }
            return properties;
        }

        private static Material m_DefaultMaterial;
        public static Material defaultMaterial
        {
            get
            {
                if (m_DefaultMaterial == null)
                    m_DefaultMaterial = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Material.mat");
                return m_DefaultMaterial;
            }
        }

#if UNITY_2018_1_OR_NEWER
        public static List<Shader> GetAllShaders(bool supportedOnly)
        {
            List<Shader> shaders = new List<Shader>();
            ShaderInfo[] shaderInfos = ShaderUtil.GetAllShaderInfo();
            for (int i = 0; i < shaderInfos.Length; i++)
            {
                ShaderInfo info = shaderInfos[i];
                if (supportedOnly && !info.supported) continue;
                shaders.Add(Shader.Find(info.name));
            }
            return shaders;
        }
#endif

        private static List<Shader> m_BuiltinShaders;
        public static List<Shader> builtinShaders
        {
            get
            {
                if (m_BuiltinShaders == null)
                {
                    m_BuiltinShaders = new List<Shader>();
                    foreach (UObject asset in AssetDatabase.LoadAllAssetsAtPath("Resources/unity_builtin_extra")
                                                .Where(obj => { Debug.Log(obj.GetType()); return obj is Shader; }))
                    {
                        m_BuiltinShaders.Add(asset as Shader);
                    }
                    m_BuiltinShaders.Sort((s1, s2) => string.Compare(s1.name, s2.name));
                }
                return m_BuiltinShaders;
            }
        }
    }
}