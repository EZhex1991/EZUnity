/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-07-02 17:30:02
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;

namespace EZhex1991.EZUnity
{
    [CustomEditor(typeof(EZPath))]
    public class EZPathEditor : Editor
    {
        private EZPath path;

        private void OnEnable()
        {
            path = target as EZPath;
        }

        private void OnSceneGUI()
        {
            if (path.pathMode == EZPath.PathMode.Bezier)
            {
                for (int i = 0; i < path.pathPoints.Count; i++)
                {
                    EZPathPointEditor.DrawTangentHandles(path.pathPoints[i]);
                }
            }
        }
    }
}
