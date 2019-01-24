/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-12-18 19:34:31
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZUnity.PhysicsCompnent
{
    [RequireComponent(typeof(Collider))]
    public class EZPhysicsBoneCollider : EZPhysicsBoneColliderBase
    {
        [SerializeField]
        private Collider m_ReferenceCollider;
        public Collider referenceCollider
        {
            get
            {
                if (m_ReferenceCollider == null)
                    m_ReferenceCollider = GetComponent<Collider>();
                return m_ReferenceCollider;
            }
        }

        [SerializeField]
        private float m_Margin;
        public float margin { get { return m_Margin; } }

        [SerializeField]
        private bool m_InsideMode;
        public bool insideMode { get { return m_InsideMode; } set { m_InsideMode = value; } }

        public override void Collide(ref Vector3 position, float spacing)
        {
            if (referenceCollider is SphereCollider)
            {
                SphereCollider collider = referenceCollider as SphereCollider;
                if (insideMode) EZPhysicsUtility.PointInsideSphere(ref position, collider, spacing + margin);
                else EZPhysicsUtility.PointOutsideSphere(ref position, collider, spacing + margin);
            }
            else if (referenceCollider is CapsuleCollider)
            {
                CapsuleCollider collider = referenceCollider as CapsuleCollider;
                if (insideMode) EZPhysicsUtility.PointInsideCapsule(ref position, collider, spacing + margin);
                else EZPhysicsUtility.PointOutsideCapsule(ref position, collider, spacing + margin);
            }
            else if (referenceCollider is BoxCollider)
            {
                BoxCollider collider = referenceCollider as BoxCollider;
                if (insideMode) EZPhysicsUtility.PointInsideBox(ref position, collider, spacing + margin);
                else EZPhysicsUtility.PointOutsideBox(ref position, collider, spacing + margin);
            }
            else if (referenceCollider is MeshCollider)
            {
                if (!CheckConvex(referenceCollider as MeshCollider))
                {
                    Debug.LogError("Non-Convex Mesh Collider is not supported", this);
                    enabled = false;
                    return;
                }
                if (insideMode)
                {
                    Debug.LogError("Inside Mode On Mesh Collider is not supported", this);
                    insideMode = false;
                    return;
                }
                EZPhysicsUtility.PointOutsideCollider(ref position, referenceCollider, spacing + margin);
            }
        }

        private bool CheckConvex(MeshCollider meshCollider)
        {
            return meshCollider.sharedMesh != null && meshCollider.convex;
        }

        private void Reset()
        {
            m_ReferenceCollider = GetComponent<Collider>();
        }
    }
}
