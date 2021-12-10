using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GamePlayerManager : Stone_Manager
{
    public const string Name = "GamePlayerManager";
    public override string GetName()
    {
        return GamePlayerManager.Name;
    }

    public enum GamePlayType
    {
        ThreeDimensionalSpace = 1,  //三维空间
    }

    private Dictionary<GamePlayType, Type> GamePlayType2ControllerDict = new Dictionary<GamePlayType, Type>()
    {
        {GamePlayType.ThreeDimensionalSpace,typeof(GamePlay_ThreeDimensionalSpace) }
    };

    public class UserGamePlayInfo : Stone_BaseUserConfigData
    {
        public int GamePlayType;    //类型
        public string ExpandConfigName; //扩展信息配置名
    }

    private UserGamePlayInfo m_GamePlayInfo;
    private IGamePlayController m_GamePlay;

    public override void Init()
    {

    }

    public override void UnInit()
    {
        m_GamePlay?.UnInit();
    }

    public void CreateGamePlay(string gamePlayName)
    {
        Stone_UserConfigManager userConfigManager = Stone_RunTime.GetManager<Stone_UserConfigManager>(Stone_UserConfigManager.Name);
        m_GamePlayInfo = userConfigManager.GetConfig<UserGamePlayInfo>(gamePlayName);

        GamePlayType gamePlayTypeEnum = (GamePlayType)m_GamePlayInfo.GamePlayType;
        Type gamePlayTypeClass = GamePlayType2ControllerDict[gamePlayTypeEnum];

        System.Object gamePlayerObject = Activator.CreateInstance(gamePlayTypeClass, null);

        m_GamePlay = gamePlayerObject as IGamePlayController;
        m_GamePlay.Init(m_GamePlayInfo.ExpandConfigName);
    }
}
