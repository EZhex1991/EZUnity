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
        /// Collection Asset
        /// </summary>
        _Section_0 = 0,
        EZListAsset_String,
        EZListAsset_Object,
        EZMapAsset_String_String,
        EZMapAsset_String_Int,
        EZMapAsset_String_Object,
        EZMapAsset_String_TextCollection,

        /// <summary>
        /// Asset Generator
        /// </summary>
        _Section_1 = _Section_0 + 100,
        EZPlaneGenerator,
        EZGradientTextureGenerator,
        EZWaveTextureGenerator,
        EZColorLerpTextureGenerator,
        EZNoiseTextureGenerator,
        EZPerlinNoiseTextureGenerator,
        EZTextureChannelModifier,
        EZTextureCombiner,
        EZTextureAntialiasing,

        /// <summary>
        /// Tools
        /// </summary>
        _Section_2 = _Section_0 + 200,
        EZImageCapture,
        EZScriptStatistics,

        /// <summary>
        /// Builder
        /// </summary>
        _Section_3 = _Section_0 + 300,
        EZBundleBuilder,
        EZPlayerBuilder,

        /// <summary>
        /// Asset Manager
        /// </summary>
        _Section_4 = _Section_0 + 400,
        EZAssetListRenamer,

        _Section_5 = _Section_0 + 500,

        _Section_6 = _Section_0 + 600,

        _Section_7 = _Section_0 + 700,

        _Section_8 = _Section_0 + 800,

        _Section_9 = _Section_0 + 900,
    }
}
