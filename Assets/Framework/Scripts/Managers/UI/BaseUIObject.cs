using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANFramework
{
    public class BaseUIObject : BaseBehaviour
    {
        public string UIName;
        public string ResourceName;
        public GameObject GameObject;
        public Transform Transform;

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

        public override void Init()
        {
            CallLuaFunction("Init");
        }

        public override void Start()
        {
            CallLuaFunction("Start");
        }

        public void Close()
        {
            CallLuaFunction("Close");
        }

        public override void Destroy()
        {
            CallLuaFunction("Destroy");
        }

        private void CallLuaFunction(string funName)
        {
            ANF.Core.Mgr.Lua.CallTableFunc("ANF.UIMgr.__CallUIFunction", UIName, funName);
        }

        public bool IsOpen()
        {
            return GameObject.activeSelf;
        }
    }
}