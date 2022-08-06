/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-09-09 14:54:39
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity.ShaderAttributes
{
    public class EZTextureKeywordDecorator : MaterialPropertyDrawer
    {
        private readonly string keyword;

        public EZTextureKeywordDecorator(string keyword)
        {
            this.keyword = keyword.ToUpperInvariant();
        }

        private static bool IsPropertyTypeSuitable(MaterialProperty property)
        {
            return property.type == MaterialProperty.PropType.Texture;
        }

        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
        {
            return 0;
        }
        public override void OnGUI(Rect position, MaterialProperty prop, GUIContent label, MaterialEditor editor)
        {
            if (!IsPropertyTypeSuitable(prop))
            {
                EditorGUI.HelpBox(position, "EZTextureKeyword used on a non-texture property: " + prop.name, MessageType.Warning);
                return;
            }

            if (!prop.hasMixedValue)
            {
                foreach (Material mat in editor.targets)
                {
                    mat.SetKeyword(keyword, prop.textureValue != null);
                }
            }
        }
    }
}
