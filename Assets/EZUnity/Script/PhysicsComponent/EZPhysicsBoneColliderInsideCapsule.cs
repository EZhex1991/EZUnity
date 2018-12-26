/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-12-20 11:17:28
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZUnity.PhysicsCompnent
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class EZPhysicsBoneColliderInsideCapsule : EZPhysicsBoneColliderBase
    {
        [SerializeField]
        private CapsuleCollider m_ReferenceCollider;
        public CapsuleCollider referenceCollider
        {
            get
            {
                if (m_ReferenceCollider == null)
                    m_ReferenceCollider = GetComponent<CapsuleCollider>();
                return m_ReferenceCollider;
            }
        }

        public override void Collide(ref Vector3 position, float spacing)
        {
            EZPhysicsUtility.SphereInsideCapsule(ref position, referenceCollider, spacing);
        }

        private void Reset()
        {
            m_ReferenceCollider = GetComponent<CapsuleCollider>();
        }
    }
}
