/* Author:          熊哲
 * CreateTime:      2018-05-09 09:47:08
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

namespace EZUnityEditor
{
    public class EZAssetReferenceViewer : EZEditorWindow
    {
        private Object target;

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
            dependencies.Clear();
            sceneReferences.Clear();
            target = Selection.activeObject;

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
            if (PrefabUtility.GetPrefabType(obj) == PrefabType.PrefabInstance && PrefabUtility.GetPrefabParent(obj) == target)
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
            EditorGUILayout.ObjectField(target, typeof(Object), true);
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
                foreach (Object obj in dependencies)
                {
                    EditorGUILayout.ObjectField(obj, typeof(Object), true);
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
