/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-07-25 15:51:49
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity.ShaderAttributes
{
    public class EZMinMaxSliderDrawer : MaterialPropertyDrawer
    {
        public readonly bool fixedLimit;
        public bool showAsVectorValue;
        public float limitMin;
        public float limitMax;

        public EZMinMaxSliderDrawer()
        {
            // limits will be retrived from zw component of the vector
            fixedLimit = false;
            limitMin = 0;
            limitMax = 1;
        }
        public EZMinMaxSliderDrawer(float min, float max)
        {
            fixedLimit = true;
            limitMin = min;
            limitMax = max;
        }

        private static bool IsPropertyTypeSuitable(MaterialProperty property)
        {
            return property.type == MaterialProperty.PropType.Vector;
        }

        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
        {
            if (!IsPropertyTypeSuitable(prop))
            {
                return EditorGUIUtility.singleLineHeight * 2.5f;
            }
            return base.GetPropertyHeight(prop, label, editor);
        }
        public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor)
        {
            if (!IsPropertyTypeSuitable(prop))
            {
                EditorGUI.HelpBox(position, "EZMinMaxSlider used on a non-vector property: " + prop.name, MessageType.Warning);
                return;
            }


            float oldLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 0;
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent(label));
            EditorGUIUtility.labelWidth = oldLabelWidth;

            if (!fixedLimit)
            {
                showAsVectorValue = EditorGUI.Foldout(new Rect(position) { width = 0 }, showAsVectorValue, GUIContent.none, false);
            }

            if (showAsVectorValue)
            {
                editor.VectorProperty(position, prop, "");
            }
            else
            {
                Vector4 value = prop.vectorValue;
                if (!fixedLimit)
                {
                    limitMin = value.z;
                    limitMax = value.w;
                }
                EditorGUI.showMixedValue = prop.hasMixedValue;
                EditorGUI.BeginChangeCheck();
                value = EZEditorGUIUtility.MinMaxSliderV4(position, value, limitMin, limitMax);
                if (EditorGUI.EndChangeCheck())
                {
                    prop.vectorValue = value;
                }
                EditorGUI.showMixedValue = false;
            }
        }
    }
}
