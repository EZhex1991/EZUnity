/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-01-03 16:28:23
 * Organization:    #ORGANIZATION#
 * Description:     
 */
#if UNITYADDRESSABLES
using System;
using System.Collections.Generic;
using XLua;

namespace EZhex1991.EZUnity.Example
{
    public static class GenListForUnityAddressables
    {
        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp = new List<Type>() {
            typeof(UnityEngine.AddressableAssets.Addressables),
            typeof(UnityEngine.AddressableAssets.AssetLabelReference),
            typeof(UnityEngine.AddressableAssets.AssetReference),
            typeof(UnityEngine.AddressableAssets.AssetReferenceGameObject),
            typeof(UnityEngine.AddressableAssets.AssetReferenceSprite),
            typeof(UnityEngine.AddressableAssets.AssetReferenceTexture),

            typeof(UnityEngine.ResourceManagement.ResourceManager),
        };
    }
}
#endif
