/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-02-18 13:42:01
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEngine;

namespace EZUnity
{
    [ExecuteInEditMode]
    public class EZReflection : MonoBehaviour
    {
        public const string TAG_REFLECTION = "EZReflection";
        public const string TAG_REFRACTION = "EZRefraction";

        public static bool isRendering;

        public bool renderReflection = true;
        public bool renderRefraction = false;

        [UnityEngine.Serialization.FormerlySerializedAs("renderTexture")]
        public RenderTexture reflectionTexture;
        public RenderTexture refractionTexture;

        public LayerMask reflectionLayers = -1;
        public LayerMask refractionLayers = -1;

        [UnityEngine.Serialization.FormerlySerializedAs("reflectionNormal")]
        public Vector3 normalDirection = Vector3.forward;

        public float clipPlaneOffset = 0.05f;

        private Dictionary<Camera, Camera> m_ReflectionCameras = new Dictionary<Camera, Camera>();
        private Dictionary<Camera, Camera> m_RefractionCameras = new Dictionary<Camera, Camera>();

        private Camera GetRenderCamera(Dictionary<Camera, Camera> dict, Camera targetCamera, string tag)
        {
            Camera renderCamera;
            dict.TryGetValue(targetCamera, out renderCamera);
            if (renderCamera == null)
            {
                GameObject go = new GameObject(string.Format("{0}-{1}-{2}", GetInstanceID(), targetCamera.GetInstanceID(), tag));
                renderCamera = go.AddComponent<Camera>();
                renderCamera.enabled = false;
                renderCamera.transform.position = transform.position;
                renderCamera.transform.rotation = transform.rotation;
                go.AddComponent<FlareLayer>();
                go.AddComponent<Skybox>();
                go.hideFlags = HideFlags.HideAndDontSave;
                dict[targetCamera] = renderCamera;
            }
            return renderCamera;
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
            if (!enabled) return;

            Camera targetCamera = Camera.current;
            if (targetCamera == null) return;
            if (isRendering) return;
            isRendering = true;

            Vector3 position = transform.position;
            Vector3 normal = transform.TransformDirection(normalDirection);

            if (renderRefraction && refractionTexture != null)
            {
                Camera refractionCamera = GetRenderCamera(m_RefractionCameras, targetCamera, TAG_REFRACTION);
                SetCamera(targetCamera, refractionCamera);
                refractionCamera.cullingMask = ~(1 << 4) & refractionLayers;
                refractionCamera.targetTexture = refractionTexture;
                RenderTexture.active = refractionTexture;

                refractionCamera.worldToCameraMatrix = targetCamera.worldToCameraMatrix;
                Vector4 clipPlane = GetCameraSpacePlane(refractionCamera, position, normal, -1);
                refractionCamera.projectionMatrix = targetCamera.CalculateObliqueMatrix(clipPlane);
                refractionCamera.cullingMatrix = targetCamera.projectionMatrix * targetCamera.worldToCameraMatrix;

                refractionCamera.transform.position = targetCamera.transform.position;
                refractionCamera.transform.rotation = targetCamera.transform.rotation;
                refractionCamera.Render();
            }
            if (renderReflection && reflectionTexture != null)
            {
                Camera reflectionCamera = GetRenderCamera(m_ReflectionCameras, targetCamera, TAG_REFLECTION);
                SetCamera(targetCamera, reflectionCamera);
                reflectionCamera.cullingMask = ~(1 << 4) & reflectionLayers;
                reflectionCamera.targetTexture = reflectionTexture;
                RenderTexture.active = reflectionTexture;

                float offset = -Vector3.Dot(normal, position) - clipPlaneOffset;
                Vector4 reflectionPlane = new Vector4(normal.x, normal.y, normal.z, offset);

                Matrix4x4 reflectionMatrix = Matrix4x4.zero;
                EZUtility.GetReflectionMatrix(reflectionPlane, ref reflectionMatrix);

                reflectionCamera.worldToCameraMatrix = targetCamera.worldToCameraMatrix * reflectionMatrix;
                Vector4 clipPlane = GetCameraSpacePlane(reflectionCamera, position, normal);
                reflectionCamera.projectionMatrix = targetCamera.CalculateObliqueMatrix(clipPlane);
                reflectionCamera.cullingMatrix = targetCamera.projectionMatrix * targetCamera.worldToCameraMatrix;

                GL.invertCulling = true;
                reflectionCamera.transform.position = reflectionMatrix.MultiplyPoint(targetCamera.transform.position);
                Vector3 forward = reflectionMatrix.MultiplyVector(targetCamera.transform.forward);
                Vector3 up = reflectionMatrix.MultiplyVector(targetCamera.transform.up);
                reflectionCamera.transform.rotation = Quaternion.LookRotation(forward, up);
                reflectionCamera.Render();
                reflectionCamera.transform.position = targetCamera.transform.position;
                GL.invertCulling = false;
            }

            RenderTexture.active = null;
            isRendering = false;
        }
        private void OnDisable()
        {
            foreach (var pair in m_ReflectionCameras)
            {
                DestroyImmediate(pair.Value.gameObject);
            }
            m_ReflectionCameras.Clear();
        }
    }
}
