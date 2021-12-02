using GameLogic;
using LuaInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ANFramework
{
    public class LuaManager : BaseManager
    {
        protected LuaState m_LuaState = null;
        protected LuaLooper m_LuaLoop = null;

        /// <summary>
        /// 用于加载Bundle类型的Lua
        /// </summary>
        private LuaLoader m_LuaLoader = null;


        public override void Init()
        {
            m_LuaLoader = new LuaLoader();
            m_LuaState = new LuaState();
            OpenLibs();
            m_LuaState.LuaSetTop(0);
            Bind();
        }

        public override void Start()
        {
            InitLuaPath();
            InitLuaBundle();
            m_LuaState.Start();
            StartLooper();
            DoFile("EngineGame.lua");
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
            m_LuaState.OpenLibs(LuaDLL.luaopen_pb);
            m_LuaState.OpenLibs(LuaDLL.luaopen_struct);
            m_LuaState.OpenLibs(LuaDLL.luaopen_lpeg);
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
            LuaBinder.Bind(m_LuaState);
            DelegateFactory.Init();
            LuaCoroutine.Register(m_LuaState, ANF.Core.Mgr.Coroutine);
        }

        protected void OpenLuaSocket()
        {
            LuaConst.openLuaSocket = true;

            m_LuaState.BeginPreLoad();
            m_LuaState.RegFunction("socket.core", LuaOpen_Socket_Core);
            m_LuaState.RegFunction("mime.core", LuaOpen_Mime_Core);
            m_LuaState.EndPreLoad();
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
                m_LuaState.AddSearchPath(LuaConst.zbsDir);
            }

            m_LuaState.LuaDoString(string.Format("DebugServerIp = '{0}'", ip), "@LuaClient.cs");
        }

        private void StartLooper()
        {
            m_LuaLoop = ANF.RootGameObject.AddComponent<LuaLooper>();
            m_LuaLoop.luaState = m_LuaState;
        }

        //*----lua----*

        /// <summary>
        /// 初始化Lua代码加载路径
        /// </summary>
        private void InitLuaPath()
        {
            if (Application.isEditor)
            {
                if (FrameworkDefine.LUA_BUNDLE_MODE)
                {
                    Debug.Log("LuaSearchPath 1 : " + FrameworkUtil.DataPath + FrameworkDefine.LUA_DIR);
                    m_LuaState.AddSearchPath(FrameworkUtil.DataPath + FrameworkDefine.LUA_DIR);
                }
                else
                {
                    Debug.Log("LuaSearchPath 2 : " + Application.dataPath + "/" +
                              FrameworkDefine.LUAPATHCONFIG_DEFAULT);
                    m_LuaState.AddSearchPath(Application.dataPath + "/" + FrameworkDefine.LUAPATHCONFIG_DEFAULT);
                    Debug.Log("LuaSearchPath 2 : " + Application.dataPath + "/" + FrameworkDefine.TOLUA_PATH);
                    m_LuaState.AddSearchPath(Application.dataPath + "/" + FrameworkDefine.TOLUA_PATH);
                    if (!FrameworkUtil.LuaPath().Equals(FrameworkDefine.LUAPATHCONFIG_DEFAULT))
                    {
                        Debug.Log("LuaSearchPath 2 : " + Application.dataPath + "/" + FrameworkUtil.LuaPath());
                        m_LuaState.AddSearchPath(Application.dataPath + "/" + FrameworkUtil.LuaPath());
                    }

                    m_LuaState.AddSearchPath(Application.dataPath + "/" + FrameworkDefine.LUAPATHCONFIG_DEFAULT +
                                             "/Data");
                }
            }
            else
            {
                Debug.Log("LuaSearchPath 3 : " + FrameworkUtil.DataPath + FrameworkDefine.LUA_DIR);
                m_LuaState.AddSearchPath(FrameworkUtil.DataPath + FrameworkDefine.LUA_DIR);
            }
        }

        /// <summary>
        /// 初始化LuaBundle
        /// </summary>
        private void InitLuaBundle()
        {
            if (m_LuaLoader.beZip)
            {
                List<string> filesList = new List<string>();
                string[] names = Directory.GetFiles(FrameworkUtil.DataPath + FrameworkDefine.LUA_DIR);
                string addbundelname = "";
                foreach (string filename in names)
                {
                    string name = Path.GetFileName(filename.Replace('\\', '/'));
                    if (name.EndsWith(FrameworkDefine.EXT_NAME))
                    {
                        addbundelname = FrameworkDefine.LUA_DIR + "/" + name;
                        filesList.Add(addbundelname);
                        m_LuaLoader.AddBundle(addbundelname);
                    }
                }
            }
        }

        /// <summary>
        /// 加载lua文件
        /// </summary>
        public void DoFile(string filename)
        {
            m_LuaState.DoFile(filename);
        }

        /* -------------------------------------------- */
        /* 由C#调用Lua的几种方法 */

        public object[] CallFunction(string functionName, params object[] args)
        {
            LuaFunction func = m_LuaState.GetFunction(functionName);

            if (func != null)
            {
                return func.LazyCall(args);
            }
            else
            {
                Debug.LogWarning("Cannot find function : " + functionName);
            }

            return null;
        }

        public void LuaGC()
        {
            m_LuaState.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
        }

        public object[] CallTableFunc(string strFullName, params object[] args)
        {
            object[] pRtnObjs = null;
            do
            {
                if (strFullName == null)
                {
                    break;
                }

                int dwLastSemIndex = -1;
                int dwSemNum = 0;
                int dwLenMax = strFullName.Length;
                {
                    for (int i = 0; i < dwLenMax; ++i)
                    {
                        if (strFullName[i] == ':')
                        {
                            dwLastSemIndex = i;
                            dwSemNum++;
                        }
                    }
                }
                //错误格式
                if (dwSemNum >= 2 || dwLastSemIndex == 0 || dwLastSemIndex >= dwLenMax - 1)
                {
                    break;
                }

                if (dwLastSemIndex > 0)
                {
                    bool bCallSucFlg = false;
                    {
                        string strFuncName = strFullName.Substring(dwLastSemIndex + 1, dwLenMax - dwLastSemIndex - 1);
                        string strTableName = strFullName.Substring(0, dwLastSemIndex);

                        try
                        {
                            LuaTable pCurTable = m_LuaState.GetTable(strTableName);
                            if (pCurTable != null)
                            {
                                pRtnObjs = pCurTable.CallTableFunc(strFuncName, args);
                                bCallSucFlg = true;
                            }

                            pCurTable.Dispose();
                            pCurTable = null;
                        }
                        catch (System.Exception ex)
                        {
                            Debug.LogError(ex.ToString());
                        }
                    }

                    if (!bCallSucFlg)
                    {
                        Debugger.LogError(string.Format("lua CallTableFunc fail={0}", strFullName));
                    }
                }
                else
                {
                    Debug.LogWarning("It's not a dynamic table, please use CallMethod instead");
                }
            } while (false);

            return pRtnObjs;
        }


    }

}