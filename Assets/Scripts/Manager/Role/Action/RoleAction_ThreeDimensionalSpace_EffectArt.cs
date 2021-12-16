using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAction_ThreeDimensionalSpace_EffectArt : RoleAction
{
    public const string Name = "RoleAction_ThreeDimensionalSpace_EffectArt";

    public static RoleAction_ThreeDimensionalSpace_EffectArt CreateAction()
    {
        return new RoleAction_ThreeDimensionalSpace_EffectArt();
    }

    private Stone_EventManager EventManager;

    public override void Init()
    {
        EventManager = Stone_RunTime.GetManager<Stone_EventManager>(Stone_EventManager.Name);

        EventManager.AddListener<GameEventDefine.ThreeDimensionalSpace_PlayerArt_ShowEventInfo>(GameEventDefine.ThreeDimensionalSpace_PlayerArt_ShowEvent, this, ShowArt);
        EventManager.AddListener<GameEventDefine.ThreeDimensionalSpace_PlayerArt_HideEventInfo>(GameEventDefine.ThreeDimensionalSpace_PlayerArt_HideEvent, this, HideArt);
        EventManager.AddListener<GameEventDefine.ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEventInfo>(GameEventDefine.ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEvent, this, UpdateArtPosition);
    }

    public override void UnInit()
    {
        EventManager.DeleteTargetAllListener(this);
    }

    public void ShowArt(GameEventDefine.ThreeDimensionalSpace_PlayerArt_ShowEventInfo info)
    {
        int myId = m_RoleController.GetId();
        if (myId != info.PlayerId)
        {
            return;
        }

        GameObject gameObject = m_RoleController.GetGameObject();
        gameObject.SetActive(true);
    }

    public void HideArt(GameEventDefine.ThreeDimensionalSpace_PlayerArt_HideEventInfo info)
    {
        int myId = m_RoleController.GetId();
        if (myId != info.PlayerId)
        {
            return;
        }

        GameObject gameObject = m_RoleController.GetGameObject();
        gameObject.SetActive(false);
    }

    public void UpdateArtPosition(GameEventDefine.ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEventInfo info)
    {
        int myId = m_RoleController.GetId();
        if (myId != info.PlayerId)
        {
            return;
        }

        Vector3 artPosition = info.ArtPosition;
        m_RoleController.SetArtPosition(artPosition);
    }
}
