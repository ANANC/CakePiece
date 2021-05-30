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
    private EditorReousrceLoader m_EditorReousrceLoader;

    public void Init(TerrainMakerEditorWindow root)
    {
        m_Root = root;

        LogicPosition2TerrainPieceDict = new Dictionary<Vector3, TerrainPieceInfo>();

        m_InputInfo = new InputInfo();

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

        LogHelper.Trace?.Log("TerrainMakerTool", "InitDefaultInfo.",
            LogHelper.Object2String(m_ResourcePathInfo),
            LogHelper.Object2String(m_GameObjectPathInfo),
            LogHelper.Object2String(m_BuildingInfo),
            LogHelper.Object2String(m_GamePlayInfo),
            LogHelper.Object2String(m_TweenInfo),
            LogHelper.Object2String(m_ColorInfo)
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
        InitDefaultInfo();
        InitCurInputInfo();

        GameObject oldRoot = GameObject.Find("Terrain");
        if(oldRoot!=null)
        {
            GameObject.DestroyImmediate(oldRoot);
        }

        m_RootTerrainGameObject = m_EditorReousrceLoader.LoadResource<GameObject>(m_ResourcePathInfo.TerrainPath);
        m_RootTerrainGameObject = GameObject.Instantiate(m_RootTerrainGameObject);
        m_RootTerrainGameObject.name = "Terrain";
    }

    public void BuildTerrainPiece(Vector3 logicPosition)
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

        UpdatePieceSidePosition(terrainPiece);
    }

    private void UpdatePieceSidePosition(TerrainPieceInfo terrainPiece)
    {
        int index = 0;
        Dictionary<TerrainPieceDirection, bool>.Enumerator enumerator = terrainPiece.DirectionFlagDict.GetEnumerator();
        while(enumerator.MoveNext())
        {
            TerrainPieceDirection direction = enumerator.Current.Key;
            bool enable = enumerator.Current.Value;

            Transform sideTransform = terrainPiece.SideTransforms[index];
            GameObject sideGameobject = sideTransform.gameObject;

            sideGameobject.SetActive(enable);

            if (enable)
            {
                Vector3 v3Direction = m_Root.Tool.Enum2Vector3Direction[direction];
                Vector3 sidePosition = v3Direction * m_BuildingInfo.SideShiftingValue;
                sideTransform.localPosition = sidePosition;
            }
        }
        for(;index< terrainPiece.SideTransforms.Length;index++)
        {
            terrainPiece.SideTransforms[index].gameObject.SetActive(false);
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

}
