/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-06-18 13:49:30
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZUnity;
using UnityEditor;

public class EZDepthOutlineShaderGUI : ShaderGUI
{
    private MaterialProperty _SampleDistance;
    private MaterialProperty _DepthSensitivity;
    private MaterialProperty _NormalSensitivity;
    private MaterialProperty _CoverColor;
    private MaterialProperty _CoverStrength;
    private MaterialProperty _OutlineColor;
    private MaterialProperty _OutlineStrength;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        _SampleDistance = FindProperty("_SampleDistance", properties);
        _DepthSensitivity = FindProperty("_DepthSensitivity", properties);
        _NormalSensitivity = FindProperty("_NormalSensitivity", properties);
        _CoverColor = FindProperty("_CoverColor", properties);
        _CoverStrength = FindProperty("_CoverStrength", properties);
        _OutlineColor = FindProperty("_OutlineColor", properties);
        _OutlineStrength = FindProperty("_OutlineStrength", properties);

        materialEditor.ShaderProperty(_SampleDistance);
        materialEditor.ShaderProperty(_DepthSensitivity);
        materialEditor.ShaderProperty(_NormalSensitivity);
        materialEditor.ShaderProperty(_CoverColor);
        materialEditor.ShaderProperty(_CoverStrength);
        materialEditor.ShaderProperty(_OutlineColor);
        materialEditor.ShaderProperty(_OutlineStrength);
    }
}
