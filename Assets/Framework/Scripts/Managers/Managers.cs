using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers: BaseBehaviour
{
    private List<BaseBehaviour> m_Managers = new List<BaseBehaviour>();

    private LuaManager m_LuaManager = null;
    public LuaManager Lua
    {
        get
        {
            return m_LuaManager;
        }
    }

    private CoroutineManager m_CoroutineManager = null;
    public CoroutineManager Coroutine
    {
        get
        {
            return m_CoroutineManager;
        }
    }

    public override void Init()
    {
        CreateManagers();

        for(int index = 0;index< m_Managers.Count;index++)
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
        m_CoroutineManager = Framework.RootGameObject.AddComponent<CoroutineManager>();

        m_LuaManager = new LuaManager();
        m_Managers.Add(m_LuaManager);
    }
}
