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
        Stone_RunTime.AddManager(new PieceManager());
        Stone_RunTime.AddManager(new TerrainManager());

        TerrainManager terrainManager = Stone_RunTime.GetManager<TerrainManager>(TerrainManager.Name);
        terrainManager.CreateTerrain("300000_Test");
    }

    public void Exist()
    {

    }

 
}
