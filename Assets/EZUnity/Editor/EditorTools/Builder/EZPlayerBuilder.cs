/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-04-02 20:41:29
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace EZUnity
{
    [CreateAssetMenu(fileName = "EZPlayerBuilder", menuName = "EZUnity/EZPlayerBuilder", order = EZUtility.AssetOrder)]
    public class EZPlayerBuilder : ScriptableObject
    {
        public bool configButDontBuild;

        public EZBundleBuilder bundleBuilder;

        public string locationPathName;
        public SceneAsset[] scenes;

        public string companyName;
        public string productName;
        public string bundleIdentifier;
        public string bundleVersion;
        public int buildNumber;
        public Texture2D icon;

        public void Config(BuildTargetGroup buildTargetGroup, BuildTarget buildTarget)
        {
            if (!string.IsNullOrEmpty(companyName)) PlayerSettings.companyName = companyName;
            if (!string.IsNullOrEmpty(productName)) PlayerSettings.productName = productName;
            if (!string.IsNullOrEmpty(bundleVersion)) PlayerSettings.bundleVersion = bundleVersion;
            if (icon != null)
            {
                Texture2D[] icons = PlayerSettings.GetIconsForTargetGroup(buildTargetGroup, IconKind.Any);
                for (int i = 0; i < icons.Length; i++)
                {
                    icons[i] = icon;
                }
                PlayerSettings.SetIconsForTargetGroup(buildTargetGroup, icons, IconKind.Any);
            }
            if (!string.IsNullOrEmpty(bundleIdentifier)) PlayerSettings.SetApplicationIdentifier(buildTargetGroup, bundleIdentifier);
            switch (buildTargetGroup)
            {
                case BuildTargetGroup.Standalone:
                    PlayerSettings.macOS.buildNumber = buildNumber.ToString();
                    break;
                case BuildTargetGroup.iOS:
                    PlayerSettings.iOS.buildNumber = buildNumber.ToString();
                    break;
                case BuildTargetGroup.Android:
                    PlayerSettings.Android.bundleVersionCode = buildNumber;
                    break;
            }

        }
        public void Execute(BuildTargetGroup buildTargetGroup, BuildTarget buildTarget)
        {
            Config(buildTargetGroup, buildTarget);
            if (configButDontBuild) return;
            if (bundleBuilder != null)
            {
                bundleBuilder.Execute(buildTarget);
            }
            BuildPlayerOptions options = new BuildPlayerOptions();
            string[] scenePaths = new string[scenes.Length];
            for (int i = 0; i < scenePaths.Length; i++)
            {
                scenePaths[i] = AssetDatabase.GetAssetPath(scenes[i]);
            }
            options.scenes = scenePaths;
            if (string.IsNullOrEmpty(locationPathName))
            {
                locationPathName = EditorUtility.SaveFolderPanel("Choose Output Folder", "", "");
                if (string.IsNullOrEmpty(locationPathName)) return;
            }
            switch (buildTarget)
            {
                case BuildTarget.StandaloneWindows:
                    options.locationPathName = string.Format("{0}/{1}.exe", locationPathName, PlayerSettings.productName);
                    break;
                case BuildTarget.StandaloneWindows64:
                    options.locationPathName = string.Format("{0}/{1}.exe", locationPathName, PlayerSettings.productName);
                    break;
                default:
                    options.locationPathName = locationPathName;
                    break;
            }
            options.target = buildTarget;
            options.options = BuildOptions.None;
            BuildReport report = BuildPipeline.BuildPlayer(options);
            var summary = report.summary;
            switch (summary.result)
            {
                case BuildResult.Failed:
                    Debug.LogError("Build Failed");
                    break;
                case BuildResult.Succeeded:
                    Debug.Log("Build Succeeded");
                    Application.OpenURL(Directory.GetParent(summary.outputPath).FullName);
                    break;
            }
        }
    }
}
