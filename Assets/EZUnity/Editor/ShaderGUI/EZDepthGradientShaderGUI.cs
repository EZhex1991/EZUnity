/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-06-18 13:49:30
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZUnity;
using UnityEditor;

public class EZDepthGradientShaderGUI : ShaderGUI
{
    private MaterialProperty _ColorNear;
    private MaterialProperty _ColorFar;
    private MaterialProperty _GradientPower;
    private MaterialProperty _GradientSoftness;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        _ColorNear = FindProperty("_ColorNear", properties);
        _ColorFar = FindProperty("_ColorFar", properties);
        _GradientPower = FindProperty("_GradientPower", properties);
        _GradientSoftness = FindProperty("_GradientSoftness", properties);

        materialEditor.ShaderProperty(_ColorNear);
        materialEditor.ShaderProperty(_ColorFar);
        materialEditor.ShaderProperty(_GradientPower);
        materialEditor.MinMaxSliderPropertyTwoLines(_GradientSoftness);
    }
}
