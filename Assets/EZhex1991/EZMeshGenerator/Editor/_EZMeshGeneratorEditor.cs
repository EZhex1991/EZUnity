/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-10-18 15:09:15
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZMeshGenerator
{
    [CustomEditor(typeof(EZMeshGenerator), true)]
    public class EZMeshGeneratorEditor : Editor
    {
        protected EZMeshGenerator meshGenerator;
        protected Editor meshEditor;

        protected virtual void OnEnable()
        {
            meshGenerator = target as EZMeshGenerator;
            meshEditor = CreateEditor(meshGenerator.mesh);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Generate"))
            {
                Mesh mesh = meshGenerator.GenerateMesh();
                EditorUtility.SetDirty(mesh);
                AssetDatabase.SaveAssets();
            }

            serializedObject.ApplyModifiedProperties();
        }

        public override bool HasPreviewGUI()
        {
            return meshGenerator.mesh != null;
        }
        public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
        {
            meshEditor.OnInteractivePreviewGUI(r, background);
        }
    }
}
