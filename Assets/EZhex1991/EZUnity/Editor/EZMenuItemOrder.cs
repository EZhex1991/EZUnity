/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-07-11 19:36:24
 * Organization:    #ORGANIZATION#
 * Description:     
 */
namespace EZhex1991.EZUnity
{
    public enum EZMenuItemOrder
    {
        _Section_0 = 10000,
        ApplicationSettings,
        OpenPersistentFolder,
        ClearPersistentFolder,
        PlayerPrefsEditor,

        _Section_1 = _Section_0 + 100,
        SaveAssets,
        RefreshAssetDatabase,
        Renamer,

        _Section_2 = _Section_0 + 200,
        AssetBundleManager,
        AssetReferenceViewer,
        CorrespondingObjectViewer,
        FontReferenceViewer,
        MaterialOptimizer,
        HierachyDiffChecker,
        PropertyPathViewer,

        _Section_3 = _Section_0 + 300,
        GuidGenerator,
        RegexTester,
        ColorBlender,
        TimePanel,
        TypeReflectionHelper,

        _Section_4 = _Section_0 + 400,

        _Section_5 = _Section_0 + 500,

        _Section_6 = _Section_0 + 600,

        _Section_7 = _Section_0 + 700,

        _Section_8 = _Section_0 + 800,

        _Section_9 = _Section_0 + 900,
        ShaderKeywordManager,
    }
}
