using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load_first : MonoBehaviour
{
    static Dictionary<string, bool> doneMap = new Dictionary<string, bool>();
    public string test_name;
    // Start is called before the first frame update
    void Start()
    {
        //LoadScene(test_name);
        Debug.Log(Application.dataPath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string name) {
        if (doneMap.ContainsKey(name)) return; 
        doneMap[name] = true;
        StartCoroutine(Do_sceneLoad(name));
    }

    IEnumerator Do_sceneLoad(string scene_name) {
        yield return new WaitForSeconds(5);
        AsyncOperation loadOper = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene_name);
        loadOper.allowSceneActivation = true;
        loadOper.completed += OnSceneLoadComplete;
        while(!loadOper.isDone) {
            Debug.Log("progress: " + loadOper.progress);
            yield return null;
        }
    }

    public void OnSceneLoadComplete(AsyncOperation loadOper) {
        Debug.Log("load complete");
    }
}
