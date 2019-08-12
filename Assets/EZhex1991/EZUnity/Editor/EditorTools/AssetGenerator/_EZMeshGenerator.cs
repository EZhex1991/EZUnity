/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-10-18 15:05:51
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity.AssetGenerator
{
    public abstract class EZMeshGenerator : ScriptableObject
    {
        private Mesh m_TargetMesh;
        public Mesh targetMesh
        {
            get
            {
                if (m_TargetMesh == null) m_TargetMesh = new Mesh();
                return m_TargetMesh;
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
