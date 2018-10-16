/* Author:          熊哲
 * CreateTime:      2018-05-18 10:06:55
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;

namespace EZUnity.Switcher
{
    [CustomEditor(typeof(EZTimelineSwitcher))]
    public class EZTimelineSwitcherEditor : EZSwitcherEditor
    {
        private SerializedProperty m_Director;

        protected override void OnEnable()
        {
            m_Director = serializedObject.FindProperty("m_Director");
            base.OnEnable();
        }

        protected override void DrawOtherProperties()
        {
            base.DrawOtherProperties();
            EditorGUILayout.PropertyField(m_Director);
        }
    }
}
