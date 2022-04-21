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

            Vector3[] vertices = new Vector3[vertexCount];
            Vector2[] uv = new Vector2[vertexCount];
            Vector3[] normals = new Vector3[vertexCount];
            Vector3 normal = new Vector3(0, 0, -1);
            Vector4[] tangents = new Vector4[vertexCount];
            Vector4 tangent = new Vector4(1, 0, 0, -1);

            int[] triangles = new int[subdivision.x * subdivision.y * 6];
            int triangleIndex = 0;
            int vertexIndex = 0;
            for (int x = 0; x < vertexGrid.x; x++)
            {
                for (int y = 0; y < vertexGrid.y; y++)
                {
                    float locationX = (float)x / subdivision.x;
                    float locationY = (float)y / subdivision.y;
                    vertices[vertexIndex] = new Vector3(locationX - 0.5f, locationY - 0.5f, 0);
                    uv[vertexIndex] = new Vector2(locationX, locationY);
                    normals[vertexIndex] = normal;
                    tangents[vertexIndex] = tangent;

                    if (x != 0 && y != 0)
                    {
                        int rightTop = vertexIndex;
                        int leftTop = vertexIndex - vertexGrid.y;
                        int rightBottom = rightTop - 1;
                        int leftBottom = leftTop - 1;
                        triangleIndex = SetQuad(triangles, triangleIndex, leftBottom, leftTop, rightBottom, rightTop);
                    }
                    vertexIndex++;
                }
            }
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.normals = normals;
            mesh.tangents = tangents;
            mesh.triangles = triangles;
            return mesh;
        }

        protected void OnValidate()
        {
            m_Subdivision.x = Mathf.Clamp(m_Subdivision.x, 1, MAX_SUBDIVISION);
            m_Subdivision.y = Mathf.Clamp(m_Subdivision.y, 1, MAX_SUBDIVISION);
        }
    }
}
