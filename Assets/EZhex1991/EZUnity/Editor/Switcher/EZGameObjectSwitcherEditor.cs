/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-04-17 15:42:55
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;

namespace EZhex1991.EZUnity
{
    [CustomEditor(typeof(EZGameObjectSwitcher))]
    public class EZGameObjectSwitcherEditor : EZSwitcherEditor
    {
        private SerializedProperty m_Parent;

        protected override void OnEnable()
        {
            m_Parent = serializedObject.FindProperty("m_Parent");
            base.OnEnable();
        }

        protected override void DrawOtherProperties()
        {
            EditorGUILayout.PropertyField(m_Parent);
        }
    }
}
