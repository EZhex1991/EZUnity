/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-05-09 09:47:08
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZAssetReferenceViewer : EditorWindow
    {
        private Object target;

        private List<Object> dependencies = new List<Object>();
        private bool dependenciesRecursive = true;
        private bool dependenciesFoldout = true;

        private List<Object> sceneReferences = new List<Object>();
        private bool referencesFoldout = true;

        private Vector2 scrollPosition;

        protected void OnEnable()
        {
            Refresh();
        }
        protected void OnSelectionChange()
        {
            Refresh();
        }
        private void Refresh()
        {
            dependencies.Clear();
            sceneReferences.Clear();
            target = Selection.activeObject;
            if (target == null) return;
            string[] paths = AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(target), dependenciesRecursive);
            foreach (string path in paths)
            {
                dependencies.Add(AssetDatabase.LoadAssetAtPath(path, typeof(Object)));
            }

            GameObject[] gameObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject gameObject in gameObjects)
            {
                if (CheckPrefabReference(gameObject) || CheckComponentReference(gameObject)) sceneReferences.Add(gameObject);
            }
            Repaint();
        }
        private bool CheckPrefabReference(Object obj)
        {
#if UNITY_2018_3_OR_NEWER
            if (PrefabUtility.GetPrefabAssetType(obj) == PrefabAssetType.Regular && PrefabUtility.GetCorrespondingObjectFromSource(obj) == target)
#elif UNITY_2018_2
            if (PrefabUtility.GetPrefabType(obj) == PrefabType.PrefabInstance && PrefabUtility.GetCorrespondingObjectFromSource(obj) == target)
#else
            if (PrefabUtility.GetPrefabType(obj) == PrefabType.PrefabInstance && PrefabUtility.GetPrefabParent(obj) == target)
#endif
            {
                return true;
            }
            return false;
        }
        private bool CheckComponentReference(GameObject go)
        {
            Component[] components = go.GetComponents<Component>();
            foreach (Component component in components)
            {
                if (component != null)
                {
                    SerializedObject serializedComponent = new SerializedObject(component);
                    SerializedProperty iterator = serializedComponent.GetIterator();
                    while (iterator.NextVisible(true))
                    {
                        if (iterator.propertyType == SerializedPropertyType.ObjectReference && iterator.objectReferenceValue == target)
                            return true;
                    }
                }
            }
            return false;
        }

        protected void OnGUI()
        {
            EZEditorGUIUtility.WindowTitle(this);
            EditorGUILayout.ObjectField("Target", target, typeof(Object), true);

            EditorGUILayout.BeginHorizontal();
            if (dependenciesRecursive != EditorGUILayout.Toggle("Show Recursive Dependencies", dependenciesRecursive))
            {
                dependenciesRecursive = !dependenciesRecursive;
                Refresh();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            if (dependenciesFoldout = EditorGUILayout.Foldout(dependenciesFoldout, "Asset Dependencies"))
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < dependencies.Count; i++)
                {
                    if (dependencies[i] == target) continue; // dependencies contains target itself
                    EditorGUILayout.ObjectField(dependencies[i], typeof(Object), true);
                }
                EditorGUI.indentLevel--;
            }
            if (referencesFoldout = EditorGUILayout.Foldout(referencesFoldout, "Scene References"))
            {
                EditorGUI.indentLevel++;
                foreach (Object obj in sceneReferences)
                {
                    EditorGUILayout.ObjectField(obj, typeof(Object), true);
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
