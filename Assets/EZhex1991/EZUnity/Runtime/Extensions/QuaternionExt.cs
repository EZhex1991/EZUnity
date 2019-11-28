/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-11-28 14:36:44
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public static class QuaternionExt
    {
        public const double k_Epsilon = 1e-5;

        public static float Magnitude(this Quaternion q)
        {
            return Mathf.Sqrt((Quaternion.Dot(q, q)));
        }
        public static float SqrMagnitude(this Quaternion q)
        {
            return (Quaternion.Dot(q, q));
        }
        public static Quaternion Scale(this Quaternion q, float scale)
        {
            q.x *= scale;
            q.y *= scale;
            q.z *= scale;
            q.w *= scale;
            return q;
        }
        public static Quaternion NormalizeSafe(this Quaternion q)
        {
            float sqrMagnitude = Quaternion.Dot(q, q);
            if (sqrMagnitude < k_Epsilon) return Quaternion.identity;
            return q.Scale(sqrMagnitude);
        }

        public static Quaternion Cumulate(Quaternion q1, Quaternion q2)
        {
            if (Quaternion.Dot(q1, q2) < 0)
            {
                return Add(q1, new Quaternion(-q2.x, -q2.y, -q2.z, -q2.w));
            }
            else
            {
                return Add(q1, q2);
            }
        }
        public static Quaternion Cumulate(params Quaternion[] qs)
        {
            Quaternion output = new Quaternion();
            for (int i = 0; i < qs.Length; i++)
            {
                Cumulate(output, qs[i]);
            }
            return output;
        }

        public static Quaternion Add(Quaternion q1, Quaternion q2)
        {
            return new Quaternion(
                q1.x + q2.x,
                q1.y + q2.y,
                q1.z + q2.z,
                q1.w + q2.w
            );
        }
        public static Quaternion Add(params Quaternion[] qs)
        {
            Quaternion output = new Quaternion();
            for (int i = 0; i < qs.Length; i++)
            {
                Add(output, qs[i]);
            }
            return output;
        }
    }
}
