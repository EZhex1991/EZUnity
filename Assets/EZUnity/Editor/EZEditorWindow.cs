/*
 * Author:      熊哲
 * CreateTime:  3/6/2017 5:35:28 PM
 * Description:
 * 如果需要加载ScriptableObject，需要写在OnFocus中。如果写在OnEnable中，在退出Unity时未关闭窗口，下次打开时会发现加载不上的情况（5.3.8）
*/
using UnityEditor;

namespace EZUnity
{
    public abstract class EZEditorWindow : EditorWindow
    {
        protected virtual void OnEnable() { }

        protected virtual void OnFocus() { }
        protected virtual void OnLostFocus() { }

        protected virtual void OnHierarchyChange() { }
        protected virtual void OnSelectionChange() { }
        protected virtual void OnProjectChange() { }

        protected virtual void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(titleContent.text, EditorStyles.centeredGreyMiniLabel);
            EZEditorGUIUtility.ScriptTitle(this);
            EditorGUILayout.Space();
        }
        protected virtual void Update() { }
        protected virtual void OnInspectorUpdate() { }

        protected virtual void OnDestroy() { }
    }
}