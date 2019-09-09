/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-09-09 14:45:37
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity.ShaderAttributes
{
    public class EZTextureMiniDrawer : MaterialPropertyDrawer
    {
        private readonly string extraPropertyName1;
        private readonly string extraPropertyName2;

        private MaterialProperty extraProperty1;
        private MaterialProperty extraProperty2;

        public EZTextureMiniDrawer()
        {
        }
        public EZTextureMiniDrawer(string prop1)
        {
            extraPropertyName1 = prop1;
        }
        public EZTextureMiniDrawer(string prop1, string prop2)
        {
            extraPropertyName1 = prop1;
            extraPropertyName2 = prop2;
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
                EditorGUI.HelpBox(position, "EZTextureSingleLine used on a non-texture property: " + prop.name, MessageType.Warning);
                return;
            }

            if (!string.IsNullOrEmpty(extraPropertyName1))
            {
                extraProperty1 = MaterialEditor.GetMaterialProperty(editor.targets, extraPropertyName1);
            }
            if (!string.IsNullOrEmpty(extraPropertyName2))
            {
                extraProperty2 = MaterialEditor.GetMaterialProperty(editor.targets, extraPropertyName2);
            }
            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 0;
            editor.TexturePropertySingleLine(label, prop, extraProperty1, extraProperty2);
            EditorGUI.indentLevel++;
            editor.TextureScaleOffsetProperty(prop);
            EditorGUI.indentLevel--;
            EditorGUIUtility.labelWidth = labelWidth;
        }
    }
}
