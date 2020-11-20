/* Author:          ezhex1991@outlook.com
 * CreateTime:      2020-10-23 15:37:33
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    [CustomEditor(typeof(EZShadowProjector))]
    public class EZShadowProjectorEditor : Editor
    {
        private EZShadowProjector shadowProjector;

        private void OnEnable()
        {
            shadowProjector = target as EZShadowProjector;
        }

        public override bool HasPreviewGUI()
        {
            return true;
        }
        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            if (shadowProjector.shadowTexture == null) return;
            GUI.DrawTexture(r, shadowProjector.shadowTexture, ScaleMode.ScaleToFit);
        }
    }
}
