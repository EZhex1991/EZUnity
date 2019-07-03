/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-05-28 09:41:31
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZExplosive : MonoBehaviour
    {
        public GameObject debris;
        public LayerMask layerMask = -1;
        public bool disableKinematic = true;
        public bool executeOnEnable = true;
        public ForceMode forceMode = ForceMode.Impulse;

        public float explosionForce = 10;
        public float explosionRadius = 1;
        public float explosionUpwards = 2;

        public ParticleSystem particleEfx;

        private void OnEnable()
        {
            if (executeOnEnable) Explode();
        }

        public void Explode()
        {
            if (debris != null)
            {
                GameObject newDebris = Instantiate(debris, transform, true);
                newDebris.SetActive(true);
                foreach (Rigidbody rigidbody in newDebris.GetComponents<Rigidbody>())
                {
                    if (disableKinematic) rigidbody.isKinematic = false;
                    rigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpwards, ForceMode.Impulse);
                }
            }
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, layerMask);
            foreach (Collider collider in colliders)
            {
                Rigidbody rb = collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    if (disableKinematic) rb.isKinematic = false;
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpwards, ForceMode.Impulse);
                }
            }
            if (particleEfx != null) particleEfx.Play();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
