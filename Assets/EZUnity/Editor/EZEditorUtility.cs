/* Author:          熊哲
 * CreateTime:      #CREATETIME#
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EZUnity
{
    public static partial class EZEditorUtility
    {
        public static IEnumerable<Type> GetAllTypes(bool excludeGeneric = true)
        {
            List<Type> types = new List<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !(a.ManifestModule is System.Reflection.Emit.ModuleBuilder));
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