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
        /// Default Asset Order
        /// </summary>
        _Section_0 = 10000,

        /// <summary>
        /// Collection Asset
        /// </summary>
        _Section_1 = 11000,
        EZListAsset_String,
        EZListAsset_Object,
        EZMapAsset_String_String,
        EZMapAsset_String_Int,
        EZMapAsset_String_Object,
        EZMapAsset_String_TextCollection,

        /// <summary>
        /// Asset Generator
        /// </summary>
        _Section_2 = 12000,
        EZPlaneGenerator,
        EZTextureChannelModifier,
        EZTextureCombiner,
        EZGradientGenerator,
        EZNoiseGenerator,
        EZPerlinNoiseGenerator,

        /// <summary>
        /// Tools
        /// </summary>
        _Section_3 = 13000,
        EZImageCapture,
        EZScriptStatistics,

        /// <summary>
        /// Builder
        /// </summary>
        _Section_4 = 14000,
        EZBundleBuilder,
        EZPlayerBuilder,

        /// <summary>
        /// Asset Manager
        /// </summary>
        _Section_5 = 15000,
        EZAssetListRenamer,

        _Section_6 = 16000,

        _Section_7 = 17000,

        _Section_8 = 18000,

        _Section_9 = 19000,
    }
}
