/*
 * Author:      熊哲
 * CreateTime:  7/17/2017 2:15:07 PM
 * Description:
 * 
*/
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EZUnityEditor
{
    public class EZAssetPostprocessor : UnityEditor.AssetPostprocessor
    {
        string dirPath { get { return Path.GetDirectoryName(assetPath); } }
        string dirName { get { return dirPath.Substring(dirPath.LastIndexOf("/") + 1); } }
        string assetName { get { return Path.GetFileName(assetPath); } }

        void OnPreprocessTexture()
        {
            TextureImporter textureImporter = (TextureImporter)assetImporter;
            if (assetName.ToLower().StartsWith("sprite"))
            {
                textureImporter.mipmapEnabled = false;
                if (dirName.StartsWith("[") && dirName.EndsWith("]"))
                {
                    textureImporter.spritePackingTag = dirName.Substring(1, dirName.Length - 2);
                }
                // sprite_spriteName
                if (assetName.ToLower().StartsWith("sprite_"))
                {
#if UNITY_2017
                    textureImporter.textureType = TextureImporterType.Sprite;
#else
                    textureImporter.textureType = TextureImporterType.Sprite;
                    textureImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;
#endif
                }
                // sprite@_spriteName
                else if (assetName.ToLower().StartsWith("sprite@_"))
                {
#if UNITY_2017
                    textureImporter.textureType = TextureImporterType.Sprite;
#else
                    textureImporter.textureType = TextureImporterType.Advanced;
                    textureImporter.npotScale = TextureImporterNPOTScale.None;
                    textureImporter.spriteImportMode = SpriteImportMode.Single;
#endif
                }
                // sprite@RGBA32_spriteName
                else if (assetName.ToLower().StartsWith("sprite@rgba32_"))
                {
#if UNITY_2017
                    textureImporter.textureType = TextureImporterType.Sprite;
                    textureImporter.SetPlatformTextureSettings(new TextureImporterPlatformSettings
                    {
                        name = "Standalone",
                        overridden = true,
                        format = TextureImporterFormat.RGBA32,
                    }); textureImporter.SetPlatformTextureSettings(new TextureImporterPlatformSettings
                    {
                        name = "iPhone",
                        overridden = true,
                        format = TextureImporterFormat.RGBA32,
                    }); textureImporter.SetPlatformTextureSettings(new TextureImporterPlatformSettings
                    {
                        name = "Android",
                        overridden = true,
                        format = TextureImporterFormat.RGBA32,
                    });
#else
                    textureImporter.textureType = TextureImporterType.Advanced;
                    textureImporter.npotScale = TextureImporterNPOTScale.None;
                    textureImporter.spriteImportMode = SpriteImportMode.Single;
                    textureImporter.textureFormat = TextureImporterFormat.RGBA32;
#endif
                }
                // sprite@RGB24_spriteName
                else if (assetName.ToLower().StartsWith("sprite@rgb24_"))
                {
#if UNITY_2017
                    textureImporter.textureType = TextureImporterType.Sprite;
                    textureImporter.SetPlatformTextureSettings(new TextureImporterPlatformSettings
                    {
                        name = "Standalone",
                        overridden = true,
                        format = TextureImporterFormat.RGB24
                    });
                    textureImporter.SetPlatformTextureSettings(new TextureImporterPlatformSettings
                    {
                        name = "iPhone",
                        overridden = true,
                        format = TextureImporterFormat.RGB24
                    });
                    textureImporter.SetPlatformTextureSettings(new TextureImporterPlatformSettings
                    {
                        name = "Android",
                        overridden = true,
                        format = TextureImporterFormat.RGB24
                    });
#else
                    textureImporter.textureType = TextureImporterType.Advanced;
                    textureImporter.npotScale = TextureImporterNPOTScale.None;
                    textureImporter.spriteImportMode = SpriteImportMode.Single;
                    textureImporter.textureFormat = TextureImporterFormat.RGB24;
#endif
                }
            }
            // textureName_normalMap
            if (assetPath.ToLower().Contains("normalmap"))
            {
#if UNITY_2017
                textureImporter.textureType = TextureImporterType.NormalMap;
#else
                textureImporter.textureType = TextureImporterType.Bump;
#endif
            }
            // textureName_bumpMap
            else if (assetPath.ToLower().Contains("bumpmap"))
            {
#if UNITY_2017
                textureImporter.textureType = TextureImporterType.NormalMap;
                textureImporter.convertToNormalmap = true;
#else
                textureImporter.textureType = TextureImporterType.Bump;
                textureImporter.convertToNormalmap = true;
#endif
            }
        }
        void OnPostprocessTexture(Texture2D texture)
        {

        }
        void OnPostprocessSprites(Texture2D texture, Sprite[] sprites)
        {

        }

        void OnPreprocessModel()
        {
            ModelImporter modelImporter = (ModelImporter)assetImporter;
            modelImporter.globalScale = 100;
            modelImporter.importMaterials = false;
            modelImporter.animationType = ModelImporterAnimationType.None;
            // modelName@animationName
            if (assetPath.Contains("@"))
            {
                modelImporter.animationType = ModelImporterAnimationType.Generic;
            }
            // modelName_collider
            if (assetPath.ToLower().Contains("collider"))
            {
                modelImporter.addCollider = true;
                modelImporter.animationType = ModelImporterAnimationType.None;
            }
        }
        void OnPostprocessModel(GameObject gameObject)
        {
            if (gameObject.name.ToLower().Contains("collider"))
            {
                gameObject.AddComponent<MeshCollider>();
                MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
                if (mr != null)
                {
                    mr.enabled = false;
                }
            }
            foreach (Transform tr in gameObject.transform)
            {
                OnPostprocessModel(tr.gameObject);
            }
        }
        Material OnAssignMaterialModel(Material material, Renderer renderer)
        {
            if (material.name == "" // 未指定材质
                    || material.name == ("Mat") || material.name.StartsWith("Mat.") // C4D默认材质
                    || material.name.StartsWith("lambert")  // Maya默认材质
                    || material.name.EndsWith(" - Default") // 3dMax默认材质
            )
            {
                Debug.Log("Invalid Material Name: " + material.name);
                return EZAssetGenerator.GenerateMaterial();
            }
            return null;
        }

        void OnPostprocessAudio(AudioClip audioClip)
        {
            AudioImporter audioImporter = (AudioImporter)assetImporter;
            // audioName_mono
            if (assetPath.ToLower().Contains("_mono"))
            {
                audioImporter.forceToMono = true;
            }
        }
    }
}