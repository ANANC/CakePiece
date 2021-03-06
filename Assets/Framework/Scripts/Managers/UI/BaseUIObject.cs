﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANFramework
{
    public class BaseUIObject
    {
        public string UIName;
        public string ResourceName;
        public GameObject GameObject;
        public Transform Transform;

        private List<string> m_ChildUIName = null;

        public BaseUIObject(string uiName, string resourceName)
        {
            UIName = uiName;
            ResourceName = resourceName;
        }

        public void SetRoot(GameObject gameObject)
        {
            GameObject = gameObject;
            Transform = gameObject.transform;
        }

        public void AddSubUI(string uiName)
        {
            if(m_ChildUIName ==  null)
            {
                m_ChildUIName = new List<string>(1);
            }
            m_ChildUIName.Add(uiName);
        }

        public List<string> GetSubUI()
        {
            return m_ChildUIName;
        }

        public void Init()
        {
            CallLuaFunction("Init");
        }

        public void Start()
        {
            CallLuaFunction("Start");
        }

        public void Close()
        {
            CallLuaFunction("Close");
        }

        public void Destroy()
        {
            CallLuaFunction("Destroy");
        }

        private void CallLuaFunction(string funName)
        {
            ANF.Core.Mgr.Lua.CallTableFunc("ANF.UIMgr:__CallUIFunction", UIName, funName);
        }

        public bool IsOpen()
        {
            return GameObject.activeSelf;
        }
    }
}