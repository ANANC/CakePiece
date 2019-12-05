using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = System.Object;

namespace ANFramework
{

    public class ResourceManager : BaseBehaviour
    {
        private BaseBehaviour m_LoadBehaviour;
        private IReousrceLoader m_Loader;

        public override void Init()
        {
            Object loader = null;

#if UNITY_EDITOR
            string path = Application.dataPath + "/Core/Scripts/Engine/Source/Editor/FrameworkEditor.dll";
            if (!System.IO.File.Exists(path))
            {
                path = Application.dataPath + "/../Library/ScriptAssemblies/Assembly-CSharp-Editor.dll";
            }
            Assembly assembly = Assembly.LoadFile(path);
            Type type = assembly.GetType("ANFramework.EditorReousrceLoader");
            loader = Activator.CreateInstance(type, null);
#endif

            m_Loader = loader as IReousrceLoader;
            m_LoadBehaviour = loader as BaseBehaviour;
            
            m_LoadBehaviour.Init();
        }

        public override void Start()
        {
            m_LoadBehaviour.Start();
        }

        public override void Update()
        {
            m_LoadBehaviour.Update();
        }

        // ------------ GameObject加载 ------------


        public GameObject Instance(string path)
        {
            GameObject prefab = LoadResource<GameObject>(path);
            if (prefab != null)
            {
                return GameObject.Instantiate(prefab);
            }

            return null;
        }


        // ------------ 资源加载 ------------
        
        public T LoadResource<T>(string path) where T : UnityEngine.Object
        {
            T resObject = null;

            resObject = m_Loader.LoadResource<T>(path);
            
            if (resObject == null)
            {
                resObject = LoadFromResources<T>(path);
            }

            if (resObject == null)
            {
                Debug.LogError(string.Format("资源（{0}）加载失败", path));
            }

            return resObject;
        }

        private T LoadFromResources<T>(string path) where T : UnityEngine.Object
        {
            T resObject = Resources.Load<T>(path);
            return resObject;
        }
    }

}