/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-05-15 15:06:19
 * Organization:    #ORGANIZATION#
 * Description:     
 */
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
using UnityEngine;
using UnityEngine.Events;

namespace EZhex1991.EZUnity
{
    public class EZCameraZoomer : MonoBehaviour
    {
        public Camera camera;
        public Camera cameraFar;
        public Camera cameraNear;

        public float distanceScale = 1;
        public string zoomAxis = "Mouse ScrollWheel";
        public float axisScale = 1;

        public bool lerpPosition = true;
        public bool lerpRotation = true;
        public bool lerpColor = true;
        public bool lerpFOV = true;

        private float touchesDistance;
        private float screenSize;

        public event UnityAction<float> onZoom;

        [Range(0, 1)]
        public float m_Zoom = 0.8f;
        public float zoom
        {
            get
            {
                return m_Zoom;
            }
            set
            {
                m_Zoom = Mathf.Clamp01(value);
                if (lerpPosition) camera.transform.position = Vector3.Lerp(cameraFar.transform.position, cameraNear.transform.position, m_Zoom);
                if (lerpRotation) camera.transform.rotation = Quaternion.Lerp(cameraFar.transform.rotation, cameraNear.transform.rotation, m_Zoom);
                if (lerpColor) camera.backgroundColor = Color.Lerp(cameraFar.backgroundColor, cameraNear.backgroundColor, m_Zoom);
                if (lerpFOV) camera.fieldOfView = Mathf.Lerp(cameraFar.fieldOfView, cameraNear.fieldOfView, m_Zoom);
                if (onZoom != null) onZoom(m_Zoom);
            }
        }

        private void Start()
        {
            float width = Screen.width;
            float height = Screen.height;
            screenSize = new Vector2(width, height).magnitude;
        }
        private void Update()
        {
            if (!string.IsNullOrEmpty(zoomAxis)) RespondAxis(Input.GetAxis(zoomAxis));
        }

        public void RespondTouch(Touch t1, Touch t2)
        {
            float currentDistance = Vector2.Distance(t1.position, t2.position);
            if (t1.phase == TouchPhase.Began || t2.phase == TouchPhase.Began)
            {
                touchesDistance = currentDistance;
            }
            else if (t1.phase == TouchPhase.Ended || t2.phase == TouchPhase.Ended)
            {
            }
            else
            {
                zoom += (currentDistance - touchesDistance) / screenSize * distanceScale;
                touchesDistance = currentDistance;
            }
        }
        public void RespondAxis(float input)
        {
            zoom += input * axisScale;
        }

        private void OnValidate()
        {
            if (camera != null && cameraFar != null && cameraNear != null && Application.isEditor && Application.isPlaying)
            {
                camera.transform.position = Vector3.Lerp(cameraFar.transform.position, cameraNear.transform.position, m_Zoom);
                camera.transform.rotation = Quaternion.Lerp(cameraFar.transform.rotation, cameraNear.transform.rotation, m_Zoom);
                camera.backgroundColor = Color.Lerp(cameraFar.backgroundColor, cameraNear.backgroundColor, m_Zoom);
                camera.fieldOfView = Mathf.Lerp(cameraFar.fieldOfView, cameraNear.fieldOfView, m_Zoom);
            }
        }
    }
}
