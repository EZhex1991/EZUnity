/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-07-11 19:17:13
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity
{
    public class EZTimePanel : EditorWindow
    {
        public string input;

        public DateTime time;
        public string format = "yyyy-MM-dd HH:mm:ss";

        public GUIStyle style;

        private void OnEnable()
        {
            time = DateTime.Now;
        }

        private void OnGUI()
        {
            EZEditorGUIUtility.WindowTitle(this);

            DrawLabel("DateTime", time.ToString());
            EditorGUILayout.LabelField("Format", format);

            EditorGUILayout.Space();
            input = EditorGUILayout.DelayedTextField("Input", input);
            if (GUILayout.Button("Refresh"))
            {
                try
                {
                    time = DateTime.Parse(input);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
            if (GUILayout.Button("Now"))
            {
                time = DateTime.Now;
                input = time.ToString(format);
                GUI.FocusControl(null);
            }

            EditorGUILayout.Space();
            DrawLabel("ToLongDateString", time.ToLongDateString());
            DrawLabel("ToShortDateString", time.ToShortDateString());
            DrawLabel("ToLongTimeString", time.ToLongTimeString());
            DrawLabel("ToShortTimeString", time.ToShortTimeString());

            EditorGUILayout.Space();
            DrawLabel("Kind", time.Kind.ToString());
            DrawLabel("Year", time.Year.ToString());
            DrawLabel("Month", time.Month.ToString());
            DrawLabel("Day", time.Day.ToString());
            DrawLabel("Hour", time.Hour.ToString());
            DrawLabel("Minute", time.Minute.ToString());
            DrawLabel("Second", time.Second.ToString());
            DrawLabel("Millisecond", time.Millisecond.ToString());

            EditorGUILayout.Space();
            DrawLabel("DayOfWeek", time.DayOfWeek.ToString());
            DrawLabel("DayOfYear", time.DayOfYear.ToString());
            DrawLabel("TimeOfDay", time.TimeOfDay.ToString());
            DrawLabel("Ticks", time.Ticks.ToString());

            EditorGUILayout.Space();
            DrawLabel("ToBinary", time.ToBinary().ToString());
            DrawLabel("ToFileTime", time.ToFileTime().ToString());
            DrawLabel("ToFileTimeUtc", time.ToFileTimeUtc().ToString());
            DrawLabel("ToLocalTime", time.ToLocalTime().ToString());
            DrawLabel("ToOADate", time.ToOADate().ToString());
            DrawLabel("ToUniversalTime", time.ToUniversalTime().ToString());
        }

        public void DrawLabel(string label1, string text)
        {
            EditorGUILayout.TextField(label1, text, EditorStyles.wordWrappedLabel);
        }
    }
}
