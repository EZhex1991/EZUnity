/*
 * Author:      熊哲
 * CreateTime:  9/11/2017 10:23:45 AM
 * Description:
 * 
*/
using UnityEditor;
using UnityEngine;

namespace EZFramework
{
    [CustomEditor(typeof(EZWWWTask))]
    public class WWWTaskEditor : Editor
    {
        private EZWWWTask task;

        void OnEnable()
        {
            task = target as EZWWWTask;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUI.enabled = false;
            EditorGUILayout.TextField("URL", task.url);
            EditorGUILayout.Slider("progress", task.progress, 0, 1);
            GUI.enabled = true;
        }
    }
}