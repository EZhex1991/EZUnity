/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-04-02 20:41:29
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEditor;
using UnityEngine;
using System.IO;

#if UNITY_2018_1_OR_NEWER
using UnityEditor.Build.Reporting;
#endif

namespace EZhex1991.EZUnity.Builder
{
    [CreateAssetMenu(fileName = "EZPlayerBuilder", menuName = "EZUnity/EZPlayerBuilder", order = (int)EZAssetMenuOrder.EZPlayerBuilder)]
    public class EZPlayerBuilder : ScriptableObject
    {
        public static BuildTargetGroup GetGroup(BuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case BuildTarget.NoTarget: return BuildTargetGroup.Unknown;
                case BuildTarget.StandaloneOSX: return BuildTargetGroup.Standalone;
                case BuildTarget.StandaloneWindows: return BuildTargetGroup.Standalone;
                case BuildTarget.StandaloneWindows64: return BuildTargetGroup.Standalone;
                case BuildTarget.StandaloneLinux: return BuildTargetGroup.Standalone;
                case BuildTarget.StandaloneLinux64: return BuildTargetGroup.Standalone;
                case BuildTarget.StandaloneLinuxUniversal: return BuildTargetGroup.Standalone;
                case BuildTarget.iOS: return BuildTargetGroup.iOS;
                case BuildTarget.Android: return BuildTargetGroup.Android;
                case BuildTarget.WebGL: return BuildTargetGroup.WebGL;
                case BuildTarget.WSAPlayer: return BuildTargetGroup.WSA;
                case BuildTarget.PS4: return BuildTargetGroup.PS4;
                case BuildTarget.XboxOne: return BuildTargetGroup.XboxOne;
                case BuildTarget.tvOS: return BuildTargetGroup.tvOS;
                case BuildTarget.Switch: return BuildTargetGroup.Switch;
                default: return BuildTargetGroup.Unknown;
            }
        }
        public static string GetTargetName(BuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case BuildTarget.NoTarget: return "Unknow";
                case BuildTarget.StandaloneOSX: return "OSX";
                case BuildTarget.StandaloneWindows: return "Win";
                case BuildTarget.StandaloneWindows64: return "Win64";
                case BuildTarget.StandaloneLinux: return "Linux";
                case BuildTarget.StandaloneLinux64: return "Linux64";
                case BuildTarget.StandaloneLinuxUniversal: return "LinuxU";
                case BuildTarget.iOS: return "iOS";
                case BuildTarget.Android: return "Android";
                case BuildTarget.WebGL: return "WebGL";
                case BuildTarget.WSAPlayer: return "WSA";
                case BuildTarget.PS4: return "PS4";
                case BuildTarget.XboxOne: return "Xbox";
                case BuildTarget.tvOS: return "tvOS";
                case BuildTarget.Switch: return "Switch";
                default: return "Unknown";
            }
        }

        public const string Wildcard_Date = "<Date>";
        public const string Wildcard_Time = "<Time>";
        public const string Wildcard_CompanyName = "<CompanyName>";
        public const string Wildcard_ProductName = "<ProductName>";
        public const string Wildcard_BundleIdentifier = "<BundleIdentifier>";
        public const string Wildcard_BundleVersion = "<BundleVersion>";
        public const string Wildcard_BuildNumber = "<BuildNumber>";
        public const string Wildcard_BuildTarget = "<BuildTarget>";

        public BuildTarget buildTarget = BuildTarget.NoTarget;
        public BuildOptions buildOptions = BuildOptions.ShowBuiltPlayer;

        public EZBundleBuilder bundleBuilder;

        public string locationPathName = "Builds/<ProductName>-<BuildTarget>-<BundleVersion>";
        public string exeFileName = "<ProductName>-<BundleVersion>";
        public SceneAsset[] scenes;

        public string companyName;
        public string productName;
        public string bundleIdentifier;
        public string bundleVersion;
        public int buildNumber;
        public bool buildNumberIncrement = true;
        public Texture2D icon;

        public EZCopyList copyList;

        public bool CheckTarget(BuildTargetGroup buildGroup)
        {
            BuildTargetGroup selectedGroup = GetGroup(EditorUserBuildSettings.activeBuildTarget);
            if (selectedGroup != buildGroup)
            {
                string message = string.Format("Selected BuildTargetGroup is {0}, Use {1} anyway?", selectedGroup, buildTarget);
                if (!EditorUtility.DisplayDialog("BuildTarget Not Match with selected BuildTargetGroup", message, "Yes", "Cancel"))
                {
                    Debug.Log("Build Canceled");
                    return false;
                }
            }
            return true;
        }
        public void ConfigTargetGroup(BuildTargetGroup buildTargetGroup)
        {
            if (!string.IsNullOrEmpty(companyName))
            {
                PlayerSettings.companyName = companyName;
            }
            if (!string.IsNullOrEmpty(productName))
            {
                PlayerSettings.productName = productName;
            }
            if (!string.IsNullOrEmpty(bundleVersion))
            {
                PlayerSettings.bundleVersion = bundleVersion;
            }

            if (icon != null)
            {
                Texture2D[] icons = PlayerSettings.GetIconsForTargetGroup(buildTargetGroup, IconKind.Any);
                for (int i = 0; i < icons.Length; i++)
                {
                    icons[i] = icon;
                }
                PlayerSettings.SetIconsForTargetGroup(buildTargetGroup, icons, IconKind.Any);
            }

            if (!string.IsNullOrEmpty(bundleIdentifier))
            {
                PlayerSettings.SetApplicationIdentifier(buildTargetGroup, bundleIdentifier);
            }

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

        public void ConfigPlayerSettings(BuildTarget buildTarget)
        {
            BuildTargetGroup buildGroup = GetGroup(buildTarget);
            if (!CheckTarget(buildGroup)) return;

            ConfigTargetGroup(buildGroup);
        }

        public BuildPlayerOptions GetBuildOptions(string path)
        {
            BuildPlayerOptions options = new BuildPlayerOptions();
            string[] scenePaths = new string[scenes.Length];
            for (int i = 0; i < scenePaths.Length; i++)
            {
                scenePaths[i] = AssetDatabase.GetAssetPath(scenes[i]);
            }
            options.scenes = scenePaths;
            switch (buildTarget)
            {
                case BuildTarget.StandaloneWindows:
                    options.locationPathName = string.Format("{0}/{1}.exe", path, HandleWildcards(exeFileName, buildTarget));
                    break;
                case BuildTarget.StandaloneWindows64:
                    options.locationPathName = string.Format("{0}/{1}.exe", path, HandleWildcards(exeFileName, buildTarget));
                    break;
                case BuildTarget.Android:
                    options.locationPathName = string.Format("{0}.apk", path);
                    break;
                default:
                    options.locationPathName = path;
                    break;
            }
            options.target = buildTarget;
            options.options = buildOptions;
            return options;
        }
        public void BuildPlayer(BuildTarget buildTarget)
        {
            BuildTargetGroup buildGroup = GetGroup(buildTarget);
            if (!CheckTarget(buildGroup)) return;

            if (string.IsNullOrEmpty(locationPathName))
            {
                locationPathName = EditorUtility.SaveFolderPanel("Choose Output Folder", "", "");
                if (string.IsNullOrEmpty(locationPathName)) return;
            }
            string path = HandleWildcards(locationPathName, buildTarget);

            string projectSettingsPath = Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length) + "/ProjectSettings/ProjectSettings.asset";
            string oldSettings = File.ReadAllText(projectSettingsPath);

            ConfigTargetGroup(buildGroup);
            if (bundleBuilder != null)
            {
                bundleBuilder.Execute(buildTarget);
            }

#if UNITY_2018_1_OR_NEWER
            BuildReport report = BuildPipeline.BuildPlayer(GetBuildOptions(path));
            var summary = report.summary;
            switch (summary.result)
            {
                case BuildResult.Failed:
                    Debug.LogError("Build Failed");
                    break;
                case BuildResult.Succeeded:
                    Debug.Log("Build Succeeded");
                    copyList.CopyFiles(path);
                    break;
            }
#else
            Debug.Log(BuildPipeline.BuildPlayer(options));
            copyList.CopyFiles(path);
#endif
            File.WriteAllText(projectSettingsPath, oldSettings);

            if (buildNumberIncrement)
            {
                buildNumber++;
                EditorUtility.SetDirty(this);
            }
        }

        public string HandleWildcards(string text, BuildTarget buildTarget)
        {
            return text
                .Replace(Wildcard_BuildTarget, GetTargetName(buildTarget))
                .Replace(Wildcard_BuildNumber, buildNumber.ToString())
                .Replace(Wildcard_BundleIdentifier, bundleIdentifier)
                .Replace(Wildcard_BundleVersion, bundleVersion)
                .Replace(Wildcard_CompanyName, companyName)
                .Replace(Wildcard_Date, DateTime.Now.ToString("yyyyMMdd"))
                .Replace(Wildcard_ProductName, productName)
                .Replace(Wildcard_Time, DateTime.Now.ToString("HHmmss"));
        }
    }
}
