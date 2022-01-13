using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceAction_MemoryLeft : PieceAction
{
    private const float StandingWaitTime = 1f; //站立等待时间（秒）
    private const float RotationTweenTime = 0.3f; //翻转动画时间（秒）
    private const float RotationValue = 90; //翻转角度
    private Color DarkColor = new Color(0.165f, 0.165f, 0.165f, 1);  //黑色

    public const string Name = "PieceAction_MemoryLeft";
    public static PieceAction_MemoryLeft CreateAction()
    {
        return new PieceAction_MemoryLeft();
    }

    public class MemoryLeftInfo
    {
        public bool EnableTransform;    //是否允许变形
    }

    private MemoryLeftInfo m_Info;

    private int m_StandingTimer;    //站立计时器
    private int m_RotationTimer;    //翻转计时器

    private Stone_EventManager EventManager;
    private Stone_TimerManager TimerManager;
    private PieceManager PieceManager;

    public override void Init()
    {
        EventManager = Stone_RunTime.GetManager<Stone_EventManager>(Stone_EventManager.Name);
        TimerManager = Stone_RunTime.GetManager<Stone_TimerManager>(Stone_TimerManager.Name);
        PieceManager = Stone_RunTime.GetManager<PieceManager>(PieceManager.Name);

        EventManager.AddListener<GameEventDefine.ThreeDimensionalSpace_FloorArt_StandInPieceEventInfo>(GameEventDefine.ThreeDimensionalSpace_FloorArt_StandInPieceEvent, this, StandInPiece);
    }

    public override void UnInit()
    {
        if (m_StandingTimer != 0)
        {
            TimerManager.StopTimer(m_StandingTimer);
            m_StandingTimer = 0;
        }
        if (m_RotationTimer != 0)
        {
            TimerManager.StopTimer(m_RotationTimer);
            m_RotationTimer = 0;
        }

        EventManager.DeleteTargetAllListener(this);

        m_Info = null;

        EventManager = null;
        TimerManager = null;
        PieceManager = null; 
    }

    public override void SetInfo(string infoStr)
    {
        Stone_UserConfigManager userConfigManager = Stone_RunTime.GetManager<Stone_UserConfigManager>(Stone_UserConfigManager.Name);
        m_Info = userConfigManager.GetConfigByUserContent<MemoryLeftInfo>(infoStr);
    }

    public MemoryLeftInfo GetInfo()
    {
        return m_Info;
    }

    public void StandInPiece(GameEventDefine.ThreeDimensionalSpace_FloorArt_StandInPieceEventInfo info)
    {
        Vector3 myLogicPosition = m_PieceController.GetLogicPosition();
        if (myLogicPosition != info.LogicPos)
        {
            return;
        }

        if (!m_Info.EnableTransform)
        {
            return;
        }

        CreatePieceStandingTimer();
    }


    private void CreatePieceStandingTimer()
    {
        if (m_StandingTimer != 0)
        {
            TimerManager.StopTimer(m_StandingTimer);
        }
        if(m_RotationTimer!=0)
        {
            TimerManager.StopTimer(m_RotationTimer);
        }

        Vector3 logicPos = m_PieceController.GetLogicPosition();

        PieceController pieceController = PieceManager.GetPiece(logicPos);
        if (pieceController == null)
        {
            return;
        }
        Color curColor = pieceController.GetColor();
        Color offsetColor = (DarkColor - curColor) / (StandingWaitTime);

        m_StandingTimer = TimerManager.StarTimer(
        () =>
        {
            curColor = pieceController.GetColor();
            pieceController.SetColor(curColor + offsetColor * Time.deltaTime);
        },
        (isError0) =>
        {
            //进入翻转，不能移动到该地块上
            m_PieceController.SetEnableMove(false);

            m_StandingTimer = 0;

            if (isError0)
            {
                return;
            }

            float offsetRotation = (RotationValue / RotationTweenTime) * Time.deltaTime;
            bool rotationX = Random.value >= 0.5f;

            m_RotationTimer = TimerManager.StarTimer(
                () =>
                {
                    Transform transform = pieceController.GetTransform();

                    Quaternion quaternion = transform.localRotation;
                    Vector3 eulerAngles = transform.localRotation.eulerAngles;
                    if (rotationX)
                    {
                        eulerAngles.x += offsetRotation;
                    }
                    else
                    {
                        eulerAngles.z += offsetRotation;
                    }
                    quaternion.eulerAngles = eulerAngles;
                    transform.localRotation = quaternion;
                },
                (isError1) =>
                {
                    m_RotationTimer = 0;

                    if (isError1)
                    {
                        return;
                    }

                    PieceManager.DeletePiece(logicPos);
                }, updateTime: RotationTweenTime
            );
        }, updateTime: StandingWaitTime);
    }




}
