/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-02-19 13:43:14
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZUnity
{
    [ExecuteInEditMode]
    public class EZCameraReflection : MonoBehaviour
    {
        public static bool isRendering;

        public Camera targetCamera;
        public RenderTexture renderTexture;
        public Vector3 reflectionNormal = Vector3.forward;
        public float clipPlaneOffset = 0.05f;

        private Camera m_ReflectionCamera;
        private Camera reflectionCamera
        {
            get
            {
                if (m_ReflectionCamera == null)
                {
                    GameObject go = new GameObject("EZReflection-" + GetInstanceID());
                    m_ReflectionCamera = go.AddComponent<Camera>();
                    m_ReflectionCamera.enabled = false;
                    go.AddComponent<FlareLayer>();
                    go.AddComponent<Skybox>();
                    go.hideFlags = HideFlags.HideAndDontSave;
                }
                return m_ReflectionCamera;
            }
        }

        private void SetCamera()
        {
            reflectionCamera.clearFlags = targetCamera.clearFlags;
            reflectionCamera.backgroundColor = targetCamera.backgroundColor;
            reflectionCamera.cullingMask = targetCamera.cullingMask;
            if (targetCamera.clearFlags == CameraClearFlags.Skybox)
            {
                Skybox srcSky = targetCamera.GetComponent<Skybox>();
                Skybox dstSky = reflectionCamera.GetComponent<Skybox>();
                if (srcSky == null || srcSky.material == null)
                {
                    dstSky.enabled = false;
                }
                else
                {
                    dstSky.enabled = true;
                    dstSky.material = srcSky.material;
                }
            }
            reflectionCamera.orthographic = targetCamera.orthographic;
            reflectionCamera.farClipPlane = targetCamera.farClipPlane;
            reflectionCamera.nearClipPlane = targetCamera.nearClipPlane;
            reflectionCamera.fieldOfView = targetCamera.fieldOfView;
            reflectionCamera.aspect = targetCamera.aspect;
            reflectionCamera.orthographicSize = targetCamera.orthographicSize;
            reflectionCamera.targetTexture = renderTexture;
        }
        private Vector4 GetCameraSpacePlane(Camera camera, Vector3 position, Vector3 normal, float sideSign = 1)
        {
            Vector3 offsetPos = position + normal * clipPlaneOffset;
            Matrix4x4 matrix = camera.worldToCameraMatrix;
            Vector3 cPos = matrix.MultiplyPoint(offsetPos);
            Vector3 cNormal = matrix.MultiplyVector(normal).normalized * sideSign;
            return new Vector4(cNormal.x, cNormal.y, cNormal.z, -Vector3.Dot(cPos, cNormal));
        }

        private void OnWillRenderObject()
        {
            if (!enabled || targetCamera == null || reflectionCamera == null) return;
            if (isRendering) return;
            isRendering = true;
            SetCamera();
            Vector3 normal = transform.TransformDirection(reflectionNormal);
            Vector3 position = transform.position;
            float offset = -Vector3.Dot(normal, position) - clipPlaneOffset;
            Vector4 reflectionPlane = new Vector4(normal.x, normal.y, normal.z, offset);

            Matrix4x4 reflectionMatrix = Matrix4x4.zero;
            EZUtility.GetReflectionMatrix(reflectionPlane, ref reflectionMatrix);

            reflectionCamera.transform.position = reflectionMatrix.MultiplyPoint(targetCamera.transform.position);
            Vector3 up = reflectionMatrix.MultiplyVector(targetCamera.transform.up);
            Vector3 forward = reflectionMatrix.MultiplyVector(targetCamera.transform.forward);
            reflectionCamera.transform.rotation = Quaternion.LookRotation(forward, up); ;

            reflectionCamera.worldToCameraMatrix = targetCamera.worldToCameraMatrix * reflectionMatrix;
            Vector4 clipPlane = GetCameraSpacePlane(reflectionCamera, position, normal, -1);
            reflectionCamera.projectionMatrix = reflectionCamera.CalculateObliqueMatrix(clipPlane);

            GL.invertCulling = true;
            reflectionCamera.Render();
            GL.invertCulling = false;
            isRendering = false;
        }
        private void OnDisable()
        {
            if (reflectionCamera != null)
            {
                DestroyImmediate(reflectionCamera.gameObject);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!enabled || targetCamera == null || reflectionCamera == null) return;

            if (targetCamera != null)
            {
                Gizmos.color = Color.grey;
                EZUtility.DrawGizmosCamera(targetCamera);
            }

            if (reflectionCamera != null)
            {
                Gizmos.color = Color.green;
                EZUtility.DrawGizmosCamera(reflectionCamera);
            }
        }
    }
}
