/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-08-21 15:58:38
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZFollower : MonoBehaviour
    {
        public Transform target;
        public float speed = 5f;
        public bool keepDistance = true;

        private Vector3 offset;

        private void OnEnable()
        {
            if (keepDistance)
            {
                offset = transform.position - target.position;
            }
        }

        private void Update()
        {
            float delta = speed * Time.deltaTime;
            Vector3 position = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, position, delta);
        }
    }
}
