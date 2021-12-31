using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay_MemoryLeft : IGamePlayController
{

    public class UserGamePlayInfo : Stone_BaseUserConfigData
    {
        public string[] PieceActions;           //地块行为集合
        public string TerrainName;              //地形
        public string MainPlayerModelName;      //主玩家模型名
        public string[] MainPlayerActions;      //主玩家行为集合
        public string MainCameraConfigName;     //主摄像机配置名

        public Vector3 OriginLogicPosition;     //出生点
        public Vector3 EndLogicPosition;        //终结点
    }

    private UserGamePlayInfo m_GamePlayInfo;

    private Dictionary<string, Sequence> m_SequenceDict;

    private Dictionary<Vector3, bool> m_OldTerrain;         //旧地形
    private List<PieceManager.UserPieceInfo> m_NextTerrain; //下一个地形

    private PieceManager PieceManager;
    private TerrainManager TerrainManager;
    private RoleManager RoleManager;
    private Stone_EventManager EventManager;

    public void Init(string configName)
    {
        PieceManager = Stone_RunTime.GetManager<PieceManager>(PieceManager.Name);
        RoleManager = Stone_RunTime.GetManager<RoleManager>(RoleManager.Name);
        TerrainManager = Stone_RunTime.GetManager<TerrainManager>(TerrainManager.Name);
        EventManager = Stone_RunTime.GetManager<Stone_EventManager>(Stone_EventManager.Name);

        Stone_UserConfigManager userConfigManager = Stone_RunTime.GetManager<Stone_UserConfigManager>(Stone_UserConfigManager.Name);
        m_GamePlayInfo = userConfigManager.GetConfig<UserGamePlayInfo>(configName);

        EventManager.AddListener<GameEventDefine.MainPlayerMoveEventInfo>(GameEventDefine.MainPlayerMoveEvent, this, this.MainPlayerMoveEventListener);

        m_SequenceDict = new Dictionary<string, Sequence>();
        m_OldTerrain = new Dictionary<Vector3, bool>();
        m_NextTerrain = new List<PieceManager.UserPieceInfo>();

        InitGamePlayInfo(); //初始化数据
        EnterProcedure();   //进入
    }

    public void UnInit()
    {
        EventManager.DeleteTargetAllListener(this);

        PieceManager = null;
        RoleManager = null;
        TerrainManager = null;
        EventManager = null;
    }

    /// <summary>
    /// 初始化玩法数据
    /// </summary>
    private void InitGamePlayInfo()
    {
        string[] pieceActions = m_GamePlayInfo.PieceActions;
        for (int index = 0; index < pieceActions.Length; index++)
        {
            string pieceActionName = pieceActions[index];
            PieceManager.AddDefaultPieceActionName(pieceActionName);
        }

        TerrainManager.SetCurFloor((int)m_GamePlayInfo.OriginLogicPosition.y);

        TryAddNextTerrainPiece(m_GamePlayInfo.OriginLogicPosition);
    }

    /// <summary>
    /// 进入流程
    /// </summary>
    private void EnterProcedure()
    {
        TerrainManager.CreateTerrain(m_GamePlayInfo.TerrainName);

        RoleController mainPlayer = RoleManager.CreateMainPlayer(m_GamePlayInfo.MainPlayerModelName, m_GamePlayInfo.MainPlayerActions);

        ActionControlManager actionControlManager = Stone_RunTime.GetManager<ActionControlManager>(ActionControlManager.Name);
        actionControlManager.ControlMainPlayerMove(m_GamePlayInfo.OriginLogicPosition);

        CameraManager cameraManager = Stone_RunTime.GetManager<CameraManager>(CameraManager.Name);
        cameraManager.CreateMainCamera(m_GamePlayInfo.MainCameraConfigName, mainPlayer.GetTransform());
    }

    /// <summary>
    /// 监听事件：MainPlayerMoveEvent
    /// </summary>
    /// <param name="mainPlayerMoveEventInfo"></param>
    private void MainPlayerMoveEventListener(GameEventDefine.MainPlayerMoveEventInfo mainPlayerMoveEventInfo)
    {
        TryAddNextTerrainPiece(mainPlayerMoveEventInfo.NewLogicPosition);

        int curFloor = TerrainManager.GetCurFloor();
        int newFloor = (int)mainPlayerMoveEventInfo.NewLogicPosition.y;

        int floorDistance = newFloor - curFloor;
        bool isUpdateFloor = floorDistance != 0;

        if (isUpdateFloor)
        {
            TerrainManager.SetCurFloor(newFloor);
        }

        ChangePlayerPositionArt(mainPlayerMoveEventInfo.OldLogicPosition, mainPlayerMoveEventInfo.NewLogicPosition, isUpdateFloor);

    }

    private void TryAddNextTerrainPiece(Vector3 logicPos)
    {
        if(m_OldTerrain.ContainsKey(logicPos))
        {
            return;
        }

        PieceController pieceController = PieceManager.GetPiece(logicPos);
        if(pieceController == null)
        {
            return;
        }

        PieceAction_MemoryLeft memoryLeftAction = pieceController.GetPieceAction<PieceAction_MemoryLeft>();
        if(memoryLeftAction == null)
        {
            return;
        }

        PieceAction_MemoryLeft.MemoryLeftInfo info = memoryLeftAction.GetInfo();
        if (!info.EnableTransform)
        {
            return;
        }

        m_OldTerrain.Add(logicPos,true);
        m_NextTerrain.Add(pieceController.GetUserPieceInfo());
    }


    /// <summary>
    /// 改变玩家位置表现
    /// </summary>
    private void ChangePlayerPositionArt(Vector3 oldLogicPosition, Vector3 newLogicPosition,bool isUpdateFloor)
    {
        string sequenceName = "ChangePlayerPositionArt";
        Sequence sequence = DOTween.Sequence();

        m_SequenceDict.AddSequenceByOnlyRun(sequenceName, sequence);

        sequence.AppendCallback(() =>   //隐藏主玩家
        {
            GameEventDefine.ThreeDimensionalSpace_PlayerArt_HideEventInfo info = new GameEventDefine.ThreeDimensionalSpace_PlayerArt_HideEventInfo();
            info.PlayerId = RoleManager.GetMainPlayerId();

            EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_PlayerArt_HideEvent, info);
        });

        sequence.AppendCallback(() =>   //离开地块
        {
            GameEventDefine.ThreeDimensionalSpace_FloorArt_StandOutPieceEventInfo info = new GameEventDefine.ThreeDimensionalSpace_FloorArt_StandOutPieceEventInfo();
            info.LogicPos = oldLogicPosition;

            EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_FloorArt_StandOutPieceEvent, info);
        });

        if (isUpdateFloor)
        {
            sequence.AppendCallback(() =>   //隐藏旧层
            {
                GameEventDefine.ThreeDimensionalSpace_FloorArt_HideFloorEventInfo info = new GameEventDefine.ThreeDimensionalSpace_FloorArt_HideFloorEventInfo();
                info.Floor = (int)oldLogicPosition.y;

                EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_FloorArt_HideFloorEvent, info);
            });
        }

        sequence.AppendCallback(() =>   //重置主玩家位置
        {
            GameEventDefine.ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEventInfo info = new GameEventDefine.ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEventInfo();
            info.PlayerId = RoleManager.GetMainPlayerId();
            RoleController mainPlayer = RoleManager.GetMainPlayer();
            info.ArtPosition = mainPlayer.GetArtPosition();

            EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEvent, info);
        });

        if (isUpdateFloor)
        {
            sequence.AppendCallback(() =>   //显示新层
            {
                GameEventDefine.ThreeDimensionalSpace_FloorArt_ShowFloorEventInfo info = new GameEventDefine.ThreeDimensionalSpace_FloorArt_ShowFloorEventInfo();
                info.Floor = (int)newLogicPosition.y;

                EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_FloorArt_ShowFloorEvent, info);
            });
        }

        sequence.AppendCallback(() =>   //显示主玩家
        {
            GameEventDefine.ThreeDimensionalSpace_PlayerArt_ShowEventInfo info = new GameEventDefine.ThreeDimensionalSpace_PlayerArt_ShowEventInfo();
            info.PlayerId = RoleManager.GetMainPlayerId();

            EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_PlayerArt_ShowEvent, info);
        });

        sequence.AppendCallback(() =>   //进入地块
        {
            GameEventDefine.ThreeDimensionalSpace_FloorArt_StandInPieceEventInfo info = new GameEventDefine.ThreeDimensionalSpace_FloorArt_StandInPieceEventInfo();
            info.LogicPos = newLogicPosition;

            EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_FloorArt_StandInPieceEvent, info);
        });

        sequence.AppendCallback(() =>   //更新地形
        {
            DestroyOldTerrain();
            CreateNextTerrain((int)newLogicPosition.y);
        });

    }

    private void DestroyOldTerrain()
    {
        Dictionary<Vector3, bool>.Enumerator enumerator = m_OldTerrain.GetEnumerator();
        while(enumerator.MoveNext())
        {
            Vector3 logicPos = enumerator.Current.Key;
            PieceManager.DeletePiece(logicPos);
        }
    }

    private void CreateNextTerrain(int newFloor)
    {
        for(int index = 0;index< m_NextTerrain.Count;index++)
        {
            PieceManager.UserPieceInfo userPieceInfo = m_NextTerrain[index];
            userPieceInfo.LogicPosition.y = newFloor;

            PieceManager.LayPiece(userPieceInfo);
        }
    }



}
