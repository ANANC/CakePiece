﻿using System.Collections;
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
        Stone_RunTime.AddManager(new RoleManager());
        Stone_RunTime.AddManager(new ActionControlManager());
        Stone_RunTime.AddManager(new GamePlayerManager());

        GamePlayerManager gamePlayerManager = Stone_RunTime.GetManager<GamePlayerManager>(GamePlayerManager.Name);
        gamePlayerManager.CreateGamePlay("400000_GamePlay");

        GameObject keyboardGameObject = new GameObject();
        keyboardGameObject.AddComponent<KeyboardControl>();
    }

    public void Exist()
    {

    }

 
}