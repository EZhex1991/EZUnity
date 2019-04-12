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
                    Shader wireframeShader = Shader.Find("VR/SpatialMapping/Wireframe");
                    if (wireframeShader == null)
                    {
                        m_PreviewMaterial = EZEditorUtility.defaultMaterial;
                    }
                    else
                    {
                        m_PreviewMaterial = new Material(wireframeShader);
                        m_PreviewMaterial.hideFlags = HideFlags.HideAndDontSave;
                    }
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
                path = path.Substring(0, path.Length - ".asset".Length) + ".mesh";
                AssetDatabase.CreateAsset(Instantiate(generator.targetMesh), AssetDatabase.GenerateUniqueAssetPath(path));
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
            preview.DrawMesh(generator.targetMesh, Matrix4x4.identity, previewMaterial, 0);
            preview.camera.transform.SetPositionAndRotation(Vector3.forward * -5, Quaternion.identity);
            preview.Render();
            Texture texture = preview.EndPreview();
            GUI.DrawTexture(r, texture, ScaleMode.StretchToFill, true);
        }
    }
}
