/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-03-05 14:48:17
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZUnity;
using UnityEditor;
using UnityEngine;

public class EZMatcapShaderGUI : EZShaderGUI
{
    private MaterialProperty _DiffMatcap;
    private MaterialProperty _DiffColor;

    private MaterialProperty _SpecOn;
    private MaterialProperty _SpecMatcap;
    private MaterialProperty _SpecColor;

    protected override void Setup(Material mat)
    {
        base.Setup(mat);
        mat.SetKeyword(Keyword_BumpOn, _BumpOn.floatValue == 1);
        mat.SetKeyword(Keyword_SpecOn, _SpecOn.floatValue == 1);
    }
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        MainTextureWithColorGUI(materialEditor, properties);
        FeaturedBumpGUI(materialEditor, properties);

        _DiffMatcap = FindProperty("_DiffMatcap", properties);
        _DiffColor = FindProperty("_DiffColor", properties);

        materialEditor.ShaderProperty(_DiffMatcap);
        materialEditor.ShaderProperty(_DiffColor);

        _SpecOn = FindProperty("_SpecOn", properties);
        _SpecMatcap = FindProperty("_SpecMatcap", properties);
        _SpecColor = FindProperty("_SpecColor", properties);

        EditorGUILayout.Space();
        materialEditor.FeaturedPropertiesWithTexture(_SpecOn, _SpecMatcap, _SpecColor, Keyword_SpecOn);

        AdvancedOptionsGUI(materialEditor);
        Setup(materialEditor.target as Material);
    }
}
