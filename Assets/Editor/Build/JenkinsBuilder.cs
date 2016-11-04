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

    // Bump version number in PlayerSettings.bundleVersion
    private static int IncreaseBuildVersion()
    {
        return ++PlayerSettings.Android.bundleVersionCode;
    }

    [MenuItem("Build/Build Game")]
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
            string version = string.Format("v{0}.{1}", PlayerSettings.bundleVersion, IncreaseBuildVersion().ToString("D2"));
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
}