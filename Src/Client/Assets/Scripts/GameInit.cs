using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoSingleton<GameInit>
{
    protected override void OnStart()
    {
        // todo 检查场景中是否存在各种组件，不存在则创建
        if (gameObject.GetComponent<LuaBehaviour>() == null) {
            Debug.Log("add lua behaviour, should only add once");
            gameObject.AddComponent<LuaBehaviour>();
        }
    }
}
