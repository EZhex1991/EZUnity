/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-10-18 15:05:51
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZUnity
{
    [CreateAssetMenu(fileName = "EZPlaneGenerator", menuName = "EZUnity/EZPlaneGenerator", order = EZUtility.AssetOrder)]
    public class EZPlaneGenerator : EZMeshGenerator
    {
        [SerializeField]
        private Vector2Int m_Subdivision = new Vector2Int(2, 2);
        public Vector2Int subdivision { get { return m_Subdivision; } }
        public Vector2Int vertexCount { get { return m_Subdivision + Vector2Int.one; } }

        public override void GenerateMesh()
        {
            mesh.Clear();
            Vector3[] vertices = new Vector3[vertexCount.x * vertexCount.y];
            Vector2[] uv = new Vector2[vertexCount.x * vertexCount.y];
            int[] triangles = new int[(subdivision.x) * (subdivision.y) * 6];
            int triangleIndex = 0;
            for (int x = 0; x < vertexCount.x; x++)
            {
                for (int y = 0; y < vertexCount.y; y++)
                {
                    int vertexIndex = x * vertexCount.y + y;
                    vertices[vertexIndex] = new Vector3(((float)x / subdivision.x - 0.5f), ((float)y / subdivision.y - 0.5f), 0);
                    uv[vertexIndex] = new Vector2((float)x / subdivision.x, (float)y / subdivision.y);
                    if (x == 0 || y == 0) continue;
                    int topRight = vertexIndex;
                    int topLeft = vertexIndex - vertexCount.y;
                    int bottomRight = topRight - 1;
                    int bottomLeft = topLeft - 1;
                    triangles[triangleIndex++] = bottomLeft;
                    triangles[triangleIndex++] = topLeft;
                    triangles[triangleIndex++] = topRight;
                    triangles[triangleIndex++] = bottomLeft;
                    triangles[triangleIndex++] = topRight;
                    triangles[triangleIndex++] = bottomRight;
                }
            }
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
        }

        protected override void OnValidate()
        {
            if (m_Subdivision.x <= 0) m_Subdivision.x = 1;
            if (m_Subdivision.y <= 0) m_Subdivision.y = 1;
            GenerateMesh();
        }
    }
}
