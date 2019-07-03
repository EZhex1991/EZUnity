/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-05-06 17:53:16
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZCorrespondingObjectViewer : EditorWindow
    {
#if UNITY_2018_3_OR_NEWER
        private Object target;

        private PrefabAssetType prefabAssetType;
        private Object originalObject;
        private List<Object> correspondingObjects = new List<Object>();
        private List<Object> correspondingAssets = new List<Object>();

        private bool correspondingObjectsFoldout = true;

        protected void Refresh()
        {
            prefabAssetType = PrefabAssetType.NotAPrefab;
            originalObject = null;
            correspondingObjects.Clear();
            correspondingAssets.Clear();

            target = Selection.activeObject;
            if (target != null)
            {
                prefabAssetType = PrefabUtility.GetPrefabAssetType(target);
                originalObject = PrefabUtility.GetCorrespondingObjectFromOriginalSource(target);
                Object source = PrefabUtility.GetCorrespondingObjectFromSource(target);
                while (source != null)
                {
                    correspondingObjects.Add(source);
                    correspondingAssets.Add(AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GetAssetPath(source)));
                    source = PrefabUtility.GetCorrespondingObjectFromSource(source);
                }
            }
            Repaint();
        }

        private void OnEnable()
        {
            Refresh();
        }
        private void OnSelectionChange()
        {
            Refresh();
        }
        private void OnGUI()
        {
            EZEditorGUIUtility.WindowTitle(this);
            EditorGUILayout.ObjectField("Target", target, typeof(Object), false);

            EditorGUILayout.EnumPopup("Prefab Asset Type", prefabAssetType);
            EditorGUILayout.ObjectField("Original Object", originalObject, typeof(Object), false);
            correspondingObjectsFoldout = EditorGUILayout.Foldout(correspondingObjectsFoldout, "Corresponding Objects");
            if (correspondingObjectsFoldout)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < correspondingObjects.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.ObjectField(i.ToString("00"), correspondingObjects[i], typeof(Object), false);
                    float width = (EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth) / 2;
                    EditorGUILayout.ObjectField(correspondingAssets[i], typeof(Object), false, GUILayout.Width(width));
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }
        }
#else
        private void OnGUI()
        {
            EditorGUILayout.HelpBox("Available only on Unity2018.3 or newer version", MessageType.Info);
        }
#endif
    }
}
