using System;
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

    private Dictionary<string, Func<RoleAction>> m_RoleActionName2CreateFuncDict;

    private int m_AutoId;

    private RoleController m_MainPlayer;
  

    public RoleManager(Stone_IManagerLifeControl stone_ManagerLifeControl) : base(stone_ManagerLifeControl) { }

    public override void Init()
    {
        m_RoleActionName2CreateFuncDict = new Dictionary<string, Func<RoleAction>>();

        m_AutoId = 0;
    }

    public override void UnInit()
    {

    }

    public void AddRoleActionNameAndCreateFunc(string roleActionName,Func<RoleAction>createFunc)
    {
        m_RoleActionName2CreateFuncDict.Add(roleActionName, createFunc);
    }

    public void CreateMainPlayer(string modelName, string[] actionNames)
    {
        if (m_MainPlayer != null)
        {
            return;
        }

        m_MainPlayer = CreateRole<RoleContole_MainPlayer>(modelName, actionNames);
    }

    public RoleController CreateRole<T>(string modelName, string[] actionNames) where T: RoleController,new ()
    {
        RoleController roleController = new T();

        roleController.Init();

        int id = m_AutoId++;
        roleController.SetId(id);

        ModelManager modelManager = Stone_RunTime.GetManager<ModelManager>(ModelManager.Name);
        GameObject gameObject = modelManager.InstanceModel(modelName);

        roleController.SetGameObject(gameObject);

        if (actionNames != null)
        {
            for (int index = 0; index < actionNames.Length; index++)
            {
                string actionName = actionNames[index];
                Func<RoleAction> createFunc = m_RoleActionName2CreateFuncDict[actionName];
                RoleAction roleAction = createFunc();

                roleAction.SetRoleController(roleController);
                roleController.AddAction(roleAction);
            }
        }

        return roleController;
    }

    public RoleController GetMainPlayer()
    {
        return m_MainPlayer;
    }

    public int GetMainPlayerId()
    {
        if(m_MainPlayer == null)
        {
            return -1;
        }

        return m_MainPlayer.GetId();

    }
}
