/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-02-19 13:43:14
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [ExecuteInEditMode]
    public class EZCameraReflection : MonoBehaviour
    {
        public static bool isRendering;

        public Camera targetCamera;
        public RenderTexture renderTexture;
        public LayerMask reflectionLayers = -1;
        public Vector3 reflectionNormal = Vector3.forward;
        public float clipPlaneOffset = 0.05f;

        private Camera m_ReflectionCamera;
        private Camera reflectionCamera
        {
            get
            {
                if (m_ReflectionCamera == null)
                {
                    GameObject go = new GameObject("EZCameraReflection-" + GetInstanceID());
                    m_ReflectionCamera = go.AddComponent<Camera>();
                    m_ReflectionCamera.enabled = false;
                    go.AddComponent<FlareLayer>();
                    go.AddComponent<Skybox>();
                    go.hideFlags = HideFlags.HideAndDontSave;
                }
                return m_ReflectionCamera;
            }
        }

        private void SetCamera(Camera src, Camera dst)
        {
            if (src == null || dst == null) return;
            dst.clearFlags = src.clearFlags;
            dst.backgroundColor = src.backgroundColor;
            if (src.clearFlags == CameraClearFlags.Skybox)
            {
                Skybox srcSky = src.GetComponent<Skybox>();
                Skybox dstSky = dst.GetComponent<Skybox>();
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
            dst.orthographic = src.orthographic;
            dst.farClipPlane = src.farClipPlane;
            dst.nearClipPlane = src.nearClipPlane;
            dst.fieldOfView = src.fieldOfView;
            dst.aspect = src.aspect;
            dst.orthographicSize = src.orthographicSize;
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
            GL.invertCulling = true;
            RenderTexture.active = renderTexture;

            SetCamera(targetCamera, reflectionCamera);
            reflectionCamera.cullingMask = reflectionLayers;
            reflectionCamera.targetTexture = renderTexture;

            Vector3 position = transform.position;
            Vector3 normal = transform.TransformDirection(reflectionNormal);
            float offset = -Vector3.Dot(normal, position) - clipPlaneOffset;
            Vector4 reflectionPlane = new Vector4(normal.x, normal.y, normal.z, offset);

            Matrix4x4 reflectionMatrix = Matrix4x4.zero;
            EZUtility.GetReflectionMatrix(reflectionPlane, ref reflectionMatrix);

            reflectionCamera.worldToCameraMatrix = targetCamera.worldToCameraMatrix * reflectionMatrix;
            Vector4 clipPlane = GetCameraSpacePlane(reflectionCamera, position, normal);
            reflectionCamera.projectionMatrix = targetCamera.CalculateObliqueMatrix(clipPlane);

            reflectionCamera.transform.position = reflectionMatrix.MultiplyPoint(targetCamera.transform.position);
            Vector3 forward = reflectionMatrix.MultiplyVector(targetCamera.transform.forward);
            Vector3 up = reflectionMatrix.MultiplyVector(targetCamera.transform.up);
            reflectionCamera.transform.rotation = Quaternion.LookRotation(forward, up);
            reflectionCamera.Render();

            RenderTexture.active = null;
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
                EZUtility.DrawGizmosCameraFrustum(targetCamera);
            }

            if (reflectionCamera != null)
            {
                Gizmos.color = Color.green;
                EZUtility.DrawGizmosCameraFrustum(reflectionCamera);
            }
        }
    }
}
