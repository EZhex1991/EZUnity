/*
 * Author:      熊哲
 * CreateTime:  7/17/2017 2:15:07 PM
 * Description:
 * 
*/
using UnityEditor;
using UnityEngine;

namespace EZUnityTools.EZEditor
{
    public class EZAssetPostprocessor : UnityEditor.AssetPostprocessor
    {
        void OnPreprocessTexture()
        {
            // sprite_spriteName
            if (assetPath.ToLower().Contains("sprite_"))
            {
                TextureImporter textureImporter = (TextureImporter)assetImporter;
                textureImporter.textureType = TextureImporterType.Sprite;
                textureImporter.mipmapEnabled = false;
                textureImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;
            }
            // textureName_normalMap
            if (assetPath.ToLower().Contains("normalmap"))
            {
                TextureImporter textureImporter = (TextureImporter)assetImporter;
                textureImporter.textureType = TextureImporterType.Bump;
            }
            // textureName_bumpMap
            if (assetPath.ToLower().Contains("bumpmap"))
            {
                TextureImporter textureImporter = (TextureImporter)assetImporter;
                textureImporter.textureType = TextureImporterType.Bump;
                textureImporter.convertToNormalmap = true;
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
            // modelName@animationName
            if (assetPath.Contains("@"))
            {
                ModelImporter modelImporter = (ModelImporter)assetImporter;
                modelImporter.importMaterials = false;
            }
            // modelName_collider
            if (assetPath.ToLower().Contains("collider"))
            {
                ModelImporter modelImporter = (ModelImporter)assetImporter;
                modelImporter.importMaterials = false;
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
            // audioName_mono
            if (assetPath.ToLower().Contains("_mono"))
            {
                AudioImporter audioImporter = (AudioImporter)assetImporter;
                audioImporter.forceToMono = true;
            }
        }
    }
}