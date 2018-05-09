/* Author:          熊哲
 * CreateTime:      2016-12-26 16:54:18
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using EZFramework;
using UnityEditor;
using UnityEngine;

namespace EZFrameworkEditor
{
    [CustomEditor(typeof(EZSound))]
    public class EZSoundEditor : Editor
    {
        private EZSound ezSound;

        void OnEnable()
        {
            ezSound = target as EZSound;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ezSound.SpatialBlend = EditorGUILayout.Slider("Global Spatial Blend", ezSound.SpatialBlend, 0, 1);

            ezSound.BgmActive = EditorGUILayout.Toggle("BGM", ezSound.BgmActive);
            if (ezSound.BgmActive)
                ezSound.BgmVolume = EditorGUILayout.Slider(ezSound.BgmVolume, 0, 1);

            ezSound.EfxActive = EditorGUILayout.Toggle("EFX", ezSound.EfxActive);
            if (ezSound.EfxActive)
                ezSound.EfxVolume = EditorGUILayout.Slider(ezSound.EfxVolume, 0, 1);

            if (GUI.changed) EditorUtility.SetDirty(ezSound);
        }
    }
}