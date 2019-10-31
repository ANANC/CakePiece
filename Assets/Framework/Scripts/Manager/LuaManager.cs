using LuaInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LuaManager : BaseBehaviour
{
    protected LuaState luaState = null;
    protected LuaLooper loop = null;
    protected LuaFunction levelLoaded = null;

    protected bool openLuaSocket = false;
    protected bool beZbStart = false;

    public override void Init()
    {
        luaState = new LuaState();
        OpenLibs();
        luaState.LuaSetTop(0);
        Bind();
        LoadLuaFiles();
    }

    public override void Start()
    {
        
    }

    public override void Update()
    {
        
    }

    //*----lua----*

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int LuaOpen_Socket_Core(IntPtr L)
    {
        return LuaDLL.luaopen_socket_core(L);
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int LuaOpen_Mime_Core(IntPtr L)
    {
        return LuaDLL.luaopen_mime_core(L);
    }

    protected virtual void OpenLibs()
    {
        luaState.OpenLibs(LuaDLL.luaopen_pb);
        luaState.OpenLibs(LuaDLL.luaopen_struct);
        luaState.OpenLibs(LuaDLL.luaopen_lpeg);
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        luaState.OpenLibs(LuaDLL.luaopen_bit);
#endif

        if (LuaConst.openLuaSocket)
        {
            OpenLuaSocket();
        }

        if (LuaConst.openLuaDebugger)
        {
            OpenZbsDebugger();
        }
    }

    protected virtual void Bind()
    {
        LuaBinder.Bind(luaState);
        DelegateFactory.Init();
        LuaCoroutine.Register(luaState, Framework.Core.Mgr.CoroutineManager);
    }

    protected void OpenLuaSocket()
    {
        LuaConst.openLuaSocket = true;

        luaState.BeginPreLoad();
        luaState.RegFunction("socket.core", LuaOpen_Socket_Core);
        luaState.RegFunction("mime.core", LuaOpen_Mime_Core);
        luaState.EndPreLoad();
    }

    public void OpenZbsDebugger(string ip = "localhost")
    {
        if (!Directory.Exists(LuaConst.zbsDir))
        {
            Debugger.LogWarning("ZeroBraneStudio not install or LuaConst.zbsDir not right");
            return;
        }

        if (!LuaConst.openLuaSocket)
        {
            OpenLuaSocket();
        }

        if (!string.IsNullOrEmpty(LuaConst.zbsDir))
        {
            luaState.AddSearchPath(LuaConst.zbsDir);
        }

        luaState.LuaDoString(string.Format("DebugServerIp = '{0}'", ip), "@LuaClient.cs");
    }
    protected virtual void LoadLuaFiles()
    {
        OnLoadFinished();
    }

    protected virtual void OnLoadFinished()
    {
        luaState.Start();
        StartLooper();
        StartMain();
    }

    protected void StartLooper()
    {
        loop = gameObject.AddComponent<LuaLooper>();
        loop.luaState = luaState;
    }

    //*----lua----*
}
