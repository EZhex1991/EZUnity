/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-07-11 19:36:24
 * Organization:    #ORGANIZATION#
 * Description:     
 */
namespace EZhex1991.EZEditorTools
{
    public enum EZMenuItemOrder
    {
        SECTION_0_PROJECT_SETTINGS = 10000,
        ProjectSettings,

        SECTION_1_APPLICATION_SETTINGS = SECTION_0_PROJECT_SETTINGS + 100,
        OpenPersistentFolder,
        ClearPersistentFolder,
        PlayerPrefsEditor,

        SECTION_2_ASSET_OPERATIONS = SECTION_0_PROJECT_SETTINGS + 200,
        SaveAssets,
        RefreshAssetDatabase,
        Renamer,

        SECTION_3_OBJECT_VIEWER = SECTION_0_PROJECT_SETTINGS + 300,
        AssetBundleManager,
        BundleBuilder,
        PlayerBuilder,
        AssetReferenceViewer,
        CorrespondingObjectViewer,
        FontReferenceViewer,
        MaterialOptimizer,
        MaterialMap,
        HierachyDiffChecker,
        PropertyPathViewer,
        AnimatorControllerCombiner,

        SECTION_4_HELPER = SECTION_0_PROJECT_SETTINGS + 400,
        GuidGenerator,
        RegexTester,
        ColorBlender,
        TimePanel,
        TypeReflectionHelper,

        SECTION_5_ANALYSIS = SECTION_0_PROJECT_SETTINGS + 500,
        ScriptStatistics,

        SECTION_6_IMAGE_CAPTURE = SECTION_0_PROJECT_SETTINGS + 600,
        ImageCapture,

        SECTION_7_SHADER = SECTION_0_PROJECT_SETTINGS + 700,

        SECTION_8_GENERAL = SECTION_0_PROJECT_SETTINGS + 800,

        SECTION_9_EXPERIMENTAL = SECTION_0_PROJECT_SETTINGS + 900,
        ShaderKeywordManager,
    }
}

