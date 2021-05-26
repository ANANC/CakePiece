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
            //�ֶ�
            FieldInfo[] fields = typeof(BaseCloneObject).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                field.SetValue(obj, field.GetValue(this));
            }
            //����
            PropertyInfo[] properties = typeof(BaseCloneObject).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo property = properties[i];
                property.SetValue(obj, property.GetValue(this));
            }
            return obj;
        }
    }

    public class PathInfo : BaseCloneObject  //·��
    {
        public string TerrainPath;      //����
        public string TerrainPiecePath; //�ؿ�
    }
    private PathInfo m_PathInfo;
    public PathInfo Path { get { return m_PathInfo; } }

    public class BuildingInfo : BaseCloneObject  //����
    {
        public Vector3 TerrainSize;     //�ؿ��С
        public Vector3 IntervalSize;    //�ؿ�����С
    }
    private BuildingInfo m_BuildingInfo;

    public class GamePlayInfo : BaseCloneObject  //�淨
    {
        public Vector3 BirthLogicPosition;  //�����߼�λ��
        public bool HasEndLogicPosition;    //�Ƿ��н����߼�λ��
        public Vector3 EndLoigcPosition;    //�����߼�λ��
    }
    private GamePlayInfo m_GamePlayInfo;

    public class TweenInfo : BaseCloneObject //����
    {
        public float Originate;     //��ʼ�߶�
        public float MoveSpeed;     //�ƶ��ٶ� ��s��
    }
    private TweenInfo m_TweenInfo;

    public class ColorInfo: BaseCloneObject  //��ɫ
    {
        public Color Floor_Current; //��ǰվ������ɫ
        public Color Floor_Other;   //��վ������ɫ

        public Color Piece_Current; //��ǰվ���ؿ���ɫ
        public Color Piece_Other;   //��վ���ؿ���ɫ
        public Color Piece_End;     //�յ�ؿ���ɫ

        public Color Side_Current;  //��ǰվ���ؿ��ָ��Ƭ��ɫ
        public Color Side_Other;    //��վ���ؿ��ָ��Ƭ��ɫ
    }
    private ColorInfo m_ColorInfo;


    private GameObject m_RootTerrainGameObject; //���θ��ڵ�
    private GameObject m_ResourceTerrainPiece;  //�ؿ���ԴGameObject
    private Dictionary<Vector3, GameObject> LogicPosition2TerrainPieceDict; //���߼�λ�á���Ӧ���ؿ�gameobject���б� dict

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
