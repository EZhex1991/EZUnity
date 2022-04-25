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

        public static int SetQuad(int[] triangles, int triangleIndex, int v00, int v01, int v10, int v11)
        {
            triangles[triangleIndex] = v00;
            triangles[triangleIndex + 1] = triangles[triangleIndex + 4] = v01;
            triangles[triangleIndex + 2] = triangles[triangleIndex + 3] = v10;
            triangles[triangleIndex + 5] = v11;
            return triangleIndex + 6;
        }
        public static int SetPlane(int[] triangles, int triangleIndex, int vertexIndex, int subdivisionX, int subdivisionY)
        {
            int v00, v01, v10, v11;
            for (int x = 0; x < subdivisionX; x++)
            {
                for (int y = 0; y < subdivisionY; y++)
                {
                    v00 = vertexIndex;
                    v01 = v00 + 1;
                    v10 = v00 + subdivisionY + 1;
                    v11 = v10 + 1;
                    triangleIndex = SetQuad(triangles, triangleIndex, v00, v01, v10, v11);
                    vertexIndex++;
                }
                vertexIndex++;
            }
            return triangleIndex;
        }
    }
}
