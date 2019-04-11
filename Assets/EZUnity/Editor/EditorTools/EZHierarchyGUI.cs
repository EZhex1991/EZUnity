/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-04-10 16:06:21
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZUnity
{
    [InitializeOnLoad]
    public class EZHierarchyGUI
    {
        static EZHierarchyGUI()
        {
            EditorApplication.hierarchyWindowItemOnGUI += DrawChildCount;
        }

        private static void DrawChildCount(int instanceID, Rect selectionRect)
        {
            Object item = EditorUtility.InstanceIDToObject(instanceID);
            if (item is GameObject)
            {
                GameObject gameObject = item as GameObject;
                Rect activeRect = new Rect(selectionRect.x - 28, selectionRect.y, selectionRect.height, selectionRect.height);
                EditorGUI.BeginChangeCheck();
                bool active = EditorGUI.Toggle(activeRect, gameObject.activeSelf);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(gameObject, "SetActive");
                    gameObject.SetActive(active);
                }
            }
        }
    }
}
