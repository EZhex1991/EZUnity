/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-03-05 15:21:25
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZUnity;
using UnityEditor;
using UnityEngine;

public class EZShaderGUI : ShaderGUI
{
    public const string Tag_RenderType = "RenderType";
    public const string Keyword_SecondTexOn = "_SECONDTEX_ON";
    public const string Keyword_BumpOn = "_BUMP_ON";
    public const string Keyword_SpecOn = "_SPEC_ON";

    protected bool setupRequired = true;
    protected virtual void Setup(Material mat)
    {
        if (!setupRequired) return;
        setupRequired = false;
    }

    protected MaterialProperty _MainTex;
    protected MaterialProperty _Color;
    protected void MainTextureWithColorGUI(MaterialEditor materialEditor, MaterialProperty[] properties, bool scaleOffset = true)
    {
        _MainTex = FindProperty("_MainTex", properties);
        _Color = FindProperty("_Color", properties);
        EditorGUILayout.LabelField("Main", EditorStyles.boldLabel);
        materialEditor.TexturePropertySingleLine(_MainTex, _Color);
        if (scaleOffset)
        {
            materialEditor.TextureScaleOffsetProperty(_MainTex);
        }
    }

    protected MaterialProperty _SecondTex;
    protected MaterialProperty _SecondColor;
    protected void SecondTextureWithColorGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        _SecondTex = FindProperty("_SecondTex", properties);
        _SecondColor = FindProperty("_SecondColor", properties);
        materialEditor.TexturePropertyFeatured(_SecondTex, _SecondColor, Keyword_SecondTexOn, setupRequired);
    }

    protected MaterialProperty _BumpTex;
    protected MaterialProperty _Bumpiness;
    protected void BumpGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        _BumpTex = FindProperty("_BumpTex", properties);
        _Bumpiness = FindProperty("_Bumpiness", properties);
        EditorGUI.BeginChangeCheck();
        materialEditor.TexturePropertySingleLine(_BumpTex, _Bumpiness);
        if (setupRequired || EditorGUI.EndChangeCheck())
        {
            (materialEditor.target as Material).SetKeyword(Keyword_BumpOn, _BumpTex.textureValue != null && _Bumpiness.floatValue != 0);
        }
    }

    protected void AdvancedOptionsGUI(MaterialEditor materialEditor)
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Advanced Options", EditorStyles.boldLabel);
        materialEditor.RenderQueueField();
        materialEditor.EnableInstancingField();
        materialEditor.DoubleSidedGIField();
    }
}
