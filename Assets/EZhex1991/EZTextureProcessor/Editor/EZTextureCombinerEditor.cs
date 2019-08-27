/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-05-15 15:49:15
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZTextureProcessor
{
    [CustomEditor(typeof(EZTextureCombiner))]
    public class EZTextureCombinerEditor : EZTextureGeneratorEditor
    {
        private SerializedProperty m_CellSize;
        private SerializedProperty m_InputTextures;
        private SerializedProperty[,] inputTextures = new SerializedProperty[6, 6];

        protected override void GetInputProperties()
        {
            base.GetInputProperties();
            m_CellSize = serializedObject.FindProperty("cellSize");
            m_InputTextures = serializedObject.FindProperty("inputTextures");
            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    int index = y * 6 + x;
                    inputTextures[x, y] = m_InputTextures.GetArrayElementAtIndex(index);
                }
            }
        }
        protected override void DrawInputSettings()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_CellSize);
            Vector2Int cellSize = m_CellSize.vector2IntValue;
            if (EditorGUI.EndChangeCheck())
            {
                cellSize.x = Mathf.Clamp(cellSize.x, 1, 6);
                cellSize.y = Mathf.Clamp(cellSize.y, 1, 6);
                m_CellSize.vector2IntValue = cellSize;
            }

            float width = EditorGUIUtility.currentViewWidth / 6 - 15;
            for (int y = 5; y >= 0; y--)
            {
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < 6; x++)
                {
                    EditorGUILayout.BeginVertical();
                    if (x < cellSize.x && y < cellSize.y)
                    {
                        EditorGUILayout.LabelField(string.Format("[{0},{1}]", x, y), GUILayout.Width(width));
                    }
                    else
                    {
                        EditorGUILayout.LabelField("", GUILayout.Width(width));
                    }
                    EditorGUILayout.PropertyField(inputTextures[x, y], GUIContent.none, GUILayout.Width(width));
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
