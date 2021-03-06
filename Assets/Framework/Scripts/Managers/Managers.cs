﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANFramework
{
    public class Managers : BaseManager
    {
        private List<BaseManager> m_Managers = new List<BaseManager>();

        private ResourceManager m_ResourceManager = null;

        public ResourceManager Resource
        {
            get { return m_ResourceManager; }
        }

        private LuaManager m_LuaManager = null;

        public LuaManager Lua
        {
            get { return m_LuaManager; }
        }

        private UIManager m_UIManager = null;

        public UIManager UI
        {
            get { return m_UIManager; }
        }

        private CoroutineManager m_CoroutineManager = null;

        public CoroutineManager Coroutine
        {
            get { return m_CoroutineManager; }
        }

        public Managers()
        {
            CreateManagers();
        }

        public void AddManager(BaseManager baseBehaviour)
        {
            m_Managers.Add(baseBehaviour);
        }

        public override void Init()
        {
            m_CoroutineManager = ANF.RootGameObject.AddComponent<CoroutineManager>();

            for (int index = 0; index < m_Managers.Count; index++)
            {
                m_Managers[index].Init();
            }
        }

        public override void Start()
        {
            for (int index = 0; index < m_Managers.Count; index++)
            {
                m_Managers[index].Start();
            }
        }

        public override void Update()
        {
            for (int index = 0; index < m_Managers.Count; index++)
            {
                m_Managers[index].Update();
            }
        }

        public override void Destroy()
        {
            for (int index = 0; index < m_Managers.Count; index++)
            {
                m_Managers[index].Destroy();
            }
        }

        private void CreateManagers()
        {
            m_LuaManager = new LuaManager();
            m_Managers.Add(m_LuaManager);

            m_ResourceManager = new ResourceManager();
            m_Managers.Add(m_ResourceManager);

            m_UIManager = new UIManager();
            m_Managers.Add(m_UIManager);
        }
    }

}