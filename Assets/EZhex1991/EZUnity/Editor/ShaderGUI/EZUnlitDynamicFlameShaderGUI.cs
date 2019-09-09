/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-03-05 11:12:20
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;

namespace EZhex1991.EZUnity
{
    public class EZUnlitDynamicFlameShaderGUI : EZShaderGUI
    {
        private enum FlameMode
        {
            Plane,
            Volume,
        }

        private MaterialProperty _FlameTex;
        private MaterialProperty _FlameNoise1;
        private MaterialProperty _FlameNoise2;
        private MaterialProperty _FlameFactor1;
        private MaterialProperty _FlameFactor2;
        private MaterialProperty _FlameColor1;
        private MaterialProperty _FlameColor2;

        private MaterialProperty _FlameMode;
        private MaterialProperty _AlphaFactor;
        private MaterialProperty _ShapeFactor;

        public override void OnEZShaderGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            _FlameTex = FindProperty("_FlameTex", properties);
            _FlameNoise1 = FindProperty("_FlameNoise1", properties);
            _FlameNoise2 = FindProperty("_FlameNoise2", properties);
            _FlameFactor1 = FindProperty("_FlameFactor1", properties);
            _FlameFactor2 = FindProperty("_FlameFactor2", properties);
            _FlameColor1 = FindProperty("_FlameColor1", properties);
            _FlameColor2 = FindProperty("_FlameColor2", properties);

            materialEditor.ShaderProperty(_FlameTex);
            materialEditor.ShaderProperty(_FlameNoise1);
            materialEditor.ShaderProperty(_FlameNoise2);
            materialEditor.ShaderProperty(_FlameFactor1);
            materialEditor.ShaderProperty(_FlameFactor2);
            materialEditor.ShaderProperty(_FlameColor1);
            materialEditor.ShaderProperty(_FlameColor2);

            _FlameMode = FindProperty("_FlameMode", properties);
            _AlphaFactor = FindProperty("_AlphaFactor", properties);
            _ShapeFactor = FindProperty("_ShapeFactor", properties);

            materialEditor.ShaderProperty(_FlameMode);
            materialEditor.ShaderProperty(_AlphaFactor);
            switch ((FlameMode)_FlameMode.floatValue)
            {
                case FlameMode.Plane:
                    break;
                case FlameMode.Volume:
                    materialEditor.ShaderProperty(_ShapeFactor);
                    break;
            }

            AdvancedOptionsGUI(materialEditor);
        }
    }
}
