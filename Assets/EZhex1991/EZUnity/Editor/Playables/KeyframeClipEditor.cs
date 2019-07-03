/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-04-13 20:25:40
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZUnity.Playables
{
    public abstract class KeyframeClipEditor<T> : Editor
        where T : IKeyframe
    {
        protected KeyframeClip<T> asset;

        protected float startTime;
        protected float endTime;
        protected int startIndex;
        protected int endIndex;

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
            startTime = EditorGUILayout.FloatField("Start Time", startTime);
            endTime = EditorGUILayout.FloatField("End Time", endTime);
            EditorGUILayout.EndHorizontal();

            startIndex = KeyframeUtility.GetIndex(startTime, asset.keyframes);
            endIndex = KeyframeUtility.GetIndex(endTime, asset.keyframes);
            if (asset.keyframes.Count > 0 && startTime <= asset.keyframes[startIndex].time)
            {
                startIndex -= 1;
            }

            GUI.enabled = false;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.IntField("Start Index", startIndex + 1);
            EditorGUILayout.IntField("End Index", endIndex);
            EditorGUILayout.EndHorizontal();
            GUI.enabled = true;

            if (GUILayout.Button("Confirm"))
            {
                asset.keyframes.RemoveRange(startIndex + 1, endIndex - startIndex);
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
