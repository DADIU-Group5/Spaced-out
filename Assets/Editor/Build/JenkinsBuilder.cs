using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class JenkinsBuilder : MonoBehaviour
{
    private const string BUILD_FOLDER = "Builds";

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

    [MenuItem("Build/Alpha")]
    private static void BuildAlpha()
    {
        Build("SpacedOut_Alpha");
    }

    [MenuItem("Build/Beta")]
    private static void BuildBeta()
    {
        Build("SpacedOut_Beta");
    }

    [MenuItem("Build/Main")]
    private static void BuildMain()
    {
        Build("SpacedOut_Main");
    }

    private static void Build(string name)
    {
        // load the scenes
        var scenes = FindEnabledEditorScenes();

        if (scenes == null || scenes.Length == 0)
        {
            throw new UnityException("No scenes to build.");
        }

        Directory.CreateDirectory(BUILD_FOLDER);

        UnityException exc = null;

        try
        {
            BuildPipeline.BuildPlayer(
                scenes,
                BUILD_FOLDER + "/" + name + ".apk",
                BuildTarget.Android, BuildOptions.None);
        }
        catch (UnityException e)
        {
            exc = e;
        }

        if (exc != null)
            throw exc;
    }
}