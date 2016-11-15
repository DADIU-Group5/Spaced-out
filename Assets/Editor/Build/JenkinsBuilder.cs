using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class JenkinsBuilder : MonoBehaviour
{
    private const string buildFolder = "Builds";
    private const string appName = "SpacedOut";

    private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }
        return EditorScenes.ToArray();
    }

    [MenuItem("Build Server/Open Build Folder")]
    private static void OpenBuildFolder()
    {
        OpenExplorer(@"C:\Users\student\Documents\Spaced-out\Builds\");
    }

    private static void OpenExplorer(string path)
    {
        System.Diagnostics.Process.Start("explorer.exe", "/open," + path);
    }

    private static string IncreaseBuildVersion()
    {
        int version = ++PlayerSettings.Android.bundleVersionCode;
        return version.ToString("D2");
    }

    private static void SetBuildSettings()
    {
        PlayerSettings.allowedAutorotateToPortrait = false;
        PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
    }

    [MenuItem("Build Server/Build Game")]
    private static void Build()
    {
        // load the scenes
        var scenes = FindEnabledEditorScenes();

        if (scenes == null || scenes.Length == 0)
        {
            throw new UnityException("No scenes to build.");
        }
        
        Directory.CreateDirectory(buildFolder);
        UnityException exc = null;
        try
        {
            SignBuild();
            SetBuildSettings();
            string version = string.Format("v{0}.{1}", PlayerSettings.bundleVersion, IncreaseBuildVersion());
            string path = string.Format("{0}/{1}_{2}.apk", buildFolder, appName, version);
            BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
        }
        catch (UnityException e)
        {
            exc = e;
        }

        if (exc != null)
            throw exc;
    }

    private static void SignBuild()
    {
        PlayerSettings.Android.keystoreName = "C:/Users/student/Documents/Spaced-out/user.keystore";
        PlayerSettings.Android.keystorePass = "dadiutest";
        PlayerSettings.Android.keyaliasName = "dadiutest";
        PlayerSettings.Android.keyaliasPass = "dadiutest";
    }
}