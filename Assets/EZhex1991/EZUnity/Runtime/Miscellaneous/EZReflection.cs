/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-02-18 13:42:01
 * Organization:    #ORGANIZATION#
 * Description:     
 */
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
using System.Collections.Generic;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Renderer))]
    public class EZReflection : MonoBehaviour
    {
        public const string SHADER_NAME = "Hidden/EZUnity/Effects/EZReflection";
        public const string TAG_REFLECTION = "EZReflection";
        public const string TAG_REFRACTION = "EZRefraction";
        public const string KEYWORD_REFLECTION_ON = "_REFLECTION_ON";
        public const string KEYWORD_REFRACTION_ON = "_REFRACTION_ON";

        public static bool isRendering;

        [SerializeField, HideInInspector]
        private Shader m_Shader;
        public Shader shader
        {
            get
            {
                if (m_Shader == null)
                {
                    m_Shader = Shader.Find(SHADER_NAME);
                }
                return m_Shader;
            }
        }

        private Renderer m_Renderer;
        private Renderer renderer
        {
            get
            {
                if (m_Renderer == null)
                    m_Renderer = GetComponent<Renderer>();
                return m_Renderer;
            }
        }
        public Material material
        {
            get
            {
                if (shader != null && renderer.sharedMaterial.shader != shader)
                {
                    Debug.LogErrorFormat(renderer, "Shader {0} is required for EZReflection renderer", shader.name);
                }
                return renderer.sharedMaterial;
            }
        }

        public Vector3 normalDirection = Vector3.forward;

        public float clipPlaneOffset = 0.05f;

        [Header("Reflection")]
        [SerializeField]
        private bool m_ReflectionOn = true;
        public bool reflectionOn
        {
            get { return m_ReflectionOn; }
            set { m_ReflectionOn = value; material.SetKeyword(KEYWORD_REFLECTION_ON, value); }
        }
        [SerializeField, Range(0, 1)]
        private float m_ReflectionStrength = 0.5f;
        public float reflectionStrength
        {
            get { return m_ReflectionStrength; }
            set { m_ReflectionStrength = value; material.SetFloat("_ReflectionStrength", m_ReflectionStrength); }
        }
        public Vector2Int reflectionResolution = new Vector2Int(512, 512);
        private RenderTexture m_ReflectionTexture;
        private RenderTexture reflectionTexture
        {
            get
            {
                if (m_ReflectionTexture == null
                    || m_ReflectionTexture.width != reflectionResolution.x
                    || m_ReflectionTexture.height != reflectionResolution.y)
                {
                    m_ReflectionTexture = new RenderTexture(reflectionResolution.x, reflectionResolution.y, 0);
                }
                return m_ReflectionTexture;
            }
        }
        public LayerMask reflectionLayers = -1;

        [Header("Refraction")]
        [SerializeField]
        private bool m_RefractionOn = true;
        public bool refractionOn
        {
            get { return m_RefractionOn; }
            set { m_RefractionOn = value; material.SetKeyword(KEYWORD_REFRACTION_ON, value); }
        }
        [SerializeField, Range(0, 1)]
        private float m_RefractionStrength = 0.5f;
        public float refractionStrength
        {
            get { return m_RefractionStrength; }
            set { m_RefractionStrength = value; material.SetFloat("_RefractionStrength", m_RefractionStrength); }
        }
        public Vector2Int refractionResolution = new Vector2Int(512, 512);
        private RenderTexture m_RefractionTexture;
        private RenderTexture refractionTexture
        {
            get
            {
                if (m_RefractionTexture == null
                    || m_RefractionTexture.width != refractionResolution.x
                    || m_RefractionTexture.height != refractionResolution.y)
                {
                    m_RefractionTexture = new RenderTexture(refractionResolution.x, refractionResolution.y, 0);
                }
                return m_RefractionTexture;
            }
        }
        public LayerMask refractionLayers = -1;

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

            if (refractionOn && refractionTexture != null)
            {
                material.SetTexture("_ReflectionTex", m_ReflectionTexture);
                Camera refractionCamera = GetRenderCamera(m_RefractionCameras, targetCamera, TAG_REFRACTION);
                SetCamera(targetCamera, refractionCamera);
                refractionCamera.cullingMask = ~(1 << 4) & refractionLayers;
                refractionCamera.targetTexture = refractionTexture;

                refractionCamera.worldToCameraMatrix = targetCamera.worldToCameraMatrix;
                Vector4 clipPlane = GetCameraSpacePlane(refractionCamera, position, normal, -1);
                refractionCamera.projectionMatrix = targetCamera.CalculateObliqueMatrix(clipPlane);
                refractionCamera.cullingMatrix = targetCamera.projectionMatrix * targetCamera.worldToCameraMatrix;

                refractionCamera.transform.position = targetCamera.transform.position;
                refractionCamera.transform.rotation = targetCamera.transform.rotation;
                refractionCamera.Render();
            }
            if (reflectionOn && reflectionTexture != null)
            {
                material.SetTexture("_RefractionTex", m_RefractionTexture);
                Camera reflectionCamera = GetRenderCamera(m_ReflectionCameras, targetCamera, TAG_REFLECTION);
                SetCamera(targetCamera, reflectionCamera);
                reflectionCamera.cullingMask = ~(1 << 4) & reflectionLayers;
                reflectionCamera.targetTexture = reflectionTexture;

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

        private void OnValidate()
        {
            material.SetFloat("_ReflectionStrength", m_ReflectionStrength);
            material.SetFloat("_RefractionStrength", m_RefractionStrength);
            material.SetKeyword(KEYWORD_REFLECTION_ON, reflectionOn);
            material.SetKeyword(KEYWORD_REFRACTION_ON, refractionOn);
        }

    }
}
