/* Author:          ezhex1991@outlook.com
 * CreateTime:      2020-04-16 10:44:29
 * Organization:    #ORGANIZATION#
 * Description:     
 */
#if UNITY_2018_3_OR_NEWER
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZAssetImporterManager : EZProjectSettingsSingleton<EZAssetImporterManager>
    {
        [Serializable]
        public class Importer
        {
            [SerializeField]
            private Preset m_Preset;
            public Preset preset { get { return m_Preset; } }
            [SerializeField]
            private DefaultAsset m_Association;
            public DefaultAsset association { get { return m_Association; } }
            [SerializeField]
            private bool m_Recursive;
            public bool recursive { get { return m_Recursive; } }

            public bool Match(string assetPath)
            {
                if (preset == null || association == null) return false;

                string assosiationPath = AssetDatabase.GetAssetPath(association);
                string dirPath = Path.GetDirectoryName(assetPath).Replace('\\', '/');
                if (recursive)
                {
                    return assetPath.StartsWith(assosiationPath);
                }
                else
                {
                    return dirPath == assosiationPath;
                }
            }
        }

        [Serializable]
        public class ModelImporterOverrides
        {
            public bool enabled = false;
            public ModelImporterNormals blendShapeNormals = ModelImporterNormals.Import;
            public bool resampleCurves = false;
            public ModelImporterAnimationCompression animationCompression = ModelImporterAnimationCompression.Off;
        }
        [Serializable]
        public class TextureImporterOverrides
        {
            public bool enabled = false;
            public bool alphaIsTransparency = true;
            public bool mipmapEnabled = false;
        }

        public override string assetPath { get { return "ProjectSettings/EZAssetImporterManager.asset"; } }

        [SerializeField]
        private ModelImporterOverrides m_DefaultModelImporterOverrides = new ModelImporterOverrides();
        public ModelImporterOverrides defaultModelImporterOverrides { get { return m_DefaultModelImporterOverrides; } }
        [SerializeField]
        private Importer[] m_ModelImporters;
        public Importer[] modelImporters { get { return m_ModelImporters; } }

        [SerializeField]
        private TextureImporterOverrides m_DefaultTextureImporterOverrides = new TextureImporterOverrides();
        public TextureImporterOverrides defaultTextureImporterOverrides { get { return m_DefaultTextureImporterOverrides; } }
        [SerializeField]
        private Importer[] m_TextureImporters;
        public Importer[] textureImporters { get { return m_TextureImporters; } }

        [SerializeField]
        private Importer[] m_AudioImporters;
        public Importer[] audioImporters { get { return m_AudioImporters; } }
    }
}
#endif
