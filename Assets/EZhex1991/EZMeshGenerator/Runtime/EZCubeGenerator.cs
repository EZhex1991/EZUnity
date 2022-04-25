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

        [SerializeField]
        private int m_Roundness;
        public int roundness { get { return m_Roundness; } set { m_Roundness = value; } }

        private Vector3Int vertexGrid;
        private Vector3Int innerGrid;
        private int vertexCount;

        Vector3[] vertices;
        Vector2[] uv;
        Vector3[] normals;
        Vector4[] tangents;
        int[] triangles;

        public override string GetMeshName()
        {
            return string.Format("{0}_{1}x{2}x{3}", nameof(EZCubeGenerator), subdivision.x, subdivision.y, subdivision.z);
        }

        public override Mesh GenerateMesh()
        {
            mesh.Clear();
            mesh.name = GetMeshName();
            SetMesh();
            SetRoundness();
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.normals = normals;
            mesh.tangents = tangents;
            mesh.triangles = triangles;
            return mesh;
        }
        private void SetMesh()
        {
            vertexGrid = subdivision + Vector3Int.one;
            int vertexCountXY = vertexGrid.x * vertexGrid.y;
            int vertexCountXZ = vertexGrid.x * vertexGrid.z;
            int vertexCountYZ = vertexGrid.y * vertexGrid.z;
            vertexCount = (vertexCountXY + vertexCountXZ + vertexCountYZ) * 2;

            vertices = new Vector3[vertexCount];
            uv = new Vector2[vertexCount];
            normals = new Vector3[vertexCount];
            tangents = new Vector4[vertexCount];
            triangles = new int[vertexCount * 6];

            Vector3 center = new Vector3(0.5f * subdivision.x, 0.5f * subdivision.y, 0.5f * subdivision.z);
            int vertexIndex0 = 0;
            int triangleIndex0 = 0;
            int vertexIndex1 = SetLatticeXY(vertexIndex0, center);
            int triangleIndex1 = SetPlane(triangles, triangleIndex0, vertexIndex0, subdivision.x, subdivision.y);
            int vertexIndex2 = SetLatticeXY(vertexIndex1, center, true);
            int triangleIndex2 = SetPlane(triangles, triangleIndex1, vertexIndex1, subdivision.x, subdivision.y);
            int vertexIndex3 = SetLatticeZY(vertexIndex2, center);
            int triangleIndex3 = SetPlane(triangles, triangleIndex2, vertexIndex2, subdivision.z, subdivision.y);
            int vertexIndex4 = SetLatticeZY(vertexIndex3, center, true);
            int triangleIndex4 = SetPlane(triangles, triangleIndex3, vertexIndex3, subdivision.z, subdivision.y);
            int vertexIndex5 = SetLatticeXZ(vertexIndex4, center);
            int triangleIndex5 = SetPlane(triangles, triangleIndex4, vertexIndex4, subdivision.x, subdivision.z);
            int vertexIndex6 = SetLatticeXZ(vertexIndex5, center, true);
            int triangleIndex6 = SetPlane(triangles, triangleIndex5, vertexIndex5, subdivision.x, subdivision.z);
        }
        public int SetLatticeXY(int vertexIndex, Vector3 center, bool mirrored = false)
        {
            for (int x = 0; x <= subdivision.x; x++)
            {
                for (int y = 0; y <= subdivision.y; y++)
                {
                    float u = (float)x / subdivision.x;
                    float v = (float)y / subdivision.y;
                    if (mirrored)
                    {
                        vertices[vertexIndex] = new Vector3(center.x - x, y - center.y, center.z);
                        normals[vertexIndex] = Vector3.forward;
                    }
                    else
                    {
                        vertices[vertexIndex] = new Vector3(x - center.x, y - center.y, -center.z);
                        normals[vertexIndex] = Vector3.back;
                    }
                    uv[vertexIndex] = new Vector2(u, v);
                    tangents[vertexIndex] = Vector3.up;
                    vertexIndex++;
                }
            }
            return vertexIndex;
        }
        public int SetLatticeZY(int vertexIndex, Vector3 center, bool mirrored = false)
        {
            for (int z = 0; z <= subdivision.z; z++)
            {
                for (int y = 0; y <= subdivision.y; y++)
                {
                    float u = (float)z / subdivision.z;
                    float v = (float)y / subdivision.y;
                    if (mirrored)
                    {
                        vertices[vertexIndex] = new Vector3(-center.x, y - center.y, center.z - z);
                        normals[vertexIndex] = Vector3.left;
                    }
                    else
                    {
                        vertices[vertexIndex] = new Vector3(center.x, y - center.y, z - center.z);
                        normals[vertexIndex] = Vector3.right;
                    }
                    uv[vertexIndex] = new Vector2(u, v);
                    tangents[vertexIndex] = Vector3.up;
                    vertexIndex++;
                }
            }
            return vertexIndex;
        }
        public int SetLatticeXZ(int vertexIndex, Vector3 center, bool mirrored = false)
        {
            for (int x = 0; x <= subdivision.x; x++)
            {
                for (int z = 0; z <= subdivision.z; z++)
                {
                    float u = (float)x / subdivision.x;
                    float v = (float)z / subdivision.z;
                    if (mirrored)
                    {
                        vertices[vertexIndex] = new Vector3(center.x - x, -center.y, z - center.z);
                        normals[vertexIndex] = Vector3.down;
                    }
                    else
                    {
                        vertices[vertexIndex] = new Vector3(x - center.x, center.y, z - center.z);
                        normals[vertexIndex] = Vector3.up;
                    }
                    uv[vertexIndex] = new Vector2(u, v);
                    tangents[vertexIndex] = Vector3.forward;
                    vertexIndex++;
                }
            }
            return vertexIndex;
        }

        public void SetRoundness()
        {
            if (roundness <= 0) return;
            for (int i = 0; i < vertexCount; i++)
            {
                Vector3 position = vertices[i];
                Vector3 normal = normals[i];
                Vector3 tangent = tangents[i];
                Vector3 inner = position;
                float boundX = subdivision.x * 0.5f - roundness;
                float boundY = subdivision.y * 0.5f - roundness;
                float boundZ = subdivision.z * 0.5f - roundness;
                inner.x = Mathf.Clamp(inner.x, -boundX, boundX);
                inner.y = Mathf.Clamp(inner.y, -boundY, boundY);
                inner.z = Mathf.Clamp(inner.z, -boundZ, boundZ);
                normals[i] = (position - inner).normalized;
                tangents[i] = Quaternion.FromToRotation(normal, normals[i]) * tangent;
                vertices[i] = inner + normals[i] * roundness;
            }
        }

        protected void OnValidate()
        {
            m_Subdivision.x = Mathf.Clamp(m_Subdivision.x, 1, MAX_SUBDIVISION);
            m_Subdivision.y = Mathf.Clamp(m_Subdivision.y, 1, MAX_SUBDIVISION);
            m_Subdivision.z = Mathf.Clamp(m_Subdivision.z, 1, MAX_SUBDIVISION);
            int minComponent = Mathf.Min(Mathf.Min(m_Subdivision.x, m_Subdivision.y), m_Subdivision.z);
            m_Roundness = Mathf.Clamp(m_Roundness, 0, minComponent / 2);
        }
    }
}
