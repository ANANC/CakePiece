using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class TerrainMakerSceneController
{
    public class BaseCloneObject : ICloneable
    {
        public object Clone()
        {
            object obj = new BaseCloneObject();
            //字段
            FieldInfo[] fields = typeof(BaseCloneObject).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                field.SetValue(obj, field.GetValue(this));
            }
            //属性
            PropertyInfo[] properties = typeof(BaseCloneObject).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo property = properties[i];
                property.SetValue(obj, property.GetValue(this));
            }
            return obj;
        }
    }

    public class PathInfo : BaseCloneObject  //路径
    {
        public string TerrainPath;      //地形
        public string TerrainPiecePath; //地块
    }
    private PathInfo m_PathInfo;
    public PathInfo Path { get { return m_PathInfo; } }

    public class BuildingInfo : BaseCloneObject  //建筑
    {
        public Vector3 TerrainSize;     //地块大小
        public Vector3 IntervalSize;    //地块间隔大小
    }
    private BuildingInfo m_BuildingInfo;

    public class GamePlayInfo : BaseCloneObject  //玩法
    {
        public Vector3 BirthLogicPosition;  //出生逻辑位置
        public bool HasEndLogicPosition;    //是否有结束逻辑位置
        public Vector3 EndLoigcPosition;    //结束逻辑位置
    }
    private GamePlayInfo m_GamePlayInfo;

    public class TweenInfo : BaseCloneObject //动画
    {
        public float Originate;     //起始高度
        public float MoveSpeed;     //移动速度 （s）
    }
    private TweenInfo m_TweenInfo;

    public class ColorInfo: BaseCloneObject  //颜色
    {
        public Color Floor_Current; //当前站立层颜色
        public Color Floor_Other;   //非站立层颜色

        public Color Piece_Current; //当前站立地块颜色
        public Color Piece_Other;   //非站立地块颜色
        public Color Piece_End;     //终点地块颜色

        public Color Side_Current;  //当前站立地块的指向片颜色
        public Color Side_Other;    //非站立地块的指向片颜色
    }
    private ColorInfo m_ColorInfo;


    private GameObject m_RootTerrainGameObject; //地形根节点
    private GameObject m_ResourceTerrainPiece;  //地块资源GameObject
    private Dictionary<Vector3, GameObject> LogicPosition2TerrainPieceDict; //【逻辑位置】对应【地块gameobject】列表 dict

    private TerrainMakerTool m_Root;

    public void Init(TerrainMakerTool root)
    {
        m_Root = root;

    }

    public void UnInit()
    {

    }

    public void InitDefaultInfo()
    {
        TerrainMakerDefine.ToolSetting toolSetting = m_Root.TerrainMakerDefine.Setting;

        m_PathInfo = toolSetting.DefaultGameInfo.PathInfo.Clone() as PathInfo;
        m_BuildingInfo = toolSetting.DefaultGameInfo.BuildingInfo.Clone() as BuildingInfo;
        m_GamePlayInfo = toolSetting.DefaultGameInfo.GamePlayInfo.Clone() as GamePlayInfo;
        m_TweenInfo = toolSetting.DefaultGameInfo.TweenInfo.Clone() as TweenInfo;
        m_ColorInfo = toolSetting.DefaultGameInfo.ColorInfo.Clone() as ColorInfo;
    }

    public void InitBuild()
    {
        InitDefaultInfo();

        m_RootTerrainGameObject = AssetDatabase.LoadAssetAtPath<GameObject>(m_PathInfo.TerrainPath);
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
            m_ResourceTerrainPiece = AssetDatabase.LoadAssetAtPath<GameObject>(m_PathInfo.TerrainPiecePath); ;
        }

        GameObject terrainPiece = GameObject.Instantiate(m_ResourceTerrainPiece);
        terrainPiece.name = "x:" + logicPosition.x + " y:" + logicPosition.y + " z:" + logicPosition.z;

        Transform terrainPieceTransform = terrainPiece.transform;
        terrainPieceTransform.SetParent(m_RootTerrainGameObject.transform);

        Vector3 worldPosition = LogicPositionToWorldPosition(logicPosition);
        terrainPieceTransform.localPosition = worldPosition;

        terrainPieceTransform.localScale = m_BuildingInfo.TerrainSize;

        LogicPosition2TerrainPieceDict.Add(logicPosition, terrainPiece);
    }

    public void UpdateAllTerrain()
    {
        Dictionary<Vector3, GameObject>.Enumerator enumerator = LogicPosition2TerrainPieceDict.GetEnumerator();
        while(enumerator.MoveNext())
        {
            Vector3 logicPosition = enumerator.Current.Key;
            GameObject terrainPiece = enumerator.Current.Value;

            Transform terrainPieceTransform = terrainPiece.transform;

            Vector3 worldPosition = LogicPositionToWorldPosition(logicPosition);
            terrainPieceTransform.localPosition = worldPosition;
        }
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
