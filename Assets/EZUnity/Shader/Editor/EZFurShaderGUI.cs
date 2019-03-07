/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-03-07 10:38:33
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZUnity;
using UnityEditor;
using UnityEngine;

public class EZFurShaderGUI : EZShaderGUI
{
    public enum LightingMode
    {
        Vertex,
        Pixel,
    }

    protected MaterialProperty _LightingMode;

    protected override void Setup(Material mat)
    {
        base.Setup(mat);
        mat.SetKeyword((LightingMode)_LightingMode.floatValue);
    }
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        _LightingMode = FindProperty("_LightingMode", properties);
        materialEditor.EnumPopup<LightingMode>(_LightingMode, (mat, selection) =>
        {
            mat.SetKeyword((LightingMode)selection);
        });

        materialEditor.PropertiesDefaultGUI(properties);

        Setup(materialEditor.target as Material);
    }
}
