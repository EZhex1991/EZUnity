/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-04-15 15:52:18
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZUnity
{
    [InitializeOnLoad]
    public class EZEditorSettings : EZProjectSettings<EZEditorSettings>
    {
        public override string assetPath => "ProjectSettings/EZEditorSettings.asset";

        [Header("Editor Modifier")]
        public bool hierarchyToggleEnabled;

        [Header("Asset Importer")]
        public bool importerPresetEnabled;
        public string audioImporterName = "EZAudioImporter";
        public string[] audioTags = new string[] { "_Mono" };
        public string modelImporterName = "EZModelImporter";
        public string[] modelTags = new string[] { "@" };
        public string textureImporterName = "EZTextureImporter";
        public string[] textureTags = new string[] { "_Sprite", "_Bump", "_AO" };

        static EZEditorSettings()
        {
            EditorApplication.hierarchyWindowItemOnGUI += DrawActiveToggleOnHierarchy;
        }

        private static void DrawActiveToggleOnHierarchy(int instanceID, Rect selectionRect)
        {
            if (!Instance.hierarchyToggleEnabled) return;
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
