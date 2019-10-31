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
                m_RootGo = GameObject.Find(FrameworkDefine.FrameworkGameObjectName);
            }
            return m_RootGo;
        }
    }

    Managers m_Managers;
    public Managers Mgr
    {
        get { return m_Managers; }
    }

    private Framework()
    {
        m_Managers = new Managers();
    }

    public void Init()
    {
        m_Managers.Init();
    }

    public void Start()
    {
        m_Managers.Start();
    }

    public void Update()
    {
        m_Managers.Update();
    }

}
