/* Author:          #AUTHORNAME#
 * CreateTime:      #CREATETIME#
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace EZUnityEditor
{
    // 自定义Asset的存放路径(需要使用AssetDatabase加载，因此必须相对于工程路径)
    public class AssetPathAttribute : Attribute
    {

    }

    public static class EZEditorUtility
    {
        public static string assetDirPath = "Assets/EZUnityTools/EZAssets/";

        static EZEditorUtility()
        {
            IEnumerable<Type> types = GetStaticTypes();
            BindingFlags flags = BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic;
            foreach (FieldInfo field in GetFields(types, flags))
            {
                if (field.FieldType == typeof(string) && field.IsDefined(typeof(AssetPathAttribute), false))
                    assetDirPath = field.GetValue(null) as string;
            }
            foreach (PropertyInfo property in GetProperties(types, flags))
            {
                if (property.PropertyType == typeof(string) && property.IsDefined(typeof(AssetPathAttribute), false))
                    assetDirPath = property.GetValue(null, null) as string;
            }
            if (!assetDirPath.EndsWith("/") && !assetDirPath.EndsWith("\\")) assetDirPath = assetDirPath + "/";
            if (!Directory.Exists(assetDirPath)) { Directory.CreateDirectory(assetDirPath); }
        }

        public static IEnumerable<Type> GetAllTypes(bool excludeGeneric = true)
        {
            List<Type> types = new List<Type>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
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
    }
}