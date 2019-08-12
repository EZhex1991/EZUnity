/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-04-15 15:52:18
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [InitializeOnLoad]
    public class EZEditorSettings : EZProjectSettingsSingleton<EZEditorSettings>
    {
        public override string assetPath { get { return "ProjectSettings/EZEditorSettings.asset"; } }

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
#if UNITY_2019_1_OR_NEWER
                Rect activeRect = new Rect(selectionRect.x - 27, selectionRect.y, selectionRect.height, selectionRect.height);
#elif UNITY_2019_1_0
                Rect activeRect = new Rect(selectionRect.x - 3, selectionRect.y, selectionRect.height, selectionRect.height);
#else
                Rect activeRect = new Rect(selectionRect.x - 28, selectionRect.y, selectionRect.height, selectionRect.height);
#endif
                EditorGUI.BeginChangeCheck();
                bool active = EditorGUI.Toggle(activeRect, gameObject.activeSelf);
                if (EditorGUI.EndChangeCheck())
                {
                    SetActive(gameObject, active);
                }
            }
        }

        private static void SetActive(GameObject gameObject, bool active)
        {
            GameObject[] selections = Selection.GetFiltered<GameObject>(SelectionMode.Editable | SelectionMode.ExcludePrefab);
            if (selections.Contains(gameObject))
            {
                Undo.RecordObjects(selections, "Set Active");
                for (int i = 0; i < selections.Length; i++)
                {
                    selections[i].SetActive(active);
                }
            }
            else
            {
                Undo.RecordObject(gameObject, "Set Active");
                gameObject.SetActive(active);
            }
        }
    }
}
