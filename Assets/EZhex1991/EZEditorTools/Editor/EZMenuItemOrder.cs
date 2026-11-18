/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-07-11 19:36:24
 * Organization:    #ORGANIZATION#
 * Description:     
 */
namespace EZhex1991.EZEditorTools
{
    public enum EZMenuItemOrder
    {
        SECTION_0_RENAMING = 10000,
        Renamer,

        SECTION_1_OBJECT_VIEWER = SECTION_0_RENAMING + 100,
        AssetReferenceViewer,
        CorrespondingObjectViewer,
        FontReferenceViewer,
        HierachyDiffChecker,
        PropertyPathViewer,
        AnimatorControllerCombiner,

        SECTION_2_HELPER = SECTION_0_RENAMING + 200,
        GuidGenerator,
        RegexTester,
        ColorBlender,
        PlayerPrefsEditor,
        TypeReflectionHelper,

        SECTION_3_ANALYSIS = SECTION_0_RENAMING + 300,
        ScriptStatistics,
    }
}

