/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-04-12 15:57:09
 * Organization:    #ORGANIZATION#
 * Description:     
 */
namespace EZUnity
{
    public class EZUnityMenuOrder
    {
        private const int AssetOrder = 1000;

        public const int EZStringAsset = AssetOrder + 100;
        public const int EZPhysicsBoneMaterial = AssetOrder + 101;

        #region Editor Only
        public const int EZPlaneGenerator = AssetOrder + 200;
        public const int EZTextureChannelModifier = AssetOrder + 201;
        public const int EZGradientGenerator = AssetOrder + 202;

        public const int EZImageCapture = AssetOrder + 301;

        public const int EZBundleBuilder = AssetOrder + 401;
        public const int EZPlayerBuilder = AssetOrder + 402;

        public const int EZScriptStatistics = AssetOrder + 501;
        #endregion
    }
}
