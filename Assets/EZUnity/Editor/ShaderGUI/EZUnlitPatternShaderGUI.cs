/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-02-25 19:52:09
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZUnity;
using UnityEditor;
using UnityEngine;

public class EZUnlitPatternShaderGUI : EZShaderGUI
{
    public enum PatternType
    {
        Chessboard,
        Diamond,
        Frame,
        Spot,
        Stripe,
        Triangle,
        Wave,
    }

    private MaterialProperty _PatternType;
    private MaterialProperty _CoordMode;
    private MaterialProperty _SecondColor;
    private MaterialProperty _DensityFactor;
    private MaterialProperty _FillRatio;

    public override void OnEZShaderGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        MainTextureWithColorGUI(materialEditor, properties);

        _PatternType = FindProperty("_PatternType", properties);
        _CoordMode = FindProperty("_CoordMode", properties);
        _SecondColor = FindProperty("_SecondColor", properties);
        _DensityFactor = FindProperty("_DensityFactor", properties);
        _FillRatio = FindProperty("_FillRatio", properties);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Pattern", EditorStyles.boldLabel);
        EditorGUI.BeginChangeCheck();
        materialEditor.ShaderProperty(_PatternType);
        materialEditor.ShaderProperty(_CoordMode);
        materialEditor.ShaderProperty(_SecondColor);
        materialEditor.ShaderProperty(_DensityFactor);
        switch ((PatternType)_PatternType.floatValue)
        {
            case PatternType.Frame:
                materialEditor.ShaderProperty(_FillRatio);
                break;
            case PatternType.Spot:
                materialEditor.ShaderProperty(_FillRatio);
                break;
            case PatternType.Stripe:
                materialEditor.ShaderProperty(_FillRatio);
                break;
        }

        AdvancedOptionsGUI(materialEditor);
    }
}
