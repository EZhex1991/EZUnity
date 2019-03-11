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

    private MaterialProperty _SpecMatcap;
    private MaterialProperty _SpecColor;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        MainTextureWithColorGUI(materialEditor, properties);
        BumpGUI(materialEditor, properties);

        _DiffMatcap = FindProperty("_DiffMatcap", properties);
        _DiffColor = FindProperty("_DiffColor", properties);

        materialEditor.TexturePropertySingleLine(_DiffMatcap, _DiffColor);

        _SpecMatcap = FindProperty("_SpecMatcap", properties);
        _SpecColor = FindProperty("_SpecColor", properties);

        materialEditor.TexturePropertyFeatured(_SpecMatcap, _SpecColor, Keyword_SpecOn, setupRequired);

        AdvancedOptionsGUI(materialEditor);
        Setup(materialEditor.target as Material);
    }
}
