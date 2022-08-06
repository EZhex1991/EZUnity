/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-09-09 11:11:57
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity.ShaderAttributes
{
    public class EZSingleLineDrawer : MaterialPropertyDrawer
    {
        private readonly string extraPropertyName;
        private MaterialProperty extraProperty;

        public EZSingleLineDrawer(string prop)
        {
            extraPropertyName = prop;
        }

        private static bool IsPropertyTypeSuitable(MaterialProperty prop)
        {
            return prop.type == MaterialProperty.PropType.Color || prop.type == MaterialProperty.PropType.Float;
        }
        private static bool IsExtraPropertyTypeSuitable(MaterialProperty prop)
        {
            return prop.type != MaterialProperty.PropType.Texture;
        }

        public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor)
        {
            if (!IsPropertyTypeSuitable(prop))
            {
                EditorGUI.HelpBox(position, prop.type + " is not supported for EZSingleLineDrawer: " + prop.name, MessageType.Warning);
                return;
            }

            if (string.IsNullOrEmpty(extraPropertyName))
            {
                EditorGUI.HelpBox(position, "ExtraPropertyName not specified for EZSingleLineDrawer on " + prop.name, MessageType.Warning);
                return;
            }
            extraProperty = MaterialEditor.GetMaterialProperty(editor.targets, extraPropertyName);
            if (!IsExtraPropertyTypeSuitable(extraProperty))
            {
                EditorGUI.HelpBox(position, extraProperty.type + " is not supported for EZSingleLineDrawer: " + prop.name, MessageType.Warning);
                return;
            }

            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 0;
            EditorGUI.PrefixLabel(position, new GUIContent(label));
            editor.DefaultShaderProperty(MaterialEditor.GetLeftAlignedFieldRect(position), prop, "");

            int oldIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            Rect prop2Rect;
            if (extraProperty.type == MaterialProperty.PropType.Color)
            {
                prop2Rect = MaterialEditor.GetRightAlignedFieldRect(position);
            }
            else if (extraProperty.type == MaterialProperty.PropType.Float)
            {
                prop2Rect = MaterialEditor.GetRightAlignedFieldRect(position);
            }
            else
            {
                prop2Rect = MaterialEditor.GetFlexibleRectBetweenFieldAndRightEdge(position);
            }
            editor.ShaderProperty(prop2Rect, extraProperty, GUIContent.none);
            EditorGUI.indentLevel = oldIndentLevel;
            EditorGUIUtility.labelWidth = labelWidth;
        }
    }
}
