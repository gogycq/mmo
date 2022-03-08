using System.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneManager : MonoSingleton<SceneManager>
{
    public UnityAction<float> onProgress = null;

    public UnityAction onSceneLoadDone = null;

    // Use this for initialization
    protected override void OnStart()
    {
        ResourceManager.InitScenePath();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void LoadScene(string name)
    {
        StartCoroutine(LoadLevel(name));
    }

    IEnumerator LoadLevel(string name)
    {
        Debug.LogFormat("LoadLevel: {0}", name);
        
        // load scene view
        string scenepath = ResourceManager.GetScenePath(name);
        #if UNITY_EDITOR
        
        AsyncOperation async = UnityEditor.SceneManagement.EditorSceneManager.LoadSceneAsyncInPlayMode(scenepath, 
                                new UnityEngine.SceneManagement.LoadSceneParameters(UnityEngine.SceneManagement.LoadSceneMode.Single));
        #else
        AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scenepath);
        #endif
        
        async.allowSceneActivation = true;
        async.completed += LevelLoadCompleted;
        while (!async.isDone)
        {
            if (onProgress != null)
                onProgress(async.progress);
            yield return null;
        }
    }

    private void LevelLoadCompleted(AsyncOperation obj)
    {
        if (onProgress != null)
            onProgress(1f);
        Debug.Log("LevelLoadCompleted:" + obj.progress);
        if (onSceneLoadDone != null)
            onSceneLoadDone();
    }
}
