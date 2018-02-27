/* Author:          熊哲
 * CreateTime:      2018-02-27 12:36:14
 * Orgnization:     #ORGNIZATION#
 * Description:     
 */
using EZFramework.XLuaExtension;
using EZUnityEditor;
using UnityEditor;

namespace EZFrameworkEditor.XLuaExtension
{
    [CustomEditor(typeof(LuaBehaviour))]
    public class LuaBehaviourEditor : LuaInjectorEditor
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
            injectionList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}