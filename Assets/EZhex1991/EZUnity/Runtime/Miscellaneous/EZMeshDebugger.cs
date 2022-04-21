/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-03-14 13:11:08
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [RequireComponent(typeof(MeshFilter))]
    public class EZMeshDebugger : MonoBehaviour
    {
        private MeshFilter m_MeshFilter;
        public MeshFilter meshFilter
        {
            get
            {
                if (m_MeshFilter == null)
                {
                    m_MeshFilter = GetComponent<MeshFilter>();
                }
                return m_MeshFilter;
            }
        }

        public Mesh mesh { get { return meshFilter.sharedMesh; } }

        public int startIndex = 0;
        public int maxCount = 1000;

        public float vectorLength = 0.2f;
        public float vertexSize = 0.02f;

        public bool showVertexNumbers = true;
        public bool showNormals = true;
        public bool showTangents = false;

        private int meshId;
        private Vector3[] vertices;
        private Vector3[] normals;
        private Vector4[] tangents;

        private void OnEnable()
        {
            GetMeshData();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (vertices == null) return;

            Gizmos.matrix = UnityEditor.Handles.matrix = transform.localToWorldMatrix;
            int count = Mathf.Min(maxCount + startIndex, vertices.Length);
            if (vertices != null)
            {
                for (int i = startIndex; i < count; i++)
                {
                    Gizmos.DrawSphere(vertices[i], vertexSize);
                    if (showVertexNumbers)
                    {
                        UnityEditor.Handles.Label(vertices[i], i.ToString());
                    }
                }
            }
            if (showNormals && normals != null)
            {
                for (int i = startIndex; i < count; i++)
                {
                    if (i >= normals.Length) break;
                    Gizmos.color = Color.blue;
                    Gizmos.DrawRay(vertices[i], normals[i] * vectorLength);
                }
            }
            if (showTangents && tangents != null)
            {
                for (int i = startIndex; i < count; i++)
                {
                    if (i >= tangents.Length) break;
                    Gizmos.color = Color.blue;
                    Gizmos.DrawRay(vertices[i], tangents[i] * vectorLength);
                }
            }
        }
#endif

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
                }
            }
        }
    }
}
