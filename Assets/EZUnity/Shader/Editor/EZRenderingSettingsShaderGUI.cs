/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-02-21 17:44:54
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;

public class EZRenderingSettingsShaderGUI : EZShaderGUI
{
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        materialEditor.PropertiesDefaultGUI(properties);
        RenderingSettingsGUI(materialEditor, properties);
    }
}
