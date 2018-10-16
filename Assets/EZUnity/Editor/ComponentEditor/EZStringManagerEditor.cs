/* Author:          熊哲
 * CreateTime:      2018-06-07 20:56:04
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZUnity
{
    [CustomEditor(typeof(EZStringManager))]
    public class EZStringManagerEditor : Editor
    {
        private SerializedProperty stringAsset;
        private SerializedProperty subscribers;
        private ReorderableList subscriberList;

        private EZStringAsset asset;
        private float lineHeight = EditorGUIUtility.singleLineHeight;

        private void OnEnable()
        {
            stringAsset = serializedObject.FindProperty("stringAsset");
            subscribers = serializedObject.FindProperty("subscribers");
            subscriberList = new ReorderableList(serializedObject, subscribers, true, false, true, true)
            {
                drawHeaderCallback = DrawSubscriberHeader,
                drawElementCallback = DrawSubscriberListElement
            };
        }

        private void DrawSubscriberHeader(Rect rect)
        {
            rect.x += 15; rect.width -= 15;
            float width = rect.width / 2;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width, lineHeight), "Text");
            rect.x += width;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width, lineHeight), "Key");
        }

        private void DrawSubscriberListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            Color color = GUI.color;
            SerializedProperty subscriber = subscribers.GetArrayElementAtIndex(index);
            SerializedProperty text = subscriber.FindPropertyRelative("text");
            SerializedProperty key = subscriber.FindPropertyRelative("key");
            if (asset != null && !asset.Contains(key.stringValue)) GUI.color = Color.red;
            float width = rect.width / 2; float margin = 5;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width - margin, lineHeight), text, GUIContent.none);
            rect.x += width;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width - margin, lineHeight), key, GUIContent.none);
            GUI.color = color;
        }

        public override void OnInspectorGUI()
        {
            EZEditorGUIUtility.ScriptTitle(target);
            serializedObject.Update();
            asset = stringAsset.objectReferenceValue as EZStringAsset;
            EditorGUILayout.PropertyField(stringAsset);
            subscriberList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
