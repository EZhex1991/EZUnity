/* Author:          熊哲
 * CreateTime:      2018-04-13 20:25:40
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZUnity.Playables
{
    public abstract class KeyframeClipEditor<T> : Editor
        where T : IKeyframe
    {
        protected KeyframeClip<T> asset;

        protected Vector2 timeRange;
        protected Vector2Int indexRange;

        protected virtual void OnEnable()
        {
            asset = target as KeyframeClip<T>;
        }

        public override void OnInspectorGUI()
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject(target as ScriptableObject), typeof(MonoScript), false);
            GUI.enabled = true;
            serializedObject.Update();
            DrawInfoGUI();
            EditorGUILayout.Space();
            DrawRemoveGUI();
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void DrawInfoGUI()
        {
            EditorGUILayout.LabelField("Info", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Actual Duration", asset.duration.ToString("f2"));
            EditorGUILayout.LabelField("Keyframe Count", asset.keyframes.Count.ToString());
        }
        protected virtual void DrawRemoveGUI()
        {
            EditorGUILayout.LabelField("Remove By Time", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            timeRange.x = EditorGUILayout.FloatField("Start Time", timeRange.x);
            timeRange.y = EditorGUILayout.FloatField("End Time", timeRange.y);
            EditorGUILayout.EndHorizontal();

            indexRange = KeyframeUtility.GetRangeByTime(asset.keyframes, timeRange);
            if (asset.keyframes.Count > 0 && timeRange.x <= asset.keyframes[indexRange.x].time)
            {
                indexRange.x -= 1;
            }

            GUI.enabled = false;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.IntField("Start Index", indexRange.x + 1);
            EditorGUILayout.IntField("End Index", indexRange.y);
            EditorGUILayout.EndHorizontal();
            GUI.enabled = true;

            if (GUILayout.Button("Confirm"))
            {
                asset.keyframes.RemoveRange(indexRange.x + 1, indexRange.y - indexRange.x);
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
            }
        }
        protected virtual void DrawBaseGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
