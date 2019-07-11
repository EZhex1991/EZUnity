/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-04-12 15:57:09
 * Organization:    #ORGANIZATION#
 * Description:     
 */
namespace EZhex1991.EZUnity
{
    public enum EZAssetMenuOrder
    {
        /// <summary>
        /// Default Order
        /// </summary>
        _Section_0 = 1000,

        /// <summary>
        /// Collection Asset
        /// </summary>
        _Section_1 = 1100,
        EZListAsset_String,
        EZListAsset_Object,
        EZMapAsset_String_String,
        EZMapAsset_String_Int,
        EZMapAsset_String_Object,
        EZMapAsset_String_TextCollection,

        /// <summary>
        /// Asset Generator
        /// </summary>
        _Section_2 = 1200,
        EZPlaneGenerator,
        EZTextureChannelModifier,
        EZTextureCombiner,
        EZGradientGenerator,
        EZNoiseGenerator,
        EZPerlinNoiseGenerator,

        /// <summary>
        /// Tools
        /// </summary>
        _Section_3 = 1300,
        EZImageCapture,
        EZScriptStatistics,

        /// <summary>
        /// Builder
        /// </summary>
        _Section_4 = 1400,
        EZBundleBuilder,
        EZPlayerBuilder,

        /// <summary>
        /// Asset Manager
        /// </summary>
        _Section_5 = 1500,
        EZAssetListRenamer,

        _Section_6 = 1600,

        _Section_7 = 1700,
    }
}
