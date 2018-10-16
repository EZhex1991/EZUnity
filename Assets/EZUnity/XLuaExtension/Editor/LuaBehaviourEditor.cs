/* Author:          熊哲
 * CreateTime:      2018-02-27 12:36:14
 * Organization:    #ORGANIZATION#
 * Description:     
 */
#if XLUA
using UnityEditor;

namespace EZUnity.XLuaExtension
{
    [CustomEditor(typeof(LuaBehaviour))]
    public class LuaBehaviourEditor : EZDictionaryEditor
    {
        protected SerializedProperty m_ModuleName;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_ModuleName = serializedObject.FindProperty("moduleName");
        }

        public override void OnInspectorGUI()
        {
            EZEditorGUIUtility.ScriptTitle(target);
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_ModuleName);
            elementList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
