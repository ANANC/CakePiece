using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEventDefine;

public class PieceAction_ThreeDimensionalSpace_FloorArt : PieceAction
{
    public const string Name = "PieceAction_ThreeDimensionalSpace_FloorArt";
    public static PieceAction_ThreeDimensionalSpace_FloorArt CreateAction()
    {
        return new PieceAction_ThreeDimensionalSpace_FloorArt();
    }

    public class ThreeDimensionalSpaceArtInfo : Stone_BaseUserConfigData
    {
        public string PieceDirectionUpPath;     //块方向上        资源路径
        public string PieceDirectionDownPath;   //块方向下        资源路径
        public string PieceObstructLeftPath;    //块拦截方向左    资源路径
        public string PieceObstructRightPath;   //块拦截方向右    资源路径
        public string PieceObstructForwardPath; //块拦截方向前    资源路径
        public string PieceObstructBackPath;    //块拦截方向后    资源路径
    }

    private ThreeDimensionalSpaceArtInfo m_UserPieceArtInfo;

    private Dictionary<string, Sequence> m_SequenceDict;

    private SpriteRenderer m_DirectionUpSpriteRenderer;
    private SpriteRenderer m_DirectionDownSpriteRenderer;
    private SpriteRenderer m_ObstructLeftSpriteRenderer;
    private SpriteRenderer m_ObstructRightSpriteRenderer;
    private SpriteRenderer m_ObstructForwardSpriteRenderer;
    private SpriteRenderer m_ObstructBackSpriteRenderer;

    private PieceManager PieceManager;
    private Stone_EventManager EventManager;

    public override void Init()
    {
        m_SequenceDict = new Dictionary<string, Sequence>();

        EventManager = Stone_RunTime.GetManager<Stone_EventManager>(Stone_EventManager.Name);
        PieceManager = Stone_RunTime.GetManager<PieceManager>(PieceManager.Name);

        Stone_UserConfigManager userConfigManager = Stone_RunTime.GetManager<Stone_UserConfigManager>(Stone_UserConfigManager.Name);
        m_UserPieceArtInfo = userConfigManager.GetConfig<ThreeDimensionalSpaceArtInfo>();

        EventManager.AddListener<PieceCreateEventInfo>(PieceCreateEvent, this, PieceCreateEventListener);
        EventManager.AddListener<PieceDestroyEventInfo>(PieceDestroyEvent, this, PieceDestroyEventListener);
        EventManager.AddListener<PieceEnableDirectionChangeEventInfo>(PieceEnableDirectionChangeEvent, this, PieceEnableDirectionChangeEventListener);
        EventManager.AddListener<ThreeDimensionalSpace_FloorArt_StandInPieceEventInfo>(ThreeDimensionalSpace_FloorArt_StandInPieceEvent, this, StandInPiece);
        EventManager.AddListener<ThreeDimensionalSpace_FloorArt_StandOutPieceEventInfo>(ThreeDimensionalSpace_FloorArt_StandOutPieceEvent, this, StandOutPiece);
        EventManager.AddListener<ThreeDimensionalSpace_FloorArt_UpFloorEventInfo>(ThreeDimensionalSpace_FloorArt_UpFloorEvent, this, UpFloor);
        EventManager.AddListener<ThreeDimensionalSpace_FloorArt_DownFloorEventInfo>(ThreeDimensionalSpace_FloorArt_DownFloorEvent, this, DownFloor);
        EventManager.AddListener<ThreeDimensionalSpace_FloorArt_ShowFloorEventInfo>(ThreeDimensionalSpace_FloorArt_ShowFloorEvent, this, ShowFloor);
        EventManager.AddListener<ThreeDimensionalSpace_FloorArt_HideFloorEventInfo>(ThreeDimensionalSpace_FloorArt_HideFloorEvent, this, HideFloor);
        EventManager.AddListener<ThreeDimensionalSpace_FloorArt_ResetArtPositionFromLogicEventInfo>(ThreeDimensionalSpace_FloorArt_ResetArtPositionFromLogicEvent, this, ResetArtPositionFromLogic);

        InitFloorArt();
    }

    public override void UnInit()
    {
        EventManager.DeleteTargetAllListener(this);
        m_SequenceDict.StopAllSequence();

        UnInitSpriteRenderer(m_UserPieceArtInfo.PieceDirectionUpPath,ref m_DirectionUpSpriteRenderer);
        UnInitSpriteRenderer(m_UserPieceArtInfo.PieceDirectionDownPath, ref m_DirectionDownSpriteRenderer);
        UnInitSpriteRenderer(m_UserPieceArtInfo.PieceObstructLeftPath, ref m_ObstructLeftSpriteRenderer);
        UnInitSpriteRenderer(m_UserPieceArtInfo.PieceObstructRightPath, ref m_ObstructRightSpriteRenderer);
        UnInitSpriteRenderer(m_UserPieceArtInfo.PieceObstructForwardPath, ref m_ObstructForwardSpriteRenderer);
        UnInitSpriteRenderer(m_UserPieceArtInfo.PieceObstructBackPath, ref m_ObstructBackSpriteRenderer);

        PieceManager = null;
        EventManager = null;
    }

    private void InitFloorArt()
    {
        Vector3 myLogicPosition = m_PieceController.GetLogicPosition();
        int myFloor = (int)myLogicPosition.y;

        TerrainManager terrainManager = Stone_RunTime.GetManager<TerrainManager>(TerrainManager.Name);
        int curFloor = terrainManager.GetCurFloor();

        GameObject gameObject = m_PieceController.GetGameObject();
        gameObject.SetActive(curFloor == myFloor);
        Transform transform = gameObject.transform;

        InitSpriteRenderer(Vector3.up, true, m_UserPieceArtInfo.PieceDirectionUpPath, transform, ref m_DirectionUpSpriteRenderer);
        InitSpriteRenderer(Vector3.down, true, m_UserPieceArtInfo.PieceDirectionDownPath, transform, ref m_DirectionDownSpriteRenderer);
        InitSpriteRenderer(Vector3.left, false, m_UserPieceArtInfo.PieceObstructLeftPath, transform, ref m_ObstructLeftSpriteRenderer);
        InitSpriteRenderer(Vector3.right, false, m_UserPieceArtInfo.PieceObstructRightPath, transform, ref m_ObstructRightSpriteRenderer);
        InitSpriteRenderer(Vector3.forward, false, m_UserPieceArtInfo.PieceObstructForwardPath, transform, ref m_ObstructForwardSpriteRenderer);
        InitSpriteRenderer(Vector3.back, false, m_UserPieceArtInfo.PieceObstructBackPath, transform, ref m_ObstructBackSpriteRenderer);
    }

    private void InitSpriteRenderer(Vector3 direction,bool enable,string resourcePath,Transform parent,ref SpriteRenderer spriteRenderer)
    {
        bool curEnable = m_PieceController.IsDirectionEnable(direction) == enable;
        if(direction == Vector3.up || direction == Vector3.down)
        {
            Vector3 nextLogicPos = m_PieceController.GetLogicPosition() + direction;
            curEnable = PieceManager.HasPiece(nextLogicPos) && curEnable;
        }

        if (curEnable)
        {
            GameObject artGameObject = PieceManager.GetOrCreateResourceGameObject(resourcePath);
            artGameObject.transform.SetParent(parent);
            Transform artTransform = artGameObject.transform;
            artTransform.localPosition = Vector3.zero;
            artTransform.localRotation = Quaternion.identity;
            artTransform.localScale = Vector3.one;

            spriteRenderer = artTransform.Find("Sprite").GetComponent<SpriteRenderer>();

            artGameObject.SetActive(true);
        }
        else
        {
            spriteRenderer = null;
        }
    }

    private void UnInitSpriteRenderer(string resourcePath, ref SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer != null)
        {
            //tip:对应find的路径
            PieceManager.PushResourceGameObject(resourcePath, spriteRenderer.transform.parent.gameObject);
        }
        spriteRenderer = null;
    }

    private void PieceCreateEventListener(PieceCreateEventInfo info)
    {
        Vector3 curLogicPos = m_PieceController.GetLogicPosition();
        if (info.LogicPos == curLogicPos + Vector3.up || info.LogicPos == curLogicPos + Vector3.down)
        {
            GameObject gameObject = m_PieceController.GetGameObject();
            Transform transform = gameObject.transform;

            ResetSpriteRenderer(Vector3.up, true, m_UserPieceArtInfo.PieceDirectionUpPath, transform, ref m_DirectionUpSpriteRenderer);
            ResetSpriteRenderer(Vector3.down, true, m_UserPieceArtInfo.PieceDirectionDownPath, transform, ref m_DirectionDownSpriteRenderer);
        }
    }

    private void PieceDestroyEventListener(PieceDestroyEventInfo info)
    {
        Vector3 curLogicPos = m_PieceController.GetLogicPosition();
        if (info.LogicPos == curLogicPos + Vector3.up || info.LogicPos == curLogicPos + Vector3.down)
        {
            GameObject gameObject = m_PieceController.GetGameObject();
            Transform transform = gameObject.transform;

            ResetSpriteRenderer(Vector3.up, true, m_UserPieceArtInfo.PieceDirectionUpPath, transform, ref m_DirectionUpSpriteRenderer);
            ResetSpriteRenderer(Vector3.down, true, m_UserPieceArtInfo.PieceDirectionDownPath, transform, ref m_DirectionDownSpriteRenderer);
        }
    }

    private void PieceEnableDirectionChangeEventListener(PieceEnableDirectionChangeEventInfo info)
    {
        Vector3 curLogicPos = m_PieceController.GetLogicPosition();
        if(curLogicPos != info.LogicPos)
        {
            return;
        }

        GameObject gameObject = m_PieceController.GetGameObject();
        Transform transform = gameObject.transform;

        ResetSpriteRenderer(Vector3.up, true, m_UserPieceArtInfo.PieceDirectionUpPath, transform, ref m_DirectionUpSpriteRenderer);
        ResetSpriteRenderer(Vector3.down, true, m_UserPieceArtInfo.PieceDirectionDownPath, transform, ref m_DirectionDownSpriteRenderer);
        ResetSpriteRenderer(Vector3.left, false, m_UserPieceArtInfo.PieceObstructLeftPath, transform, ref m_ObstructLeftSpriteRenderer);
        ResetSpriteRenderer(Vector3.right, false, m_UserPieceArtInfo.PieceObstructRightPath, transform, ref m_ObstructRightSpriteRenderer);
        ResetSpriteRenderer(Vector3.forward, false, m_UserPieceArtInfo.PieceObstructForwardPath, transform, ref m_ObstructForwardSpriteRenderer);
        ResetSpriteRenderer(Vector3.back, false, m_UserPieceArtInfo.PieceObstructBackPath, transform, ref m_ObstructBackSpriteRenderer);
    }


    private void ResetSpriteRenderer(Vector3 direction, bool enable, string resourcePath, Transform parent, ref SpriteRenderer spriteRenderer)
    {
        bool curEnable = m_PieceController.IsDirectionEnable(direction) == enable;
        if (direction == Vector3.up || direction == Vector3.down)
        {
            Vector3 nextLogicPos = m_PieceController.GetLogicPosition() + direction;
            curEnable = PieceManager.HasPiece(nextLogicPos) && curEnable;
        }

        if (curEnable)
        {
            if (spriteRenderer == null)
            {
                InitSpriteRenderer(direction, enable, resourcePath, parent, ref spriteRenderer);
            }
        }
        else
        {
            if (spriteRenderer != null)
            {
                UnInitSpriteRenderer(resourcePath, ref spriteRenderer);
            }
        }
    }



    public void StandInPiece(ThreeDimensionalSpace_FloorArt_StandInPieceEventInfo info)
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

    public void StandOutPiece(ThreeDimensionalSpace_FloorArt_StandOutPieceEventInfo info)
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
    public void UpFloor(ThreeDimensionalSpace_FloorArt_UpFloorEventInfo info)
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
    public void DownFloor(ThreeDimensionalSpace_FloorArt_DownFloorEventInfo info)
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
    public void ShowFloor(ThreeDimensionalSpace_FloorArt_ShowFloorEventInfo info)
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
    public void HideFloor(ThreeDimensionalSpace_FloorArt_HideFloorEventInfo info)
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

    public void ResetArtPositionFromLogic(ThreeDimensionalSpace_FloorArt_ResetArtPositionFromLogicEventInfo info)
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
