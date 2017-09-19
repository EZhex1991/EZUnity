/*
 * Author:      熊哲
 * CreateTime:  10/13/2016 2:28:55 PM
 * Description:
 * 挂载到对象上，运行时物件会保持面向摄像机
*/
using UnityEngine;

namespace EZComponent
{
    [ExecuteInEditMode]
    public class FaceToCamera : MonoBehaviour
    {
        public Transform cameraObject;
        public bool reverseZ;

        void LateUpdate()
        {
            if (cameraObject != null)
            {
                transform.LookAt(cameraObject);
                if (reverseZ) transform.Rotate(Vector3.up, 180, Space.Self);
            }
        }
    }
}