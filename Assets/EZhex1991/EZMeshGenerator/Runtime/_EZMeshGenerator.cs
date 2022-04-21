/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-10-18 15:05:51
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZMeshGenerator
{
    public abstract class EZMeshGenerator : ScriptableObject
    {
        // Do not store multi meshes in one generator, lag could occur
        [SerializeField]
        private Mesh m_Mesh;
        public Mesh mesh
        {
            get
            {
                if (m_Mesh == null)
                {
                    m_Mesh = new Mesh();
                    m_Mesh.name = "Empty Mesh";
#if UNITY_EDITOR
                    UnityEditor.AssetDatabase.AddObjectToAsset(m_Mesh, this);
                    UnityEditor.EditorUtility.SetDirty(this);
#endif
                }
                return m_Mesh;
            }
        }

        public abstract string GetMeshName();
        public abstract Mesh GenerateMesh();

        public static int SetQuad(int[] triangles, int i, int v00, int v01, int v10, int v11)
        {
            triangles[i] = v00;
            triangles[i + 1] = triangles[i + 4] = v01;
            triangles[i + 2] = triangles[i + 3] = v10;
            triangles[i + 5] = v11;
            return i + 6;
        }
    }
}
