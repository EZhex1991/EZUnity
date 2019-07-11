/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-07-11 19:36:24
 * Organization:    #ORGANIZATION#
 * Description:     
 */
namespace EZhex1991.EZUnity
{
    public enum EZMenuItemOrder
    {
        /// <summary>
        /// Default MenuItem Order
        /// </summary>
        _Section_0 = 10000,

        /// <summary>
        /// Application
        /// </summary>
        _Section_1 = 11000,
        ApplicationSettings,
        OpenPersistentFolder,
        ClearPersistentFolder,
        PlayerPrefsEditor,

        /// <summary>
        /// Asset Manager
        /// </summary>
        _Section_2 = 12000,
        SaveAssets,
        RefreshAssetDatabase,
        Renamer,

        /// <summary>
        /// Viewer
        /// </summary>
        _Section_3 = 13000,
        AssetBundleManager,
        AssetReferenceViewer,
        CorrespondingObjectViewer,
        FontReferenceViewer,

        /// <summary>
        /// Helper
        /// </summary>
        _Section_4 = 14000,
        GuidGenerator,
        RegexTester,
        ColorBlender,
        TimePanel,

        _Section_5 = 15000,

        _Section_6 = 16000,

        _Section_7 = 17000,

        /// <summary>
        /// XLuaExtension
        /// </summary>
        _Section_8 = 18000,
        LuaToTxt,
        ClearLuaTextFolder,

        /// <summary>
        /// Experimental
        /// </summary>
        _Section_9 = 19000,
        ShaderKeywordManager,
    }
}
