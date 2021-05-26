using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TerrainMakerDefine
{
    private const string TerrainMakerDefineSettingPath = "../output/TerrainMakerDefine/TerrainMakerSetting.txt";       //编辑器配置文件路径

    public class ToolSetting
    {
        public string DefaultTerrainInfoPath;   //【默认地形配置文件】路径

        public DefaultGameInfo DefaultGameInfo;     //默认【游戏配置】
    }
    private ToolSetting m_ToolSetting;

    public class DefaultGameInfo    //默认配置
    {
        public TerrainMakerSceneController.PathInfo PathInfo;
        public TerrainMakerSceneController.BuildingInfo BuildingInfo;
        public TerrainMakerSceneController.GamePlayInfo GamePlayInfo;
        public TerrainMakerSceneController.TweenInfo TweenInfo;
        public TerrainMakerSceneController.ColorInfo ColorInfo;
    }

    public const string DefaultTerrainInfo_DefaultPath = "Config/Setting/Terrain";   //默认地形配置文件 默认地址


    private TerrainMakerTool m_Root;

    public ToolSetting Setting
    {
        get { return m_ToolSetting; }
    }


    public void Init(TerrainMakerTool root)
    {
        m_Root = root;

        //更新默认配置
        if (File.Exists(TerrainMakerDefineSettingPath))
        {
            string defineSettingStr = File.ReadAllText(TerrainMakerDefineSettingPath);
            m_ToolSetting = LitJson.JsonMapper.ToObject<ToolSetting>(defineSettingStr);
        }
        else
        {
            m_ToolSetting = new ToolSetting();
        }

        __CreateToolConfig();
        __CreateDefaultGameInfo();


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
    private void __CreateDefaultGameInfo()
    {
        //路径
        if (m_ToolSetting.DefaultGameInfo.PathInfo == null)
        {
            TerrainMakerSceneController.PathInfo pathInfo = new TerrainMakerSceneController.PathInfo();

            pathInfo.TerrainPath = "Prefab/Terrain";
            pathInfo.TerrainPiecePath = "Prefab/TerrainPiece";

            m_ToolSetting.DefaultGameInfo.PathInfo = pathInfo;
        }

        //建筑
        if(m_ToolSetting.DefaultGameInfo.BuildingInfo == null)
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

            m_ToolSetting.DefaultGameInfo.BuildingInfo = buildingInfo;
        }

        //玩法
        if(m_ToolSetting.DefaultGameInfo.GamePlayInfo == null)
        {
            TerrainMakerSceneController.GamePlayInfo gamePlayInfo = new TerrainMakerSceneController.GamePlayInfo();

            gamePlayInfo.BirthLogicPosition = Vector3.zero;
            gamePlayInfo.HasEndLogicPosition = false;
            gamePlayInfo.EndLoigcPosition = Vector3.zero;

            m_ToolSetting.DefaultGameInfo.GamePlayInfo = gamePlayInfo;
        }

        //动画
        if(m_ToolSetting.DefaultGameInfo.TweenInfo == null)
        {
            TerrainMakerSceneController.TweenInfo tweenInfo = new TerrainMakerSceneController.TweenInfo();

            tweenInfo.Originate = 8;
            tweenInfo.MoveSpeed = 1.8f;

            m_ToolSetting.DefaultGameInfo.TweenInfo = tweenInfo;
        }

        //颜色
        if(m_ToolSetting.DefaultGameInfo.ColorInfo == null)
        {
            TerrainMakerSceneController.ColorInfo colorInfo = new TerrainMakerSceneController.ColorInfo();

            colorInfo.Floor_Current = new Color(1, 1, 1, 1);
            colorInfo.Floor_Other = new Color(0.8f, 0.8f, 0.8f, 0.29f);

            colorInfo.Piece_Current = new Color(1, 1, 1, 1);
            colorInfo.Piece_Other = new Color(0.75f, 0.78f, 0.79f, 1);
            colorInfo.Piece_End = new Color(0.82f, 0.3f, 0.3f, 1);

            colorInfo.Side_Current = new Color(0.36f, 0.33f, 0.3f, 1);
            colorInfo.Side_Other = new Color(0.37f, 0.38f, 0.35f, 1);

            m_ToolSetting.DefaultGameInfo.ColorInfo = colorInfo;
        }

    }

    #endregion

    #region set

    #endregion

}
