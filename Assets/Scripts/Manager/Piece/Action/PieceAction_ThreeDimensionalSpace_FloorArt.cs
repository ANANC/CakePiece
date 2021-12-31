using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceAction_ThreeDimensionalSpace_FloorArt : PieceAction
{
    public const string Name = "PieceAction_ThreeDimensionalSpace_FloorArt";
    public static PieceAction_ThreeDimensionalSpace_FloorArt CreateAction()
    {
        return new PieceAction_ThreeDimensionalSpace_FloorArt();
    }

    private Dictionary<string, Sequence> m_SequenceDict;

    private SpriteRenderer m_DirectionUpSpriteRenderer;
    private SpriteRenderer m_DirectionDownSpriteRenderer;

    private Stone_EventManager EventManager;

    public override void Init()
    {
        m_SequenceDict = new Dictionary<string, Sequence>();

        EventManager = Stone_RunTime.GetManager<Stone_EventManager>(Stone_EventManager.Name);

        EventManager.AddListener<GameEventDefine.ThreeDimensionalSpace_FloorArt_StandInPieceEventInfo>(GameEventDefine.ThreeDimensionalSpace_FloorArt_StandInPieceEvent, this, StandInPiece);
        EventManager.AddListener<GameEventDefine.ThreeDimensionalSpace_FloorArt_StandOutPieceEventInfo>(GameEventDefine.ThreeDimensionalSpace_FloorArt_StandOutPieceEvent, this, StandOutPiece);
        EventManager.AddListener<GameEventDefine.ThreeDimensionalSpace_FloorArt_UpFloorEventInfo>(GameEventDefine.ThreeDimensionalSpace_FloorArt_UpFloorEvent, this, UpFloor);
        EventManager.AddListener<GameEventDefine.ThreeDimensionalSpace_FloorArt_DownFloorEventInfo>(GameEventDefine.ThreeDimensionalSpace_FloorArt_DownFloorEvent, this, DownFloor);
        EventManager.AddListener<GameEventDefine.ThreeDimensionalSpace_FloorArt_ShowFloorEventInfo>(GameEventDefine.ThreeDimensionalSpace_FloorArt_ShowFloorEvent, this, ShowFloor);
        EventManager.AddListener<GameEventDefine.ThreeDimensionalSpace_FloorArt_HideFloorEventInfo>(GameEventDefine.ThreeDimensionalSpace_FloorArt_HideFloorEvent, this, HideFloor);
        EventManager.AddListener<GameEventDefine.ThreeDimensionalSpace_FloorArt_ResetArtPositionFromLogicEventInfo>(GameEventDefine.ThreeDimensionalSpace_FloorArt_ResetArtPositionFromLogicEvent, this, ResetArtPositionFromLogic);

        InitFloorArt();
    }

    public override void UnInit()
    {
        EventManager.DeleteTargetAllListener(this);
    }

    private void InitFloorArt()
    {
        Vector3 myLogicPosition = m_PieceController.GetLogicPosition();
        int myFloor = (int)myLogicPosition.y;

        TerrainManager terrainManager = Stone_RunTime.GetManager<TerrainManager>(TerrainManager.Name);
        int curFloor = terrainManager.GetCurFloor();

        GameObject gameObject = m_PieceController.GetGameObject();
        gameObject.SetActive(curFloor == myFloor);

        PieceManager pieceManager = Stone_RunTime.GetManager<PieceManager>(PieceManager.Name);
        PieceManager.UserPieceArtInfo userPieceArtInfo = pieceManager.GetUserPieceArtInfo();
        if (m_PieceController.IsDirectionEnable(Vector3.up))
        {
            GameObject up = pieceManager.GetOrCreateResourceGameObject(userPieceArtInfo.PieceDirectionUpPath);
            Transform upTransform = up.transform;
            upTransform.SetParent(gameObject.transform);
            upTransform.localPosition = Vector3.zero;
            upTransform.localRotation = Quaternion.identity;
            upTransform.localScale = Vector3.one;

            m_DirectionUpSpriteRenderer = up.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        }
        if (m_PieceController.IsDirectionEnable(Vector3.down))
        {
            GameObject down = pieceManager.GetOrCreateResourceGameObject(userPieceArtInfo.PieceDirectionDownPath);
            down.transform.SetParent(gameObject.transform);
            Transform downTransform = down.transform;
            downTransform.localPosition = Vector3.zero;
            downTransform.localRotation = Quaternion.identity;
            downTransform.localScale = Vector3.one;

            m_DirectionDownSpriteRenderer = down.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        }
    }

    public void StandInPiece(GameEventDefine.ThreeDimensionalSpace_FloorArt_StandInPieceEventInfo info)
    {
        Vector3 myLogicPosition = m_PieceController.GetLogicPosition();
        if (myLogicPosition != info.LogicPos)
        {
            return;
        }

        float newAlpha = 0.0f;

        if(m_DirectionUpSpriteRenderer != null)
        {
            Color color = m_DirectionUpSpriteRenderer.color;
            color.a = newAlpha;
            m_DirectionUpSpriteRenderer.color = color;
        }
        if (m_DirectionDownSpriteRenderer != null)
        {
            Color color = m_DirectionDownSpriteRenderer.color;
            color.a = newAlpha;
            m_DirectionDownSpriteRenderer.color = color;
        }
    }

    public void StandOutPiece(GameEventDefine.ThreeDimensionalSpace_FloorArt_StandOutPieceEventInfo info)
    {
        Vector3 myLogicPosition = m_PieceController.GetLogicPosition();
        if (myLogicPosition != info.LogicPos)
        {
            return;
        }

        float newAlpha = 0.6f;

        if (m_DirectionUpSpriteRenderer != null)
        {
            Color color = m_DirectionUpSpriteRenderer.color;
            color.a = newAlpha;
            m_DirectionUpSpriteRenderer.color = color;
        }
        if (m_DirectionDownSpriteRenderer != null)
        {
            Color color = m_DirectionDownSpriteRenderer.color;
            color.a = newAlpha;
            m_DirectionDownSpriteRenderer.color = color;
        }
    }

    /// <summary>
    /// 上升特效
    /// </summary>
    /// <param name="info"></param>
    public void UpFloor(GameEventDefine.ThreeDimensionalSpace_FloorArt_UpFloorEventInfo info)
    {
        Vector3 myLogicPosition = m_PieceController.GetLogicPosition();
        if ((int)myLogicPosition.y != info.UpFloor)
        {
            return;
        }

        string sequenceName = "UpFloor";

        Transform transform = m_PieceController.GetTransform();
        GameObject gameObject = m_PieceController.GetGameObject();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(transform.position + Vector3.up * info.TweenMoveLength, info.TweenUpdateTime));
        sequence.AppendCallback(() => { gameObject.SetActive(false); });

        m_SequenceDict.AddSequenceByOnlyRun(sequenceName, sequence);
    }

    /// <summary>
    /// 下降特效
    /// </summary>
    /// <param name="info"></param>
    public void DownFloor(GameEventDefine.ThreeDimensionalSpace_FloorArt_DownFloorEventInfo info)
    {
        Vector3 myLogicPosition = m_PieceController.GetLogicPosition();
        if ((int)myLogicPosition.y != info.DownFloor)
        {
            return;
        }

        string sequenceName = "DownFloor";

        Transform transform = m_PieceController.GetTransform();
        GameObject gameObject = m_PieceController.GetGameObject();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(transform.position + Vector3.down * info.TweenMoveLength, info.TweenUpdateTime));
        sequence.AppendCallback(() => { gameObject.SetActive(false); });

        m_SequenceDict.AddSequenceByOnlyRun(sequenceName, sequence);
    }

    /// <summary>
    /// 显示特效
    /// </summary>
    /// <param name="info"></param>
    public void ShowFloor(GameEventDefine.ThreeDimensionalSpace_FloorArt_ShowFloorEventInfo info)
    {
        Vector3 myLogicPosition = m_PieceController.GetLogicPosition();
        if ((int)myLogicPosition.y != info.Floor)
        {
            return;
        }

        m_SequenceDict.StopAllSequence();

        GameObject gameObject = m_PieceController.GetGameObject();
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 隐藏特效
    /// </summary>
    /// <param name="info"></param>
    public void HideFloor(GameEventDefine.ThreeDimensionalSpace_FloorArt_HideFloorEventInfo info)
    {
        Vector3 myLogicPosition = m_PieceController.GetLogicPosition();
        if ((int)myLogicPosition.y != info.Floor)
        {
            return;
        }

        m_SequenceDict.StopAllSequence();

        GameObject gameObject = m_PieceController.GetGameObject();
        gameObject.SetActive(false);
    }

    public void ResetArtPositionFromLogic(GameEventDefine.ThreeDimensionalSpace_FloorArt_ResetArtPositionFromLogicEventInfo info)
    {
        Vector3 myLogicPosition = m_PieceController.GetLogicPosition();
        if ((int)myLogicPosition.y != info.Floor)
        {
            return;
        }

        m_SequenceDict.StopAllSequence();

        Vector3 artPosition = m_PieceController.GetArtPosition();

        Transform transform = m_PieceController.GetTransform();
        transform.localPosition = artPosition;
    }

}
