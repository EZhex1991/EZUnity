/* Author:          ezhex1991@outlook.com
 * CreateTime:      2023-07-01 15:31:10
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZhex1991.EZUnity.EZCollectionAsset;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CreateAssetMenu(
        fileName = "EZMaterialMap",
        menuName = "EZUnity/EZMaterialMap",
        order = (int)EZAssetMenuOrder.EZMaterialMap
        )]
    public class EZMaterialMap : EZMapAsset<string, Material>
    {
        [SerializeField, EZButtonProperty("Set Materials", EZButtonPropertyAttribute.ButtonLayout.Below)]
        private GameObject m_TargetModelAsset;
        public GameObject targetModelAssets { get { return m_TargetModelAsset; } }

        private string modelPath;
        private ModelImporter modelImporter;
        public Dictionary<AssetImporter.SourceAssetIdentifier, Object> externalObjectMap;
        public Dictionary<string, Material> materialMap = new Dictionary<string, Material>();

        public void SetMaterials()
        {
            if (targetModelAssets == null) return;

            if (modelImporter.materialImportMode == ModelImporterMaterialImportMode.None)
            {
                Debug.LogWarning("Material Creation Mode is [None], no changes will made");
                return;
            }

            Undo.RegisterImporterUndo(modelPath, "Set Materials in Model");
            foreach (var pair in this)
            {
                if (!string.IsNullOrEmpty(pair.Key))
                {
                    var materialIdentifier = new AssetImporter.SourceAssetIdentifier(typeof(Material), pair.Key);
                    modelImporter.RemoveRemap(materialIdentifier);
                    if (pair.Value != null)
                    {
                        modelImporter.AddRemap(materialIdentifier, pair.Value);
                    }
                }
            }
            modelImporter.SaveAndReimport();
            GetMaterialIdentifiers();

        }

        private void OnValidate()
        {
            if (targetModelAssets != null)
            {
                modelPath = AssetDatabase.GetAssetPath(targetModelAssets);
                modelImporter = AssetImporter.GetAtPath(modelPath) as ModelImporter;
                GetMaterialIdentifiers();
            }
        }
        public void GetMaterialIdentifiers()
        {
            externalObjectMap = modelImporter.GetExternalObjectMap();

            materialMap.Clear();
            var matHolders = targetModelAssets.GetComponentsInChildren<Renderer>();
            foreach (var render in matHolders)
            {
                foreach (var mat in render.sharedMaterials)
                {
                    if (mat != null)
                    {
                        materialMap[mat.name] = mat;
                    }
                }
            }
        }
    }
}
