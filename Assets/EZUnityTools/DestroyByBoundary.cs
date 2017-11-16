/*
 * Author:      熊哲
 * CreateTime:  9/5/2016 11:09:36 AM
 * Description:
 * 
*/
using UnityEngine;

namespace EZComponent
{
    public class DestroyByBoundary : MonoBehaviour
    {
        void OnTriggerExit(Collider other)
        {
            Destroy(other.gameObject);
        }
    }
}