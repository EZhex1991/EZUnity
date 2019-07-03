/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-03-14 13:11:08
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZMeshDebugger : MonoBehaviour
    {
        public Mesh mesh;
        public int startIndex = 0;
        public int maxCount = 1000;
        public float lineLength = 0.2f;

        public bool showNormal = true;
        public bool showTangent = false;
        public bool showBitangent = false;

        private int meshId;
        private Vector3[] vertices;
        private Vector3[] normals;
        private Vector4[] tangents;
        private Vector3[] bitangents;

        private void OnEnable()
        {
            GetMeshData();
        }

        private void OnDrawGizmos()
        {
            if (vertices == null) return;
            Gizmos.matrix = transform.localToWorldMatrix;
            int count = Mathf.Min(maxCount + startIndex, vertices.Length);
            for (int i = startIndex; i < count; i++)
            {
                Vector3 vertex = vertices[i];
                Vector3 normal = normals[i];
                Vector3 tangent = tangents[i];
                Vector3 bitangent = bitangents[i];
                if (showNormal)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawRay(vertex, normal * lineLength);
                }
                if (showTangent)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawRay(vertex, tangent * lineLength);
                }
                if (showBitangent)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawRay(vertex, bitangent * lineLength);
                }
            }
        }

        private void OnValidate()
        {
            GetMeshData();
        }

        private void GetMeshData()
        {
            if (mesh == null)
            {
                meshId = -1;
                vertices = null;
                normals = null;
                tangents = null;
                bitangents = null;
            }
            else
            {
                int id = mesh.GetInstanceID();
                if (id != meshId)
                {
                    meshId = id;
                    vertices = mesh.vertices;
                    normals = mesh.normals;
                    tangents = mesh.tangents;
                    bitangents = new Vector3[vertices.Length];
                    for (int i = 0; i < bitangents.Length; i++)
                    {
                        bitangents[i] = Vector3.Cross(normals[i], tangents[i]);
                    }
                }
            }
        }
    }
}
