using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoSingleton<GameInit>
{
    protected override void OnStart()
    {
        GameObject scene_manager = new GameObject();
        scene_manager.name = "scene_manager";
        scene_manager.AddComponent<SceneManager>();
        if (gameObject.GetComponent<LuaBehaviour>() == null) {
            Debug.Log("add lua behaviour, should only add once");
            gameObject.AddComponent<LuaBehaviour>();
        }

        //UnityEngine.SceneManagement.SceneManager.LoadScene("test");
        SceneManager.Instance.LoadScene("test");
    }
}
