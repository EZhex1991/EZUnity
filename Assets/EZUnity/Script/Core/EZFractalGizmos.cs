/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-01-27 13:44:14
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZUnity
{
    public class EZFractalGizmos : MonoBehaviour
    {
        public enum Shape { KochSnowFlake, FlowSnake, SierpinskiTriangle, FractalTree, FractalTree2 }

        public Shape shape;
        [Range(0, 8)]
        public int subDivisions;

        private static float sqrt3 = Mathf.Sqrt(3f);
        private static float sqrt5 = Mathf.Sqrt(5f);
        private static float sqrt7 = Mathf.Sqrt(7f);

        private static Matrix4x4 scaleMatrix_1_3rd = Matrix4x4.Scale(Vector3.one / 3);
        private static Matrix4x4 scaleMatrix_05 = Matrix4x4.Scale(Vector3.one * 0.5f);
        private static Matrix4x4 scaleMatrix_07 = Matrix4x4.Scale(Vector3.one * 0.7f);

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
            }
        }

        public static void DrawKochSnowFlake(int sub)
        {
            Matrix4x4 matrix = Gizmos.matrix;
            DrawKochSnowFlakeSub(matrix * Matrix4x4.Translate(new Vector3(0, sqrt3 / 3)), sub);
            DrawKochSnowFlakeSub(matrix * Matrix4x4.Rotate(Quaternion.Euler(0, 0, 120)) * Matrix4x4.Translate(new Vector3(0, sqrt3 / 3)), sub);
            DrawKochSnowFlakeSub(matrix * Matrix4x4.Rotate(Quaternion.Euler(0, 0, -120)) * Matrix4x4.Translate(new Vector3(0, sqrt3 / 3)), sub);
        }
        private static void DrawKochSnowFlakeSub(Matrix4x4 matrix, int sub)
        {
            if (sub > 0)
            {
                matrix *= scaleMatrix_1_3rd;
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
                matrix *= Matrix4x4.Rotate(Quaternion.Euler(0, 0, -19));
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
                matrix *= scaleMatrix_05;
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
                DrawFractalTreeSub(matrix * scaleMatrix_07, sub - 1);
                DrawFractalTreeSub(matrix * scaleMatrix_1_3rd * Matrix4x4.Rotate(Quaternion.Euler(0, 0, -60)), sub - 1);
                DrawFractalTreeSub(matrix * scaleMatrix_1_3rd * Matrix4x4.Rotate(Quaternion.Euler(0, 0, 60)), sub - 1);
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
                matrix *= Matrix4x4.Translate(Vector3.up) * scaleMatrix_07;
                DrawFractalTree2Sub(matrix * Matrix4x4.Rotate(Quaternion.Euler(0, 0, -30)), sub - 1);
                DrawFractalTree2Sub(matrix * Matrix4x4.Rotate(Quaternion.Euler(0, 0, 30)), sub - 1);
            }
            else
            {
                Gizmos.matrix = matrix;
                Gizmos.DrawLine(Vector3.zero, Vector3.up);
            }
        }
    }
}
