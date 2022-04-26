/* Author:          ezhex1991@outlook.com
 * CreateTime:      2022-04-21 15:37:58
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZMeshGenerator
{
    [CreateAssetMenu(fileName = nameof(EZUVSphereGenerator), menuName = nameof(EZMeshGenerator) + "/" + nameof(EZUVSphereGenerator))]
    public class EZUVSphereGenerator : EZMeshGenerator
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
            return string.Format("{0}_{1}x{2}", nameof(EZUVSphereGenerator), subdivision.x, subdivision.y);
        }

        public override Mesh GenerateMesh()
        {
            mesh.Clear();
            mesh.name = GetMeshName();
            SetMesh();

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.normals = normals;
            mesh.tangents = tangents;
            mesh.triangles = triangles;
            return mesh;
        }
        public void SetMesh()
        {
            vertexGrid = subdivision + Vector2Int.one;
            vertexCount = vertexGrid.x * vertexGrid.y - 2;
            mesh.indexFormat = (vertexCount < short.MaxValue)
                ? UnityEngine.Rendering.IndexFormat.UInt16
                : UnityEngine.Rendering.IndexFormat.UInt32;

            vertices = new Vector3[vertexCount];
            uv = new Vector2[vertexCount];
            normals = new Vector3[vertexCount];
            tangents = new Vector4[vertexCount];
            triangles = new int[subdivision.x * (subdivision.y - 1) * 6];

            int vertexIndex = 0;
            for (int x = 0; x < subdivision.x; x++)
            {
                vertices[vertexIndex] = Vector3.down;
                uv[vertexIndex] = new Vector2((x - 0.5f) / subdivision.x, 0);
                normals[vertexIndex] = Vector3.down;
                Vector4 tangent = Quaternion.Euler(0, x / subdivision.x * 360f, 0) * Vector3.left;
                tangent.w = -1;
                tangents[vertexIndex] = tangent;
                vertexIndex++;
            }
            int vertexIndex1 = vertexIndex;
            for (int y = 1; y < subdivision.y; y++)
            {
                Vector3 position;
                position.y = -Mathf.Cos(Mathf.PI / subdivision.y * y);
                float radius = Mathf.Sin(Mathf.PI / subdivision.y * y);
                for (int x = 0; x <= subdivision.x; x++)
                {
                    float u = (float)x / subdivision.x;
                    float v = (float)y / subdivision.y;
                    float degree = Mathf.PI * 2 / subdivision.x * x;
                    position.x = -Mathf.Sin(degree) * radius;
                    position.z = Mathf.Cos(degree) * radius;
                    vertices[vertexIndex] = position;
                    uv[vertexIndex] = new Vector2(u, v);
                    normals[vertexIndex] = position;
                    tangents[vertexIndex] = new Vector4(-Mathf.Cos(degree), 0, -Mathf.Sin(degree));
                    vertexIndex++;
                }
            }
            int vertexIndex2 = vertexIndex;
            for (int x = 0; x < subdivision.x; x++)
            {
                vertices[vertexIndex] = Vector3.up;
                uv[vertexIndex] = new Vector2((x - 0.5f) / subdivision.x, 1);
                normals[vertexIndex] = Vector3.up;
                Vector4 tangent = Quaternion.Euler(0, x / subdivision.x * 360f, 0) * Vector3.left;
                tangent.w = -1;
                tangents[vertexIndex] = tangent;
                vertexIndex++;
            }

            int triangleIndex1 = SetBottomPlane(triangles, 0, 0);
            int triangleIndex2 = SetPlane(triangles, triangleIndex1, vertexIndex1, subdivision.x, subdivision.y - 2);
            int triangleIndex3 = SetTopPlane(triangles, triangleIndex2, vertexIndex2);
        }
        public int SetBottomPlane(int[] triangles, int triangleIndex, int vertexIndex)
        {
            for (int i = 0; i < subdivision.x; i++)
            {
                int v00 = vertexIndex;
                int v01 = vertexIndex + subdivision.x;
                int v11 = v01 + 1;
                triangleIndex = SetTriangle(triangles, triangleIndex, v00, v01, v11);
                vertexIndex++;
            }
            return triangleIndex;
        }
        public int SetTopPlane(int[] triangles, int triangleIndex, int vertexIndex)
        {
            for (int i = 0; i < subdivision.x; i++)
            {
                int v11 = vertexIndex;
                int v10 = vertexIndex - subdivision.x;
                int v00 = v10 - 1;
                triangleIndex = SetTriangle(triangles, triangleIndex, v11, v10, v00);
                vertexIndex++;
            }
            return triangleIndex;
        }

        protected void OnValidate()
        {
            m_Subdivision.x = Mathf.Clamp(m_Subdivision.x, 3, MAX_SUBDIVISION);
            m_Subdivision.y = Mathf.Clamp(m_Subdivision.y, 2, MAX_SUBDIVISION);
        }
    }
}
