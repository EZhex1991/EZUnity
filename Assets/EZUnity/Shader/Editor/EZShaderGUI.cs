/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-02-21 17:44:54
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEditor;
using UnityEngine;

public class EZShaderGUI : ShaderGUI
{
    public enum RenderModePresets
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

    public static readonly string[] RenderModeNames = Enum.GetNames(typeof(RenderModePresets));
    public static readonly string[] BlendModeNames = Enum.GetNames(typeof(UnityEngine.Rendering.BlendMode));
    public static readonly string[] CullModeNames = Enum.GetNames(typeof(UnityEngine.Rendering.CullMode));

    public static bool RenderModePresetFoldout = false;

    public static void DrawEnumPopup<T>(MaterialEditor materialEditor, MaterialProperty property) where T : Enum
    {
        EditorGUI.showMixedValue = property.hasMixedValue;
        float value = property.floatValue;

        EditorGUI.BeginChangeCheck();
        value = EditorGUILayout.Popup(property.name, (int)value, Enum.GetNames(typeof(T)));
        if (EditorGUI.EndChangeCheck())
        {
            materialEditor.RegisterPropertyChangeUndo(property.name);
            property.floatValue = value;
        }
        EditorGUI.showMixedValue = false;
    }
    public static void DrawRenderModePopup(MaterialEditor materialEditor, MaterialProperty property)
    {
        EditorGUI.showMixedValue = property.hasMixedValue;
        RenderModePresets renderingMode = (RenderModePresets)property.floatValue;

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginHorizontal();
        RenderModePresetFoldout = EditorGUILayout.Foldout(RenderModePresetFoldout, property.name);
        renderingMode = (RenderModePresets)EditorGUILayout.Popup((int)renderingMode, RenderModeNames);
        EditorGUILayout.EndHorizontal();
        if (EditorGUI.EndChangeCheck())
        {
            materialEditor.RegisterPropertyChangeUndo(property.name);
            property.floatValue = (float)renderingMode;
            foreach (Material mat in property.targets)
            {
                SetupBlendMode(mat, renderingMode);
            }
        }
        EditorGUI.showMixedValue = false;
    }
    public static void DrawZWriteToggle(MaterialEditor materialEditor, MaterialProperty property)
    {
        EditorGUI.showMixedValue = property.hasMixedValue;
        bool zWrite = property.floatValue == 1;

        EditorGUI.BeginChangeCheck();
        zWrite = EditorGUILayout.Toggle(property.name, zWrite);
        if (EditorGUI.EndChangeCheck())
        {
            materialEditor.RegisterPropertyChangeUndo(property.name);
            property.floatValue = zWrite ? 1 : 0;
        }
        EditorGUI.showMixedValue = false;
    }

    public static void SetupBlendMode(Material material, RenderModePresets blendMode)
    {
        switch (blendMode)
        {
            case RenderModePresets.Opaque:
                material.SetOverrideTag(Tag_RenderType, "");
                material.SetInt(Property_SrcBlendMode, (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt(Property_DstBlendMode, (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt(Property_ZWriteMode, 1);
                material.DisableKeyword("alpha");
                material.renderQueue = -1;
                break;
            case RenderModePresets.Cutout:
                material.SetOverrideTag(Tag_RenderType, "TransparentCutout");
                material.SetInt(Property_SrcBlendMode, (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt(Property_DstBlendMode, (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt(Property_ZWriteMode, 1);
                material.EnableKeyword("alpha");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest;
                break;
            case RenderModePresets.Fade:
                material.SetOverrideTag(Tag_RenderType, "Transparent");
                material.SetInt(Property_SrcBlendMode, (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt(Property_DstBlendMode, (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt(Property_ZWriteMode, 0);
                material.EnableKeyword("alpha");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                break;
            case RenderModePresets.Transparent:
                material.SetOverrideTag(Tag_RenderType, "Transparent");
                material.SetInt(Property_SrcBlendMode, (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt(Property_DstBlendMode, (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt(Property_ZWriteMode, 0);
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                break;
        }
    }

    private MaterialProperty _RenderingMode;
    private MaterialProperty _SrcBlend;
    private MaterialProperty _DstBlend;
    private MaterialProperty _AlphaCutoff;

    private MaterialProperty _ZWrite;
    private MaterialProperty _CullMode;

    private MaterialProperty _OffsetFactor;
    private MaterialProperty _OffsetUnit;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        materialEditor.PropertiesDefaultGUI(properties);

        _RenderingMode = FindProperty(Property_RenderingMode, properties);
        _SrcBlend = FindProperty(Property_SrcBlendMode, properties);
        _DstBlend = FindProperty(Property_DstBlendMode, properties);
        _AlphaCutoff = FindProperty(Property_AlphaCutoff, properties);
        _ZWrite = FindProperty(Property_ZWriteMode, properties);
        _CullMode = FindProperty(Property_CullMode, properties);
        _OffsetFactor = FindProperty(Property_OffsetFactor, properties);
        _OffsetUnit = FindProperty(Property_OffsetUnit, properties);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("EZShaderGUI Properties", EditorStyles.boldLabel);
        DrawRenderModePopup(materialEditor, _RenderingMode);
        EditorGUI.indentLevel++;
        if (RenderModePresetFoldout)
        {
            DrawEnumPopup<UnityEngine.Rendering.BlendMode>(materialEditor, _SrcBlend);
            DrawEnumPopup<UnityEngine.Rendering.BlendMode>(materialEditor, _DstBlend);
            DrawZWriteToggle(materialEditor, _ZWrite);
        }
        EditorGUI.indentLevel--;
        if ((RenderModePresets)(_RenderingMode.floatValue) == RenderModePresets.Cutout)
        {
            materialEditor.ShaderProperty(_AlphaCutoff, Property_AlphaCutoff);
        }

        DrawEnumPopup<UnityEngine.Rendering.CullMode>(materialEditor, _CullMode);

        materialEditor.ShaderProperty(_OffsetFactor, Property_OffsetFactor);
        materialEditor.ShaderProperty(_OffsetUnit, Property_OffsetUnit);
    }
}
