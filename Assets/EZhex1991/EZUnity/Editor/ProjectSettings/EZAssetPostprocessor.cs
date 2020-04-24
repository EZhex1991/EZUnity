/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-07-17 14:15:07
 * Organization:    #ORGANIZATION#
 * Description:     
 */
#if UNITY_2018_3_OR_NEWER
using System.IO;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZAssetPostprocessor : AssetPostprocessor
    {
        public EZAssetImporterManager importerManager { get { return EZAssetImporterManager.Instance; } }

        public string dirPath { get { return Path.GetDirectoryName(assetPath); } }
        public string dirName { get { return dirPath.Substring(dirPath.LastIndexOf("/") + 1); } }
        public string fileName { get { return Path.GetFileName(assetPath); } }
        public string assetName { get { return Path.GetFileNameWithoutExtension(assetPath); } }

        private void ImportWithPreset(string importerName, string[] importerTags)
        {
            if (string.IsNullOrEmpty(importerName)) return;
            string taggedImporterName = importerName;
            for (int i = 0; i < importerTags.Length; i++)
            {
                string tag = importerTags[i];
                if (string.IsNullOrEmpty(tag)) continue;
                if (fileName.Contains(tag))
                {
                    taggedImporterName += tag;
                    break;
                }
            }

            string dir = dirPath;
            while (!string.IsNullOrEmpty(dir))
            {
                string presetPath = string.Format("{0}/{1}.preset", dir, taggedImporterName);
                Preset preset = AssetDatabase.LoadAssetAtPath<Preset>(presetPath);
                if (preset == null)
                {
                    presetPath = string.Format("{0}/{1}.preset", dir, importerName);
                    preset = AssetDatabase.LoadAssetAtPath<Preset>(presetPath);
                }
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
        private bool MatchImporter(EZAssetImporterManager.Importer[] importers, string targetTypeName)
        {
            foreach (var importer in importers)
            {
                if (importer.preset.GetTargetFullTypeName() != targetTypeName)
                {
                    Debug.LogError("Importer type mismatch in EZAssetImporterManager");
                    continue;
                }
                if (importer.Match(assetPath))
                {
                    importer.preset.ApplyTo(assetImporter);
                    return true;
                }
            }
            return false;
        }

        private void OnPreprocessTexture()
        {
            if (assetImporter.importSettingsMissing)
            {
                MatchImporter(importerManager.textureImporters, typeof(TextureImporter).FullName);
                var overrides = importerManager.defaultTextureImporterOverrides;
                if (overrides.enabled)
                {
                    var textureImporter = assetImporter as TextureImporter;
                    textureImporter.alphaIsTransparency = overrides.alphaIsTransparency;
                    textureImporter.mipmapEnabled = overrides.mipmapEnabled;
                }
            }
        }
        private void OnPreprocessModel()
        {
            if (assetImporter.importSettingsMissing)
            {
                if (MatchImporter(importerManager.modelImporters, "UnityEditor.FBXImporter")) return;
                var overrides = importerManager.defaultModelImporterOverrides;
                if (overrides.enabled)
                {
                    var modelImporter = assetImporter as ModelImporter;
                    modelImporter.importBlendShapeNormals = overrides.blendShapeNormals;
                    modelImporter.resampleCurves = overrides.resampleCurves;
                    modelImporter.animationCompression = overrides.animationCompression;
                }
            }
        }
        private void OnPreprocessAudio()
        {
            if (assetImporter.importSettingsMissing)
            {
                MatchImporter(importerManager.audioImporters, typeof(AudioImporter).FullName);
            }
        }
    }
}
#endif
