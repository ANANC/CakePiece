using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Framework 
{
    private static Framework m_Instance;
    public static Framework Core
    {
        get
        {
            if(m_Instance == null)
            {
                m_Instance = new Framework();
            }
            return m_Instance;
        }
    }

    private static GameObject m_RootGo;
    public static GameObject RootGameObject
    {
        get
        {
            if(m_RootGo == null)
            {
                m_RootGo = GameObject.Find(FrameworkDefine.ROOT_GAMEOBJECT_NAME);
            }
            return m_RootGo;
        }
    }

    private Managers m_Managers;
    public Managers Mgr
    {
        get { return m_Managers; }
    }

    private States m_States;

    private Framework()
    {
        m_Managers = new Managers();
        m_States = new States();
    }

    public void Init()
    {
        m_Managers.Init();
        m_States.Init();
    }

    public void Start()
    {
        m_Managers.Start();
        m_States.Start();
    }

    public void Update()
    {
        m_Managers.Update();
        m_States.Update();
    }

    public void Destroy()
    {
        m_Managers.Destroy();
        m_States.Destroy();
    }

}
