using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : Stone_Manager
{
    public const string Name = "TerrainManager";

    public override string GetName()
    {
        return TerrainManager.Name;
    }

    public class UserTerrainInfo : Stone_BaseUserConfigData
    {
        public string Name; //名

        public PieceManager.UserPieceInfo[] PieceInfos; //块信息
    }


    private UserTerrainInfo m_CurUserTerrainInfo;

    private int m_CurFloor; //当前层

    private PieceManager PieceManager;

    public override void Init()
    {

    }
    public override void UnInit()
    {

    }

    public override void Active()
    {
        PieceManager = Stone_RunTime.GetManager<PieceManager>(PieceManager.Name);
    }

    public void CreateTerrain(string terrainName)
    {
        if(m_CurUserTerrainInfo!=null)
        {
            return;
        }

        Stone_UserConfigManager userConfigManager = Stone_RunTime.GetManager<Stone_UserConfigManager>(Stone_UserConfigManager.Name);
        m_CurUserTerrainInfo = userConfigManager.GetConfig<UserTerrainInfo>(terrainName);

        PieceManager.UserPieceInfo[] userPieceInfos = m_CurUserTerrainInfo.PieceInfos;
        int pieceInfoCount = userPieceInfos.Length;
        for (int index = 0;index< pieceInfoCount; index++)
        {
            PieceManager.LayPiece(userPieceInfos[index]);
        }
    }

    public void SetCurFloor(int floor)
    {
        m_CurFloor = floor;
    }

    public int GetCurFloor()
    {
        return m_CurFloor;
    }

}
