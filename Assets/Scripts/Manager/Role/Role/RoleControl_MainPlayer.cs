using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleContole_MainPlayer : RoleControl_Player
{
    Stone_EventManager EventManager;

    protected override void _Init()
    {
        EventManager = Stone_RunTime.GetManager<Stone_EventManager>(Stone_EventManager.Name);

    }

    protected override void _UnInit()
    {

    }

    public override void SetPosition(Vector3 logicPosition, Vector3 artPosition)
    {
        GameEventDefine.MainPlayerMoveEventInfo mainPlayerMoveEventInfo = new GameEventDefine.MainPlayerMoveEventInfo();
        mainPlayerMoveEventInfo.OldLogicPosition = m_LogicPosition;
        mainPlayerMoveEventInfo.NewLogicPosition = logicPosition;
        mainPlayerMoveEventInfo.NewArtPosition = artPosition;

        m_LogicPosition = logicPosition;
        m_ArtPosition = artPosition;

        EventManager.Execute(GameEventDefine.MainPlayerMoveEvent, mainPlayerMoveEventInfo);
    }

}
