using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleManager : Stone_Manager
{
    public const string Name = "RoleManager";

    public override string GetName()
    {
        return RoleManager.Name;
    }

    private RoleController m_MainPlayer;

    public override void Init()
    {

    }

    public override void UnInit()
    {

    }

    public void CreateMainPlayer(string modelName)
    {
        if(m_MainPlayer!=null)
        {
            return;
        }

        m_MainPlayer = new RoleController();

        ModelManager modelManager = Stone_RunTime.GetManager<ModelManager>(ModelManager.Name);
        GameObject gameObject = modelManager.InstanceModel(modelName);

        m_MainPlayer.SetGameObject(gameObject);
    }

    public RoleController GetMainPlayer()
    {
        return m_MainPlayer;
    }
}
