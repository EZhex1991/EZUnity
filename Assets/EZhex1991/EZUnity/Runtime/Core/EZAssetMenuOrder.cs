/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-04-12 15:57:09
 * Organization:    #ORGANIZATION#
 * Description:     
 */
namespace EZhex1991.EZUnity
{
    public enum EZAssetMenuOrder
    {
        _Section_1 = 1100,
        EZListAsset_String,
        EZListAsset_Object,
        EZMapAsset_String_String,
        EZMapAsset_String_Int,
        EZMapAsset_String_Object,
        EZMapAsset_String_TextCollection,

        _Section_2 = 1200,
        EZPlaneGenerator,
        EZTextureChannelModifier,
        EZTextureCombiner,
        EZGradientGenerator,
        EZNoiseGenerator,
        EZPerlinNoiseGenerator,

        _Section_3 = 1300,
        EZImageCapture,

        _Section_4 = 1400,
        EZBundleBuilder,
        EZPlayerBuilder,

        _Section_5 = 1500,
        EZScriptStatistics,

        _Section_6 = 1600,
        EZPhysicsBoneMaterial,

        _Section_7 = 1700,
        EZAssetListRenamer,
    }
}
