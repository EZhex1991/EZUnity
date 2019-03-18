/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-03-06 17:35:28
 * Organization:    #ORGANIZATION#
 * Description:     如果需要加载ScriptableObject，需要写在OnFocus中。如果写在OnEnable中，在退出Unity时未关闭窗口，下次打开时会发现加载不上的情况（5.3.8）
 */
using UnityEditor;

namespace EZUnity
{
    public abstract class EZEditorWindow : EditorWindow
    {
        public void DrawWindowHeader()
        {
            //EditorGUILayout.Space();
            EditorGUILayout.LabelField(titleContent.text, EditorStyles.centeredGreyMiniLabel);
            EZEditorGUIUtility.ScriptTitle(this);
            EditorGUILayout.Space();
        }
    }
}