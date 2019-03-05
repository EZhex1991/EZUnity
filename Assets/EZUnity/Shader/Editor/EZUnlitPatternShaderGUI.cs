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
    public enum CoordMode
    {
        UV,
        LocalPos_XY,
        LocalPos_XZ,
        LocalPos_YZ,
        ScreenPos,
    }

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

    private MaterialProperty _CoordMode;
    private MaterialProperty _DensityFactor;
    private MaterialProperty _PatternType;
    private MaterialProperty _SecondColor;
    private MaterialProperty _FillRatio;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        MainTextureWithColorGUI(materialEditor, properties);

        _CoordMode = FindProperty("_CoordMode", properties);
        _DensityFactor = FindProperty("_DensityFactor", properties);
        _PatternType = FindProperty("_PatternType", properties);
        _SecondColor = FindProperty("_SecondColor", properties);
        _FillRatio = FindProperty("_FillRatio", properties);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Pattern", EditorStyles.boldLabel);
        EditorGUI.BeginChangeCheck();
        materialEditor.EnumPopup<CoordMode>(_CoordMode, (mat, selection) => mat.SetKeyword((CoordMode)selection));
        materialEditor.ShaderProperty(_DensityFactor);
        materialEditor.EnumPopup<PatternType>(_PatternType, (mat, selection) => mat.SetKeyword((PatternType)selection));
        materialEditor.ShaderProperty(_SecondColor);
        PatternType type = (PatternType)(int)_PatternType.floatValue;
        switch (type)
        {
            case PatternType.Chessboard:
                break;
            case PatternType.Diamond:
                break;
            case PatternType.Triangle:
                break;
            case PatternType.Wave:
                break;
            default:
                materialEditor.ShaderProperty(_FillRatio);
                break;
        }

        AdvancedOptionsGUI(materialEditor);
        Setup(materialEditor.target as Material);
    }
}
