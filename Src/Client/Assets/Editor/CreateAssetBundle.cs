using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine; 
using UnityEditor;

public class CreateAssetBundles : EditorWindow
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles() 
    {
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles",
                                        BuildAssetBundleOptions.ChunkBasedCompression,
                                        BuildTarget.Android);
    }

    private static Dictionary<string, string> dicArgs;
    [MenuItem("Build/Build AssetBundle Android")]
    public static void BuildAssetBundleAndroid()
    {
        BuildAssetBundles(BuildTarget.Android);
    }

    [MenuItem("Build/Build AssetBundle PC")]
    public static void BuildAssetBundlePC()
    {
        BuildAssetBundles(BuildTarget.StandaloneWindows64);
    }
    
    public static void BuildAssetBundles(BuildTarget buildTarget)
    {
        string p = Application.dataPath.Replace("Assets", "AssetBundles");

        if (Directory.Exists(p) == true) {
            Directory.Delete(p, true);
        }
        Directory.CreateDirectory(p);

        p = Application.dataPath + "/StreamingAssets/AssetBundle";
        if (Directory.Exists(p) == true) {
            Directory.Delete(p, true);
        }
        Directory.CreateDirectory(p);
        CreateBundle();

        BuildPipeline.BuildAssetBundles("AssetBundles", BuildAssetBundleOptions.ChunkBasedCompression, buildTarget);
        CreateAssetslist();

        Debug.Log("BuildAssetBundles End");
    }

    public static void CreateBundle()
    {
        ClearAll();
        string path = "Assets/AssetBundlesLocal";

        string[] allDir = Directory.GetDirectories(path);
        Debug.Log(" allDir.Length   " +  allDir.Length);
        for (int i = 0; i < allDir.Length; i++) {
            string[] allFiles = Directory.GetFiles(allDir[i]);
            string dirName = Path.GetFileName(allDir[i]);
            Debug.Log("dirName   " + dirName);
            
            for (int j = 0; j < allFiles.Length; j++) {
                Debug.Log("allFiles[j]  " +allFiles[j]);
                EditorUtility.DisplayProgressBar(dirName, allFiles[j], (1f+j)/allFiles.Length);
                if (Path.GetExtension(allFiles[j]) == ".meta") {
                    continue;   
                }

                AssetImporter importer = AssetImporter.GetAtPath(allFiles[j]);
                if(importer != null) {
                    importer.assetBundleName = dirName.ToLower();
                }
            }
 
        }
                 
        CreateLuaBundler();
        CreateSceneBundle();
        //CreateSoundBundle();
        //CreateFontBundle();
        //CreateCustomBundle();
        //CreateSpriteBundle();
        //CreatePrefabBundle();
		//CreateMoudleBundle();
		//CreateTextureBundle();
        EditorUtility.ClearProgressBar();

        AssetDatabase.Refresh();
    }

    private static void CreateSceneBundle() 
    {
        string path = "Assets/Scenes";
        string[] allScenes = Directory.GetFiles(path);
        AssetImporter importer = AssetImporter.GetAtPath(path);
        Debug.Log("importer " + importer);
        importer.assetBundleName = "scenes";

        path = "Assets/AssetBundlesLocal/SceneResources";
        string[] allDir = Directory.GetDirectories(path);
        for (int i = 0; i < allDir.Length; i++) {
            string[] allFiles = Directory.GetFiles(allDir[i]);
            string dirName = Path.GetFileName(allDir[i]); 
            
            for (int j = 0; j < allFiles.Length; j++) { 
                EditorUtility.DisplayProgressBar("Module "+dirName, allFiles[j], (1f+j)/allFiles.Length);
                if (Path.GetExtension(allFiles[j]) == ".meta") {
                    continue;
                }
                importer = AssetImporter.GetAtPath(allFiles[j]);
                if(importer != null) {
                    importer.assetBundleName = "scene_" + dirName.ToLower();
                }
            }
 
        } 
    }

    private static void CreateMoudleBundle()
    {
        string path = "Assets/AssetBundlesLocal/Module";
        string[] allDir = Directory.GetDirectories(path);
        for (int i = 0; i < allDir.Length; i++) {
            string[] allFiles = Directory.GetFiles(allDir[i]);
            string dirName = Path.GetFileName(allDir[i]); 
            
            for (int j = 0; j < allFiles.Length; j++) { 
                EditorUtility.DisplayProgressBar("Module "+dirName, allFiles[j], (1f+j)/allFiles.Length);
                if (Path.GetExtension(allFiles[j]) == ".meta") {
                    continue;
                }
                AssetImporter importer = AssetImporter.GetAtPath(allFiles[j]);
                if(importer != null) {
                    importer.assetBundleName = dirName.ToLower();
                }
            }
 
        } 
    }

    private static void CreatePrefabBundle() 
    {
        string path = "Assets/AssetBundlesLocal/Prefab";
        string[] allDir = Directory.GetDirectories(path);
        for (int i = 0; i < allDir.Length; i++) {
            string[] allFiles = Directory.GetFiles(allDir[i]);
            string dirName = Path.GetFileName(allDir[i]); 
            
            for (int j = 0; j < allFiles.Length; j++) { 
                EditorUtility.DisplayProgressBar("Prefab "+dirName, allFiles[j], (1f+j)/allFiles.Length);
                if (Path.GetExtension(allFiles[j]) == ".meta") {
                    continue;
                }
                AssetImporter importer = AssetImporter.GetAtPath(allFiles[j]);
                if(importer != null) {
                    importer.assetBundleName = dirName.ToLower();
                }
            }
 
        } 
    }
    
    private static void CreateSpriteBundle(){
        string path = "Assets/AssetBundlesLocal/Sprite";
        string[] allDir = Directory.GetDirectories(path);

        for (int i = 0; i < allDir.Length; i++) {
            string[] allFiles = Directory.GetFiles(allDir[i]);
            string dirName = Path.GetFileName(allDir[i]); 
            
            for (int j = 0; j < allFiles.Length; j++) { 
                EditorUtility.DisplayProgressBar("Sprite "+dirName, allFiles[j], (1f+j)/allFiles.Length);
                if (Path.GetExtension(allFiles[j]) == ".meta") {
                    continue;
                }
                AssetImporter importer = AssetImporter.GetAtPath(allFiles[j]);
                if(importer != null) {
                    importer.assetBundleName = dirName.ToLower();
                }
            }
 
        } 
    } 
    
    private static void CreateAssetslist() {
        string txtInfo = GetCurrentTimeUnix().ToString();
        string txtInfoAtStreaming = txtInfo;
        string path = Application.dataPath.Replace("Assets", "AssetBundles");
        string[] arrFiles = Directory.GetFiles(path);
        string[] bundleNameAtStreaming = new string[] {"AtlasPublic","AtlasItem" 
                                                        ,"MainUI","Login" };

        string type = "";

        if (dicArgs != null && dicArgs.ContainsKey("BundleType") == true) {
            type = dicArgs["BundleType"];
        }


        List<string> arrFilesLevel1 = new List<string>() { "atlaspublic","atlasitem" 
                                                            ,"mainui","login" };

        for (int i = 0; i < arrFiles.Length; i++) {
            EditorUtility.DisplayProgressBar("BuildAssetBundles", arrFiles[i], 1f * (1 + i) / arrFiles.Length);
            string fileName = Path.GetFileNameWithoutExtension(arrFiles[i]);
            string extension = Path.GetExtension(arrFiles[i]);
            int level = 0;

            if (extension == ".manifest") {
                File.Delete(arrFiles[i]);
                continue;
            }

            if(arrFilesLevel1.IndexOf(fileName) > -1) {
                level = 1;
            }

            FileStream file = new FileStream(arrFiles[i], FileMode.Open);
            string md5 = GetMD5HashFromFile(file);
            string info = "0," + fileName + "," + md5 + "," + file.Length + "," + level;
            file.Close();

            txtInfo += "\n" + info;

            if (type == "Normal" && Array.IndexOf(bundleNameAtStreaming, fileName) > -1 || type == "Full") {
                txtInfoAtStreaming += "\n" + info;
                File.Copy(arrFiles[i], Application.dataPath + "/StreamingAssets/AssetBundle/" + Path.GetFileName(arrFiles[i]));
            }

            File.Move(arrFiles[i], Path.Combine(path, fileName + "_" + md5 + extension));
        }

        EditorUtility.ClearProgressBar();

        File.WriteAllText(Application.dataPath.Replace("Assets", "AssetBundles") + "/assetslist.txt", txtInfo);
        File.WriteAllText(Application.dataPath + "/StreamingAssets/AssetBundle/assetslist.txt", txtInfoAtStreaming);
    }



    public static void ClearAll() {
        string[] allFiles = AssetDatabase.GetAllAssetPaths();
        Debug.Log(" allFiles.Length "+ allFiles.Length );
        for (int i = 0; i < allFiles.Length; i++) {

            EditorUtility.DisplayProgressBar("clear bundler", allFiles[i], (1f + i) / allFiles.Length);
            string fileExtension = Path.GetExtension(allFiles[i]);
            if (fileExtension == ".cs"|| fileExtension == ".js") {
                continue;
            }
            
            Debug.Log(" fileExtension   "+ fileExtension);
            AssetImporter importer = AssetImporter.GetAtPath(allFiles[i]);
            if(importer != null) {
                importer.assetBundleName = "";
            }
        }
        EditorUtility.ClearProgressBar();
    }


    public static void CreateLuaBundler()
    {
        string path = "Assets/Lua";
        string[] allFiles = Directory.GetFiles(path);
        EditorUtility.DisplayProgressBar("CreateLuaBundler", "wait", 0);
        AssetImporter importer = AssetImporter.GetAtPath(path);
        Debug.Log("importer " + importer);
        importer.assetBundleName = "lua";
    }
    
	public static void CreateTextureBundle()
	{
		string path = "Assets/AssetBundlesLocal/texture";
		string[] allFiles = Directory.GetFiles(path);
		for (int i = 0; i < allFiles.Length; i++) {
			string fileExtension = Path.GetExtension(allFiles[i]);
			if (fileExtension != ".png") {
                continue;
            }
			AssetImporter importer = AssetImporter.GetAtPath(allFiles[i]);
			importer.assetBundleName = "texture";
		}
	}

    public static void CreateSoundBundle() {
        string path = "Assets/AssetBundlesLocal/Sound";
        string[] allFiles = Directory.GetFiles(path);

        for (int i = 0; i < allFiles.Length; i++) {
            string fileExtension = Path.GetExtension(allFiles[i]);
            if (fileExtension != ".mp3") {
                continue;
            }
            AssetImporter importer = AssetImporter.GetAtPath(allFiles[i]);
            importer.assetBundleName = "sound";
        }
    }

    private static void CreateFontBundle()
    {
        string path = "";
        path = "Assets/AssetBundlesLocal/Font";
        string[] allFiles = Directory.GetFiles(path);
        for (int i = 0; i < allFiles.Length; i++) {
            string fileExtension = Path.GetExtension(allFiles[i]);
            if (fileExtension == ".meta") {
                continue;
            }
            AssetImporter importer = AssetImporter.GetAtPath(allFiles[i]);
            importer.assetBundleName = "font";
        }
    }

    private static void CreateCustomBundle() {
        string path = "Assets/AssetBundlesLocal/custom";
        string[] allFiles = Directory.GetFiles(path);
        for (int i = 0; i < allFiles.Length; i++) {
            string fileExtension = Path.GetExtension(allFiles[i]);
            if (fileExtension == ".meta") {
                continue;
            }
            AssetImporter importer = AssetImporter.GetAtPath(allFiles[i]);
            importer.assetBundleName = "custom";
        }
    }



    private static long GetCurrentTimeUnix() {
        TimeSpan cha = (DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)));
        long t = (long)cha.TotalSeconds;
        return t;
    }

    private static string GetMD5HashFromFile(FileStream file) {
        try {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++) {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        } catch (Exception ex) {
            throw new Exception("GetMD5HashFromFile() fail, error:" + ex.Message);
        }
    }
    
    public static void GetAllFiles(string path,List<string> listName)
    {
        string[] files = Directory.GetFiles(path);
        listName.AddRange(files);

        string[] dirs = Directory.GetDirectories(path);
        for (int i = 0; i < dirs.Length; i++) {
            GetAllFiles(dirs[i], listName);
        }
    }
}
