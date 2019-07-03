/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-08-21 16:17:44
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZLookAt : MonoBehaviour
    {
        public Transform target;
        public Transform center;

        public float distanceMultiplier = 1;
        public float distanceOffset = 1;

        public float speed = 5f;

        private void Update()
        {
            float delta = speed * Time.deltaTime;
            Vector3 direction = target.position - center.position;
            Vector3 position = center.position + direction * distanceMultiplier + direction.normalized * distanceOffset;
            transform.position = Vector3.Lerp(transform.position, position, delta);
            Quaternion rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, delta);
        }
    }
}
