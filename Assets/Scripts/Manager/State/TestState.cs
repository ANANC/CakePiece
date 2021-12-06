using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestState : Stone_IState
{
    public const string Name = "TestState";
    public string GetName()
    {
        return TestState.Name;
    }

    public void Init()
    {

    }

    public void UnInit()
    {

    }

    public void Enter()
    {
        ModelManager modelManager = Stone_RunTime.GetManager<ModelManager>(ModelManager.Name);
        modelManager.InstanceModel("100000_Piece");
    }

    public void Exist()
    {

    }

 
}
