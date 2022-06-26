using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEventDefine;

public class GamePlay_MemoryLeft : IGamePlayController
{
    public const float DownTime = 1.2f;        //下降时间（单位：秒）
    public const float DownDistance = 2.5f;    //下架距离（单位：米）

    public class UserGamePlayInfo : Stone_BaseUserConfigData
    {
        public string PieceArtInfoName;         //地形配置文件名
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

    private RoleController m_MainPlayerController;

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

        PieceManager.SetArtInfoName(m_GamePlayInfo.PieceArtInfoName);

        m_SequenceDict = new Dictionary<string, Sequence>();
        m_OldTerrain = new Dictionary<Vector3, bool>();
        m_NextTerrain = new List<PieceManager.UserPieceInfo>();

        InitGamePlayInfo(); //初始化数据
        EnterProcedure();   //进入
        AddListeners();     //添加监听
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

        m_MainPlayerController = RoleManager.CreateMainPlayer(m_GamePlayInfo.MainPlayerModelName, m_GamePlayInfo.MainPlayerActions);

        m_MainPlayerController.SetPosition(m_GamePlayInfo.OriginLogicPosition, PieceManager.LogicPositionToArtPosition(m_GamePlayInfo.OriginLogicPosition));

        ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEventInfo info = new ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEventInfo();
        info.PlayerId = m_MainPlayerController.GetId();
        info.ArtPosition = m_MainPlayerController.GetArtPosition();
        EventManager.Execute(ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEvent, info);

        CameraManager cameraManager = Stone_RunTime.GetManager<CameraManager>(CameraManager.Name);
        cameraManager.CreateMainCamera(m_GamePlayInfo.MainCameraConfigName, m_MainPlayerController.GetTransform());

    }

    private void AddListeners()
    {
        EventManager.AddListener<MainPlayerMoveEventInfo>(MainPlayerMoveEvent, this, MainPlayerMoveEventListener);
        EventManager.AddListener<PieceDestroyEventInfo>(PieceDestroyEvent, this, PieceDestroyEventListener);
        EventManager.AddListener(GameResetRequestEvent, this, GameResetRequestEventListener);
    }

    /// <summary>
    /// 监听事件：MainPlayerMoveEvent
    /// </summary>
    /// <param name="mainPlayerMoveEventInfo"></param>
    private void MainPlayerMoveEventListener(MainPlayerMoveEventInfo mainPlayerMoveEventInfo)
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

        if (!isUpdateFloor)
        {
            List<Vector3> floorLogicPosList = PieceManager.GetFloorLogicPositions(newFloor);
            for(int index = 0;index<floorLogicPosList.Count;index++)
            {
                Vector3 logicPos = floorLogicPosList[index];
                PieceController pieceController = PieceManager.GetPiece(logicPos);
                ChangePieceEnableDirection(pieceController);
            }
        }
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
            ThreeDimensionalSpace_PlayerArt_HideEventInfo info = new ThreeDimensionalSpace_PlayerArt_HideEventInfo();
            info.PlayerId = RoleManager.GetMainPlayerId();

            EventManager.Execute(ThreeDimensionalSpace_PlayerArt_HideEvent, info);
        });

        sequence.AppendCallback(() =>   //离开地块
        {
            ThreeDimensionalSpace_FloorArt_StandOutPieceEventInfo info = new ThreeDimensionalSpace_FloorArt_StandOutPieceEventInfo();
            info.LogicPos = oldLogicPosition;

            EventManager.Execute(ThreeDimensionalSpace_FloorArt_StandOutPieceEvent, info);
        });

        if (isUpdateFloor)
        {
            sequence.AppendCallback(() =>   //隐藏旧层
            {
                ThreeDimensionalSpace_FloorArt_HideFloorEventInfo info = new ThreeDimensionalSpace_FloorArt_HideFloorEventInfo();
                info.Floor = (int)oldLogicPosition.y;

                EventManager.Execute(ThreeDimensionalSpace_FloorArt_HideFloorEvent, info);
            });
        }

        sequence.AppendCallback(() =>   //重置主玩家位置
        {
            ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEventInfo info = new ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEventInfo();
            info.PlayerId = RoleManager.GetMainPlayerId();
            RoleController mainPlayer = RoleManager.GetMainPlayer();
            info.ArtPosition = mainPlayer.GetArtPosition();

            EventManager.Execute(ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEvent, info);
        });

        if (isUpdateFloor)
        {
            sequence.AppendCallback(() =>   //显示新层
            {
                ThreeDimensionalSpace_FloorArt_ShowFloorEventInfo info = new ThreeDimensionalSpace_FloorArt_ShowFloorEventInfo();
                info.Floor = (int)newLogicPosition.y;

                EventManager.Execute(ThreeDimensionalSpace_FloorArt_ShowFloorEvent, info);
            });
        }

        sequence.AppendCallback(() =>   //显示主玩家
        {
            ThreeDimensionalSpace_PlayerArt_ShowEventInfo info = new ThreeDimensionalSpace_PlayerArt_ShowEventInfo();
            info.PlayerId = RoleManager.GetMainPlayerId();

            EventManager.Execute(ThreeDimensionalSpace_PlayerArt_ShowEvent, info);
        });

        if (isUpdateFloor)
        {
            sequence.AppendCallback(() =>   //更新地形
            {
                DestroyOldTerrain((int)oldLogicPosition.y);
                CreateNextTerrain((int)newLogicPosition.y);

                m_OldTerrain.Clear();
                m_NextTerrain.Clear();
            });
        }

        sequence.AppendCallback(() =>   //进入地块
        {
            ThreeDimensionalSpace_FloorArt_StandInPieceEventInfo info = new ThreeDimensionalSpace_FloorArt_StandInPieceEventInfo();
            info.LogicPos = newLogicPosition;

            EventManager.Execute(ThreeDimensionalSpace_FloorArt_StandInPieceEvent, info);
        });
    }

    private void DestroyOldTerrain(int oldFloor)
    {
        List<Vector3> logicPositions = PieceManager.GetFloorLogicPositions(oldFloor);

        Vector3 logicPos;
        for (int index = 0; index < logicPositions.Count; index++)
        {
            logicPos = logicPositions[index];

            PieceController pieceController = PieceManager.GetPiece(logicPos);
            if (pieceController == null)
            {
                continue;
            }

            PieceAction_MemoryLeft memoryLeftAction = pieceController.GetPieceAction<PieceAction_MemoryLeft>();
            if (memoryLeftAction == null)
            {
                continue;
            }

            PieceAction_MemoryLeft.MemoryLeftInfo info = memoryLeftAction.GetInfo();
            if (!info.EnableTransform)
            {
                continue;
            }

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

    private void ChangePieceEnableDirection(PieceController pieceController)
    {
        //顺时针改变，上下不改变

        bool left = pieceController.IsDirectionEnable(Vector3.back);
        bool forward = pieceController.IsDirectionEnable(Vector3.left);
        bool right = pieceController.IsDirectionEnable(Vector3.forward);
        bool back = pieceController.IsDirectionEnable(Vector3.right);

        pieceController.UpdateDirectionEnable(Vector3.left, left);
        pieceController.UpdateDirectionEnable(Vector3.forward, forward);
        pieceController.UpdateDirectionEnable(Vector3.right, right);
        pieceController.UpdateDirectionEnable(Vector3.back, back);
    }


    private void PieceDestroyEventListener(PieceDestroyEventInfo pieceDestroyEventInfo)
    {
        Vector3 mainPlayerCurLogicPos = m_MainPlayerController.GetLogicPosition();
        if (mainPlayerCurLogicPos != pieceDestroyEventInfo.LogicPos)
        {
            return;
        }

        MainPlayerDeathAndReviveControl(true);
    }

    private void MainPlayerDeathAndReviveControl(bool isShowDeath)
    {
        //取消全部监听
        EventManager.DeleteTargetAllListener(this);

        //清理记录
        m_OldTerrain.Clear();
        m_NextTerrain.Clear();

        //不接受玩家控制
        ActionControlManager actionControlManager = Stone_RunTime.GetManager<ActionControlManager>(ActionControlManager.Name);
        actionControlManager.SetEnableReceiveUserControl(false);

        string sequenceName = "MainPlayerDeathAndRevive";
        Sequence sequence = DOTween.Sequence();

        m_SequenceDict.AddSequenceByOnlyRun(sequenceName, sequence);

        if (isShowDeath)
        {
            sequence.AppendCallback(() =>       //死亡效果
            {
                ThreeDimensionalSpace_PlayerArt_DownDeathEventInfo info = new ThreeDimensionalSpace_PlayerArt_DownDeathEventInfo();
                info.PlayerId = m_MainPlayerController.GetId();
                info.DownTime = DownTime;
                info.DownDistance = DownDistance;
                EventManager.Execute(ThreeDimensionalSpace_PlayerArt_DownDeathEvent, info);
            });
            sequence.AppendInterval(DownTime);  //死亡效果时间

            sequence.AppendInterval(0.2f);      //静止时间
        }
        else
        {
            sequence.AppendCallback(() =>   //隐藏主玩家
            {
                ThreeDimensionalSpace_PlayerArt_HideEventInfo info = new ThreeDimensionalSpace_PlayerArt_HideEventInfo();
                info.PlayerId = RoleManager.GetMainPlayerId();

                EventManager.Execute(ThreeDimensionalSpace_PlayerArt_HideEvent, info);
            });
        }

        sequence.AppendCallback(() => {     //重置地形
            Vector3 originLogicPosition = m_GamePlayInfo.OriginLogicPosition;
            TerrainManager.SetCurFloor((int)originLogicPosition.y);

            TerrainManager.ResetTerrain();
        });

        sequence.AppendCallback(() => {     //重置主玩家位置
            Vector3 originLogicPosition = m_GamePlayInfo.OriginLogicPosition;
            m_MainPlayerController.SetPosition(originLogicPosition, PieceManager.LogicPositionToArtPosition(originLogicPosition));

            ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEventInfo info = new ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEventInfo();
            info.PlayerId = m_MainPlayerController.GetId();
            info.ArtPosition = m_MainPlayerController.GetArtPosition();
            EventManager.Execute(ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEvent, info);
        });

        sequence.AppendCallback(() =>   //显示主玩家
        {
            ThreeDimensionalSpace_PlayerArt_ShowEventInfo info = new ThreeDimensionalSpace_PlayerArt_ShowEventInfo();
            info.PlayerId = RoleManager.GetMainPlayerId();

            EventManager.Execute(ThreeDimensionalSpace_PlayerArt_ShowEvent, info);
        });

        sequence.AppendCallback(() =>   //进入地块
        {
            ThreeDimensionalSpace_FloorArt_StandInPieceEventInfo info = new ThreeDimensionalSpace_FloorArt_StandInPieceEventInfo();
            info.LogicPos = m_GamePlayInfo.OriginLogicPosition;

            EventManager.Execute(ThreeDimensionalSpace_FloorArt_StandInPieceEvent, info);
        });

        sequence.AppendCallback(() =>   //重新监听
        {
            AddListeners();
        });

        sequence.AppendCallback(() =>   //接收玩家操作
        {
            actionControlManager.SetEnableReceiveUserControl(true);
        });
    }

    private void GameResetRequestEventListener()
    {
        MainPlayerDeathAndReviveControl(false);
    }

}
