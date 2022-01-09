using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionControlManager : Stone_Manager
{
    public const string Name = "ActionControlManager";
    public override string GetName()
    {
        return ActionControlManager.Name;
    }

    public enum ActionDirection
    {
        Left,
        Right,
        Forward,
        Back,
        Up,
        Down,
    }

    private const float NextWaitTime = 0.2f; //下一个操作的等待时间（S）

    private RoleController m_MainPlayer;

    private bool m_ReceiveUserControl;      //是否接收用户操作

    private Vector3 m_BeforeLogicPos;       //上一个逻辑位置
    private int m_NextActionTimer;          //下一个操作计时器

    private Stone_EventManager EventManager;
    private PieceManager PieceManager;

    public override void Init()
    {
        m_ReceiveUserControl = true;

        EventManager = Stone_RunTime.GetManager<Stone_EventManager>(Stone_EventManager.Name);
        PieceManager = Stone_RunTime.GetManager<PieceManager>(PieceManager.Name);
    }

    public override void UnInit()
    {

    }

    private void TryInitMainPlayer()
    {
        if(m_MainPlayer!=null)
        {
            return;
        }

        RoleManager roleManager = Stone_RunTime.GetManager<RoleManager>(RoleManager.Name);
        m_MainPlayer = roleManager.GetMainPlayer();
    }

    /// <summary>
    /// 控制主玩家移动
    /// </summary>
    /// <param name="actionDirection"></param>
    /// <param name="speed"></param>
    public void ControlMainPlayerMove(ActionDirection actionDirection,int speed)
    {
        if(!m_ReceiveUserControl)
        {
            return;
        }

        TryInitMainPlayer();

        Vector3 mainplayerCurLogicPos = m_MainPlayer.GetLogicPosition();
        Vector3 position = ActionDirectionToVector3(actionDirection);
        Vector3 newLogicPos = mainplayerCurLogicPos + position * speed;

        _ControlRoleMove(m_MainPlayer, newLogicPos);
    }

    /// <summary>
    /// 控制主玩家移动
    /// </summary>
    /// <param name="actionDirection"></param>
    /// <param name="speed"></param>
    public void ControlMainPlayerMove(Vector3 logicPosition)
    {
        if (!m_ReceiveUserControl)
        {
            return;
        }

        TryInitMainPlayer();

        _ControlRoleMove(m_MainPlayer, logicPosition);
    }

    /// <summary>
    /// 控制角色移动
    /// </summary>
    /// <param name="role"></param>
    /// <param name="logicPosition"></param>
    private void _ControlRoleMove(RoleController role,Vector3 logicPosition)
    {
        Vector3 newLogicPos = logicPosition;
        PieceController newPieceController = PieceManager.GetPiece(newLogicPos);
        if (newPieceController == null)
        {
            return;
        }

        if (!newPieceController.GetEnableMove())
        {
            return;
        }

        Vector3 curLogicPos = role.GetLogicPosition();
        PieceController curPieceController = PieceManager.GetPiece(curLogicPos);
        if (curPieceController == null)
        {
            return;
        }

        Vector3 direction = newLogicPos - curLogicPos;
        if (!curPieceController.IsDirectionEnable(direction))
        {
            return;
        }

        Vector3 newArtPos = PieceManager.LogicPositionToArtPosition(newLogicPos);
        role.SetPosition(newLogicPos, newArtPos);

        //如果下一个块是上升或下降，添加下一个动作
        Vector3 upOrDown = Vector3.zero;
        bool isUpOrDown = false;
        if (newPieceController.IsDirectionEnable(Vector3.up) && PieceManager.HasPiece(newLogicPos + Vector3.up))
        {
            if (m_ReceiveUserControl || (!m_ReceiveUserControl && (newLogicPos + Vector3.up != m_BeforeLogicPos)))
            {
                upOrDown = Vector3.up;
                isUpOrDown = true;
            }
        }

        if (newPieceController.IsDirectionEnable(Vector3.down) && PieceManager.HasPiece(newLogicPos + Vector3.down))
        {
            if (m_ReceiveUserControl || (!m_ReceiveUserControl && (newLogicPos + Vector3.down != m_BeforeLogicPos)))
            {
                upOrDown = Vector3.down;
                isUpOrDown = true;
            }
        }

        if (isUpOrDown)
        {
            m_ReceiveUserControl = false;
            m_BeforeLogicPos = newLogicPos;

            Stone_TimerManager stone_TimerManager = Stone_RunTime.GetManager<Stone_TimerManager>(Stone_TimerManager.Name);

            if (m_NextActionTimer != 0)
            {
                stone_TimerManager.StopTimer(m_NextActionTimer);
            }

            m_NextActionTimer = stone_TimerManager.StarTimer(
           () =>
           {
               m_NextActionTimer = 0;
               _ControlRoleMove(role, newLogicPos + upOrDown);
           }, interval: NextWaitTime, updateTime: NextWaitTime);
        }
        else
        {
            m_BeforeLogicPos = Vector3.zero;
            m_ReceiveUserControl = true;
        }
    }


    public void SetEnableReceiveUserControl(bool enable)
    {
        m_ReceiveUserControl = enable;
    }

    public bool GetEnableReceiveUserControl()
    {
        return m_ReceiveUserControl;
    }

    private Vector3 ActionDirectionToVector3(ActionDirection actionDirection)
    {
        if(actionDirection == ActionDirection.Left)
        {
            return Vector3.left;
        }else if (actionDirection == ActionDirection.Right)
        {
            return Vector3.right;
        }
        else if (actionDirection == ActionDirection.Forward)
        {
            return Vector3.forward;
        }
        else if (actionDirection == ActionDirection.Back)
        {
            return Vector3.back;
        }
        else if (actionDirection == ActionDirection.Up)
        {
            return Vector3.up;
        }
        else if (actionDirection == ActionDirection.Down)
        {
            return Vector3.down;
        }
        return Vector3.zero;
    }
}
