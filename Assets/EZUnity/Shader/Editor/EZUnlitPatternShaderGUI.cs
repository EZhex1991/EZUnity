/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-02-25 19:52:09
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZUnity;
using System;
using UnityEditor;
using UnityEngine;

public class EZUnlitPatternShaderGUI : ShaderGUI
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

    private MaterialProperty _MainTex;
    private MaterialProperty _Color;

    private MaterialProperty _CoordMode;
    private MaterialProperty _DensityFactor;
    private MaterialProperty _PatternType;
    private MaterialProperty _SecondColor;
    private MaterialProperty _FillRatio;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        _MainTex = FindProperty("_MainTex", properties);
        _Color = FindProperty("_Color", properties);

        _CoordMode = FindProperty("_CoordMode", properties);
        _DensityFactor = FindProperty("_DensityFactor", properties);
        _PatternType = FindProperty("_PatternType", properties);
        _SecondColor = FindProperty("_SecondColor", properties);
        _FillRatio = FindProperty("_FillRatio", properties);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Base", EditorStyles.boldLabel);
        materialEditor.ShaderProperty(_MainTex, "Main Texture");
        materialEditor.ShaderProperty(_Color, "Color");

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Pattern", EditorStyles.boldLabel);
        EditorGUI.BeginChangeCheck();
        EZShaderGUIUtility.DrawEnumPopup<CoordMode>(materialEditor, _CoordMode, "Coordinate Mode", (mat, selection) => EZShaderGUIUtility.SetKeyword(mat, (CoordMode)selection));
        materialEditor.ShaderProperty(_DensityFactor, "Density Factor");
        EZShaderGUIUtility.DrawEnumPopup<PatternType>(materialEditor, _PatternType, "Pattern Type", (mat, selection) => EZShaderGUIUtility.SetKeyword(mat, (PatternType)selection));
        materialEditor.ShaderProperty(_SecondColor, "Second Color");
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
                materialEditor.ShaderProperty(_FillRatio, "Fill Ratio");
                break;
        }

        EditorGUILayout.Space();
        materialEditor.RenderQueueField();
        materialEditor.DoubleSidedGIField();
    }
}
