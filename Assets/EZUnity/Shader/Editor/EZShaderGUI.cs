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
    protected void MainTextureWithColorGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        _MainTex = FindProperty("_MainTex", properties);
        _Color = FindProperty("_Color", properties);
        materialEditor.ShaderProperty(_MainTex);
        materialEditor.ShaderProperty(_Color);
    }

    protected MaterialProperty _BumpOn;
    protected MaterialProperty _BumpTex;
    protected MaterialProperty _Bumpiness;
    protected void FeaturedBumpGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        _BumpOn = FindProperty("_BumpOn", properties);
        _BumpTex = FindProperty("_BumpTex", properties);
        _Bumpiness = FindProperty("_Bumpiness", properties);
        EditorGUILayout.Space();
        materialEditor.FeaturedPropertiesWithTexture(_BumpOn, _BumpTex, _Bumpiness, Keyword_BumpOn);
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
