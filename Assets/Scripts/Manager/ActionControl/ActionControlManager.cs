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

    private RoleController m_MainPlayer;

    private Stone_EventManager EventManager;
    private PieceManager PieceManager;

    public override void Init()
    {
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

    public void ControlMainPlayerMove(ActionDirection actionDirection,int speed)
    {
        TryInitMainPlayer();

        Vector3 mainplayerCurLogicPos = m_MainPlayer.GetLogicPosition();
        Vector3 position = ActionDirectionToVector3(actionDirection);
        Vector3 newLogicPos = mainplayerCurLogicPos + position * speed;

        if(!PieceManager.HasPiece(newLogicPos))
        {
            return;
        }

        GameEventDefine.MainPlayerMoveEventInfo mainPlayerMoveEventInfo = new GameEventDefine.MainPlayerMoveEventInfo();
        mainPlayerMoveEventInfo.OldLogicPosition = m_MainPlayer.GetLogicPosition();
        mainPlayerMoveEventInfo.NewLogicPosition = newLogicPos;

        Vector3 newArtPos = PieceManager.LogicPositionToArtPosition(newLogicPos);
        m_MainPlayer.SetPosition(newLogicPos, newArtPos);
    }

    public void ControlMainPlayerMove(Vector3 logicPosition)
    {
        TryInitMainPlayer();

        Vector3 newLogicPos = logicPosition;

        if (!PieceManager.HasPiece(newLogicPos))
        {
            return;
        }

        GameEventDefine.MainPlayerMoveEventInfo mainPlayerMoveEventInfo = new GameEventDefine.MainPlayerMoveEventInfo();
        mainPlayerMoveEventInfo.OldLogicPosition = m_MainPlayer.GetLogicPosition();
        mainPlayerMoveEventInfo.NewLogicPosition = newLogicPos;

        Vector3 newArtPos = PieceManager.LogicPositionToArtPosition(newLogicPos);
        m_MainPlayer.SetPosition(newLogicPos, newArtPos);
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
