using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : Stone_Manager
{
    public const string Name = "PieceManager";

    public override string GetName()
    {
        return PieceManager.Name;
    }

    public class UserPieceInfo
    {
        public Vector3 LogicPosition;
        public Color Color;
    }

    public class UserPieceArtInfo: Stone_BaseUserConfigData
    {
        public Vector3 OriginPosition;      //原点
        public float HorizontalInterval;    //水平间隔
        public float VerticalInterval;      //垂直间隔
    }

    private GameObject m_RootGameObject;

    private Dictionary<Vector3, PieceController> m_LogicPos2PieceControllerDict;    //【逻辑位置】对应【棋子管理器】列表 dict
    private List<PieceController> m_DormancyPieceControllerList;                    //休眠棋子列表 list
    private List<GameObject> m_DormancyPieceGameObjectList;                         //休眠棋子GameObject列表 list

    private UserPieceArtInfo m_UserPieceArtInfo;

    private ModelManager ModelManager;

    public override void Init()
    {
        m_LogicPos2PieceControllerDict = new Dictionary<Vector3, PieceController>();
        m_DormancyPieceControllerList = new List<PieceController>();
        m_DormancyPieceGameObjectList = new List<GameObject>();

        ModelManager = Stone_RunTime.GetManager<ModelManager>(ModelManager.Name);
        m_RootGameObject = ModelManager.GetOrCreateNodeRoot(PieceManager.Name);
    }

    public override void UnInit()
    {
        GameObject.DestroyImmediate(m_RootGameObject);
    }

    public override void Active()
    {
        Stone_UserConfigManager userConfigManager = Stone_RunTime.GetManager<Stone_UserConfigManager>(Stone_UserConfigManager.Name);
        m_UserPieceArtInfo = userConfigManager.GetConfig<UserPieceArtInfo>();
    }

    /// <summary>
    /// 放置棋子
    /// </summary>
    public void LayPiece(UserPieceInfo pieceInfo)
    {
        if (m_LogicPos2PieceControllerDict.ContainsKey(pieceInfo.LogicPosition))
        {
            return;
        }

        PieceController pieceController = GetOrCreatePieceController();
        GameObject pieceGameObject = GetOrCreatePieceGameObject();

        m_LogicPos2PieceControllerDict.Add(pieceInfo.LogicPosition, pieceController);

        pieceController.Active(pieceGameObject);

        Vector3 logic = pieceInfo.LogicPosition;
        Vector3 art = LogicPositionToArtPosition(logic);
        pieceController.SetPosition(logic, art);

        pieceController.SetColor(pieceInfo.Color);
    }

    /// <summary>
    /// 获取或创建棋子管理器
    /// </summary>
    /// <returns></returns>
    private PieceController GetOrCreatePieceController()
    {
        PieceController pieceController;
        if (m_DormancyPieceControllerList.Count != 0)
        {
            pieceController = m_DormancyPieceControllerList[0];
            m_DormancyPieceControllerList.RemoveAt(0);
        }
        else
        {
            pieceController = new PieceController();
            pieceController.Init();
        }

        return pieceController;
    }

    /// <summary>
    /// 获取或创建棋子GameObject
    /// </summary>
    /// <returns></returns>
    private GameObject GetOrCreatePieceGameObject()
    {
        GameObject pieceGameObject;
        if (m_DormancyPieceGameObjectList.Count != 0)
        {
            pieceGameObject = m_DormancyPieceGameObjectList[0];
            m_DormancyPieceControllerList.RemoveAt(0);
        }
        else
        {
            pieceGameObject = ModelManager.InstanceModel("100000_Piece");
        }

        return pieceGameObject;
    }

    /// <summary>
    /// logic->art
    /// </summary>
    /// <param name="logicPosition"></param>
    /// <returns></returns>
    private Vector3 LogicPositionToArtPosition(Vector3 logicPosition)
    {
        Vector3 artPosition = m_UserPieceArtInfo.OriginPosition;
        artPosition.x += logicPosition.x * m_UserPieceArtInfo.HorizontalInterval;
        artPosition.z += logicPosition.z * m_UserPieceArtInfo.HorizontalInterval;
        artPosition.y += logicPosition.y * m_UserPieceArtInfo.VerticalInterval;

        return artPosition;
    }
}
