/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-10-08 11:20:03
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZUnity
{
    public class EZGuidGenerator : EZEditorWindow
    {
        public System.Guid systemGuid;
        public UnityEditor.GUID unityGuid;

        public void RefreshGuid()
        {
            systemGuid = System.Guid.NewGuid();
            unityGuid = UnityEditor.GUID.Generate();
        }

        protected void OnEnable()
        {
            RefreshGuid();
        }

        protected void OnGUI()
        {
            DrawWindowHeader();
            if (GUILayout.Button("Refresh"))
            {
                RefreshGuid();
            }
            EditorGUILayout.PrefixLabel("System Guid");
            EditorGUILayout.TextArea(systemGuid.ToString());
            EditorGUILayout.PrefixLabel("Unity Guid");
            EditorGUILayout.TextArea(unityGuid.ToString());
        }
    }
}
