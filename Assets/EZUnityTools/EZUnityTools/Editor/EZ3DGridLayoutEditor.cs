/*
 * Author:      熊哲
 * CreateTime:  8/22/2017 2:20:56 PM
 * Description:
 * 
*/
using UnityEngine;
using UnityEditor;

namespace EZUnityTools
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(EZ3DGridLayout))]
    public class EZ3DGridLayoutEditor : Editor
    {
        private EZ3DGridLayout layout;

        protected virtual void OnEnable()
        {
            layout = target as EZ3DGridLayout;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Reset Children"))
            {
                layout.ResetChildren();
            }
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
                if (layout.updateMode == EZ3DGridLayout.UpdateMode.OnChange)
                {
                    layout.ResetChildren();
                }
            }
        }
    }
}