/* Author:          ezhex1991@outlook.com
 * CreateTime:      2026-11-18
 * Organization:    #ORGANIZATION#
 * Description:     Menu order definitions for EZUnity editor items.
 */
namespace EZhex1991.EZUnity
{
    internal enum EZUnityMenuItemOrder
    {
        SECTION_0_PROJECT_SETTINGS = 10000,
        EZUnitySettings,

        SECTION_1_APPLICATION_SETTINGS = SECTION_0_PROJECT_SETTINGS + 100,
        ApplicationSettings,
        OpenPersistentFolder,
        ClearPersistentFolder,

        SECTION_2_ASSET_OPERATIONS = SECTION_0_PROJECT_SETTINGS + 200,
        SaveAssets,
        RefreshAssetDatabase,

        SECTION_3_BUILD_AND_MATERIAL = SECTION_0_PROJECT_SETTINGS + 300,
        AssetBundleManager,
        MaterialOptimizer,

        SECTION_4_TIME = SECTION_0_PROJECT_SETTINGS + 400,
        TimePanel,

        SECTION_9_EXPERIMENTAL = SECTION_0_PROJECT_SETTINGS + 900,
        ShaderKeywordManager,
    }
}
