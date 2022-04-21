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

        protected virtual void OnEnable()
        {
            meshGenerator = target as EZMeshGenerator;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Generate"))
            {
                Mesh mesh = meshGenerator.GenerateMesh();
                EditorUtility.SetDirty(mesh);
                AssetDatabase.SaveAssets();
                Selection.activeObject = mesh;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
