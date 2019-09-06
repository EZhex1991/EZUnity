/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-04-12 15:57:09
 * Organization:    #ORGANIZATION#
 * Description:     
 */
namespace EZhex1991.EZUnity
{
    public enum EZAssetMenuOrder
    {
        _Section_0 = 10000,
        EZBundleBuilder,
        EZPlayerBuilder,

        _Section_1 = _Section_0 + 100,
        EZImageCapture,
        EZScriptStatistics,

        _Section_2 = _Section_0 + 200,
        EZListAsset_String,
        EZListAsset_Object,
        EZMapAsset_String_String,
        EZMapAsset_String_Int,
        EZMapAsset_String_Object,
        EZMapAsset_String_TextCollection,

        _Section_3 = _Section_0 + 300,
        EZGaussianLutGenerator,
        EZGradient1DTextureGenerator,
        EZGradient2DTextureGenerator,
        EZPerlinNoiseTextureGenerator,
        EZPixelNoiseTextureGenerator,
        EZSimpleNoiseTextureGenerator,
        EZVoronoiTextureGenerator,
        EZWaveTextureGenerator,
        EZTextureBlurProcessor,
        EZColorBasedOutline,
        EZTextureSpherize,
        EZTextureTwirl,
        EZTextureChannelModifier,
        EZTextureCombiner,
        EZTexturePipeline,
        EZMaterialToTexture,

        _Section_4 = _Section_0 + 400,
        EZAssetListRenamer,

        _Section_5 = _Section_0 + 500,

        _Section_6 = _Section_0 + 600,

        _Section_7 = _Section_0 + 700,

        _Section_8 = _Section_0 + 800,

        _Section_9 = _Section_0 + 900,
    }
}
