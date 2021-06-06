using ANFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using static TerrainMakerData;

public class TerrainMakerSceneController
{    
    //-- 配置数据

    private ResourcePathInfo m_ResourcePathInfo;
    public ResourcePathInfo ResourcePath { get { return m_ResourcePathInfo; } }

    private GameObjectPathInfo m_GameObjectPathInfo;
    public GameObjectPathInfo GameObjectPath { get { return m_GameObjectPathInfo; } }

    private BuildingInfo m_BuildingInfo;
    public BuildingInfo Building { get { return m_BuildingInfo; } }

    private GamePlayInfo m_GamePlayInfo;
    public GamePlayInfo GamePlay { get { return m_GamePlayInfo; } }

    private TweenInfo m_TweenInfo;
    public TweenInfo Tween { get { return m_TweenInfo; } }

    private ColorInfo m_ColorInfo;
    public ColorInfo Color { get { return m_ColorInfo; } }

    private SceneArtInfo m_SceneArtInfo;
    public SceneArtInfo SceneArt { get { return m_SceneArtInfo; } }

    //-- 逻辑数据

    public class InputInfo  //输入数据
    {
        public Vector3 CurLogicPosition;   //当前逻辑位置
        public Vector3 EndLogicPosition;   //最终逻辑位置
    }
    private InputInfo m_InputInfo;
    public InputInfo Input { get { return m_InputInfo; } }

    private GameObject m_RootTerrainGameObject; //地形根节点
    private Dictionary<Vector3, TerrainPieceInfo> LogicPosition2TerrainPieceDict; //【逻辑位置】对应【地块数据】列表 dict

    private TerrainMakerEditorWindow m_Root;

    private bool m_IsBuiling;
    public bool IsBuiling { get { return m_IsBuiling; } }

    public void Init(TerrainMakerEditorWindow root)
    {
        m_Root = root;

        LogicPosition2TerrainPieceDict = new Dictionary<Vector3, TerrainPieceInfo>();

        m_InputInfo = new InputInfo();

        m_IsBuiling = false;
    }

    public void UnInit()
    {
        if (m_RootTerrainGameObject != null)
        {
            GameObject.DestroyImmediate(m_RootTerrainGameObject);
        }

    }

    /// <summary>
    /// 初始化默认配置
    /// </summary>
    public void InitDefaultInfo()
    {
        TerrainMakerDefine.DefaultTerrainInfo defaultTerrainInfo = m_Root.Define.CurrentDefaultTerrainInfo;

        m_ResourcePathInfo = defaultTerrainInfo.ResourcePathInfo.Clone() as ResourcePathInfo;
        m_GameObjectPathInfo = defaultTerrainInfo.GameObjectPathInfo.Clone() as GameObjectPathInfo;
        m_BuildingInfo = defaultTerrainInfo.BuildingInfo.Clone() as BuildingInfo;
        m_GamePlayInfo = defaultTerrainInfo.GamePlayInfo.Clone() as GamePlayInfo;
        m_TweenInfo = defaultTerrainInfo.TweenInfo.Clone() as TweenInfo;
        m_ColorInfo = defaultTerrainInfo.ColorInfo.Clone() as ColorInfo;
        m_SceneArtInfo = defaultTerrainInfo.SceneArtInfo.Clone() as SceneArtInfo;

        LogHelper.Trace?.Log("TerrainMakerTool", "InitDefaultInfo.",
            LogHelper.Object2String(m_ResourcePathInfo),
            LogHelper.Object2String(m_GameObjectPathInfo),
            LogHelper.Object2String(m_BuildingInfo),
            LogHelper.Object2String(m_GamePlayInfo),
            LogHelper.Object2String(m_TweenInfo),
            LogHelper.Object2String(m_ColorInfo),
            LogHelper.Object2String(m_SceneArtInfo)
            );
    }

    /// <summary>
    /// 初始化当前配置
    /// </summary>
    private void InitCurInputInfo()
    {
        m_InputInfo.CurLogicPosition = m_GamePlayInfo.BirthLogicPosition;
        if(m_GamePlayInfo.HasEndLogicPosition)
        {
            m_InputInfo.EndLogicPosition = m_GamePlayInfo.EndLoigcPosition;
        }
        else
        {
            m_InputInfo.EndLogicPosition = new Vector3(-1, -1, -1);
        }
    }

    public void InitBuild()
    {
        m_IsBuiling = true;

        InitDefaultInfo();
        InitCurInputInfo();

        GameObject oldRoot = GameObject.Find("Terrain");
        if(oldRoot!=null)
        {
            GameObject.DestroyImmediate(oldRoot);
        }

        m_RootTerrainGameObject = m_Root.Tool.LoadResource<GameObject>(m_ResourcePathInfo.TerrainPath);
        m_RootTerrainGameObject = GameObject.Instantiate(m_RootTerrainGameObject);
        m_RootTerrainGameObject.name = "Terrain";
    }

    #region 地块

    public void CreateTerrainPiece(Vector3 logicPosition)
    {
        if (LogicPosition2TerrainPieceDict.ContainsKey(logicPosition))
        {
            return;
        }

        TerrainPieceInfo terrainPieceInfo = m_Root.PieceFactory.CreateBasisTerrainPiece(logicPosition);
        UpdateTerrainLogic(terrainPieceInfo);
        UpdateTerrainArt(terrainPieceInfo);

        terrainPieceInfo.Transform.SetParent(m_RootTerrainGameObject.transform);

        LogicPosition2TerrainPieceDict.Add(logicPosition, terrainPieceInfo);
    }

    public void UpdateCurrentTerrainPieceArt()
    {
        TerrainPieceInfo current = GetCurrentTerrainPieceInfo();
        if(current!=null)
        {
            UpdateTerrainArt(current);
        }
    }

    public void UpdateTerrain()
    {
        Dictionary<Vector3, TerrainPieceInfo>.Enumerator enumerator = LogicPosition2TerrainPieceDict.GetEnumerator();
        while(enumerator.MoveNext())
        {
            TerrainPieceInfo terrainPieceInfo =  enumerator.Current.Value;

            UpdateTerrainLogic(terrainPieceInfo);
            UpdateTerrainArt(terrainPieceInfo);
        }
    }

    private void UpdateTerrainLogic(TerrainPieceInfo terrainPiece)
    {
        Vector3 worldPosition = m_Root.GamePlay.LogicPositionToWorldPosition(terrainPiece.LogicPosition);
        terrainPiece.WorldPosition = worldPosition;
    }

    private void UpdateTerrainArt(TerrainPieceInfo terrainPiece)
    {
        Transform terrainPieceTransform = terrainPiece.Transform;
        terrainPieceTransform.position = terrainPiece.WorldPosition;
        terrainPieceTransform.localScale = m_BuildingInfo.TerrainSize;

        Material pieceMaterial = terrainPiece.PieceMaterial;
        pieceMaterial.color = m_Root.GamePlay.LogicPositionToDynamicColor(terrainPiece);

        int pieceShaderLayer = m_Root.GamePlay.GetFloorShaderTargerLayer((int)terrainPiece.LogicPosition.y, FloorShaderLayer.Piece);
        pieceMaterial.renderQueue = pieceShaderLayer;

        UpdatePieceSidePosition(terrainPiece);

        UpdatePieceBuilding(terrainPiece);
    }

    private void UpdatePieceSidePosition(TerrainPieceInfo terrainPiece)
    {
        int index = 0;
        Dictionary<TerrainPieceDirection, bool>.Enumerator enumerator = terrainPiece.DirectionFlagDict.GetEnumerator();
        while(enumerator.MoveNext())
        {
            TerrainPieceDirection direction = enumerator.Current.Key;
            bool enable = enumerator.Current.Value;

            if (direction == TerrainPieceDirection.Up)
            {
                terrainPiece.UpFlagTransform.gameObject.SetActive(enable);
            }
            else if (direction == TerrainPieceDirection.Down)
            {
                terrainPiece.DownFlagTransform.gameObject.SetActive(enable);
            }
            else
            {
                Transform sideTransform = terrainPiece.SideTransforms[index];
                GameObject sideGameobject = sideTransform.gameObject;

                sideGameobject.SetActive(enable);

                if (enable)
                {
                    Vector3 v3Direction = m_Root.Tool.Enum2Vector3Direction[direction];
                    Vector3 sidePosition = v3Direction * m_BuildingInfo.SideShiftingValue;
                    sideTransform.localPosition = sidePosition;

                    Material material = terrainPiece.SideMaterials[index];
                    int shaderLayer = m_Root.GamePlay.GetFloorShaderTargerLayer((int)terrainPiece.LogicPosition.y, FloorShaderLayer.PieceSide);
                    material.renderQueue = shaderLayer;
                }

                index += 1;
            }
        }
    }

    private void UpdatePieceBuilding(TerrainPieceInfo terrainPiece)
    {
        TerrainPieceInfo floorTerrainPieceInfo = GetCurrentTerrainPieceInfo();
        bool isInFloor = false;
        if(floorTerrainPieceInfo !=null)
        {
            isInFloor = terrainPiece.LogicPosition.y == floorTerrainPieceInfo.LogicPosition.y;
        }

        Dictionary<GameObject, TerrainPieceBuildingInfo>.Enumerator enumerator = terrainPiece.ArtInfo.BuildingDict.GetEnumerator();
        while(enumerator.MoveNext())
        {
            GameObject buildingGameObject = enumerator.Current.Key;
            buildingGameObject.SetActive(isInFloor);
        }
    }

    /// <summary>
    /// 更新地块可以移动的方向
    /// </summary>
    /// <param name="terrainPiece"></param>
    public void ChangeTerrainPieceEnableDirection(TerrainPieceInfo terrainPiece, TerrainPieceDirection terrainPieceDirection, bool enable)
    {
        Dictionary<TerrainPieceDirection, bool> directionFlagDict = terrainPiece.DirectionFlagDict;
        Dictionary<TerrainPieceDirection, bool> updateDict = new Dictionary<TerrainPieceDirection, bool>();

        // 可以兼容的方向
        Dictionary<TerrainPieceDirection, List<TerrainPieceDirection>> terrainPieceDirectionCompatibleDict = m_Root.GamePlay.GetTerrainPieceDirectionCompatibleDict();
        List<TerrainPieceDirection> compatibleDirectionList = terrainPieceDirectionCompatibleDict[terrainPieceDirection];

        for (int index = 0; index < compatibleDirectionList.Count; index++)
        {
            TerrainPieceDirection compatibleDirection = compatibleDirectionList[index];
            updateDict.Add(compatibleDirection, directionFlagDict[compatibleDirection]);
        }

        updateDict[terrainPieceDirection] = enable;

        //不能兼容的方向
        Dictionary<TerrainPieceDirection, bool>.Enumerator enumerator = directionFlagDict.GetEnumerator();
        while (enumerator.MoveNext())
        {
            TerrainPieceDirection curTerrainPieceDirection = enumerator.Current.Key;
            if (!updateDict.ContainsKey(curTerrainPieceDirection))
            {
                if(enable)
                {
                    updateDict.Add(curTerrainPieceDirection, false);
                }
                else
                {
                    updateDict.Add(curTerrainPieceDirection, directionFlagDict[curTerrainPieceDirection]);
                }
            }
        }

        //更新
        enumerator = updateDict.GetEnumerator();
        while (enumerator.MoveNext())
        {
            TerrainPieceDirection curTerrainPieceDirection = enumerator.Current.Key;
            directionFlagDict[curTerrainPieceDirection] = enumerator.Current.Value;
        }
    }


    /// <summary>
    /// 得到当前站立的地块信息
    /// </summary>
    /// <returns></returns>
    public TerrainPieceInfo GetCurrentTerrainPieceInfo()
    {
        TerrainPieceInfo terrainPieceInfo;
        if (LogicPosition2TerrainPieceDict.TryGetValue(m_InputInfo.CurLogicPosition,out terrainPieceInfo))
        {
            return terrainPieceInfo;
        }
        return null;
    }

    public bool HasTerrainPieceInfo(Vector3 logicPosition)
    {
        return LogicPosition2TerrainPieceDict.ContainsKey(logicPosition);
    }

    #endregion

    #region 建筑

    public void CreateBuilding(string filePath)
    {
        TerrainPieceInfo terrainPieceInfo = GetCurrentTerrainPieceInfo();

        GameObject buildingResource = m_Root.Tool.LoadResource<GameObject>(filePath);
        GameObject buildingGameObject = GameObject.Instantiate(buildingResource);
        Transform buildingTransfrom = buildingGameObject.transform;

        buildingTransfrom.SetParent(terrainPieceInfo.BuildingRootTransform);
        buildingTransfrom.localPosition = Vector3.zero;

        TerrainPieceBuildingInfo terrainPieceBuildingInfo = new TerrainPieceBuildingInfo();
        terrainPieceBuildingInfo.ResourcePath = filePath;

        terrainPieceInfo.ArtInfo.BuildingDict.Add(buildingGameObject, terrainPieceBuildingInfo);
    }

    #endregion
}
