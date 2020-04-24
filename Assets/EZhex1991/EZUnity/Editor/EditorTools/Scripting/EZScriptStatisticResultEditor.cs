/* Author:          ezhex1991@outlook.com
 * CreateTime:      2020-04-24 15:52:28
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;
using static EZhex1991.EZUnity.EZScriptStatisticResult;

namespace EZhex1991.EZUnity
{
    [CustomEditor(typeof(EZScriptStatisticResult))]
    public class EZScriptStatisticResultEditor : Editor
    {
        private EZScriptStatisticResult result;

        private Vector2 scrollPosition;

        private void OnEnable()
        {
            result = target as EZScriptStatisticResult;
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EZEditorGUIUtility.ScriptableObjectTitle(target as ScriptableObject, !serializedObject.isEditingMultipleObjects);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Contributors", EditorStyles.boldLabel);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            foreach (Contributor contributor in result.contributors)
            {
                EditorGUILayout.BeginHorizontal();
                float unitWidth = Mathf.Min(60, EditorGUIUtility.currentViewWidth / 10);
                string label = string.Format("{0} - (proportion:{1:P2}, files:{2})", contributor.author, contributor.proportion, contributor.scriptList.Count);
                EditorGUI.indentLevel++;
                contributor.foldout = EditorGUILayout.Foldout(contributor.foldout, label, true);
                EditorGUI.indentLevel--;
                EditorGUILayout.TextField(contributor.lineCount.ToString(), GUILayout.Width(unitWidth));
                EditorGUILayout.TextField(contributor.validLineCount.ToString(), GUILayout.Width(unitWidth));
                EditorGUILayout.EndHorizontal();

                if (contributor.foldout)
                {
                    foreach (ScriptInfo script in contributor.scriptList)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUI.indentLevel++;
                        EditorGUILayout.TextField(script.filePath);
                        EditorGUI.indentLevel--;
                        EditorGUILayout.ObjectField(script.fileObject, typeof(UnityEngine.Object), true, GUILayout.MaxWidth(unitWidth * 2.5f));
                        EditorGUILayout.TextField(script.createTime, GUILayout.MaxWidth(unitWidth * 2.5f));
                        EditorGUILayout.TextField(script.lineCount.ToString(), GUILayout.Width(unitWidth));
                        EditorGUILayout.TextField(script.validLineCount.ToString(), GUILayout.Width(unitWidth));
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
            EditorGUILayout.EndScrollView();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
