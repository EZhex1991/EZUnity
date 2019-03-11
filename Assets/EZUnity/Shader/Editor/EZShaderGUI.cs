/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-03-05 15:21:25
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZUnity;
using System;
using UnityEditor;
using UnityEngine;

public class EZShaderGUI : ShaderGUI
{
    public const string Tag_RenderType = "RenderType";
    public const string Keyword_SecondOn = "_SECOND_ON";
    public const string Keyword_BumpOn = "_BUMP_ON";
    public const string Keyword_SpecOn = "_SPEC_ON";
    public const string Keyword_AddLightOn = "_ADDLIGHT_ON";
    public const string Keyword_AOOn = "_AO_ON";
    public const string Property_AlphaMode = "_AlphaMode";
    public const string Property_AlphaClipThreshold = "_AlphaClipThreshold";
    public const string Property_SrcBlendMode = "_SrcBlendMode";
    public const string Property_DstBlendMode = "_DstBlendMode";
    public const string Property_ZWriteMode = "_ZWriteMode";
    public const string Property_CullMode = "_CullMode";
    public const string Property_OffsetFactor = "_OffsetFactor";
    public const string Property_OffsetUnit = "_OffsetUnit";

    public enum RenderingModePresets
    {
        Opaque,
        Cutout,
        Fade,   // Old school alpha-blending mode, fresnel does not affect amount of transparency
        Transparent // Physically plausible transparency mode, implemented as alpha pre-multiply
    }

    public enum AlphaMode
    {
        None,
        AlphaTest,
        AlphaBlend,
        AlphaPremultiply,
    }

    protected bool setupRequired = true;
    protected virtual void Setup(Material mat)
    {
        if (!setupRequired) return;
        setupRequired = false;
    }

    private MaterialProperty _MainTex;
    private MaterialProperty _Color;
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

    private MaterialProperty _SecondTex;
    private MaterialProperty _SecondColor;
    protected void SecondTextureWithColorGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        _SecondTex = FindProperty("_SecondTex", properties);
        _SecondColor = FindProperty("_SecondColor", properties);
        materialEditor.TexturePropertyFeatured(_SecondTex, _SecondColor, Keyword_SecondOn, setupRequired);
    }

    private MaterialProperty _BumpTex;
    private MaterialProperty _Bumpiness;
    protected void BumpGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        _BumpTex = FindProperty("_BumpTex", properties);
        _Bumpiness = FindProperty("_Bumpiness", properties);
        materialEditor.TexturePropertyFeatured(_BumpTex, _Bumpiness, Keyword_BumpOn, setupRequired);
    }

    private MaterialProperty _AlphaMode;
    private MaterialProperty _AlphaClipThreshold;
    private MaterialProperty _SrcBlendMode;
    private MaterialProperty _DstBlendMode;
    private MaterialProperty _ZWriteMode;
    private MaterialProperty _CullMode;
    private MaterialProperty _OffsetFactor;
    private MaterialProperty _OffsetUnit;
    public static bool RenderingModePresetsFoldout = true;
    protected void RenderingSettingsGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        _AlphaMode = FindProperty(Property_AlphaMode, properties);
        _AlphaClipThreshold = FindProperty(Property_AlphaClipThreshold, properties);
        _SrcBlendMode = FindProperty(Property_SrcBlendMode, properties);
        _DstBlendMode = FindProperty(Property_DstBlendMode, properties);
        _ZWriteMode = FindProperty(Property_ZWriteMode, properties);
        _CullMode = FindProperty(Property_CullMode, properties);
        _OffsetFactor = FindProperty(Property_OffsetFactor, properties);
        _OffsetUnit = FindProperty(Property_OffsetUnit, properties);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Rendering Settings", EditorStyles.boldLabel);
        if (RenderingModePresetsGUI(materialEditor))
        {
            EditorGUI.indentLevel++;
            materialEditor.KeywordEnum<AlphaMode>(_AlphaMode);
            if ((AlphaMode)(_AlphaMode.floatValue) == AlphaMode.AlphaTest)
            {
                materialEditor.ShaderProperty(_AlphaClipThreshold);
            }
            materialEditor.BlendMode(_SrcBlendMode);
            materialEditor.BlendMode(_DstBlendMode);
            materialEditor.Toggle(_ZWriteMode);
            EditorGUI.indentLevel--;
        }

        materialEditor.CullMode(_CullMode);
        materialEditor.ShaderProperty(_OffsetFactor);
        materialEditor.ShaderProperty(_OffsetUnit);

        if (setupRequired)
        {
            (materialEditor.target as Material).SetKeyword((AlphaMode)_AlphaMode.floatValue);
        }
    }
    protected bool RenderingModePresetsGUI(MaterialEditor materialEditor)
    {
        EditorGUILayout.BeginHorizontal();
        RenderingModePresetsFoldout = EditorGUILayout.Foldout(RenderingModePresetsFoldout, "Rendering Mode");
        foreach (var renderingMode in Enum.GetValues(typeof(RenderingModePresets)))
        {
            if (GUILayout.Button(renderingMode.ToString(), EditorStyles.miniButton))
            {
                foreach (Material mat in materialEditor.targets)
                {
                    SetupRenderingMode(mat, (RenderingModePresets)renderingMode);
                }
            }
        }
        EditorGUILayout.EndHorizontal();
        return RenderingModePresetsFoldout;
    }
    protected void SetupRenderingMode(Material material, RenderingModePresets renderMode)
    {
        switch (renderMode)
        {
            case RenderingModePresets.Opaque:
                material.SetOverrideTag(Tag_RenderType, "Opaque");
                material.SetKeyword(AlphaMode.None);
                material.SetInt(Property_AlphaMode, (int)AlphaMode.None);
                material.SetInt(Property_SrcBlendMode, (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt(Property_DstBlendMode, (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt(Property_ZWriteMode, 1);
                material.renderQueue = -1;
                break;
            case RenderingModePresets.Cutout:
                material.SetOverrideTag(Tag_RenderType, "TransparentCutout");
                material.SetKeyword(AlphaMode.AlphaTest);
                material.SetInt(Property_AlphaMode, (int)AlphaMode.AlphaTest);
                material.SetInt(Property_SrcBlendMode, (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt(Property_DstBlendMode, (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt(Property_ZWriteMode, 1);
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest;
                break;
            case RenderingModePresets.Fade:
                material.SetOverrideTag(Tag_RenderType, "Transparent");
                material.SetKeyword(AlphaMode.AlphaBlend);
                material.SetInt(Property_AlphaMode, (int)AlphaMode.AlphaTest);
                material.SetInt(Property_SrcBlendMode, (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt(Property_DstBlendMode, (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt(Property_ZWriteMode, 0);
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                break;
            case RenderingModePresets.Transparent:
                material.SetOverrideTag(Tag_RenderType, "Transparent");
                material.SetKeyword(AlphaMode.None);
                material.SetInt(Property_AlphaMode, (int)AlphaMode.AlphaTest);
                material.SetInt(Property_SrcBlendMode, (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt(Property_DstBlendMode, (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt(Property_ZWriteMode, 0);
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                break;
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
