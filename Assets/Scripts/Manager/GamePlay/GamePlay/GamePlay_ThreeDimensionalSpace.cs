using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay_ThreeDimensionalSpace : IGamePlayController
{
    public class UserGamePlayInfo:Stone_BaseUserConfigData
    {
        public string TerrainName;              //地形
        public Vector3 OriginLogicPosition;     //出生点
        public string PlayerModelName;          //玩家模型名
    }

    private UserGamePlayInfo m_GamePlayInfo;

    public void Init(string configName)
    {
        Stone_UserConfigManager userConfigManager = Stone_RunTime.GetManager<Stone_UserConfigManager>(Stone_UserConfigManager.Name);
        m_GamePlayInfo = userConfigManager.GetConfig<UserGamePlayInfo>(configName);

        TerrainManager terrainManager = Stone_RunTime.GetManager<TerrainManager>(TerrainManager.Name);
        terrainManager.CreateTerrain(m_GamePlayInfo.TerrainName);

        RoleManager roleManager = Stone_RunTime.GetManager<RoleManager>(RoleManager.Name);
        roleManager.CreateMainPlayer(m_GamePlayInfo.PlayerModelName);

        ActionControlManager actionControlManager = Stone_RunTime.GetManager<ActionControlManager>(ActionControlManager.Name);
        actionControlManager.ControlMainPlayerMove(m_GamePlayInfo.OriginLogicPosition);
    }

    public void UnInit()
    {

    }


}
