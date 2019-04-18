/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-04-12 15:57:09
 * Organization:    #ORGANIZATION#
 * Description:     
 */
namespace EZUnity
{
    public class EZAssetMenuOrder
    {
        private const int BaseOrder = 1000;

        public const int EZStringAsset = BaseOrder + 100;
        public const int EZPhysicsBoneMaterial = BaseOrder + 101;

        #region Editor Only
        public const int EZPlaneGenerator = BaseOrder + 200;
        public const int EZTextureChannelModifier = BaseOrder + 201;
        public const int EZGradientGenerator = BaseOrder + 202;

        public const int EZImageCapture = BaseOrder + 301;

        public const int EZBundleBuilder = BaseOrder + 401;
        public const int EZPlayerBuilder = BaseOrder + 402;

        public const int EZScriptStatistics = BaseOrder + 501;
        #endregion
    }
}
