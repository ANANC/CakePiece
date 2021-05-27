using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class TerrainMakerDefine
{
    private const string TerrainMakerDefineSettingPath = "../output/TerrainMakerDefine/TerrainMakerSetting.txt";       //编辑器配置文件路径

    public const string DefaultTerrainInfo_DefaultPath = "Config/Setting/Terrain/DefaultTerrainInfo.txt";   //默认地形配置文件 默认地址

    public class ToolSetting    //编辑器配置
    {
        public string DefaultTerrainInfoPath;   //【默认地形配置文件】路径 初始值：DefaultTerrainInfo_DefaultPath
    }
    private ToolSetting m_ToolSetting;

    public class DefaultTerrainInfo    //默认地形配置
    {
        public TerrainMakerSceneController.PathInfo PathInfo;
        public TerrainMakerSceneController.BuildingInfo BuildingInfo;
        public TerrainMakerSceneController.GamePlayInfo GamePlayInfo;
        public TerrainMakerSceneController.TweenInfo TweenInfo;
        public TerrainMakerSceneController.ColorInfo ColorInfo;
    }
    private DefaultTerrainInfo m_CurrentDefaultTerrainInfo;   //当前更新的配置内容
    private DefaultTerrainInfo m_RecordDefaultTerrainInfo;    //读取的配置内容，不做修改，用于还原
    public DefaultTerrainInfo CurrentDefaultTerrainInfo { get { return m_CurrentDefaultTerrainInfo; } }
    public DefaultTerrainInfo RecordDefaultTerrainInfo { get { return m_RecordDefaultTerrainInfo; } }

    private TerrainMakerTool m_Root;

    public void Init(TerrainMakerTool root)
    {
        m_Root = root;

        //更新编辑器配置
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


        //更新地形配置
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
    /// 创建默认配置
    /// </summary>
    private void __CreateDefaultTerrainInfo()
    {
        //路径
        if (m_CurrentDefaultTerrainInfo.PathInfo == null)
        {
            TerrainMakerSceneController.PathInfo pathInfo = new TerrainMakerSceneController.PathInfo();

            pathInfo.TerrainPath = "Prefab/Terrain";
            pathInfo.TerrainPiecePath = "Prefab/TerrainPiece";

            m_CurrentDefaultTerrainInfo.PathInfo = pathInfo;
        }

        //建筑
        if(m_CurrentDefaultTerrainInfo.BuildingInfo == null)
        {
            TerrainMakerSceneController.BuildingInfo buildingInfo = new TerrainMakerSceneController.BuildingInfo();
            //地块大小
            buildingInfo.TerrainSize = new Vector3();
            buildingInfo.TerrainSize.x = 4;
            buildingInfo.TerrainSize.y = 1;
            buildingInfo.TerrainSize.z = 4;

            //间隔
            buildingInfo.IntervalSize = new Vector3();
            buildingInfo.IntervalSize.x = 1;
            buildingInfo.IntervalSize.y = 1.6f;
            buildingInfo.IntervalSize.z = 1;

            m_CurrentDefaultTerrainInfo.BuildingInfo = buildingInfo;
        }

        //玩法
        if(m_CurrentDefaultTerrainInfo.GamePlayInfo == null)
        {
            TerrainMakerSceneController.GamePlayInfo gamePlayInfo = new TerrainMakerSceneController.GamePlayInfo();

            gamePlayInfo.BirthLogicPosition = Vector3.zero;
            gamePlayInfo.HasEndLogicPosition = false;
            gamePlayInfo.EndLoigcPosition = Vector3.zero;

            m_CurrentDefaultTerrainInfo.GamePlayInfo = gamePlayInfo;
        }

        //动画
        if(m_CurrentDefaultTerrainInfo.TweenInfo == null)
        {
            TerrainMakerSceneController.TweenInfo tweenInfo = new TerrainMakerSceneController.TweenInfo();

            tweenInfo.Originate = 8;
            tweenInfo.MoveSpeed = 1.8f;

            m_CurrentDefaultTerrainInfo.TweenInfo = tweenInfo;
        }

        //颜色
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
