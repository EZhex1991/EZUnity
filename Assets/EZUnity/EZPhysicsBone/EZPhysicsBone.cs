/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-12-18 19:33:50
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZUnity.PhysicsCompnent
{
    public class EZPhysicsBone : MonoBehaviour
    {
        public class TreeNode : IDisposable
        {
            public TreeNode parent;
            public List<TreeNode> children = new List<TreeNode>();

            public Transform transform;

            public int depth;
            public float nodeLength;
            public float boneLength;

            public float treeLength;
            public float normalizedLength;
            public float radius;

            public Vector3 position;
            public Vector3 speed;

            public Vector3 originalLocalPosition;
            public Quaternion originalLocalRotation = Quaternion.identity;

            public TreeNode() { }
            public TreeNode(Transform t, float endLength, int startDepth, int depth = 0, float localLength = 0, float parentLength = 0)
            {
                transform = t;
                if (t != null)
                {
                    position = t.position;
                    originalLocalPosition = t.localPosition;
                    originalLocalRotation = t.localRotation;
                }
                this.depth = depth;
                nodeLength = localLength;
                if (depth > startDepth)
                {
                    boneLength = parentLength + nodeLength;
                }
                treeLength = boneLength;
                if (t.childCount > 0)
                {
                    for (int i = 0; i < t.childCount; i++)
                    {
                        Transform child = t.GetChild(i);
                        // local length should ignore parent scales
                        TreeNode node = new TreeNode(transform.GetChild(i), endLength, startDepth, depth + 1, (child.position - t.position).magnitude, boneLength);
                        node.parent = this;
                        children.Add(node);
                        treeLength = Mathf.Max(treeLength, node.treeLength);
                    }
                }
                else
                {
                    // end node
                    TreeNode node = new TreeNode();
                    if (t.parent != null)
                    {
                        // extend last bone to end node using local position
                        node.originalLocalPosition = t.InverseTransformPoint(t.parent.TransformPoint(t.localPosition * 2)) * endLength;
                    }
                    node.depth = depth + 1;
                    node.position = t.TransformPoint(node.originalLocalPosition);
                    node.nodeLength = node.originalLocalPosition.magnitude;
                    node.boneLength = boneLength + node.nodeLength;
                    node.treeLength = node.boneLength;
                    node.parent = this;
                    children.Add(node);
                    treeLength = node.treeLength;
                }
            }

            public void Inflate(float baseRadius, AnimationCurve radiusCurve, bool recursive)
            {
                if (treeLength <= 0) return;
                normalizedLength = boneLength / treeLength;
                radius = radiusCurve.Evaluate(normalizedLength) * baseRadius;
                if (recursive)
                {
                    for (int i = 0; i < children.Count; i++)
                    {
                        children[i].Inflate(baseRadius, radiusCurve, recursive);
                    }
                }
            }

            public void RevertTransforms(bool recursive)
            {
                if (transform != null)
                {
                    transform.localPosition = originalLocalPosition;
                    transform.localRotation = originalLocalRotation;
                }
                if (recursive)
                {
                    for (int i = 0; i < children.Count; i++)
                    {
                        children[i].RevertTransforms(recursive);
                    }
                }
            }
            public void ApplyToTransform(bool recursive)
            {
                if (transform != null)
                {
                    // rotate if has only one child
                    if (children.Count == 1)
                    {
                        TreeNode child = children[0];
                        Quaternion rotation = Quaternion.FromToRotation(transform.TransformDirection(child.originalLocalPosition), child.position - position);
                        transform.rotation = rotation * transform.rotation;
                    }
                    transform.position = position;
                }
                if (recursive)
                {
                    for (int i = 0; i < children.Count; i++)
                    {
                        children[i].ApplyToTransform(recursive);
                    }
                }
            }
            public void SyncPosition(bool recursive)
            {
                if (transform != null)
                {
                    position = transform.position;
                }
                if (recursive)
                {
                    for (int i = 0; i < children.Count; i++)
                    {
                        children[i].SyncPosition(recursive);
                    }
                }
            }

            public void Dispose()
            {
                Dispose(true);
            }
            public void Dispose(bool recursive = true)
            {
                if (recursive)
                {
                    for (int i = 0; i < children.Count; i++)
                    {
                        children[i].Dispose(recursive);
                    }
                }
                parent = null;
                children.Clear();
                transform = null;
            }
        }

        [SerializeField]
        private List<Transform> m_RootBones;
        public List<Transform> rootBones { get { return m_RootBones; } }

        [SerializeField]
        private int m_StartDepth;
        public int startDepth { get { return m_StartDepth; } }

        [SerializeField]
        private float m_EndNodeLength;
        public float endNodeLength { get { return m_EndNodeLength; } }

        [SerializeField]
        private EZPhysicsBoneMaterial m_Material;
        public EZPhysicsBoneMaterial sharedMaterial
        {
            get
            {
                if (m_Material == null)
                    m_Material = EZPhysicsBoneMaterial.defaultMaterial;
                return m_Material;
            }
            set
            {
                m_Material = value;
            }
        }

        [SerializeField]
        private AnimatorUpdateMode m_UpdateMode = AnimatorUpdateMode.Normal;
        public AnimatorUpdateMode updateMode { get { return m_UpdateMode; } }

        [SerializeField]
        private float m_MinDeltaTime = 0.02f;
        public float minDeltaTime { get { return m_MinDeltaTime; } }

        [SerializeField]
        private float m_SleepThreshold = 0.005f;
        public float sleepThreshold { get { return m_SleepThreshold; } set { m_SleepThreshold = Mathf.Max(0, value); } }

        [Header("Collision")]
        [SerializeField]
        private LayerMask m_CollisionLayers = 0;
        public LayerMask collisionLayers { get { return m_CollisionLayers; } }
        [SerializeField]
        private List<Collider> m_ExtraColliders = new List<Collider>();
        public List<Collider> extraColliders { get { return m_ExtraColliders; } }
        [SerializeField]
        private float m_Radius = 0;
        public float radius { get { return m_Radius; } }
        [SerializeField, EZCurveRange(0, 0, 1, 1)]
        private AnimationCurve m_RadiusCurve = AnimationCurve.Constant(0, 1, 1);
        public AnimationCurve radiusCurve { get { return m_RadiusCurve; } }

        [Header("Force")]
        [SerializeField]
        private Vector3 m_Gravity;
        public Vector3 gravity { get { return m_Gravity; } set { m_Gravity = value; } }
        [SerializeField]
        private EZPhysicsBoneForce m_ForceModule;
        public EZPhysicsBoneForce forceModule { get { return m_ForceModule; } set { m_ForceModule = value; } }

        private List<TreeNode> m_PhysicsTrees = new List<TreeNode>();
        private float deltaTime;

        private void Start()
        {
            InitPhysicsTrees();
        }
        private void OnEnable()
        {
            ResyncPhysicsTrees();
        }
        private void FixedUpdate()
        {
            if (updateMode == AnimatorUpdateMode.AnimatePhysics)
                UpdatePhysicsTrees(Time.fixedDeltaTime);
        }
        private void Update()
        {
            if (updateMode != AnimatorUpdateMode.AnimatePhysics)
            {
                deltaTime += Time.deltaTime;
                if (deltaTime > minDeltaTime)
                {
                    UpdatePhysicsTrees(deltaTime);
                    deltaTime = 0;
                }
            }
        }
        private void LateUpdate()
        {
            if (rootBones == null || rootBones.Count == 0) return;
            ApplyToTransforms();
        }
        private void OnDisable()
        {
            RevertTransforms();
        }

        private void OnValidate()
        {
            m_StartDepth = Mathf.Max(0, m_StartDepth);
            m_EndNodeLength = Mathf.Max(0, m_EndNodeLength);
            m_MinDeltaTime = Mathf.Max(0, m_MinDeltaTime);
            m_SleepThreshold = Mathf.Max(0, m_SleepThreshold);
            m_Radius = Mathf.Max(0, m_Radius);
            if (Application.isEditor && Application.isPlaying)
            {
                RevertTransforms();
                InitPhysicsTrees();
            }
        }
        private void OnDrawGizmosSelected()
        {
            if (!enabled) return;

            if (Application.isEditor && !Application.isPlaying && transform.hasChanged)
            {
                InitPhysicsTrees();
            }

            for (int i = 0; i < m_PhysicsTrees.Count; i++)
            {
                DrawNodeGizmos(m_PhysicsTrees[i]);
            }
        }

        private void InitPhysicsTrees()
        {
            m_PhysicsTrees.Clear();
            if (rootBones == null || rootBones.Count == 0) return;
            for (int i = 0; i < rootBones.Count; i++)
            {
                if (rootBones[i] == null) continue;
                TreeNode tree = new TreeNode(rootBones[i], endNodeLength, startDepth);
                tree.Inflate(radius, radiusCurve, true);
                m_PhysicsTrees.Add(tree);
            }
        }
        private void RevertTransforms()
        {
            if (m_PhysicsTrees == null || m_PhysicsTrees.Count == 0) return;
            for (int i = 0; i < m_PhysicsTrees.Count; i++)
            {
                m_PhysicsTrees[i].RevertTransforms(true);
            }
        }
        private void ResyncPhysicsTrees()
        {
            if (m_PhysicsTrees == null || m_PhysicsTrees.Count == 0) return;
            for (int i = 0; i < m_PhysicsTrees.Count; i++)
            {
                m_PhysicsTrees[i].SyncPosition(true);
            }
        }
        private void UpdatePhysicsTrees(float deltaTime)
        {
            for (int i = 0; i < m_PhysicsTrees.Count; i++)
            {
                m_PhysicsTrees[i].RevertTransforms(true);
                UpdateNode(m_PhysicsTrees[i], deltaTime);
            }
        }
        private void UpdateNode(TreeNode node, float deltaTime)
        {
            if (node.depth > startDepth)
            {
                Vector3 lastPosition = node.position;

                // Damping (inertia attenuation)
                if (node.speed.sqrMagnitude < sleepThreshold)
                {
                    node.speed = Vector3.zero;
                }
                else
                {
                    node.position += node.speed * deltaTime * (1 - sharedMaterial.GetDamping(node.normalizedLength));
                }

                // Resistance (outside force resistance)
                Vector3 force = gravity;
                if (forceModule != null)
                {
                    force += forceModule.GetForce(node.normalizedLength);
                }
                node.position += force * deltaTime * (1 - sharedMaterial.GetResistance(node.normalizedLength));

                // Stiffness (shape keeper)
                Vector3 parentOffset = node.parent.position - node.parent.transform.position;
                Vector3 expectedPos = node.parent.transform.TransformPoint(node.originalLocalPosition) + parentOffset;
                node.position = Vector3.Lerp(node.position, expectedPos, sharedMaterial.GetStiffness(node.normalizedLength));

                // Collision
                if (node.radius > 0)
                {
                    foreach (EZPhysicsBoneColliderBase collider in EZPhysicsBoneColliderBase.EnabledColliders)
                    {
                        if (node.transform != collider.transform && collisionLayers.Contains(collider.gameObject.layer))
                            collider.Collide(ref node.position, node.radius);
                    }
                    foreach (Collider collider in extraColliders)
                    {
                        if (node.transform != collider.transform && collider.enabled)
                            EZPhysicsUtility.SphereOutsideCollider(ref node.position, collider, node.radius);
                    }
                }

                // Slackness (length keeper)
                Vector3 nodeDir = (node.position - node.parent.position).normalized;
                Vector3 lengthKeeper = node.parent.position + nodeDir * node.nodeLength;
                node.position = Vector3.Lerp(lengthKeeper, node.position, sharedMaterial.GetSlackness(node.normalizedLength));

                node.speed = (node.position - lastPosition) / deltaTime;
            }
            else
            {
                node.position = node.transform.position;
            }

            for (int i = 0; i < node.children.Count; ++i)
            {
                UpdateNode(node.children[i], deltaTime);
            }
        }
        private void ApplyToTransforms()
        {
            for (int i = 0; i < m_PhysicsTrees.Count; i++)
            {
                m_PhysicsTrees[i].ApplyToTransform(true);
            }
        }

        private void DrawNodeGizmos(TreeNode node)
        {
            for (int i = 0; i < node.children.Count; i++)
            {
                DrawNodeGizmos(node.children[i]);
            }
            Gizmos.color = Color.Lerp(Color.white, Color.red, node.normalizedLength);
            if (node.depth > startDepth)
                Gizmos.DrawWireSphere(node.position, node.radius);
            if (node.parent != null)
                Gizmos.DrawLine(node.parent.position, node.position);
        }
    }
}
