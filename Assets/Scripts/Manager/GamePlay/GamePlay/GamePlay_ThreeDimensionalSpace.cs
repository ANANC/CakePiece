using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay_ThreeDimensionalSpace : IGamePlayController
{
    public const float TweenUpdateTime = 1;    //动画更新时间（单位：s）
    public const float TweenMoveLength = 2;    //动画移动距离（单位：米）

    public class UserGamePlayInfo:Stone_BaseUserConfigData
    {
        public string[] PieceActions;           //地块行为集合
        public string TerrainName;              //地形
        public Vector3 OriginLogicPosition;     //出生点
        public string MainPlayerModelName;      //主玩家模型名
        public string[] MainPlayerActions;      //主玩家行为集合
    }

    private UserGamePlayInfo m_GamePlayInfo;

    private Dictionary<string, Sequence> m_SequenceDict;

    private PieceManager PieceManager;
    private TerrainManager TerrainManager;
    private RoleManager RoleManager;
    private Stone_EventManager EventManager;

    public void Init(string configName)
    {
        m_SequenceDict = new Dictionary<string, Sequence>();

        PieceManager = Stone_RunTime.GetManager<PieceManager>(PieceManager.Name);
        RoleManager = Stone_RunTime.GetManager<RoleManager>(RoleManager.Name);
        TerrainManager = Stone_RunTime.GetManager<TerrainManager>(TerrainManager.Name);

        Stone_UserConfigManager userConfigManager = Stone_RunTime.GetManager<Stone_UserConfigManager>(Stone_UserConfigManager.Name);
        m_GamePlayInfo = userConfigManager.GetConfig<UserGamePlayInfo>(configName);

        EventManager = Stone_RunTime.GetManager<Stone_EventManager>(Stone_EventManager.Name);
        EventManager.AddListener<GameEventDefine.MainPlayerMoveEventInfo>(GameEventDefine.MainPlayerMoveEvent, this, this.MainPlayerMoveEventListener);

        InitGamePlayInfo(); //初始化数据

        EnterProcedure();   //进入
    }

    public void UnInit()
    {
        EventManager.DeleteTargetAllListener(this);
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
    }

    /// <summary>
    /// 进入流程
    /// </summary>
    private void EnterProcedure()
    {
        TerrainManager.CreateTerrain(m_GamePlayInfo.TerrainName);

        RoleManager.CreateMainPlayer(m_GamePlayInfo.MainPlayerModelName, m_GamePlayInfo.MainPlayerActions);

        ActionControlManager actionControlManager = Stone_RunTime.GetManager<ActionControlManager>(ActionControlManager.Name);
        actionControlManager.ControlMainPlayerMove(m_GamePlayInfo.OriginLogicPosition);
    }

    /// <summary>
    /// 监听事件：MainPlayerMoveEvent
    /// </summary>
    /// <param name="mainPlayerMoveEventInfo"></param>
    private void MainPlayerMoveEventListener(GameEventDefine.MainPlayerMoveEventInfo mainPlayerMoveEventInfo)
    {
        int curFloor = TerrainManager.GetCurFloor();//(int)mainPlayerMoveEventInfo.OldLogicPosition.y;
        int newFloor = (int)mainPlayerMoveEventInfo.NewLogicPosition.y;

        int floorDistance = newFloor - curFloor;
        bool isUpdateFloor = floorDistance != 0;

        if (isUpdateFloor)
        {
            TerrainManager.SetCurFloor(newFloor);
            ChangeFloorArt(curFloor,newFloor);
        }
        else
        {
            ChangePlayerPositionArt();
        }
    }

    /// <summary>
    /// 改变层级表现
    /// </summary>
    /// <param name="newFloor"></param>
    private void ChangeFloorArt(int curFloor,int newFloor)
    {
        string sequenceName = "ChangeFloorArt";
        Sequence sequence = DOTween.Sequence();

        m_SequenceDict.AddSequenceByOnlyRun(sequenceName, sequence);

        sequence.AppendCallback(() =>   //隐藏主玩家
        {
            GameEventDefine.ThreeDimensionalSpace_PlayerArt_HideEventInfo info = new GameEventDefine.ThreeDimensionalSpace_PlayerArt_HideEventInfo();
            info.PlayerId = RoleManager.GetMainPlayerId();

            EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_PlayerArt_HideEvent, info);
        });

        //往下走
        if (curFloor > newFloor)
        {
            sequence.AppendCallback(() =>   //原本的层下降
            {
                GameEventDefine.ThreeDimensionalSpace_FloorArt_DownFloorEventInfo info = new GameEventDefine.ThreeDimensionalSpace_FloorArt_DownFloorEventInfo();
                info.DownFloor = curFloor;
                info.TweenMoveLength = TweenMoveLength;
                info.TweenUpdateTime = TweenUpdateTime;

                EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_FloorArt_DownFloorEvent, info);
            });
            sequence.AppendInterval(TweenUpdateTime);   //插入动画时间
        }

        // 往上走
        else if (curFloor < newFloor)
        {
            sequence.AppendCallback(() =>   //原本的层上升
            {
                GameEventDefine.ThreeDimensionalSpace_FloorArt_UpFloorEventInfo info = new GameEventDefine.ThreeDimensionalSpace_FloorArt_UpFloorEventInfo();
                info.UpFloor = curFloor;
                info.TweenMoveLength = TweenMoveLength;
                info.TweenUpdateTime = TweenUpdateTime;

                EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_FloorArt_UpFloorEvent, info);
            });
            sequence.AppendInterval(TweenUpdateTime);   //插入动画时间
        }

        sequence.AppendCallback(() =>   //重置新层的位置
        {
            GameEventDefine.ThreeDimensionalSpace_FloorArt_ResetArtPositionFromLogicEventInfo info = new GameEventDefine.ThreeDimensionalSpace_FloorArt_ResetArtPositionFromLogicEventInfo();
            info.Floor = newFloor;

            EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_FloorArt_ResetArtPositionFromLogicEvent, info);
        });

        sequence.AppendCallback(() =>   //显示新层
        {
            GameEventDefine.ThreeDimensionalSpace_FloorArt_ShowFloorEventInfo info = new GameEventDefine.ThreeDimensionalSpace_FloorArt_ShowFloorEventInfo();
            info.Floor = newFloor;

            EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_FloorArt_ShowFloorEvent, info);
        });

        sequence.AppendCallback(() =>   //重置主玩家位置
        {
            GameEventDefine.ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEventInfo info = new GameEventDefine.ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEventInfo();
            info.PlayerId = RoleManager.GetMainPlayerId();
            RoleController mainPlayer = RoleManager.GetMainPlayer();
            info.ArtPosition = mainPlayer.GetArtPosition();

            EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEvent, info);
        });

        sequence.AppendCallback(() =>   //显示主玩家
        {
            GameEventDefine.ThreeDimensionalSpace_PlayerArt_ShowEventInfo info = new GameEventDefine.ThreeDimensionalSpace_PlayerArt_ShowEventInfo();
            info.PlayerId = RoleManager.GetMainPlayerId();

            EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_PlayerArt_ShowEvent, info);
        });
    }

    /// <summary>
    /// 改变玩家位置表现
    /// </summary>
    private void ChangePlayerPositionArt()
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

        sequence.AppendCallback(() =>   //重置主玩家位置
        {
            GameEventDefine.ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEventInfo info = new GameEventDefine.ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEventInfo();
            info.PlayerId = RoleManager.GetMainPlayerId();
            RoleController mainPlayer = RoleManager.GetMainPlayer();
            info.ArtPosition = mainPlayer.GetArtPosition();

            EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEvent, info);
        });

        sequence.AppendCallback(() =>   //显示主玩家
        {
            GameEventDefine.ThreeDimensionalSpace_PlayerArt_ShowEventInfo info = new GameEventDefine.ThreeDimensionalSpace_PlayerArt_ShowEventInfo();
            info.PlayerId = RoleManager.GetMainPlayerId();

            EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_PlayerArt_ShowEvent, info);
        });
    }


    private void AddSequenceByOnlyRun(string name, Sequence sequence)
    {
        StopAllSequence();

        m_SequenceDict.Add(name, sequence);
    }

    private void StopAllSequence()
    {
        Dictionary<string, Sequence>.Enumerator enumerator = m_SequenceDict.GetEnumerator();

        while (enumerator.MoveNext())
        {
            Sequence tween = enumerator.Current.Value;
            tween.Kill();
        }
        m_SequenceDict.Clear();
    }
}
