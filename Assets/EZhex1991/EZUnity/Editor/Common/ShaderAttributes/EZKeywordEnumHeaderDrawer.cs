/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-07-25 15:07:43
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity.ShaderAttributes
{
    public class EZKeywordEnumHeaderDrawer : MaterialPropertyDrawer
    {
        private readonly GUIContent[] keywords;

        public EZKeywordEnumHeaderDrawer(string kw1) : this(new[] { kw1 }) { }
        public EZKeywordEnumHeaderDrawer(string kw1, string kw2) : this(new[] { kw1, kw2 }) { }
        public EZKeywordEnumHeaderDrawer(string kw1, string kw2, string kw3) : this(new[] { kw1, kw2, kw3 }) { }
        public EZKeywordEnumHeaderDrawer(string kw1, string kw2, string kw3, string kw4) : this(new[] { kw1, kw2, kw3, kw4 }) { }
        public EZKeywordEnumHeaderDrawer(string kw1, string kw2, string kw3, string kw4, string kw5) : this(new[] { kw1, kw2, kw3, kw4, kw5 }) { }
        public EZKeywordEnumHeaderDrawer(string kw1, string kw2, string kw3, string kw4, string kw5, string kw6) : this(new[] { kw1, kw2, kw3, kw4, kw5, kw6 }) { }
        public EZKeywordEnumHeaderDrawer(string kw1, string kw2, string kw3, string kw4, string kw5, string kw6, string kw7) : this(new[] { kw1, kw2, kw3, kw4, kw5, kw6, kw7 }) { }
        public EZKeywordEnumHeaderDrawer(string kw1, string kw2, string kw3, string kw4, string kw5, string kw6, string kw7, string kw8) : this(new[] { kw1, kw2, kw3, kw4, kw5, kw6, kw7, kw8 }) { }
        public EZKeywordEnumHeaderDrawer(string kw1, string kw2, string kw3, string kw4, string kw5, string kw6, string kw7, string kw8, string kw9) : this(new[] { kw1, kw2, kw3, kw4, kw5, kw6, kw7, kw8, kw9 }) { }
        public EZKeywordEnumHeaderDrawer(params string[] keywords)
        {
            this.keywords = new GUIContent[keywords.Length];
            for (int i = 0; i < keywords.Length; ++i)
                this.keywords[i] = new GUIContent(keywords[i]);
        }

        private static bool IsPropertyTypeSuitable(MaterialProperty prop)
        {
            return prop.type == MaterialProperty.PropType.Float || prop.type == MaterialProperty.PropType.Range;
        }

        private void SetKeyword(MaterialProperty prop, int index)
        {
            for (int i = 0; i < keywords.Length; ++i)
            {
                string keyword = GetKeywordName(prop.name, keywords[i].text);
                foreach (Material material in prop.targets)
                {
                    if (index == i)
                        material.EnableKeyword(keyword);
                    else
                        material.DisableKeyword(keyword);
                }
            }
        }

        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
        {
            if (!IsPropertyTypeSuitable(prop))
            {
                return EditorGUIUtility.singleLineHeight * 2.5f;
            }
            return 24f;
        }

        public override void OnGUI(Rect position, MaterialProperty prop, GUIContent label, MaterialEditor editor)
        {
            if (!IsPropertyTypeSuitable(prop))
            {
                EditorGUI.HelpBox(position, "EZKeywordEnumHeader used on a non-float property: " + prop.name, MessageType.Warning);
                return;
            }

            position.y += 8;
            position = EditorGUI.IndentedRect(position);
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = prop.hasMixedValue;
            EditorStyles.label.fontStyle = FontStyle.Bold;
            var value = (int)prop.floatValue;
            value = EditorGUI.Popup(position, label, value, keywords);
            EditorGUI.showMixedValue = false;
            EditorStyles.label.fontStyle = FontStyle.Normal;
            if (EditorGUI.EndChangeCheck())
            {
                prop.floatValue = value;
                SetKeyword(prop, value);
            }
        }

        public override void Apply(MaterialProperty prop)
        {
            base.Apply(prop);
            if (!IsPropertyTypeSuitable(prop))
                return;

            if (prop.hasMixedValue)
                return;

            SetKeyword(prop, (int)prop.floatValue);
        }

        // Final keyword name: property name + "_" + display name. Uppercased,
        // and spaces replaced with underscores.
        private static string GetKeywordName(string propName, string name)
        {
            string n = propName + "_" + name;
            return n.Replace(' ', '_').ToUpperInvariant();
        }
    }
}
