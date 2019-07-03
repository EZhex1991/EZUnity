/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-05-28 10:26:34
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CustomEditor(typeof(EZExplosive)), CanEditMultipleObjects]
    public class EZExplosiveEditor : Editor
    {
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
