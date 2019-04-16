/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-07-17 14:15:07
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.IO;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

namespace EZUnity
{
    public class EZAssetPostprocessor : AssetPostprocessor
    {
        public EZEditorSettings settings { get { return EZEditorSettings.Instance; } }

        public string dirPath { get { return Path.GetDirectoryName(assetPath); } }
        public string dirName { get { return dirPath.Substring(dirPath.LastIndexOf("/") + 1); } }
        public string assetName { get { return Path.GetFileName(assetPath); } }

        private void ImportWithPreset(string importerName, string[] importerTags)
        {
            if (string.IsNullOrEmpty(importerName)) return;
            for (int i = 0; i < importerTags.Length; i++)
            {
                string tag = importerTags[i];
                if (string.IsNullOrEmpty(tag)) continue;
                if (assetName.Contains(tag))
                {
                    importerName += tag;
                    break;
                }
            }

            string dir = dirPath;
            while (!string.IsNullOrEmpty(dir))
            {
                string presetPath = string.Format("{0}/{1}.preset", dir, importerName);
                Preset preset = AssetDatabase.LoadAssetAtPath<Preset>(presetPath);
                if (preset != null)
                {
                    if (preset.ApplyTo(assetImporter))
                    {
                        Debug.LogFormat("Applied {0} To {1}", presetPath, assetPath);
                    }
                    else
                    {
                        Debug.LogWarningFormat("Failed To Apply {0} To {1}", presetPath, assetPath);
                    }
                    return;
                }
                // search in the parent folder
                dir = Path.GetDirectoryName(dir);
            }
        }

        private void OnPreprocessAsset()
        {
            if (!settings.importerPresetEnabled) return;
            if (assetImporter.importSettingsMissing)
            {
            }
        }

        private void OnPreprocessTexture()
        {
            if (assetImporter.importSettingsMissing && settings.importerPresetEnabled)
            {
                ImportWithPreset(settings.textureImporterName, settings.textureTags);
            }
        }

        private void OnPreprocessModel()
        {
            if (assetImporter.importSettingsMissing && settings.importerPresetEnabled)
            {
                ImportWithPreset(settings.modelImporterName, settings.modelTags);
            }
        }

        private void OnPreprocessAudio()
        {
            if (assetImporter.importSettingsMissing && settings.importerPresetEnabled)
            {
                ImportWithPreset(settings.audioImporterName, settings.audioTags);
            }
        }
    }
}