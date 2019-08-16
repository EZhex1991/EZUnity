/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-01-27 13:44:14
 * Organization:    #ORGANIZATION#
 * Description:     
 */
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

        public Color color1 = Color.red;
        public Color color2 = Color.yellow;

        private void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            switch (shape)
            {
                case Shape.FlowSnake:
                    DrawFlowSnake(subDivisions);
                    break;
                case Shape.KochSnowFlake:
                    DrawKochSnowFlake(subDivisions);
                    break;
                case Shape.SierpinskiTriangle:
                    DrawSierpinskiTriangle(subDivisions);
                    break;
                case Shape.FractalTree:
                    DrawFractalTree(subDivisions);
                    break;
                case Shape.FractalTree2:
                    DrawFractalTree2(subDivisions);
                    break;
                case Shape.FractalRect:
                    DrawFractalRect(subDivisions, color1, color2);
                    break;
                case Shape.FractalTriangle:
                    DrawFractalTriangle(subDivisions, color1, color2);
                    break;
            }
        }

        public static void DrawKochSnowFlake(int sub)
        {
            Matrix4x4 matrix = Gizmos.matrix;
            DrawKochSnowFlakeSub(matrix * Matrix4x4.Translate(new Vector3(0, sqrt3 * d3)), sub);
            DrawKochSnowFlakeSub(matrix * Matrix4x4.Rotate(Quaternion.Euler(0, 0, 120)) * Matrix4x4.Translate(new Vector3(0, sqrt3 * d3)), sub);
            DrawKochSnowFlakeSub(matrix * Matrix4x4.Rotate(Quaternion.Euler(0, 0, -120)) * Matrix4x4.Translate(new Vector3(0, sqrt3 * d3)), sub);
        }
        private static void DrawKochSnowFlakeSub(Matrix4x4 matrix, int sub)
        {
            if (sub > 0)
            {
                matrix *= scaleMatrix_d3;
                DrawKochSnowFlakeSub(matrix * Matrix4x4.Translate(new Vector3(-2, 0)), sub - 1);
                DrawKochSnowFlakeSub(matrix * Matrix4x4.Translate(new Vector3(2, 0)), sub - 1);
                DrawKochSnowFlakeSub(matrix * Matrix4x4.Translate(new Vector3(-1, sqrt3) * 0.5f) * Matrix4x4.Rotate(Quaternion.Euler(0, 0, 60)), sub - 1);
                DrawKochSnowFlakeSub(matrix * Matrix4x4.Translate(new Vector3(1, sqrt3) * 0.5f) * Matrix4x4.Rotate(Quaternion.Euler(0, 0, -60)), sub - 1);
            }
            else
            {
                Gizmos.matrix = matrix;
                Gizmos.DrawLine(new Vector3(-1, 0), new Vector3(1, 0));
            }
        }

        public static void DrawFlowSnake(int sub)
        {
            Matrix4x4 matrix = Gizmos.matrix;
            DrawFlowSnakeSub(matrix, sub);
        }
        private static void DrawFlowSnakeSub(Matrix4x4 matrix, int sub)
        {
            if (sub > 0)
            {
                matrix *= Matrix4x4.Scale(Vector3.one / sqrt7);
                matrix *= Matrix4x4.Rotate(Quaternion.Euler(0, 0, -19.1f));
                DrawFlowSnakeSub(matrix, sub - 1);
                matrix *= Matrix4x4.Translate(Vector3.right) * Matrix4x4.Rotate(Quaternion.Euler(0, 0, 60)) * Matrix4x4.Translate(Vector3.right);
                DrawFlowSnakeSub(matrix * Matrix4x4.Rotate(Quaternion.Euler(0, 0, 180)), sub - 1);
                matrix *= Matrix4x4.Rotate(Quaternion.Euler(0, 0, 120)) * Matrix4x4.Translate(Vector3.right);
                DrawFlowSnakeSub(matrix * Matrix4x4.Rotate(Quaternion.Euler(0, 0, 180)), sub - 1);
                matrix *= Matrix4x4.Rotate(Quaternion.Euler(0, 0, -60));
                DrawFlowSnakeSub(matrix, sub - 1);
                matrix *= Matrix4x4.Translate(Vector3.right) * Matrix4x4.Rotate(Quaternion.Euler(0, 0, -120));
                DrawFlowSnakeSub(matrix, sub - 1);
                matrix *= Matrix4x4.Translate(Vector3.right);
                DrawFlowSnakeSub(matrix, sub - 1);
                matrix *= Matrix4x4.Translate(Vector3.right) * Matrix4x4.Rotate(Quaternion.Euler(0, 0, -60)) * Matrix4x4.Translate(Vector3.right);
                DrawFlowSnakeSub(matrix * Matrix4x4.Rotate(Quaternion.Euler(0, 0, 180)), sub - 1);
            }
            else
            {
                Gizmos.matrix = matrix;
                Gizmos.DrawLine(Vector3.zero, Vector3.right);
            }
        }

        public static void DrawSierpinskiTriangle(int sub)
        {
            Matrix4x4 matrix = Gizmos.matrix;
            DrawSierpinskiTriangleSub(matrix, sub);
        }
        private static void DrawSierpinskiTriangleSub(Matrix4x4 matrix, int sub)
        {
            if (sub > 0)
            {
                matrix *= scaleMatrix_d2;
                DrawSierpinskiTriangleSub(matrix * Matrix4x4.Rotate(Quaternion.Euler(0, 0, 120)) * Matrix4x4.Translate(new Vector3(0, -sqrt3)), sub - 1);
                DrawSierpinskiTriangleSub(matrix * Matrix4x4.Translate(new Vector3(0, sqrt3)), sub - 1);
                DrawSierpinskiTriangleSub(matrix * Matrix4x4.Rotate(Quaternion.Euler(0, 0, -120)) * Matrix4x4.Translate(new Vector3(0, -sqrt3)), sub - 1);
            }
            else
            {
                Gizmos.matrix = matrix;
                Gizmos.DrawLine(new Vector3(-1, 0), new Vector3(1, 0));
            }
        }

        public static void DrawFractalTree(int sub)
        {
            DrawFractalTreeSub(Gizmos.matrix, sub);
        }
        private static void DrawFractalTreeSub(Matrix4x4 matrix, int sub)
        {
            if (sub > 0)
            {
                Gizmos.matrix = matrix;
                Gizmos.DrawLine(Vector3.zero, Vector3.up);
                matrix *= Matrix4x4.Translate(Vector3.up);
                DrawFractalTreeSub(matrix * scaleMatrix_m07, sub - 1);
                DrawFractalTreeSub(matrix * scaleMatrix_d3 * Matrix4x4.Rotate(Quaternion.Euler(0, 0, -60)), sub - 1);
                DrawFractalTreeSub(matrix * scaleMatrix_d3 * Matrix4x4.Rotate(Quaternion.Euler(0, 0, 60)), sub - 1);
            }
            else
            {
                Gizmos.matrix = matrix;
                Gizmos.DrawLine(Vector3.zero, Vector3.up);
            }
        }

        public static void DrawFractalTree2(int sub)
        {
            Matrix4x4 matrix = Gizmos.matrix;
            DrawFractalTree2Sub(matrix, sub);
        }
        private static void DrawFractalTree2Sub(Matrix4x4 matrix, int sub)
        {
            if (sub > 0)
            {
                Gizmos.matrix = matrix;
                Gizmos.DrawLine(Vector3.zero, Vector3.up);
                matrix *= Matrix4x4.Translate(Vector3.up) * scaleMatrix_m07;
                DrawFractalTree2Sub(matrix * Matrix4x4.Rotate(Quaternion.Euler(0, 0, -30)), sub - 1);
                DrawFractalTree2Sub(matrix * Matrix4x4.Rotate(Quaternion.Euler(0, 0, 30)), sub - 1);
            }
            else
            {
                Gizmos.matrix = matrix;
                Gizmos.DrawLine(Vector3.zero, Vector3.up);
            }
        }

        public static void DrawFractalRect(int sub, Color color1, Color color2)
        {
            Matrix4x4 matrix = Gizmos.matrix;
            DrawFractalRectSub(matrix, sub, color1, color2);
        }
        private static void DrawFractalRectSub(Matrix4x4 matrix, int sub, Color c1, Color c2)
        {
            if (sub > 0)
            {
                matrix *= Matrix4x4.Scale(Vector3.one / 3);

                Matrix4x4 matrix1 = matrix * Matrix4x4.Translate(new Vector3(-2, 0, 0));
                Matrix4x4 matrix2 = matrix;
                Matrix4x4 matrix3 = matrix * Matrix4x4.Translate(new Vector3(2, 0, 0));
                Matrix4x4 matrix4 = matrix * Matrix4x4.Translate(new Vector3(1, 1, 0)) * Matrix4x4.Rotate(Quaternion.Euler(0, 0, 90));
                Matrix4x4 matrix5 = matrix * Matrix4x4.Translate(new Vector3(0, 2, 0)) * Matrix4x4.Rotate(Quaternion.Euler(0, 0, 180));
                Matrix4x4 matrix6 = matrix4 * Matrix4x4.Translate(new Vector3(0, 2, 0)) * Matrix4x4.Rotate(Quaternion.Euler(0, 0, 180));
                Matrix4x4 matrix7 = matrix6 * Matrix4x4.Translate(new Vector3(2, 0, 0));
                Matrix4x4 matrix8 = matrix2 * Matrix4x4.Translate(new Vector3(0, -2, 0));
                Matrix4x4 matrix9 = matrix4 * Matrix4x4.Translate(new Vector3(-2, 0, 0));

                DrawFractalRectSub(matrix1, sub - 1, c1, Color.Lerp(c1, c2, d9));
                DrawFractalRectSub(matrix7, sub - 1, Color.Lerp(c1, c2, d9), Color.Lerp(c1, c2, d9 * 2));
                DrawFractalRectSub(matrix8, sub - 1, Color.Lerp(c1, c2, d9 * 2), Color.Lerp(c1, c2, d9 * 3));
                DrawFractalRectSub(matrix9, sub - 1, Color.Lerp(c1, c2, d9 * 3), Color.Lerp(c1, c2, d9 * 4));
                DrawFractalRectSub(matrix4, sub - 1, Color.Lerp(c1, c2, d9 * 4), Color.Lerp(c1, c2, d9 * 5));
                DrawFractalRectSub(matrix5, sub - 1, Color.Lerp(c1, c2, d9 * 5), Color.Lerp(c1, c2, d9 * 6));
                DrawFractalRectSub(matrix6, sub - 1, Color.Lerp(c1, c2, d9 * 6), Color.Lerp(c1, c2, d9 * 7));
                DrawFractalRectSub(matrix2, sub - 1, Color.Lerp(c1, c2, d9 * 7), Color.Lerp(c1, c2, d9 * 8));
                DrawFractalRectSub(matrix3, sub - 1, Color.Lerp(c1, c2, d9 * 8), c2);
            }
            else
            {
                Gizmos.matrix = matrix;
                Gizmos.color = Color.Lerp(c1, c2, 0.5f);
                Gizmos.DrawLine(Vector3.left, Vector3.right);
            }
        }

        public static void DrawFractalTriangle(int sub, Color color1, Color color2)
        {
            Matrix4x4 matrix = Gizmos.matrix;
            DrawFractalTriangleSub(matrix, sub, color1, color2);
        }
        private static void DrawFractalTriangleSub(Matrix4x4 matrix, int sub, Color c1, Color c2)
        {
            if (sub > 0)
            {
                matrix *= Matrix4x4.Scale(Vector3.one * 0.5f);

                Matrix4x4 matrix1 = matrix * Matrix4x4.Translate(new Vector3(-1, 0, 0));
                Matrix4x4 matrix2 = matrix * Matrix4x4.Rotate(Quaternion.Euler(0, 0, -60)) * Matrix4x4.Translate(new Vector3(-1, 0, 0));
                Matrix4x4 matrix3 = matrix * Matrix4x4.Translate(new Vector3(0, sqrt3, 0));
                Matrix4x4 matrix4 = matrix * Matrix4x4.Rotate(Quaternion.Euler(0, 0, 120)) * Matrix4x4.Translate(new Vector3(0, -sqrt3, 0));

                DrawFractalTriangleSub(matrix1, sub - 1, c1, Color.Lerp(c1, c2, 0.25f));
                DrawFractalTriangleSub(matrix2, sub - 1, Color.Lerp(c1, c2, 0.25f), Color.Lerp(c1, c2, 0.5f));
                DrawFractalTriangleSub(matrix3, sub - 1, Color.Lerp(c1, c2, 0.5f), Color.Lerp(c1, c2, 0.75f));
                DrawFractalTriangleSub(matrix4, sub - 1, Color.Lerp(c1, c2, 0.75f), c2);
            }
            else
            {
                Gizmos.matrix = matrix;
                Gizmos.color = Color.Lerp(c1, c2, 0.5f);
                Gizmos.DrawLine(Vector3.left, Vector3.right);
            }
        }
    }
}
