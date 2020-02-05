using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = System.Object;

namespace ANFramework
{

    public class ResourceManager : BaseManager
    {
        private IReousrceLoader m_Loader;

        public override void Init()
        {

#if UNITY_EDITOR
            Object loader = null;
            string path = Application.dataPath + "/Core/Scripts/Engine/Source/Editor/FrameworkEditor.dll";
            if (!System.IO.File.Exists(path))
            {
                path = Application.dataPath + "/../Library/ScriptAssemblies/Assembly-CSharp-Editor.dll";
            }
            Assembly assembly = Assembly.LoadFile(path);
            Type type = assembly.GetType("ANFramework.EditorReousrceLoader");
            loader = Activator.CreateInstance(type, null);

            m_Loader = loader as IReousrceLoader;
#endif

            m_Loader.Init();

        }

        public override void Start()
        {
            m_Loader.Start();
        }

        public override void Update()
        {
            m_Loader.Update();
        }

         //------------ GameObject加载 ------------


        public GameObject Instance(string path)
        {
            GameObject prefab = LoadResource<GameObject>(path);
            if (prefab != null)
            {
                return GameObject.Instantiate(prefab);
            }

            return null;
        }


         //------------ 资源加载 ------------
        
        public T LoadResource<T>(string path) where T : UnityEngine.Object
        {
            T resObject = null;

            resObject = m_Loader.LoadResource<T>(path);

            if (resObject == null)
            {
                Debug.LogError(string.Format("资源（{0}）加载失败", path));
            }

            return resObject;
        }

        public T ResourcesLoad<T>(string path) where T : UnityEngine.Object
        {
            T resObject = Resources.Load<T>(path);
            return resObject;
        }

         //------------ 安全销毁 ------------

        public void DestroyGameObject(GameObject go)
        {
            if (go == null)
            {
                Debug.LogError("资源失败失败！因为资源为空");
                return;
            }
            GameObject.Destroy(go);
        }
    }

}