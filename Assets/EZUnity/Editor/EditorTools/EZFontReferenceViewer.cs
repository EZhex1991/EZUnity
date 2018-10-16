/* Author:          熊哲
 * CreateTime:      2018-05-14 20:13:43
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace EZUnity
{
    public class EZFontReferenceViewer : EZEditorWindow
    {
        private Text[] texts;
        private Vector2 scrollPosition;

        protected override void OnEnable()
        {
            base.OnEnable();
            texts = FindObjectsOfType<Text>();
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            for (int i = 0; i < texts.Length; i++)
            {
                Text text = texts[i];
                EditorGUILayout.BeginHorizontal();
                GUI.enabled = false;
                EditorGUILayout.ObjectField(text.gameObject, typeof(GameObject), true);
                GUI.enabled = true;
                text.font = (Font)EditorGUILayout.ObjectField(text.font, typeof(Font), true);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
