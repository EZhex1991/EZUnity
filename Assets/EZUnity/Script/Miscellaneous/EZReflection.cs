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
        public static bool isRendering;

        public RenderTexture renderTexture;
        public LayerMask reflectionLayers = -1;
        public Vector3 reflectionNormal = Vector3.forward;
        public float clipPlaneOffset = 0.05f;

        private Dictionary<Camera, Camera> reflectionCameras = new Dictionary<Camera, Camera>();

        private Camera GetReflectionCamera(Camera camera)
        {
            Camera reflectionCamera;
            reflectionCameras.TryGetValue(camera, out reflectionCamera);
            if (reflectionCamera == null)
            {
                GameObject go = new GameObject(string.Format("EZReflection-{0}-{1}", GetInstanceID(), camera.GetInstanceID()));
                reflectionCamera = go.AddComponent<Camera>();
                reflectionCamera.enabled = false;
                reflectionCamera.transform.position = transform.position;
                reflectionCamera.transform.rotation = transform.rotation;
                go.AddComponent<FlareLayer>();
                go.AddComponent<Skybox>();
                go.hideFlags = HideFlags.HideAndDontSave;
                reflectionCameras[camera] = reflectionCamera;
            }
            return reflectionCamera;
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
            if (!enabled || renderTexture == null) return;

            Camera targetCamera = Camera.current;
            if (targetCamera == null) return;
            Camera reflectionCamera = GetReflectionCamera(targetCamera);

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
            foreach (var pair in reflectionCameras)
            {
                DestroyImmediate(pair.Value.gameObject);
            }
            reflectionCameras.Clear();
        }
    }
}
