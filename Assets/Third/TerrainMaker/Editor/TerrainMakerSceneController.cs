using ANFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
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

    //-- 逻辑数据

    public class InputInfo
    {
        public int CurLogicPosition;   //当前逻辑位置
    }


    private GameObject m_RootTerrainGameObject; //地形根节点
    private GameObject m_ResourceTerrainPiece;  //地块资源GameObject
    private Dictionary<Vector3, TerrainPieceInfo> LogicPosition2TerrainPieceDict; //【逻辑位置】对应【地块数据】列表 dict

    private TerrainMakerTool m_Root;
    private EditorReousrceLoader m_EditorReousrceLoader;

    public void Init(TerrainMakerTool root)
    {
        m_Root = root;

        LogicPosition2TerrainPieceDict = new Dictionary<Vector3, TerrainPieceInfo>();

        m_EditorReousrceLoader = new EditorReousrceLoader();
        m_EditorReousrceLoader.Init();
    }

    public void UnInit()
    {
        if (m_RootTerrainGameObject != null)
        {
            GameObject.DestroyImmediate(m_RootTerrainGameObject);
        }

    }

    public void InitDefaultInfo()
    {
        TerrainMakerDefine.DefaultTerrainInfo defaultTerrainInfo = m_Root.Define.CurrentDefaultTerrainInfo;

        m_ResourcePathInfo = defaultTerrainInfo.ResourcePathInfo.Clone() as ResourcePathInfo;
        m_GameObjectPathInfo = defaultTerrainInfo.GameObjectPathInfo.Clone() as GameObjectPathInfo;
        m_BuildingInfo = defaultTerrainInfo.BuildingInfo.Clone() as BuildingInfo;
        m_GamePlayInfo = defaultTerrainInfo.GamePlayInfo.Clone() as GamePlayInfo;
        m_TweenInfo = defaultTerrainInfo.TweenInfo.Clone() as TweenInfo;
        m_ColorInfo = defaultTerrainInfo.ColorInfo.Clone() as ColorInfo;

        LogHelper.Trace?.Log("TerrainMakerTool", "InitDefaultInfo.",
            LogHelper.Object2String(m_ResourcePathInfo),
            LogHelper.Object2String(m_GameObjectPathInfo),
            LogHelper.Object2String(m_BuildingInfo),
            LogHelper.Object2String(m_GamePlayInfo),
            LogHelper.Object2String(m_TweenInfo),
            LogHelper.Object2String(m_ColorInfo)
            );
    }

    public void InitBuild()
    {
        InitDefaultInfo();

        m_RootTerrainGameObject = m_EditorReousrceLoader.LoadResource<GameObject>(m_ResourcePathInfo.TerrainPath);
        m_RootTerrainGameObject = GameObject.Instantiate(m_RootTerrainGameObject);
        m_RootTerrainGameObject.name = "Terrain";
    }

    public void BuildTerrain(Vector3 logicPosition)
    {
        if (LogicPosition2TerrainPieceDict.ContainsKey(logicPosition))
        {
            return;
        }

        if (m_ResourceTerrainPiece == null)
        {
            m_ResourceTerrainPiece = m_EditorReousrceLoader.LoadResource<GameObject>(m_ResourcePathInfo.TerrainPiecePath);
        }

        TerrainPieceInfo terrainPieceInfo = new TerrainPieceInfo();

        //logic
        terrainPieceInfo.LogixPosition = logicPosition;
        UpdateTerrainLogic(terrainPieceInfo);

        //art
        GameObject terrainPiece = GameObject.Instantiate(m_ResourceTerrainPiece);
        terrainPiece.name = "x:" + logicPosition.x + " y:" + logicPosition.y + " z:" + logicPosition.z;
        terrainPieceInfo.GameObject = terrainPiece;

        Transform terrainPieceTransform = terrainPiece.transform;
        terrainPieceTransform.SetParent(m_RootTerrainGameObject.transform);
        terrainPieceInfo.Transform = terrainPieceTransform;

        // -- 预制体内的对象
        Transform pieceTransform = terrainPieceTransform.Find(m_GameObjectPathInfo.PiecePath);
        Material pieceMaterial = pieceTransform.GetComponent<Renderer>().material;
        terrainPieceInfo.PieceMaterial = pieceMaterial;

        Transform sideRootTransform = terrainPieceTransform.Find(m_GameObjectPathInfo.SidePath);
        int childCount = sideRootTransform.childCount;
        Transform[] sides = new Transform[childCount];
        Material[] sideMaterials = new Material[childCount];
        for (int index = 0;index< sides.Length;index++)
        {
            Transform sideTransform = sideRootTransform.GetChild(index);
            sides[index] = sideTransform;
            sideMaterials[index] = sideTransform.GetComponent<Renderer>().material;
        }

        Transform upFlagTransform = terrainPieceTransform.Find(m_GameObjectPathInfo.UpPath);
        terrainPieceInfo.UpFlagTransform = upFlagTransform;

        Transform downFlagTransform = terrainPieceTransform.Find(m_GameObjectPathInfo.DownPath);
        terrainPieceInfo.DownFlagTransform = downFlagTransform;

        UpdateTerrainArt(terrainPieceInfo);

        LogicPosition2TerrainPieceDict.Add(logicPosition, terrainPieceInfo);
    }

    public void UpdateAllTerrain()
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
        Vector3 worldPosition = LogicPositionToWorldPosition(terrainPiece.LogixPosition);
        terrainPiece.WorldPosition = worldPosition;
    }

    private void UpdateTerrainArt(TerrainPieceInfo terrainPiece)
    {
        Transform terrainPieceTransform = terrainPiece.Transform;
        terrainPieceTransform.localPosition = terrainPiece.WorldPosition;
        terrainPieceTransform.localScale = m_BuildingInfo.TerrainSize;

        //todo:更新表现 颜色
    }

    public Vector3 LogicPositionToWorldPosition(Vector3 logicPosition)
    {
        Vector3 worldPosition = new Vector3(
            logicPosition.x * (m_BuildingInfo.TerrainSize.x + m_BuildingInfo.IntervalSize.x),
            logicPosition.y * -m_BuildingInfo.IntervalSize.y,
            logicPosition.z * (m_BuildingInfo.TerrainSize.z + m_BuildingInfo.IntervalSize.z)
            );

        return worldPosition;
    }

}
