using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System;

public class JenkinsBuilder : MonoBehaviour
{
    private const string versionFile = "playStoreVersion.txt";
    private const string buildFolder = "Builds";
    private const string appName = "SpacedOut";

    // returns the absolute path from project folder
    private static string AbsolutePath(string relativePath)
    {
        var fullPath = Application.dataPath.Replace("Assets", "") + relativePath;
        return fullPath.Replace("/", "\\");
    }

    // open build folder in windows
    [MenuItem("Build/Open Build Folder")]
    private static void OpenBuildFolder()
    {
        Process.Start("explorer.exe", "/open," + AbsolutePath(buildFolder));
    }

    // this will be called by jenkins
    private static void JenkinsBuild()
    {
        // first set version to last play store version 
        int version = GetLastGooglePlayStoreVersion();
        PlayerSettings.Android.bundleVersionCode = version;
        Build();

        // build successful, update version file
        UpdateGooglePlayStoreVersion();
    }

    // called from unity menu
    [MenuItem("Build/Build Game Locally")]
    private static void LocalBuild()
    {
        string apkPath = AbsolutePath(Build());
        Process.Start("explorer.exe", "/select," + apkPath);
    }

    // update the google play store version file
    private static void UpdateGooglePlayStoreVersion()
    {
        try
        {
            string filePath = AbsolutePath("/" + versionFile);
            File.WriteAllText(filePath, PlayerSettings.Android.bundleVersionCode.ToString());
        }
        catch
        {
            throw new UnityException("Failed to update google play store version file");
        }
    }

    // read latest google play store version from file
    private static int GetLastGooglePlayStoreVersion()
    {
        try
        {
            string filePath = AbsolutePath("/" + versionFile);
            return int.Parse(File.ReadAllText(filePath));
        }
        catch
        {
            throw new UnityException("Failed to read google play store version");
        }
    }

    // Builds the game and return path to apk
    private static string Build()
    {
        // load the scenes
        var scenes = FindEnabledEditorScenes();
        if (scenes == null || scenes.Length == 0)
        {
            throw new UnityException("No scenes to build.");
        }

        // set build settings
        string version = IncrementVersion();
        SignBuild();

        // sets the path for apk
        Directory.CreateDirectory(buildFolder);
        string path = string.Format("{0}/{1}_v{2}.apk", buildFolder, appName, version);

        // build apk
        BuildPipeline.BuildPlayer(scenes, path, BuildTarget.Android, BuildOptions.None);
        return path;
    }

    // gets an array of scenes in build options
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

    // increments the version and returns the version string
    private static string IncrementVersion()
    {
        int version = ++PlayerSettings.Android.bundleVersionCode;
        string versionString = string.Format("{0}.{1}.{2}", version / 10000, (version % 10000) / 100, (version % 100));
        PlayerSettings.bundleVersion = versionString;
        return versionString;
    }

    // signs the build for google play store
    private static void SignBuild()
    {
        PlayerSettings.Android.keystoreName = "C:/Users/student/Documents/Spaced-out/user.keystore";
        PlayerSettings.Android.keystorePass = "dadiutest";
        PlayerSettings.Android.keyaliasName = "dadiutest";
        PlayerSettings.Android.keyaliasPass = "dadiutest";
    }
}