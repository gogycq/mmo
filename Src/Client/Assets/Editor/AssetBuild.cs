using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetBuild
{
    [MenuItem("AB/Build")]
    public static void BuildAB() {
        string abOutPath = string.Empty;
        abOutPath = Application.streamingAssetsPath;

        if (!Directory.Exists(abOutPath)) {

            Directory.CreateDirectory(abOutPath);
        }

        BuildPipeline.BuildAssetBundles(abOutPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }
}
