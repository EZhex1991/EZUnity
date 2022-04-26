/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-10-18 15:05:51
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEngine;

namespace EZhex1991.EZMeshGenerator
{
    [CreateAssetMenu(fileName = nameof(EZPlaneGenerator), menuName = nameof(EZMeshGenerator) + "/" + nameof(EZPlaneGenerator))]
    public class EZPlaneGenerator : EZMeshGenerator
    {
        // hug lag would occur with big numbers
        private const int MAX_SUBDIVISION = 256;

        [SerializeField]
        private Vector2Int m_Subdivision = new Vector2Int(2, 2);
        public Vector2Int subdivision { get { return m_Subdivision; } }

        private Vector2Int vertexGrid;
        private int vertexCount;

        Vector3[] vertices;
        Vector2[] uv;
        Vector3[] normals;
        Vector4[] tangents;
        int[] triangles;

        public override string GetMeshName()
        {
            return string.Format("{0}_{1}x{2}", nameof(EZPlaneGenerator), subdivision.x, subdivision.y);
        }

        public override Mesh GenerateMesh()
        {
            mesh.Clear();
            mesh.name = GetMeshName();

            vertexGrid = subdivision + Vector2Int.one;
            vertexCount = vertexGrid.x * vertexGrid.y;
            mesh.indexFormat = (vertexCount < short.MaxValue)
                ? UnityEngine.Rendering.IndexFormat.UInt16
                : UnityEngine.Rendering.IndexFormat.UInt32;

            vertices = new Vector3[vertexCount];
            uv = new Vector2[vertexCount];
            normals = new Vector3[vertexCount];
            tangents = new Vector4[vertexCount];
            triangles = new int[subdivision.x * subdivision.y * 6];

            Vector3 normal = new Vector3(0, 0, -1);
            Vector4 tangent = new Vector4(1, 0, 0, -1);
            SetLatticeXY(normal, tangent);
            SetPlane(triangles, 0, 0, subdivision.x, subdivision.y);

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.normals = normals;
            mesh.tangents = tangents;
            mesh.triangles = triangles;
            return mesh;
        }
        public int SetLatticeXY(Vector3 normal, Vector3 tangent)
        {
            int vertexIndex = 0;
            for (int y = 0; y <= subdivision.y; y++)
            {
                for (int x = 0; x <= subdivision.x; x++)
                {
                    float u = (float)x / subdivision.x;
                    float v = (float)y / subdivision.y;
                    vertices[vertexIndex] = new Vector3(x, y, 0) - new Vector3(0.5f * subdivision.x, 0.5f * subdivision.y, 0);
                    uv[vertexIndex] = new Vector2(u, v);
                    normals[vertexIndex] = normal;
                    tangents[vertexIndex] = tangent;
                    vertexIndex++;
                }
            }
            return vertexIndex;
        }

        protected void OnValidate()
        {
            m_Subdivision.x = Mathf.Clamp(m_Subdivision.x, 1, MAX_SUBDIVISION);
            m_Subdivision.y = Mathf.Clamp(m_Subdivision.y, 1, MAX_SUBDIVISION);
        }
    }
}
