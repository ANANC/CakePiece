using System;
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
        public Vector3 LogicPosition;       //逻辑位置
        public Vector3 EnableDirection;     //可以到达的方向
        public Color Color;                 //块颜色
    }

    public class UserPieceArtInfo: Stone_BaseUserConfigData
    {
        public Vector3 OriginPosition;      //原点
        public float HorizontalInterval;    //水平间隔
        public float VerticalInterval;      //垂直间隔
    }

    private Dictionary<Vector3, PieceController> m_LogicPos2PieceControllerDict;    //【逻辑位置】对应【棋子管理器】列表 dict
    private List<PieceController> m_PieceControllerPool;                            //棋子池 list
    private List<GameObject> m_PieceGameObjectPool;                                 //棋子GameObject池 list

    private Dictionary<string,Func<PieceAction>> m_PieceActionName2CreateFuncDict;  //【行为名】对应【创建函数】列表 dict
    private List<string> m_DefaultPieceActionNameList;                              //默认行为名列表 list

    private UserPieceArtInfo m_UserPieceArtInfo;

    private ModelManager ModelManager;

    public PieceManager(Stone_IManagerLifeControl stone_ManagerLifeControl) : base(stone_ManagerLifeControl) { }


    public override void Init()
    {
        m_LogicPos2PieceControllerDict = new Dictionary<Vector3, PieceController>();
        m_PieceControllerPool = new List<PieceController>();
        m_PieceGameObjectPool = new List<GameObject>();

        m_PieceActionName2CreateFuncDict = new Dictionary<string, Func<PieceAction>>();
        m_DefaultPieceActionNameList = new List<string>();

        ModelManager = Stone_RunTime.GetManager<ModelManager>(ModelManager.Name);
    }

    public override void UnInit()
    {

    }

    public override void Active()
    {
        Stone_UserConfigManager userConfigManager = Stone_RunTime.GetManager<Stone_UserConfigManager>(Stone_UserConfigManager.Name);
        m_UserPieceArtInfo = userConfigManager.GetConfig<UserPieceArtInfo>();
    }

    public void AddAcitonCreateFunc(string actionName,Func<PieceAction> createFunc)
    {
        m_PieceActionName2CreateFuncDict.Add(actionName, createFunc);
    }

    public void AddDefaultPieceActionName(string actionName)
    {
        m_DefaultPieceActionNameList.Add(actionName);
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
        pieceController.SetEnableDirection(pieceInfo.EnableDirection);

        pieceController.SetColor(pieceInfo.Color);

        for (int index = 0; index < m_DefaultPieceActionNameList.Count; index++)
        {
            string actionName = m_DefaultPieceActionNameList[index];
            Func<PieceAction> createFunc = m_PieceActionName2CreateFuncDict[actionName];

            PieceAction pieceAction = createFunc();

            pieceAction.SetPieceController(pieceController);
            pieceController.AddAction(pieceAction);
        }
    }

    /// <summary>
    /// 获取或创建棋子管理器
    /// </summary>
    /// <returns></returns>
    private PieceController GetOrCreatePieceController()
    {
        PieceController pieceController;
        if (m_PieceControllerPool.Count != 0)
        {
            pieceController = m_PieceControllerPool[0];
            m_PieceControllerPool.RemoveAt(0);
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
        if (m_PieceGameObjectPool.Count != 0)
        {
            pieceGameObject = m_PieceGameObjectPool[0];
            m_PieceGameObjectPool.RemoveAt(0);
        }
        else
        {
            pieceGameObject = ModelManager.InstanceModel("100000_Piece", PieceManager.Name);
        }

        return pieceGameObject;
    }

    /// <summary>
    /// logic->art
    /// </summary>
    /// <param name="logicPosition"></param>
    /// <returns></returns>
    public Vector3 LogicPositionToArtPosition(Vector3 logicPosition)
    {
        Vector3 artPosition = m_UserPieceArtInfo.OriginPosition;
        artPosition.x += logicPosition.x * m_UserPieceArtInfo.HorizontalInterval;
        artPosition.z += logicPosition.z * m_UserPieceArtInfo.HorizontalInterval;
        artPosition.y += logicPosition.y * m_UserPieceArtInfo.VerticalInterval;

        return artPosition;
    }

    public bool HasPiece(Vector3 logicPosition)
    {
        return m_LogicPos2PieceControllerDict.ContainsKey(logicPosition);
    }

    public PieceController GetPiece(Vector3 logicPosition)
    {
        PieceController pieceController;
        if(m_LogicPos2PieceControllerDict.TryGetValue(logicPosition,out pieceController))
        {
            return pieceController;
        }

        return null;
    }

}
