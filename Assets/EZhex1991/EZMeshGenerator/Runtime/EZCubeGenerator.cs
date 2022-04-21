/* Author:          ezhex1991@outlook.com
 * CreateTime:      2022-04-21 15:37:58
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZMeshGenerator
{
    [CreateAssetMenu(fileName = nameof(EZCubeGenerator), menuName = nameof(EZMeshGenerator) + "/" + nameof(EZCubeGenerator))]
    public class EZCubeGenerator : EZMeshGenerator
    {
        // hug lag would occur with big numbers
        private const int MAX_SUBDIVISION = 256;

        [SerializeField]
        private Vector3Int m_Subdivision = new Vector3Int(2, 2, 2);
        public Vector3Int subdivision { get { return m_Subdivision; } }

        private Vector3Int vertexGrid;
        private Vector3Int innerGrid;
        private int vertexCount;

        public override string GetMeshName()
        {
            return string.Format("{0}_{1}x{2}x{3}", nameof(EZCubeGenerator), subdivision.x, subdivision.y, subdivision.z);
        }

        public override Mesh GenerateMesh()
        {
            mesh.Clear();
            mesh.name = GetMeshName();
            CreateVertices();
            CreateTriangles();
            return mesh;
        }
        private void CreateVertices()
        {
            vertexGrid = subdivision + Vector3Int.one;
            innerGrid = subdivision - Vector3Int.one;

            int cornerVertices = 8;
            int edgeVertices = (innerGrid.x + innerGrid.y + innerGrid.z) * 4;
            int faceVertices = 2 * (innerGrid.x * innerGrid.y
                + innerGrid.x * innerGrid.z
                + innerGrid.y * innerGrid.z);
            vertexCount = cornerVertices + edgeVertices + faceVertices;
            mesh.indexFormat = (vertexCount < short.MaxValue)
                ? UnityEngine.Rendering.IndexFormat.UInt16
                : UnityEngine.Rendering.IndexFormat.UInt32;

            Vector3[] vertices = new Vector3[vertexCount];
            Vector2[] uv = new Vector2[vertexCount];
            Vector3[] normals = new Vector3[vertexCount];
            Vector3 normal = new Vector3(0, 0, -1);
            Vector4[] tangents = new Vector4[vertexCount];
            Vector4 tangent = new Vector4(1, 0, 0, -1);

            int vertexIndex = 0;
            for (int y = 0; y < vertexGrid.y; y++)
            {
                for (int x = 0; x < vertexGrid.x; x++)
                {
                    vertices[vertexIndex++] = new Vector3(x, y, 0);
                }
                for (int z = 1; z < vertexGrid.z; z++)
                {
                    vertices[vertexIndex++] = new Vector3(subdivision.x, y, z);
                }
                for (int x = innerGrid.x; x >= 0; x--)
                {
                    vertices[vertexIndex++] = new Vector3(x, y, subdivision.z);
                }
                for (int z = innerGrid.z; z > 0; z--)
                {
                    vertices[vertexIndex++] = new Vector3(0, y, z);
                }
            }
            for (int z = 1; z < subdivision.z; z++)
            {
                for (int x = 1; x < subdivision.x; x++)
                {
                    vertices[vertexIndex++] = new Vector3(x, subdivision.y, z);
                }
            }
            for (int z = 1; z < subdivision.z; z++)
            {
                for (int x = 1; x < subdivision.x; x++)
                {
                    vertices[vertexIndex++] = new Vector3(x, 0, z);
                }
            }
            mesh.vertices = vertices;
        }
        private void CreateTriangles()
        {
            int quads = (subdivision.x * subdivision.y + subdivision.x * subdivision.z + subdivision.y * subdivision.z) * 2;
            int[] triangles = new int[quads * 6];

            int ring = (subdivision.x + subdivision.z) * 2;
            int triangleIndex = 0, vertexIndex = 0;
            int v00, v01, v10, v11;
            for (int y = 0; y < subdivision.y; y++, vertexIndex++)
            {
                for (int quadIndex = 0; quadIndex < ring - 1; quadIndex++, vertexIndex++)
                {
                    v00 = vertexIndex;
                    v01 = vertexIndex + ring;
                    v10 = v00 + 1;
                    v11 = v01 + 1;
                    triangleIndex = SetQuad(triangles, triangleIndex, v00, v01, v10, v11);
                }
                v00 = vertexIndex;
                v01 = vertexIndex + ring;
                v10 = v00 + 1 - ring;
                v11 = v01 + 1 - ring;
                triangleIndex = SetQuad(triangles, triangleIndex, v00, v01, v10, v11);
            }
            triangleIndex = CreateTopFace(triangles, triangleIndex, ring);
            triangleIndex = CreateBottomFace(triangles, triangleIndex, ring);

            mesh.triangles = triangles;
        }
        private int CreateTopFace(int[] triangles, int triangleIndex, int ring)
        {
            int vertexIndex = ring * subdivision.y;
            int v00, v01, v10, v11;
            for (int x = 0; x < innerGrid.x; x++, vertexIndex++)
            {
                v00 = vertexIndex;
                v01 = v00 + ring - 1;
                v10 = v00 + 1;
                v11 = v00 + ring;
                triangleIndex = SetQuad(triangles, triangleIndex, v00, v01, v10, v11);
            }
            v00 = vertexIndex;
            v01 = v00 + ring - 1;
            v10 = v00 + 1;
            v11 = v10 + 1;
            triangleIndex = SetQuad(triangles, triangleIndex, v00, v01, v10, v11);

            int vMin = ring * vertexGrid.y - 1;
            int vMid = vMin + 1;
            int vMax = vertexIndex + 2;
            for (int z = 1; z < innerGrid.z; z++, vMin--, vMid++, vMax++)
            {
                v00 = vMin;
                v01 = vMin - 1;
                v10 = vMid;
                v11 = vMid + innerGrid.x;
                triangleIndex = SetQuad(triangles, triangleIndex, v00, v01, v10, v11);
                for (int x = 1; x < innerGrid.x; x++, vMid++)
                {
                    v00 = vMid;
                    v01 = vMid + innerGrid.x;
                    v10 = v00 + 1;
                    v11 = v01 + 1;
                    triangleIndex = SetQuad(triangles, triangleIndex, v00, v01, v10, v11);
                }
                v00 = vMid;
                v01 = vMid + innerGrid.x;
                v10 = vMax;
                v11 = vMax + 1;
                triangleIndex = SetQuad(triangles, triangleIndex, v00, v01, v10, v11);
            }

            int vTop = vMin - 2;
            v00 = vMin;
            v01 = vTop + 1;
            v10 = vMid;
            v11 = vTop;
            triangleIndex = SetQuad(triangles, triangleIndex, v00, v01, v10, v11);
            for (int x = 1; x < innerGrid.x; x++, vTop--, vMid++)
            {
                v00 = vMid;
                v01 = vTop;
                v10 = v00 + 1;
                v11 = v01 - 1;
                triangleIndex = SetQuad(triangles, triangleIndex, v00, v01, v10, v11);
            }
            v00 = vMid;
            v01 = vTop;
            v10 = vTop - 2;
            v11 = vTop - 1;
            triangleIndex = SetQuad(triangles, triangleIndex, v00, v01, v10, v11);

            return triangleIndex;
        }
        private int CreateBottomFace(int[] triangles, int triangleIndex, int ring)
        {
            int vertexIndex = 1;
            int v00, v01, v10, v11;

            int vMid = vertexCount - innerGrid.x * innerGrid.z;

            // flip vertical
            v00 = ring - 1;
            v01 = 0;
            v10 = vMid;
            v11 = 1;
            triangleIndex = SetQuad(triangles, triangleIndex, v00, v01, v10, v11);
            for (int x = 1; x < innerGrid.x; x++, vertexIndex++, vMid++)
            {
                v00 = vMid;
                v01 = vertexIndex;
                v10 = v00 + 1;
                v11 = v01 + 1;
                triangleIndex = SetQuad(triangles, triangleIndex, v00, v01, v10, v11);
            }
            v00 = vMid;
            v01 = vertexIndex;
            v10 = vertexIndex + 2;
            v11 = vertexIndex + 1;
            triangleIndex = SetQuad(triangles, triangleIndex, v00, v01, v10, v11);

            int vMin = ring - 2;
            vMid -= subdivision.x - 2;
            int vMax = vertexIndex + 2;
            for (int z = 1; z < innerGrid.z; z++, vMin--, vMid++, vMax++)
            {
                v00 = vMin;
                v01 = vMin + 1;
                v10 = vMid + innerGrid.x;
                v11 = vMid;
                triangleIndex = SetQuad(triangles, triangleIndex, v00, v01, v10, v11);
                for (int x = 1; x < innerGrid.x; x++, vMid++)
                {
                    v00 = vMid + innerGrid.x;
                    v01 = vMid;
                    v10 = v00 + 1;
                    v11 = v01 + 1;
                    triangleIndex = SetQuad(triangles, triangleIndex, v00, v01, v10, v11);
                }
                v00 = vMid + innerGrid.x;
                v01 = vMid;
                v10 = vMax + 1;
                v11 = vMax;
                triangleIndex = SetQuad(triangles, triangleIndex, v00, v01, v10, v11);
            }

            int vTop = vMin - 1;
            v00 = vMin;
            v01 = v00 + 1;
            v10 = vTop;
            v11 = vMid;
            triangleIndex = SetQuad(triangles, triangleIndex, v00, v01, v10, v11);
            for (int x = 1; x < innerGrid.x; x++, vTop--, vMid++)
            {
                v00 = vTop;
                v01 = vMid;
                v10 = v00 - 1;
                v11 = v01 + 1;
                triangleIndex = SetQuad(triangles, triangleIndex, v00, v01, v10, v11);
            }
            v00 = vTop;
            v01 = vMid;
            v10 = vTop - 1;
            v11 = vTop - 2;
            triangleIndex = SetQuad(triangles, triangleIndex, v00, v01, v10, v11);

            return triangleIndex;
        }

        protected void OnValidate()
        {
            m_Subdivision.x = Mathf.Clamp(m_Subdivision.x, 1, MAX_SUBDIVISION);
            m_Subdivision.y = Mathf.Clamp(m_Subdivision.y, 1, MAX_SUBDIVISION);
            m_Subdivision.z = Mathf.Clamp(m_Subdivision.z, 1, MAX_SUBDIVISION);
        }
    }
}
