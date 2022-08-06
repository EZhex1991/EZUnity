/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-09-09 14:29:45
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity.ShaderAttributes
{
    public class EZVectorSingleLineDrawer : MaterialPropertyDrawer
    {
        private static bool IsPropertyTypeSuitable(MaterialProperty property)
        {
            return property.type == MaterialProperty.PropType.Vector;
        }

        public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor)
        {
            if (!IsPropertyTypeSuitable(prop))
            {
                EditorGUI.HelpBox(position, "EZVectorSingleLine used on a non-vector property: " + prop.name, MessageType.Warning);
                return;
            }

            prop.vectorValue = editor.VectorProperty(position, prop, label);
        }
    }
}
