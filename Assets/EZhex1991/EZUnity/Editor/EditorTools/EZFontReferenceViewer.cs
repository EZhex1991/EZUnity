/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-05-14 20:13:43
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace EZhex1991.EZUnity
{
    public class EZFontReferenceViewer : EditorWindow
    {
        private Text[] texts;
        private Vector2 scrollPosition;

        protected void OnEnable()
        {
            texts = FindObjectsOfType<Text>();
        }

        protected void OnGUI()
        {
            EZEditorGUIUtility.WindowTitle(this);
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
