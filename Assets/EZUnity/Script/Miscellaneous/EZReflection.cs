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

        public int textureSize = 256;
        public LayerMask reflectionLayers = -1;
        public float clipPlaneOffset = 0.05f;
        public Vector3 reflectionNormal = Vector3.forward;

        private Renderer m_Renderer;
        public Renderer renderer
        {
            get
            {
                if (m_Renderer == null)
                    m_Renderer = GetComponent<Renderer>();
                return m_Renderer;
            }
        }

        private Dictionary<Camera, Camera> reflectionCameras = new Dictionary<Camera, Camera>();
        private RenderTexture reflectionTexture;
        private int oldTextureSize;

        private bool IsEnabled()
        {
            if (!enabled) return false;
            if (renderer == null || !renderer.enabled || renderer.sharedMaterial == null) return false;
            return true;
        }
        private Camera GetReflectionCamera(Camera camera)
        {
            if (reflectionTexture == null || oldTextureSize != textureSize)
            {
                if (reflectionTexture != null) DestroyImmediate(reflectionTexture);
                reflectionTexture = new RenderTexture(textureSize, textureSize, 16);
                reflectionTexture.name = string.Format("EZMirrorReflectionTexture-{0}", GetInstanceID());
                reflectionTexture.isPowerOfTwo = true;
                reflectionTexture.hideFlags = HideFlags.DontSave;
                oldTextureSize = textureSize;
            }
            Camera reflectionCamera;
            reflectionCameras.TryGetValue(camera, out reflectionCamera);
            if (reflectionCamera == null)
            {
                GameObject go = new GameObject(string.Format("EZMirrorReflectionCamera-{0}-{1}", GetInstanceID(), camera.GetInstanceID()));
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
            if (dst == null) return;
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
        private Vector4 GetCameraSpacePlane(Camera camera, Vector3 position, Vector3 normal, float sideSign)
        {
            Vector3 offsetPos = position + normal * clipPlaneOffset;
            Matrix4x4 matrix = camera.worldToCameraMatrix;
            Vector3 cPos = matrix.MultiplyPoint(offsetPos);
            Vector3 cNormal = matrix.MultiplyVector(normal).normalized * sideSign;
            return new Vector4(cNormal.x, cNormal.y, cNormal.z, -Vector3.Dot(cPos, cNormal));
        }

        private void OnWillRenderObject()
        {
            if (!IsEnabled()) return;

            Camera camera = Camera.current;
            if (camera == null) return;

            if (isRendering) return;
            isRendering = true;

            Camera reflectionCamera = GetReflectionCamera(camera);
            SetCamera(camera, reflectionCamera);
            reflectionCamera.cullingMask = reflectionLayers;

            Vector3 position = transform.position;
            Vector3 normal = transform.TransformDirection(reflectionNormal);

            float depth = -Vector3.Dot(normal, position) - clipPlaneOffset;
            Vector4 reflectionPlane = new Vector4(normal.x, normal.y, normal.z, depth);

            Matrix4x4 reflectionMatrix = Matrix4x4.zero;
            EZUtility.GetReflectionMatrix(reflectionPlane, ref reflectionMatrix);

            reflectionCamera.worldToCameraMatrix = camera.worldToCameraMatrix * reflectionMatrix;
            Vector4 clipPlane = GetCameraSpacePlane(reflectionCamera, position, normal, 1.0f);
            reflectionCamera.projectionMatrix = camera.CalculateObliqueMatrix(clipPlane);

            bool oldCulling = GL.invertCulling;
            GL.invertCulling = !oldCulling;
            Vector3 oldPostion = camera.transform.position;
            Vector3 newPosition = reflectionMatrix.MultiplyPoint(oldPostion);
            reflectionCamera.transform.position = newPosition;
            Vector3 euler = camera.transform.eulerAngles;
            reflectionCamera.transform.eulerAngles = new Vector3(-euler.x, euler.y, euler.z);
            reflectionCamera.targetTexture = reflectionTexture;
            reflectionCamera.Render();
            reflectionCamera.transform.position = oldPostion;
            GL.invertCulling = oldCulling;

            renderer.sharedMaterial.SetTexture("_ReflectionTex", reflectionTexture);

            isRendering = false;
        }
        private void OnDisable()
        {
            if (reflectionTexture != null)
            {
                DestroyImmediate(reflectionTexture);
                reflectionTexture = null;
            }
            foreach (var pair in reflectionCameras)
            {
                DestroyImmediate(pair.Value.gameObject);
            }
            reflectionCameras.Clear();
        }
    }
}
