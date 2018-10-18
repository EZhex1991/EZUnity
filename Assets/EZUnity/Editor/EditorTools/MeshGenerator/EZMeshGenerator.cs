/* Author:          熊哲
 * CreateTime:      2018-10-18 15:05:51
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZUnity
{
    public abstract class EZMeshGenerator : ScriptableObject
    {
        private Mesh m_Mesh;
        public Mesh mesh
        {
            get
            {
                if (m_Mesh == null) m_Mesh = new Mesh();
                return m_Mesh;
            }
        }

        public abstract void GenerateMesh();

        protected virtual void Reset()
        {
            GenerateMesh();
        }

        protected virtual void OnValidate()
        {
            GenerateMesh();
        }
    }
}
