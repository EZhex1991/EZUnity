/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-05-09 09:47:08
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EZUnity.AssetProcessor
{
    public class EZAssetReferenceViewer : EZEditorWindow
    {
        private Object target;

        private List<string> dependenciesPath = new List<string>();
        private List<Object> dependencies = new List<Object>();
        private bool dependenciesFoldout = true;
        private bool dependenciesRecursive = true;

        private List<Object> sceneReferences = new List<Object>();
        private bool referencesFoldout = true;
        private bool referencesRecursive = true;

        protected override void OnEnable()
        {
            base.OnEnable();
            Refresh();
        }
        protected override void OnSelectionChange()
        {
            base.OnSelectionChange();
            Refresh();
        }
        private void Refresh()
        {
            dependenciesPath.Clear();
            dependencies.Clear();
            sceneReferences.Clear();
            target = Selection.activeObject;
            if (target == null) return;
            string[] paths = AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(target), dependenciesRecursive);
            foreach (string path in paths)
            {
                dependenciesPath.Add(path);
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
#if UNITY_2018_2
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

        protected override void OnGUI()
        {
            base.OnGUI();
            EditorGUILayout.ObjectField("Target", target, typeof(Object), true);
            EditorGUILayout.BeginHorizontal();

            if (dependenciesRecursive != EditorGUILayout.Toggle("Show Recursive Dependencies", dependenciesRecursive))
            {
                dependenciesRecursive = !dependenciesRecursive;
                Refresh();
            }
            if (referencesRecursive != EditorGUILayout.Toggle("Show Recursive References", referencesRecursive))
            {
                referencesRecursive = !referencesRecursive;
                Refresh();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (dependenciesFoldout = EditorGUILayout.Foldout(dependenciesFoldout, "Dependencies"))
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < dependencies.Count; i++)
                {
                    if (dependencies[i] == target) continue; // dependencies contains target itself
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.TextField(dependenciesPath[i]);
                    EditorGUILayout.ObjectField(dependencies[i], typeof(Object), true);
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }
            if (referencesFoldout = EditorGUILayout.Foldout(referencesFoldout, "References"))
            {
                EditorGUI.indentLevel++;
                foreach (Object obj in sceneReferences)
                {
                    EditorGUILayout.ObjectField(obj, typeof(Object), true);
                }
                EditorGUI.indentLevel--;
            }
        }
    }
}
