/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-10-18 15:09:15
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZUnity
{
    [CustomEditor(typeof(EZMeshGenerator), true)]
    public class EZMeshGeneratorEditor : Editor
    {
        private static Material m_PreviewMaterial;
        protected static Material previewMaterial
        {
            get
            {
                if (m_PreviewMaterial == null)
                {
                    m_PreviewMaterial = new Material(Shader.Find("VR/SpatialMapping/Wireframe"));
                    m_PreviewMaterial.SetInt("_WireThickness", 400);
                }
                return m_PreviewMaterial;
            }
        }

        protected EZMeshGenerator generator;
        protected PreviewRenderUtility preview;

        protected virtual void OnEnable()
        {
            generator = target as EZMeshGenerator;
            preview = new PreviewRenderUtility();
        }
        protected virtual void OnDisable()
        {
            preview.Cleanup();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Generate Mesh"))
            {
                string path = AssetDatabase.GetAssetPath(target);
                AssetDatabase.ImportAsset(path); // unity bug (2018.2.7), Reset option set asset name to empty
                string assetName = "New Mesh.mesh";
                path = path.Substring(0, path.LastIndexOf('/') + 1) + assetName;
                AssetDatabase.CreateAsset(Instantiate(generator.mesh), AssetDatabase.GenerateUniqueAssetPath(path));
            }
        }

        public override bool HasPreviewGUI()
        {
            return true;
        }
        public override bool RequiresConstantRepaint()
        {
            return false;
        }
        public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
        {
            preview.BeginPreview(r, background);
            preview.DrawMesh(generator.mesh, Matrix4x4.identity, previewMaterial, 0);
            preview.camera.transform.SetPositionAndRotation(Vector3.forward * -5, Quaternion.identity);
            preview.Render();
            Texture texture = preview.EndPreview();
            GUI.DrawTexture(r, texture, ScaleMode.StretchToFill, true);
        }
    }
}
