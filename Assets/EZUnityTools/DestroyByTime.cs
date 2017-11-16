/*
 * Author:      熊哲
 * CreateTime:  11/14/2017 7:09:31 PM
 * Description:
 * 
*/
using UnityEngine;

namespace EZComponent
{
    public class DestroyByTime : MonoBehaviour
    {
        [SerializeField]
        private float m_LifeTime = 1;
        public float lifeTime { get { return m_LifeTime; } set { m_LifeTime = value; } }

        void Start()
        {
            Destroy(gameObject, lifeTime);
        }
    }
}