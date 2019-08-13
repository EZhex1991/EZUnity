/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-07-25 15:51:49
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity.MaterialAttribute
{
    public class EZMinMaxSliderDrawer : MaterialPropertyDrawer
    {
        public readonly bool fixedLimit;
        public float minLimit;
        public float maxLimit;

        public EZMinMaxSliderDrawer()
        {
            // limit will be retrived from zw component of the vector
            // you can change limit on Debug(Inspector) Window
            fixedLimit = false;
        }
        public EZMinMaxSliderDrawer(float min, float max)
        {
            fixedLimit = true;
            minLimit = min;
            maxLimit = max;
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

            float unitWidth = position.width / 5; float margin = 5;
            Vector4 value = prop.vectorValue;
            if (!fixedLimit)
            {
                if (value.z >= value.w)
                {
                    minLimit = 0;
                    maxLimit = 1;
                }
                else
                {
                    minLimit = value.z;
                    maxLimit = value.w;
                }
            }
            EditorGUI.showMixedValue = prop.hasMixedValue;
            EditorGUI.BeginChangeCheck();

            position.width = unitWidth - margin;
            value.x = EditorGUI.FloatField(position, value.x);

            position.x += position.width + margin;
            position.width = unitWidth * 3;
            EditorGUI.MinMaxSlider(position, ref value.x, ref value.y, minLimit, maxLimit);

            position.x += position.width + margin;
            position.width = unitWidth - margin;
            value.y = EditorGUI.FloatField(position, value.y);

            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            {
                prop.vectorValue = value;
            }
        }
    }
}
