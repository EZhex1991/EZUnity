/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-02-21 17:44:54
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZUnity;
using System;
using UnityEditor;
using UnityEngine;

public class EZShaderGUI : ShaderGUI
{
    public enum RenderingModePresets
    {
        Opaque,
        Cutout,
        Fade,   // Old school alpha-blending mode, fresnel does not affect amount of transparency
        Transparent // Physically plausible transparency mode, implemented as alpha pre-multiply
    }

    public const string Tag_RenderType = "RenderType";
    public const string Property_RenderingMode = "_RenderingMode";
    public const string Property_SrcBlendMode = "_SrcBlendMode";
    public const string Property_DstBlendMode = "_DstBlendMode";
    public const string Property_ZWriteMode = "_ZWriteMode";
    public const string Property_CullMode = "_CullMode";
    public const string Property_AlphaCutoff = "_AlphaCutoff";
    public const string Property_OffsetFactor = "_OffsetFactor";
    public const string Property_OffsetUnit = "_OffsetUnit";

    public static readonly string[] RenderModeNames = Enum.GetNames(typeof(RenderingModePresets));

    public static void DrawRenderingModePresets(MaterialEditor materialEditor, MaterialProperty property)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(property.displayName, EditorStyles.wordWrappedLabel);
        foreach (var renderingMode in Enum.GetValues(typeof(RenderingModePresets)))
        {
            if (GUILayout.Button(renderingMode.ToString(), EditorStyles.miniButton))
            {
                materialEditor.RegisterPropertyChangeUndo(property.name);
                property.floatValue = (int)renderingMode;
                foreach (Material mat in property.targets)
                {
                    SetupRenderingMode(mat, (RenderingModePresets)renderingMode);
                }
            }
        }
        EditorGUILayout.EndHorizontal();
    }
    public static void SetupRenderingMode(Material material, RenderingModePresets renderMode)
    {
        switch (renderMode)
        {
            case RenderingModePresets.Opaque:
                material.SetOverrideTag(Tag_RenderType, "");
                material.SetInt(Property_SrcBlendMode, (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt(Property_DstBlendMode, (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt(Property_ZWriteMode, 1);
                material.renderQueue = -1;
                break;
            case RenderingModePresets.Cutout:
                material.SetOverrideTag(Tag_RenderType, "TransparentCutout");
                material.SetInt(Property_SrcBlendMode, (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt(Property_DstBlendMode, (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt(Property_ZWriteMode, 1);
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest;
                break;
            case RenderingModePresets.Fade:
                material.SetOverrideTag(Tag_RenderType, "Transparent");
                material.SetInt(Property_SrcBlendMode, (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt(Property_DstBlendMode, (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt(Property_ZWriteMode, 0);
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                break;
            case RenderingModePresets.Transparent:
                material.SetOverrideTag(Tag_RenderType, "Transparent");
                material.SetInt(Property_SrcBlendMode, (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt(Property_DstBlendMode, (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt(Property_ZWriteMode, 0);
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                break;
        }
    }

    private MaterialProperty _RenderingMode;
    private MaterialProperty _SrcBlendMode;
    private MaterialProperty _DstBlendMode;
    private MaterialProperty _AlphaCutoff;

    private MaterialProperty _ZWriteMode;
    private MaterialProperty _CullMode;

    private MaterialProperty _OffsetFactor;
    private MaterialProperty _OffsetUnit;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        materialEditor.PropertiesDefaultGUI(properties);
        DrawEZShaderProperties(materialEditor, properties);
    }

    public void DrawEZShaderProperties(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        _RenderingMode = FindProperty(Property_RenderingMode, properties);
        _SrcBlendMode = FindProperty(Property_SrcBlendMode, properties);
        _DstBlendMode = FindProperty(Property_DstBlendMode, properties);
        _AlphaCutoff = FindProperty(Property_AlphaCutoff, properties);
        _ZWriteMode = FindProperty(Property_ZWriteMode, properties);
        _CullMode = FindProperty(Property_CullMode, properties);
        _OffsetFactor = FindProperty(Property_OffsetFactor, properties);
        _OffsetUnit = FindProperty(Property_OffsetUnit, properties);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("EZShaderGUI Properties", EditorStyles.boldLabel);
        DrawRenderingModePresets(materialEditor, _RenderingMode);
        EditorGUI.indentLevel++;
        {
            materialEditor.BlendModeProperty(_SrcBlendMode);
            materialEditor.BlendModeProperty(_DstBlendMode);
            materialEditor.Toggle(_ZWriteMode);
        }
        EditorGUI.indentLevel--;
        if ((RenderingModePresets)(_RenderingMode.floatValue) == RenderingModePresets.Cutout)
        {
            materialEditor.ShaderProperty(_AlphaCutoff);
        }

        materialEditor.CullModeProperty(_CullMode);

        materialEditor.ShaderProperty(_OffsetFactor);
        materialEditor.ShaderProperty(_OffsetUnit);
    }
}
