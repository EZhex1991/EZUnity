/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-01-27 13:44:14
 * Organization:    #ORGANIZATION#
 * Description:     
 */
#pragma warning disable 0414
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZFractalGizmos : MonoBehaviour
    {
        public enum Shape { KochSnowFlake, FlowSnake, SierpinskiTriangle, FractalTree, FractalTree2, FractalRect, FractalTriangle }

        private static readonly float sqrt3 = Mathf.Sqrt(3f);
        private static readonly float sqrt5 = Mathf.Sqrt(5f);
        private static readonly float sqrt7 = Mathf.Sqrt(7f);

        private static readonly float d3 = 1 / 3f;
        private static readonly float d7 = 1 / 7f;
        private static readonly float d9 = 1 / 9f;

        private static Matrix4x4 scaleMatrix_d3 = Matrix4x4.Scale(Vector3.one * d3);
        private static Matrix4x4 scaleMatrix_d2 = Matrix4x4.Scale(Vector3.one * 0.5f);
        private static Matrix4x4 scaleMatrix_m07 = Matrix4x4.Scale(Vector3.one * 0.7f);

        public Shape shape;
        [Range(0, 8)]
        public int subDivisions;

        public Color colorStart = Color.red;
        public Color colorEnd = Color.yellow;

        public float treeBranchAngle = 30f;

        private void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            switch (shape)
            {
                case Shape.FlowSnake:
                    DrawFlowSnake(subDivisions, colorStart, colorEnd);
                    break;
                case Shape.KochSnowFlake:
                    DrawKochSnowFlake(subDivisions, colorStart, colorEnd);
                    break;
                case Shape.SierpinskiTriangle:
                    DrawSierpinskiTriangle(subDivisions, colorStart, colorEnd);
                    break;
                case Shape.FractalTree:
                    DrawFractalTree(subDivisions, treeBranchAngle, colorStart, colorEnd);
                    break;
                case Shape.FractalTree2:
                    DrawFractalTree2(subDivisions, treeBranchAngle, colorStart, colorEnd);
                    break;
                case Shape.FractalRect:
                    DrawFractalRect(subDivisions, colorStart, colorEnd);
                    break;
                case Shape.FractalTriangle:
                    DrawFractalTriangle(subDivisions, colorStart, colorEnd);
                    break;
            }
        }

        public static void DrawKochSnowFlake(int sub, Color colorStart, Color colorEnd)
        {
            Matrix4x4 matrix = Gizmos.matrix;
            DrawKochSnowFlakeSub(matrix * Matrix4x4.Translate(new Vector3(0, sqrt3 * d3)), sub, colorStart, colorEnd);
            DrawKochSnowFlakeSub(matrix * Matrix4x4.Rotate(Quaternion.Euler(0, 0, 120)) * Matrix4x4.Translate(new Vector3(0, sqrt3 * d3)), sub, colorStart, colorEnd);
            DrawKochSnowFlakeSub(matrix * Matrix4x4.Rotate(Quaternion.Euler(0, 0, -120)) * Matrix4x4.Translate(new Vector3(0, sqrt3 * d3)), sub, colorStart, colorEnd);
        }
        private static void DrawKochSnowFlakeSub(Matrix4x4 matrix, int sub, Color colorStart, Color colorEnd)
        {
            if (sub > 0)
            {
                matrix *= scaleMatrix_d3;

                Color c1 = Color.Lerp(colorStart, colorEnd, d3);

                DrawKochSnowFlakeSub(matrix * Matrix4x4.Translate(new Vector3(-2, 0)), sub - 1, c1, colorEnd);
                DrawKochSnowFlakeSub(matrix * Matrix4x4.Translate(new Vector3(2, 0)), sub - 1, c1, colorEnd);
                DrawKochSnowFlakeSub(matrix * Matrix4x4.Translate(new Vector3(-1, sqrt3) * 0.5f) * Matrix4x4.Rotate(Quaternion.Euler(0, 0, 60)), sub - 1, c1, colorEnd);
                DrawKochSnowFlakeSub(matrix * Matrix4x4.Translate(new Vector3(1, sqrt3) * 0.5f) * Matrix4x4.Rotate(Quaternion.Euler(0, 0, -60)), sub - 1, c1, colorEnd);
            }
            else
            {
                Gizmos.matrix = matrix;
                Gizmos.color = colorStart;
                Gizmos.DrawLine(new Vector3(-1, 0), new Vector3(1, 0));
            }
        }

        public static void DrawFlowSnake(int sub, Color colorStart, Color colorEnd)
        {
            Matrix4x4 matrix = Gizmos.matrix * Matrix4x4.Translate(new Vector3(-0.5f, -0.25f));
            DrawFlowSnakeSub(matrix, sub, colorStart, colorEnd);
        }
        private static void DrawFlowSnakeSub(Matrix4x4 matrix, int sub, Color colorStart, Color colorEnd)
        {
            if (sub > 0)
            {
                Color c1 = Color.Lerp(colorStart, colorEnd, d7);
                Color c2 = Color.Lerp(colorStart, colorEnd, d7 * 2);
                Color c3 = Color.Lerp(colorStart, colorEnd, d7 * 3);
                Color c4 = Color.Lerp(colorStart, colorEnd, d7 * 4);
                Color c5 = Color.Lerp(colorStart, colorEnd, d7 * 5);
                Color c6 = Color.Lerp(colorStart, colorEnd, d7 * 6);

                matrix *= Matrix4x4.Scale(Vector3.one / sqrt7) * Matrix4x4.Rotate(Quaternion.Euler(0, 0, 19.1f));
                Matrix4x4 m1 = matrix * Matrix4x4.Rotate(Quaternion.Euler(0, 0, -120)) * Matrix4x4.Translate(Vector3.left);
                Matrix4x4 m2 = m1 * Matrix4x4.Rotate(Quaternion.Euler(0, 0, 120));
                Matrix4x4 m3 = m2 * Matrix4x4.Translate(Vector3.right);
                Matrix4x4 m4 = m3 * Matrix4x4.Translate(Vector3.right) * Matrix4x4.Rotate(Quaternion.Euler(0, 0, -120));
                Matrix4x4 m5 = m4 * Matrix4x4.Translate(Vector3.right) * Matrix4x4.Rotate(Quaternion.Euler(0, 0, 120)) * Matrix4x4.Translate(Vector3.left);
                Matrix4x4 m6 = m5 * Matrix4x4.Rotate(Quaternion.Euler(0, 0, 120)) * Matrix4x4.Translate(Vector3.left);
                Matrix4x4 m7 = m6 * Matrix4x4.Rotate(Quaternion.Euler(0, 0, -120));

                DrawFlowSnakeSub(m1, sub - 1, c1, colorStart);
                DrawFlowSnakeSub(m2, sub - 1, c1, c2);
                DrawFlowSnakeSub(m3, sub - 1, c2, c3);
                DrawFlowSnakeSub(m4, sub - 1, c3, c4);
                DrawFlowSnakeSub(m5, sub - 1, c5, c4);
                DrawFlowSnakeSub(m6, sub - 1, c6, c5);
                DrawFlowSnakeSub(m7, sub - 1, c6, colorEnd);
            }
            else
            {
                Gizmos.matrix = matrix;
                Gizmos.color = colorStart;
                Gizmos.DrawLine(Vector3.zero, Vector3.right);
            }
        }

        public static void DrawSierpinskiTriangle(int sub, Color colorStart, Color colorEnd)
        {
            Matrix4x4 matrix = Gizmos.matrix * Matrix4x4.Translate(new Vector3(0, sqrt3 * -0.33f));
            DrawSierpinskiTriangleSub(matrix, sub, colorStart, colorEnd);
        }
        private static void DrawSierpinskiTriangleSub(Matrix4x4 matrix, int sub, Color colorStart, Color colorEnd)
        {
            if (sub > 0)
            {
                Color c1 = Color.Lerp(colorStart, colorEnd, d3);
                Color c2 = Color.Lerp(colorStart, colorEnd, d3 * 2);

                matrix *= scaleMatrix_d2;
                Matrix4x4 m1 = matrix * Matrix4x4.Rotate(Quaternion.Euler(0, 0, -120)) * Matrix4x4.Translate(new Vector3(0, -sqrt3));
                Matrix4x4 m2 = matrix * Matrix4x4.Translate(new Vector3(0, sqrt3));
                Matrix4x4 m3 = matrix * Matrix4x4.Rotate(Quaternion.Euler(0, 0, 120)) * Matrix4x4.Translate(new Vector3(0, -sqrt3));

                DrawSierpinskiTriangleSub(m1, sub - 1, c1, colorStart);
                DrawSierpinskiTriangleSub(m2, sub - 1, c1, c2);
                DrawSierpinskiTriangleSub(m3, sub - 1, colorEnd, c2);
            }
            else
            {
                Gizmos.matrix = matrix;
                Gizmos.color = colorStart;
                Gizmos.DrawLine(new Vector3(-1, 0), new Vector3(1, 0));
            }
        }

        public static void DrawFractalTree(int sub, float branchAngle, Color colorStart, Color colorEnd)
        {
            DrawFractalTreeSub(Gizmos.matrix, sub, branchAngle, colorStart, colorEnd);
        }
        private static void DrawFractalTreeSub(Matrix4x4 matrix, int sub, float branchAngle, Color colorStart, Color colorEnd)
        {
            if (sub > 0)
            {
                Color c1 = Color.Lerp(colorStart, colorEnd, 0.5f);

                Gizmos.matrix = matrix;
                Gizmos.color = c1;
                Gizmos.DrawLine(Vector3.zero, Vector3.up);

                matrix *= Matrix4x4.Translate(Vector3.up);
                Matrix4x4 m1 = matrix * scaleMatrix_m07;
                Matrix4x4 m2 = matrix * scaleMatrix_d3 * Matrix4x4.Rotate(Quaternion.Euler(0, 0, -branchAngle));
                Matrix4x4 m3 = matrix * scaleMatrix_d3 * Matrix4x4.Rotate(Quaternion.Euler(0, 0, branchAngle));

                DrawFractalTreeSub(m1, sub - 1, branchAngle, c1, colorEnd);
                DrawFractalTreeSub(m2, sub - 1, branchAngle, c1, colorEnd);
                DrawFractalTreeSub(m3, sub - 1, branchAngle, c1, colorEnd);
            }
            else
            {
                Gizmos.matrix = matrix;
                Gizmos.color = colorStart;
                Gizmos.DrawLine(Vector3.zero, Vector3.up);
            }
        }

        public static void DrawFractalTree2(int sub, float branchAngle, Color colorStart, Color colorEnd)
        {
            Matrix4x4 matrix = Gizmos.matrix;
            DrawFractalTree2Sub(matrix, sub, branchAngle, colorStart, colorEnd);
        }
        private static void DrawFractalTree2Sub(Matrix4x4 matrix, int sub, float branchAngle, Color colorStart, Color colorEnd)
        {
            if (sub > 0)
            {
                Color c1 = Color.Lerp(colorStart, colorEnd, 0.5f);

                Gizmos.color = c1;
                Gizmos.matrix = matrix;
                Gizmos.DrawLine(Vector3.zero, Vector3.up);

                matrix *= Matrix4x4.Translate(Vector3.up) * scaleMatrix_m07;
                Matrix4x4 m1 = matrix * Matrix4x4.Rotate(Quaternion.Euler(0, 0, -branchAngle));
                Matrix4x4 m2 = matrix * Matrix4x4.Rotate(Quaternion.Euler(0, 0, branchAngle));

                DrawFractalTree2Sub(m1, sub - 1, branchAngle, c1, colorEnd);
                DrawFractalTree2Sub(m2, sub - 1, branchAngle, c1, colorEnd);
            }
            else
            {
                Gizmos.matrix = matrix;
                Gizmos.color = colorStart;
                Gizmos.DrawLine(Vector3.zero, Vector3.up);
            }
        }

        public static void DrawFractalRect(int sub, Color colorStart, Color colorEnd)
        {
            Matrix4x4 matrix = Gizmos.matrix;
            DrawFractalRectSub(matrix, sub, colorStart, colorEnd);
        }
        private static void DrawFractalRectSub(Matrix4x4 matrix, int sub, Color colorStart, Color colorEnd)
        {
            if (sub > 0)
            {
                Color c1 = Color.Lerp(colorStart, colorEnd, d9);
                Color c2 = Color.Lerp(colorStart, colorEnd, d9 * 2);
                Color c3 = Color.Lerp(colorStart, colorEnd, d9 * 3);
                Color c4 = Color.Lerp(colorStart, colorEnd, d9 * 4);
                Color c5 = Color.Lerp(colorStart, colorEnd, d9 * 5);
                Color c6 = Color.Lerp(colorStart, colorEnd, d9 * 6);
                Color c7 = Color.Lerp(colorStart, colorEnd, d9 * 7);
                Color c8 = Color.Lerp(colorStart, colorEnd, d9 * 8);

                matrix *= Matrix4x4.Scale(Vector3.one / 3);
                Matrix4x4 m1 = matrix * Matrix4x4.Translate(new Vector3(-2, 0, 0));
                Matrix4x4 m2 = matrix;
                Matrix4x4 m3 = matrix * Matrix4x4.Translate(new Vector3(2, 0, 0));
                Matrix4x4 m4 = matrix * Matrix4x4.Translate(new Vector3(1, 1, 0)) * Matrix4x4.Rotate(Quaternion.Euler(0, 0, 90));
                Matrix4x4 m5 = matrix * Matrix4x4.Translate(new Vector3(0, 2, 0)) * Matrix4x4.Rotate(Quaternion.Euler(0, 0, 180));
                Matrix4x4 m6 = m4 * Matrix4x4.Translate(new Vector3(0, 2, 0)) * Matrix4x4.Rotate(Quaternion.Euler(0, 0, 180));
                Matrix4x4 m7 = m6 * Matrix4x4.Translate(new Vector3(2, 0, 0));
                Matrix4x4 m8 = m2 * Matrix4x4.Translate(new Vector3(0, -2, 0));
                Matrix4x4 m9 = m4 * Matrix4x4.Translate(new Vector3(-2, 0, 0));

                DrawFractalRectSub(m1, sub - 1, colorStart, c1);
                DrawFractalRectSub(m7, sub - 1, c1, c2);
                DrawFractalRectSub(m8, sub - 1, c2, c3);
                DrawFractalRectSub(m9, sub - 1, c3, c4);
                DrawFractalRectSub(m4, sub - 1, c4, c5);
                DrawFractalRectSub(m5, sub - 1, c5, c6);
                DrawFractalRectSub(m6, sub - 1, c6, c7);
                DrawFractalRectSub(m2, sub - 1, c7, c8);
                DrawFractalRectSub(m3, sub - 1, c8, colorEnd);
            }
            else
            {
                Gizmos.matrix = matrix;
                Gizmos.color = colorStart;
                Gizmos.DrawLine(Vector3.left, Vector3.right);
            }
        }

        public static void DrawFractalTriangle(int sub, Color colorStart, Color colorEnd)
        {
            Matrix4x4 matrix = Gizmos.matrix * Matrix4x4.Translate(new Vector3(0, sqrt3 * -0.33f));
            DrawFractalTriangleSub(matrix, sub, colorStart, colorEnd);
        }
        private static void DrawFractalTriangleSub(Matrix4x4 matrix, int sub, Color colorStart, Color colorEnd)
        {
            if (sub > 0)
            {
                Color c1 = Color.Lerp(colorStart, colorEnd, 0.25f);
                Color c2 = Color.Lerp(colorStart, colorEnd, 0.50f);
                Color c3 = Color.Lerp(colorStart, colorEnd, 0.75f);

                matrix *= Matrix4x4.Scale(Vector3.one * 0.5f);
                Matrix4x4 m1 = matrix * Matrix4x4.Translate(new Vector3(-1, 0, 0));
                Matrix4x4 m2 = matrix * Matrix4x4.Rotate(Quaternion.Euler(0, 0, -60)) * Matrix4x4.Translate(new Vector3(-1, 0, 0));
                Matrix4x4 m3 = matrix * Matrix4x4.Translate(new Vector3(0, sqrt3, 0));
                Matrix4x4 m4 = matrix * Matrix4x4.Rotate(Quaternion.Euler(0, 0, 120)) * Matrix4x4.Translate(new Vector3(0, -sqrt3, 0));

                DrawFractalTriangleSub(m1, sub - 1, colorStart, c1);
                DrawFractalTriangleSub(m2, sub - 1, c1, c2);
                DrawFractalTriangleSub(m3, sub - 1, c2, c3);
                DrawFractalTriangleSub(m4, sub - 1, c3, colorEnd);
            }
            else
            {
                Gizmos.matrix = matrix;
                Gizmos.color = colorStart;
                Gizmos.DrawLine(Vector3.left, Vector3.right);
            }
        }
    }
}
