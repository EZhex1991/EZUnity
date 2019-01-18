/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-05-28 10:26:34
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZUnity
{
    [CustomEditor(typeof(EZExplosive)), CanEditMultipleObjects]
    public class EZExplosiveEditor : Editor
    {
        private Rect uiRect = new Rect(0, 0, 80, 20);
        private Vector2 offset = new Vector2(10, 50);

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (EditorApplication.isPlaying && GUILayout.Button("Explode"))
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    (targets[i] as EZExplosive).Explode();
                }
            }
        }
    }
}
