using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class TerrainMakerDefine
{
    private const string TerrainMakerDefineSettingPath = "../output/TerrainMakerDefine/TerrainMakerSetting.txt";       //�༭�������ļ�·��

    public const string DefaultTerrainInfo_DefaultPath = "Config/Setting/Terrain/DefaultTerrainInfo.txt";   //Ĭ�ϵ��������ļ� Ĭ�ϵ�ַ

    public class ToolSetting    //�༭������
    {
        public string DefaultTerrainInfoPath;   //��Ĭ�ϵ��������ļ���·�� ��ʼֵ��DefaultTerrainInfo_DefaultPath
    }
    private ToolSetting m_ToolSetting;

    public class DefaultTerrainInfo    //Ĭ�ϵ�������
    {
        public TerrainMakerSceneController.PathInfo PathInfo;
        public TerrainMakerSceneController.BuildingInfo BuildingInfo;
        public TerrainMakerSceneController.GamePlayInfo GamePlayInfo;
        public TerrainMakerSceneController.TweenInfo TweenInfo;
        public TerrainMakerSceneController.ColorInfo ColorInfo;
    }
    private DefaultTerrainInfo m_CurrentDefaultTerrainInfo;   //��ǰ���µ���������
    private DefaultTerrainInfo m_RecordDefaultTerrainInfo;    //��ȡ���������ݣ������޸ģ����ڻ�ԭ
    public DefaultTerrainInfo CurrentDefaultTerrainInfo { get { return m_CurrentDefaultTerrainInfo; } }
    public DefaultTerrainInfo RecordDefaultTerrainInfo { get { return m_RecordDefaultTerrainInfo; } }

    private TerrainMakerTool m_Root;

    public void Init(TerrainMakerTool root)
    {
        m_Root = root;

        //���±༭������
        string totalTerrainMakerDefineSettingPath = Application.dataPath + "/" + TerrainMakerDefineSettingPath;
        LogHelper.Trace?.Log("TerrainMakerTool", "totalTerrainMakerDefineSettingPath", totalTerrainMakerDefineSettingPath);
        if (File.Exists(totalTerrainMakerDefineSettingPath))
        {
            string defineSettingStr = File.ReadAllText(totalTerrainMakerDefineSettingPath);
            m_ToolSetting = LitJson.JsonMapper.ToObject<ToolSetting>(defineSettingStr);
        }
        else
        {
            m_ToolSetting = new ToolSetting();
        }
        __CreateToolConfig();

        LogHelper.Trace?.Log("TerrainMakerTool", "m_ToolSetting", LogHelper.Object2String(m_ToolSetting));


        //���µ�������
        string totalDefaultTerrainInfoPath = Application.dataPath + "/" + m_ToolSetting.DefaultTerrainInfoPath;
        LogHelper.Trace?.Log("TerrainMakerTool", "totalDefaultTerrainInfoPath", totalDefaultTerrainInfoPath);
        if (File.Exists(totalDefaultTerrainInfoPath))
        {
            string defaultTerrainInfo = File.ReadAllText(totalDefaultTerrainInfoPath);
            m_CurrentDefaultTerrainInfo = LitJson.JsonMapper.ToObject<DefaultTerrainInfo>(defaultTerrainInfo);
        }
        else
        {
            m_CurrentDefaultTerrainInfo = new DefaultTerrainInfo();
        }
        __CreateDefaultTerrainInfo();
        m_RecordDefaultTerrainInfo = CloneHelper.DeepClone(m_CurrentDefaultTerrainInfo) as DefaultTerrainInfo;

        LogHelper.Trace?.Log("TerrainMakerTool", "m_RecordDefaultTerrainInfo", LogHelper.Object2String(m_RecordDefaultTerrainInfo));

    }

    public void UnInit()
    {

    }

    #region init set

    private void __CreateToolConfig()
    {
        if(string.IsNullOrEmpty(m_ToolSetting.DefaultTerrainInfoPath))
        {
            m_ToolSetting.DefaultTerrainInfoPath = DefaultTerrainInfo_DefaultPath;
        }
    }


    /// <summary>
    /// ����Ĭ������
    /// </summary>
    private void __CreateDefaultTerrainInfo()
    {
        //·��
        if (m_CurrentDefaultTerrainInfo.PathInfo == null)
        {
            TerrainMakerSceneController.PathInfo pathInfo = new TerrainMakerSceneController.PathInfo();

            pathInfo.TerrainPath = "Prefab/Terrain";
            pathInfo.TerrainPiecePath = "Prefab/TerrainPiece";

            m_CurrentDefaultTerrainInfo.PathInfo = pathInfo;
        }

        //����
        if(m_CurrentDefaultTerrainInfo.BuildingInfo == null)
        {
            TerrainMakerSceneController.BuildingInfo buildingInfo = new TerrainMakerSceneController.BuildingInfo();
            //�ؿ��С
            buildingInfo.TerrainSize = new Vector3();
            buildingInfo.TerrainSize.x = 4;
            buildingInfo.TerrainSize.y = 1;
            buildingInfo.TerrainSize.z = 4;

            //���
            buildingInfo.IntervalSize = new Vector3();
            buildingInfo.IntervalSize.x = 1;
            buildingInfo.IntervalSize.y = 1.6f;
            buildingInfo.IntervalSize.z = 1;

            m_CurrentDefaultTerrainInfo.BuildingInfo = buildingInfo;
        }

        //�淨
        if(m_CurrentDefaultTerrainInfo.GamePlayInfo == null)
        {
            TerrainMakerSceneController.GamePlayInfo gamePlayInfo = new TerrainMakerSceneController.GamePlayInfo();

            gamePlayInfo.BirthLogicPosition = Vector3.zero;
            gamePlayInfo.HasEndLogicPosition = false;
            gamePlayInfo.EndLoigcPosition = Vector3.zero;

            m_CurrentDefaultTerrainInfo.GamePlayInfo = gamePlayInfo;
        }

        //����
        if(m_CurrentDefaultTerrainInfo.TweenInfo == null)
        {
            TerrainMakerSceneController.TweenInfo tweenInfo = new TerrainMakerSceneController.TweenInfo();

            tweenInfo.Originate = 8;
            tweenInfo.MoveSpeed = 1.8f;

            m_CurrentDefaultTerrainInfo.TweenInfo = tweenInfo;
        }

        //��ɫ
        if(m_CurrentDefaultTerrainInfo.ColorInfo == null)
        {
            TerrainMakerSceneController.ColorInfo colorInfo = new TerrainMakerSceneController.ColorInfo();

            colorInfo.Floor_Current = new Color(1, 1, 1, 1);
            colorInfo.Floor_Other = new Color(0.8f, 0.8f, 0.8f, 0.29f);

            colorInfo.Piece_Current = new Color(1, 1, 1, 1);
            colorInfo.Piece_Other = new Color(0.75f, 0.78f, 0.79f, 1);
            colorInfo.Piece_End = new Color(0.82f, 0.3f, 0.3f, 1);

            colorInfo.Side_Current = new Color(0.36f, 0.33f, 0.3f, 1);
            colorInfo.Side_Other = new Color(0.37f, 0.38f, 0.35f, 1);

            m_CurrentDefaultTerrainInfo.ColorInfo = colorInfo;
        }

    }

    #endregion

    #region record

    public void WriteDefaultTerrainInfoFile()
    {
        string defaultTerrainInfoJson = LitJson.JsonMapper.ToJson(m_CurrentDefaultTerrainInfo);

        string totalDirectoryPath = Application.dataPath + "/" + m_ToolSetting.DefaultTerrainInfoPath;

        string directoryPath = IOHelper.GetDirectoryPath(totalDirectoryPath);
        IOHelper.SafeCreateDirectory(directoryPath);

        File.WriteAllText(totalDirectoryPath, defaultTerrainInfoJson);
    }

    public void WriteToolSettingFile()
    {
        string toolSettingJson = LitJson.JsonMapper.ToJson(m_ToolSetting);

        string totalDirectoryPath = Application.dataPath + "/" + TerrainMakerDefineSettingPath;

        string directoryPath = IOHelper.GetDirectoryPath(totalDirectoryPath);
        IOHelper.SafeCreateDirectory(directoryPath);

        File.WriteAllText(totalDirectoryPath, toolSettingJson);
    }

    #endregion

}
