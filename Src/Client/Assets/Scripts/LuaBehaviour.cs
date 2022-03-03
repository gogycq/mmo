using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System;
using System.IO;

public class LuaBehaviour : MonoBehaviour
{
    public static LuaTable ScriptEnv;
    public string MainScriptText;

    internal static LuaEnv luaEnv = new LuaEnv();
    internal static float lastGCTime = 0;
    internal const float gcInterval = 1;
    private string luaFilePath;

    private static Action luaStart;
    private static Action luaUpdate;
    private static Action luaOnDestroy;
    
    
    void Awake()
    {
        luaFilePath = Application.dataPath + "/" + "Lua/" ;    
        LuaEnv.CustomLoader method = CustomLoaderMethod;
        luaEnv = new LuaEnv();
        luaEnv.AddLoader(method);

        ScriptEnv = luaEnv.NewTable();
        // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
		LuaTable meta = luaEnv.NewTable();
        meta.Set("__index", luaEnv.Global);
        ScriptEnv.SetMetaTable(meta);
        meta.Dispose();
        ScriptEnv.Set("self", this);
        LoadLua();
    }

    void Update()
    {
        if (luaUpdate != null) {
            luaUpdate();
        }

        if (Time.time - LuaBehaviour.lastGCTime > gcInterval) {
            luaEnv.Tick();
            LuaBehaviour.lastGCTime = Time.time;
        }
    }

    
    void OnDestroy()
    {
        if (luaOnDestroy != null) {
            luaOnDestroy();
        }
        luaOnDestroy = null;
        luaUpdate = null;
        luaStart = null;
        ScriptEnv.Dispose();
    }

    void LoadLua() {
        TextAsset mainText = Resources.Load<TextAsset>("main.lua");
        MainScriptText = mainText.text;
        InitLuaActions();
    }

    void InitLuaActions() {
        luaEnv.DoString(MainScriptText, "LuaBehaviour", null);
        Action luaAwake = ScriptEnv.Get<Action>("Awake");
        ScriptEnv.Get("Start", out luaStart);
        ScriptEnv.Get("Update", out luaUpdate);
        ScriptEnv.Get("OnDestroy", out luaOnDestroy);

        if (luaAwake != null) luaAwake();
        DoStart();
    }

    void DoStart() {
        if (luaStart != null) {
            luaStart();
        }
    }

    private byte[] CustomLoaderMethod(ref string fileName) {
        fileName = luaFilePath + fileName.Replace('.', '/') + ".lua";
        if (File.Exists(fileName)) {

			return File.ReadAllBytes(fileName.ToLower());
        } else {
            return null;
        }
    }
}
