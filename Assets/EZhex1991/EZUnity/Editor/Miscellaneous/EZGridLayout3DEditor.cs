/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-08-22 14:20:56
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CanEditMultipleObjects, CustomEditor(typeof(EZGridLayout3D))]
    public class EZ3DGridLayoutEditor : Editor
    {
        private EZGridLayout3D layout;

        protected virtual void OnEnable()
        {
            layout = target as EZGridLayout3D;
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
                if (layout.updateMode == EZGridLayout3D.UpdateMode.OnChange)
                {
                    layout.ResetChildren();
                }
            }
        }
    }
}