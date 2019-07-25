/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-05-27 17:55:15
 * Organization:    ezhex1991@outlook.com
 * Description:     
 */
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZRegexTester : EditorWindow
    {
        public string inputString = "This is a test";
        public string regexString = "([Tt]\\w*)";

        private float height = EditorGUIUtility.singleLineHeight * 5;

        protected void OnGUI()
        {
            EZEditorGUIUtility.WindowTitle(this);

            EditorGUILayout.LabelField("Input", EditorStyles.boldLabel);
            inputString = EditorGUILayout.TextArea(inputString, GUILayout.MinHeight(height));

            EditorGUILayout.LabelField("Regex", EditorStyles.boldLabel);
            regexString = EditorGUILayout.TextArea(regexString, GUILayout.MinHeight(height));

            EditorGUILayout.LabelField("Matches", EditorStyles.boldLabel);
            try
            {
                if (string.IsNullOrEmpty(regexString)) return;
                foreach (Match match in Regex.Matches(inputString, regexString))
                {
#if UNITY_2018_1_OR_NEWER && NET_4_6
                    EditorGUILayout.LabelField(string.Format("Match:\tIndex: {0}\tName: {1}\tValue: {2}", match.Index, match.Name, match.Value));
#else
                    EditorGUILayout.LabelField(string.Format("Match:\tIndex: {0}\tValue: {1}", match.Index, match.Value));
#endif
                    EditorGUI.indentLevel++;
                    foreach (Group group in match.Groups)
                    {
#if UNITY_2018_1_OR_NEWER && NET_4_6
                        EditorGUILayout.LabelField(string.Format("Group:\tIndex: {0}\tName: {1}\tValue: {2}", group.Index, group.Name, group.Value));
#else
                        EditorGUILayout.LabelField(string.Format("Group:\tIndex: {0}\tValue: {1}", group.Index, group.Value));
#endif
                        EditorGUI.indentLevel++;
                        foreach (Capture capture in group.Captures)
                        {
                            EditorGUILayout.LabelField(string.Format("Capture:\tIndex: {0}\tValue: {1}", capture.Index, capture.Value));
                        }
                        EditorGUI.indentLevel--;
                    }
                    EditorGUI.indentLevel--;
                }
            }
            catch
            {

            }
        }
    }
}
