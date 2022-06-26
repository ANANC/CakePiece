using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GamePlay_Jigsaw : IGamePlayController
{
    public const float TweenUpdateTime = 1;    //��������ʱ�䣨��λ��s��
    public const float TweenMoveLength = 2;    //�����ƶ����루��λ���ף�

    public class UserGamePlayInfo : Stone_BaseUserConfigData
    {
        public string PieceArtInfoName;         //���������ļ���
        public string[] PieceActions;           //�ؿ���Ϊ����
        public string TerrainName;              //����
        public Vector3 OriginLogicPosition;     //������
        public string MainPlayerModelName;      //�����ģ����
        public string[] MainPlayerActions;      //�������Ϊ����
        public string MainCameraConfigName;     //�������������
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

        PieceManager.SetArtInfoName(m_GamePlayInfo.PieceArtInfoName);

        EventManager = Stone_RunTime.GetManager<Stone_EventManager>(Stone_EventManager.Name);
        EventManager.AddListener<GameEventDefine.MainPlayerMoveEventInfo>(GameEventDefine.MainPlayerMoveEvent, this, this.MainPlayerMoveEventListener);

        InitGamePlayInfo(); //��ʼ������

        EnterProcedure();   //����
    }

    public void UnInit()
    {
        EventManager.DeleteTargetAllListener(this);
    }

    /// <summary>
    /// ��ʼ���淨����
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
    /// ��������
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
    /// �����¼���MainPlayerMoveEvent
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
            ChangeFloorArt(curFloor, newFloor, mainPlayerMoveEventInfo.OldLogicPosition, mainPlayerMoveEventInfo.NewLogicPosition);
        }
        else
        {
            ChangePlayerPositionArt(mainPlayerMoveEventInfo.OldLogicPosition, mainPlayerMoveEventInfo.NewLogicPosition);
        }
    }

    /// <summary>
    /// �ı�㼶����
    /// </summary>
    /// <param name="newFloor"></param>
    private void ChangeFloorArt(int curFloor, int newFloor, Vector3 oldLogicPosition, Vector3 newLogicPosition)
    {
        string sequenceName = "ChangeFloorArt";
        Sequence sequence = DOTween.Sequence();

        m_SequenceDict.AddSequenceByOnlyRun(sequenceName, sequence);

        sequence.AppendCallback(() =>   //���������
        {
            GameEventDefine.ThreeDimensionalSpace_PlayerArt_HideEventInfo info = new GameEventDefine.ThreeDimensionalSpace_PlayerArt_HideEventInfo();
            info.PlayerId = RoleManager.GetMainPlayerId();

            EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_PlayerArt_HideEvent, info);
        });

        sequence.AppendCallback(() =>   //�뿪�ؿ�
        {
            GameEventDefine.ThreeDimensionalSpace_FloorArt_StandOutPieceEventInfo info = new GameEventDefine.ThreeDimensionalSpace_FloorArt_StandOutPieceEventInfo();
            info.LogicPos = oldLogicPosition;

            EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_FloorArt_StandOutPieceEvent, info);
        });

        //������
        if (curFloor > newFloor)
        {
            sequence.AppendCallback(() =>   //ԭ���Ĳ��½�
            {
                GameEventDefine.ThreeDimensionalSpace_FloorArt_DownFloorEventInfo info = new GameEventDefine.ThreeDimensionalSpace_FloorArt_DownFloorEventInfo();
                info.DownFloor = curFloor;
                info.TweenMoveLength = TweenMoveLength;
                info.TweenUpdateTime = TweenUpdateTime;

                EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_FloorArt_DownFloorEvent, info);
            });
            sequence.AppendInterval(TweenUpdateTime);   //���붯��ʱ��
        }

        // ������
        else if (curFloor < newFloor)
        {
            sequence.AppendCallback(() =>   //ԭ���Ĳ�����
            {
                GameEventDefine.ThreeDimensionalSpace_FloorArt_UpFloorEventInfo info = new GameEventDefine.ThreeDimensionalSpace_FloorArt_UpFloorEventInfo();
                info.UpFloor = curFloor;
                info.TweenMoveLength = TweenMoveLength;
                info.TweenUpdateTime = TweenUpdateTime;

                EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_FloorArt_UpFloorEvent, info);
            });
            sequence.AppendInterval(TweenUpdateTime);   //���붯��ʱ��
        }

        sequence.AppendCallback(() =>   //�����²��λ��
        {
            GameEventDefine.ThreeDimensionalSpace_FloorArt_ResetArtPositionFromLogicEventInfo info = new GameEventDefine.ThreeDimensionalSpace_FloorArt_ResetArtPositionFromLogicEventInfo();
            info.Floor = newFloor;

            EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_FloorArt_ResetArtPositionFromLogicEvent, info);
        });

        sequence.AppendCallback(() =>   //��ʾ�²�
        {
            GameEventDefine.ThreeDimensionalSpace_FloorArt_ShowFloorEventInfo info = new GameEventDefine.ThreeDimensionalSpace_FloorArt_ShowFloorEventInfo();
            info.Floor = newFloor;

            EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_FloorArt_ShowFloorEvent, info);
        });

        sequence.AppendCallback(() =>   //���������λ��
        {
            GameEventDefine.ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEventInfo info = new GameEventDefine.ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEventInfo();
            info.PlayerId = RoleManager.GetMainPlayerId();
            RoleController mainPlayer = RoleManager.GetMainPlayer();
            info.ArtPosition = mainPlayer.GetArtPosition();

            EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEvent, info);
        });

        sequence.AppendCallback(() =>   //��ʾ�����
        {
            GameEventDefine.ThreeDimensionalSpace_PlayerArt_ShowEventInfo info = new GameEventDefine.ThreeDimensionalSpace_PlayerArt_ShowEventInfo();
            info.PlayerId = RoleManager.GetMainPlayerId();

            EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_PlayerArt_ShowEvent, info);
        });


        sequence.AppendCallback(() =>   //����ؿ�
        {
            GameEventDefine.ThreeDimensionalSpace_FloorArt_StandInPieceEventInfo info = new GameEventDefine.ThreeDimensionalSpace_FloorArt_StandInPieceEventInfo();
            info.LogicPos = newLogicPosition;

            EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_FloorArt_StandInPieceEvent, info);
        });
    }

    /// <summary>
    /// �ı����λ�ñ���
    /// </summary>
    private void ChangePlayerPositionArt(Vector3 oldLogicPosition, Vector3 newLogicPosition)
    {
        string sequenceName = "ChangePlayerPositionArt";
        Sequence sequence = DOTween.Sequence();

        m_SequenceDict.AddSequenceByOnlyRun(sequenceName, sequence);

        sequence.AppendCallback(() =>   //���������
        {
            GameEventDefine.ThreeDimensionalSpace_PlayerArt_HideEventInfo info = new GameEventDefine.ThreeDimensionalSpace_PlayerArt_HideEventInfo();
            info.PlayerId = RoleManager.GetMainPlayerId();

            EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_PlayerArt_HideEvent, info);
        });

        sequence.AppendCallback(() =>   //�뿪�ؿ�
        {
            GameEventDefine.ThreeDimensionalSpace_FloorArt_StandOutPieceEventInfo info = new GameEventDefine.ThreeDimensionalSpace_FloorArt_StandOutPieceEventInfo();
            info.LogicPos = oldLogicPosition;

            EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_FloorArt_StandOutPieceEvent, info);
        });

        sequence.AppendCallback(() =>   //���������λ��
        {
            GameEventDefine.ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEventInfo info = new GameEventDefine.ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEventInfo();
            info.PlayerId = RoleManager.GetMainPlayerId();
            RoleController mainPlayer = RoleManager.GetMainPlayer();
            info.ArtPosition = mainPlayer.GetArtPosition();

            EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEvent, info);
        });

        sequence.AppendCallback(() =>   //��ʾ�����
        {
            GameEventDefine.ThreeDimensionalSpace_PlayerArt_ShowEventInfo info = new GameEventDefine.ThreeDimensionalSpace_PlayerArt_ShowEventInfo();
            info.PlayerId = RoleManager.GetMainPlayerId();

            EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_PlayerArt_ShowEvent, info);
        });

        sequence.AppendCallback(() =>   //����ؿ�
        {
            GameEventDefine.ThreeDimensionalSpace_FloorArt_StandInPieceEventInfo info = new GameEventDefine.ThreeDimensionalSpace_FloorArt_StandInPieceEventInfo();
            info.LogicPos = newLogicPosition;

            EventManager.Execute(GameEventDefine.ThreeDimensionalSpace_FloorArt_StandInPieceEvent, info);
        });
    }
}
