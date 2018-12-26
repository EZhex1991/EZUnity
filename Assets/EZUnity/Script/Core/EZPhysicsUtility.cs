/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-12-18 20:43:20
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZUnity
{
    public static partial class EZPhysicsUtility
    {
        public static void SphereOutsideSphere(ref Vector3 position, SphereCollider collider, float spacing)
        {
            Vector3 scale = collider.transform.localScale.Abs();
            float radius = collider.radius * Mathf.Max(scale.x, scale.y, scale.z);
            SphereOutsideSphere(ref position, collider.transform.TransformPoint(collider.center), radius + spacing);
        }
        public static void SphereOutsideSphere(ref Vector3 position, Vector3 spherePosition, float distance)
        {
            Vector3 bounceDir = position - spherePosition;
            if (bounceDir.magnitude < distance)
            {
                position = spherePosition + bounceDir.normalized * distance;
            }
        }
        public static void SphereInsideSphere(ref Vector3 position, SphereCollider collider, float spacing)
        {
            SphereInsideSphere(ref position, collider.transform.TransformPoint(collider.center), collider.radius - spacing);
        }
        public static void SphereInsideSphere(ref Vector3 position, Vector3 spherePosition, float distance)
        {
            Vector3 bounceDir = position - spherePosition;
            if (bounceDir.magnitude > distance)
            {
                position = spherePosition + bounceDir.normalized * distance;
            }
        }

        public static void CapsuleSpheres(CapsuleCollider collider, out Vector3 sphereP0, out Vector3 sphereP1, out float radius)
        {
            Vector3 scale = collider.transform.localScale.Abs();
            radius = collider.radius;
            sphereP0 = sphereP1 = collider.center;
            float height = collider.height * 0.5f;
            switch (collider.direction)
            {
                case 0:
                    radius *= Mathf.Max(scale.y, scale.z);
                    height = Mathf.Max(0, height - radius / scale.x);
                    sphereP0.x -= height;
                    sphereP1.x += height;
                    break;
                case 1:
                    radius *= Mathf.Max(scale.x, scale.z);
                    height = Mathf.Max(0, height - radius / scale.y);
                    sphereP0.y -= height;
                    sphereP1.y += height;
                    break;
                case 2:
                    radius *= Mathf.Max(scale.x, scale.y);
                    height = Mathf.Max(0, height - radius / scale.z);
                    sphereP0.z -= height;
                    sphereP1.z += height;
                    break;
            }
            sphereP0 = collider.transform.TransformPoint(sphereP0);
            sphereP1 = collider.transform.TransformPoint(sphereP1);
        }
        public static void SphereOutsideCapsule(ref Vector3 position, CapsuleCollider collider, float spacing)
        {
            Vector3 sphereP0, sphereP1;
            float radius;
            CapsuleSpheres(collider, out sphereP0, out sphereP1, out radius);
            SphereOutsideCapsule(ref position, sphereP0, sphereP1, radius + spacing);
        }
        public static void SphereOutsideCapsule(ref Vector3 position, Vector3 sphereP0, Vector3 sphereP1, float distance)
        {
            Vector3 capsuleDir = sphereP1 - sphereP0;
            Vector3 boneDir = position - sphereP0;

            float dot = Vector3.Dot(capsuleDir, boneDir);
            if (dot <= 0)
            {
                SphereOutsideSphere(ref position, sphereP0, distance);
            }
            else if (dot >= capsuleDir.sqrMagnitude)
            {
                SphereOutsideSphere(ref position, sphereP1, distance);
            }
            else
            {
                Vector3 bounceDir = boneDir - capsuleDir.normalized * dot / capsuleDir.magnitude;
                float bounceDis = bounceDir.magnitude;
                if (bounceDis < distance)
                {
                    position += bounceDir.normalized * (distance - bounceDis);
                }
            }
        }
        public static void SphereInsideCapsule(ref Vector3 position, CapsuleCollider collider, float spacing)
        {
            Vector3 sphereP0, sphereP1;
            float radius;
            CapsuleSpheres(collider, out sphereP0, out sphereP1, out radius);
            SphereInsideCapsule(ref position, sphereP0, sphereP1, radius - spacing);
        }
        public static void SphereInsideCapsule(ref Vector3 position, Vector3 sphereP0, Vector3 sphereP1, float distance)
        {
            Vector3 capsuleDir = sphereP1 - sphereP0;
            Vector3 boneDir = position - sphereP0;

            float dot = Vector3.Dot(capsuleDir, boneDir);
            if (dot <= 0)
            {
                SphereInsideSphere(ref position, sphereP0, distance);
            }
            else if (dot >= capsuleDir.sqrMagnitude)
            {
                SphereInsideSphere(ref position, sphereP1, distance);
            }
            else
            {
                Vector3 bounceDir = boneDir - capsuleDir.normalized * dot / capsuleDir.magnitude;
                float bounceDis = bounceDir.magnitude;
                if (bounceDis > distance)
                {
                    position += bounceDir.normalized * (distance - bounceDis);
                }
            }
        }

        public static void SphereOutsideCollider(ref Vector3 position, Collider collider, float spacing)
        {
            Vector3 closestPoint = collider.ClosestPoint(position);
            if (position == closestPoint) // inside collider
            {
                Vector3 bounceDir = position - collider.bounds.center;
                Debug.DrawLine(collider.bounds.center, closestPoint, Color.red);
                position = closestPoint + bounceDir.normalized * spacing;
            }
            else
            {
                Vector3 bounceDir = position - closestPoint;
                if (bounceDir.magnitude < spacing)
                {
                    position = closestPoint + bounceDir.normalized * spacing;
                }
            }
        }
    }
}
