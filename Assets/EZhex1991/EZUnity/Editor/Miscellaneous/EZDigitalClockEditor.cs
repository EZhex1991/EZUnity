/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-05-11 13:20:19
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;

namespace EZhex1991.EZUnity
{
    [CustomEditor(typeof(EZDigitalClock))]
    public class EZDigitalClockEditor : Editor
    {
        private EZDigitalClock clock;

        private void OnEnable()
        {
            clock = target as EZDigitalClock;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.LabelField("Time Text", clock.timeText);
        }
    }
}
