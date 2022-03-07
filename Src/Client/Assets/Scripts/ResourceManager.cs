using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ResourceAttribute {
    public string name;
    public string path;
    public string abname;
    public ResourceType rtype;
    public string nextPages;
}

public enum ResourceType {
    Sprite = 1,
    // 可重复生成的预制体
    Prefab = 2,
    // 功能模块
    Module = 3,
    // 不属于 2和3的游戏体
    GameObject = 4
}

public class ResourceManager {
    public enum UIType {
        ROOT = 1,
        MAIN = 2,
        SECOND = 3,
        POP = 4
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    static string pathRoot = LoadTools.assetBundlePath ;       // Application.dataPath + "!assets" ;
    static string assetBundlePath= "";
#elif UNITY_IOS && !UNITY_EDITOR  
	static string pathRoot = LoadTools.assetBundlePath ;       // Application.dataPath + "!assets" ;
	static string assetBundlePath= ""; 
#else
    static string assetBundlePath = "/AssetBundles";
    static string pathRoot = Application.dataPath ;
#endif // end for deal path


    public static Dictionary<string, Sprite> DicSprite = new Dictionary<string, Sprite>();
    public static Dictionary<string, ResourceAttribute> DicRecource = new Dictionary<string, ResourceAttribute>();
	public static Dictionary<string, Sprite> allSprite = new Dictionary<string, Sprite>(); 
    public static Dictionary<string, GameObject> allPrefab = new Dictionary<string, GameObject>();
    public static Dictionary<string, GameObject> allGameObject = new Dictionary<string, GameObject>();
    public static Dictionary<string, GameObject> allModule = new Dictionary<string, GameObject>();
    public static Dictionary<string, AssetBundle> allAssetBundle = new Dictionary<string, AssetBundle>();
    public static Dictionary<string, byte[]> allLuaByte = new Dictionary<string,  byte[]>();
    public static Dictionary<string, AudioClip> allAudioClip = new Dictionary<string,  AudioClip>();
	public static Dictionary<string, Font> allFont = new Dictionary<string, Font>();
	public static Dictionary<string, Texture> allTexture = new Dictionary<string, Texture>(); 

    static string Type = "one";

    public static string luaFileName ; // 更新下来的 lua ab文件的名字，每次更新后都不一样，后拼着md5等
    public static void InitFontResource(string name) {
    	AssetBundle ab = Load(name);
    }

    public static void InitSpriteResource(string name) {
        // todo check need
    }

    public static string[] GetNextPages(string name) {
        string temp = DicRecource[name].nextPages;
        return temp.Split('*');
    }

	static Texture GetTexture(string key, string path) {
		path = path.ToLower();
		if (allTexture.ContainsKey(key)) {

			return allTexture[key];
		} else {   
			if(allAssetBundle.ContainsKey("Texture")) {

                SetAssetOne("Texture", key, "Texture", allAssetBundle["Texture"]);
			} else {
				AssetBundle ab = Load("Texture"); 
				allAssetBundle["Texture"] = ab;
				SetAssetOne("Texture", key, "Texture", ab);
			}
			return allTexture[key];
		}
	}
	
    public static Texture GetTexturePath(string key, string path) { 
		if (!GameConfig.UseAssetsBundle) {

			#if UNITY_EDITOR             
			return AssetDatabase.LoadAssetAtPath<Texture>("Assets/AssetBundlesLocal/texture/"  + key + ".png");
			#else 
			return GetTexture(key, path);
			#endif
		} else {
			return GetTexture(key, path);
		}
	}
    static Sprite GetSprite(string key, string path) {  
        path = path.ToLower();
        if (allSprite.ContainsKey(path+"_"+key)) {
            
            return allSprite[path+"_"+key];
        } else {
            if(allAssetBundle.ContainsKey(path)) {

                SetAssetOne("Sprite", key, path, allAssetBundle[path]);
            } else {
                AssetBundle ab = Load(path);
                allAssetBundle[path] = ab;
                SetAssetOne("Sprite", key, path, ab);
            }
            return allSprite[path+"_"+key];
        }
    }
    
    public static AudioClip LoadAudioClip(string path, string name) {
        #if UNITY_EDITOR
        return AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/AssetBundlesLocal/" + path + "/" + name + ".mp3");
        #else
        return GetAudioClip(name, path);
        #endif
    }
    
    public static AudioClip LoadAudioClipWAV(string path, string name) {
        #if UNITY_EDITOR
        return AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/AssetBundlesLocal/" + path + "/" + name + ".wav");
        #else
        return GetAudioClip(name, path);
        #endif
    }

    static AudioClip GetAudioClip(string key ,string path) {  
        path = path.ToLower();
        if (allAudioClip.ContainsKey(path+"_"+key)) {
            
            return allAudioClip[path+"_"+key];
        } else {
            if(allAssetBundle.ContainsKey(path)) {

                SetAssetOne("AudioClip", key, path, allAssetBundle[path]);
            } else {
                AssetBundle ab = Load(path);
                allAssetBundle[path] = ab;
                SetAssetOne("AudioClip", key, path, ab);
            }
            return allAudioClip[path+"_"+key];
        }
    }
    
    public static Sprite GetSpritePath(string key, string path) {
        if (!GameConfig.UseAssetsBundle) {

            #if UNITY_EDITOR 
            return AssetDatabase.LoadAssetAtPath<Sprite>("Assets/AssetBundlesLocal/Sprite/" + path + "/" + key + ".png");
            #else 
            return GetSprite(key,path);
            #endif
        } else {
            return GetSprite(key,path);
        }
    } 
    public static void SetAssetOne(string type, string key, string path, AssetBundle ab) {
        Debug.Log("SetAssetOne key === " + key);
        Debug.Log("SetAssetOne type === " + type);
        Debug.Log("SetAssetOne path === " + path);

        if (type == "GameObject") {

            allGameObject[path+"_"+key] = ab.LoadAsset<GameObject>(key);
        } else if (type == "Sprite") {  
            allSprite[path+"_"+key] = ab.LoadAsset<Sprite>(key);
        } else if (type == "Prefab") {
            allPrefab[path+"_"+key] = ab.LoadAsset<GameObject>(key);
        } else if (type == "Module") {
            allModule[path+"_"+key] = ab.LoadAsset<GameObject>(key);
        } else if (type == "AudioClip") {
            allAudioClip[path+"_"+key] = ab.LoadAsset<AudioClip>(key);
		} else if (type == "Texture") {
			allTexture[key] = ab.LoadAsset<Texture>(key);
		}
    }
    
    static GameObject GetPrefab(string key ,string path) {  
        path = path.ToLower();
        Debug.LogError(path + "   GetPrefab    key  " + key);
       
        if (allPrefab.ContainsKey(path+"_" + key)) {

            return allPrefab[path+"_"+key];
        } else {      
            if(allAssetBundle.ContainsKey(path)) {
                SetAssetOne("Prefab",key,path, allAssetBundle[path]);
            } else {
                AssetBundle ab = Load(path);
                allAssetBundle[path] = ab;
                SetAssetOne("Prefab",key,path, ab);
            }    
            return allPrefab[path+"_"+key];
        }
    }

    static GameObject GetGameObject(string key, string path) {
        path = path.ToLower();
        Debug.LogError(path + "   GetGameObject    key  " +key);
       
        if (allGameObject.ContainsKey(path+"_"+key)) {
            
            return allGameObject[path+"_"+key];
        } else {
            if (allAssetBundle.ContainsKey(path)) {
                SetAssetOne("GameObject",key,path, allAssetBundle[path]);
            } else {
                AssetBundle ab = Load(path);
                allAssetBundle[path] = ab;
                SetAssetOne("GameObject",key,path, ab);
            }
            return allGameObject[path+"_"+key];
        }
    }
    
    static GameObject GetModule(string key, string path) {
        Debug.Log( "   GetModule  path = "+path +"  key = " +key);
        path = path.ToLower();
        if (allModule.ContainsKey(path+"_"+key)) {

            Debug.Log(" have Module " + path+"_"+key); 
            return allModule[path+"_"+key];
        } else {
            if (allAssetBundle.ContainsKey(path)) {
                
                Debug.Log("ContainsKey GetModule  " + path);
                SetAssetOne("Module",key,path, allAssetBundle[path]);
            } else {
                Debug.Log("!ContainsKey GetModule  " + path);
                AssetBundle ab = Load(path);
                allAssetBundle[path] = ab;
                SetAssetOne("Module",key,path, ab);
            }
            return allModule[path+"_"+key];
        }
    }

    public static GameObject GetPrefabPath(string key, string path) { 
        if (!GameConfig.UseAssetsBundle) {

            #if UNITY_EDITOR 
            return AssetDatabase.LoadAssetAtPath<GameObject>("Assets/AssetBundlesLocal/Prefab/" + path + "/" + key + ".prefab");
            #else 
            return GetPrefab(key,path);
            #endif
        } else {
            return GetPrefab(key,path);
        }
    }

    public static GameObject GetModulePath(string key, string path) {
        if (!GameConfig.UseAssetsBundle) {

            #if UNITY_EDITOR 
            return AssetDatabase.LoadAssetAtPath<GameObject>("Assets/AssetBundlesLocal/Module/" + path + "/" + key + ".prefab");
            #else
            return GetModule(key,path);
            #endif
        } else {
            return GetModule(key,path);
        }
    } 
    
    public static GameObject GetGameObjectPath(string key, string path) {
        if (!GameConfig.UseAssetsBundle) {

            #if UNITY_EDITOR 
            return AssetDatabase.LoadAssetAtPath<GameObject>("Assets/AssetBundlesLocal/Prefab/" + path + "/" + key + ".prefab");
            #else  
            return GetGameObject(key,path);
            #endif
        } else {
            return GetGameObject(key,path);
        }
    } 

    public static Sprite LoadSprite(string Path) {
        if (Resources.Load<GameObject>("Sprites/" + Path) != null) {

            return Resources.Load<GameObject>("Sprites/" + Path).GetComponent<Image>().sprite;
        } else {
            return null;
        }
    }

    static public AssetBundle Load(string name) {
        Debug.Log("Load ****  " + pathRoot + assetBundlePath + "/" + name);
        return AssetBundle.LoadFromFile(pathRoot + assetBundlePath + "/" +name);
    }
    
    public static void loadLuaToByte(string name) {
        Debug.Log(" loadLuaToByte name  "+ name);
        AssetBundle ab = Load(name);  
        string[] abs = ab.GetAllAssetNames();
        Debug.Log(" loadLuaToByte name2  "+ name);
        for (int i = 0; i < abs.Length; i++) {
            Debug.Log(" abs[i]  "+ abs[i]);
            string  temp = abs[i].Substring(11 );
            temp = temp.Substring(0 , temp.Length-8);
            
            if (allLuaByte.ContainsKey(temp)) {

                return;
            }
            
            allLuaByte.Add(temp, System.Text.UTF8Encoding.Default.GetBytes(ab.LoadAsset<TextAsset>(abs[i]).text));
            if(abs[i].IndexOf("login") != -1) {
                
                Debug.Log("**********loadLuaToByte  ** name  ************  "+abs[i]);
            }     
        }
    }
}
