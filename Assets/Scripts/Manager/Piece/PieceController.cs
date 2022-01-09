using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEventDefine;

public class PieceController
{
    private const string MaterialPath = "Piece"; //材质球路径

    private PieceManager.UserPieceInfo m_UserPieceInfo;

    private GameObject m_GameObject;
    private Transform m_Transform;

    private Material m_Material;

    private Vector3 m_LogicPosition;
    private Vector3 m_ArtPosition;

    private int m_EnableDirectionsX;
    private int m_EnableDirectionsY;
    private int m_EnableDirectionsZ;

    private bool m_EnableMove;   //能否移动

    private List<PieceAction> m_ActionList;

    private Stone_EventManager EventManager;
    private PieceManager PieceManager;

    public void Init()
    {
        m_EnableMove = true;

        m_ActionList = new List<PieceAction>();

        EventManager = Stone_RunTime.GetManager<Stone_EventManager>(Stone_EventManager.Name);
        PieceManager = Stone_RunTime.GetManager<PieceManager>(PieceManager.Name);
    }

    public void UnInit()
    {
        for(int index = 0;index< m_ActionList.Count;index++)
        {
            PieceAction action = m_ActionList[index];
            action.UnInit();
            action.SetPieceController(null);
        }
        m_ActionList.Clear();

        if (m_GameObject != null)
        {
            PieceManager.UserPieceArtInfo userPieceArtInfo = PieceManager.GetUserPieceArtInfo();
            PieceManager.PushResourceGameObject(userPieceArtInfo.PiecePrefabPath, m_GameObject);
        }

        m_GameObject = null;
        m_Transform = null;
        m_Material = null;

        m_UserPieceInfo = null;

        EventManager = null;
        PieceManager = null;
    }

    public void Active(GameObject gameObject)
    {
        m_GameObject = gameObject;
        m_Transform = m_GameObject.transform;

        m_Material = m_Transform.Find(MaterialPath).GetComponent<MeshRenderer>().material;
    }

    public void Dormancy()
    {
        m_GameObject = null;
        m_Transform = null;
        m_Material = null;
    }

    public void SetUserPieceInfo(PieceManager.UserPieceInfo userPieceInfo)
    {
        m_UserPieceInfo = userPieceInfo;
    }

    public PieceManager.UserPieceInfo GetUserPieceInfo()
    {
        return m_UserPieceInfo;
    }

    public GameObject GetGameObject()
    {
        return m_GameObject;
    }

    public Transform GetTransform()
    {
        return m_Transform;
    }

    public void AddAction(PieceAction action)
    {
        action.SetPieceController(this);
        action.Init();

        m_ActionList.Add(action);
    }

    public void DeleteAction(PieceAction action)
    {
        if(!m_ActionList.Contains(action))
        {
            return;
        }

        action.UnInit();
        action.SetPieceController(null);

        m_ActionList.Remove(action);
    }

    public T GetPieceAction<T>() where T : PieceAction
    {
        T action = null;

        Type findType = typeof(T);
        Type actionType;
        for (int index = 0; index < m_ActionList.Count; index++)
        {
            actionType = m_ActionList[index].GetType();

            if (actionType == findType)
            {
                action = (T)m_ActionList[index];
                continue;
            }
        }
        return action;
    }

    public void SetPosition(Vector3 logicPosition, Vector3 artPosition)
    {
        m_LogicPosition = logicPosition;
        m_ArtPosition = artPosition;

        m_Transform.localPosition = m_ArtPosition;

#if UNITY_EDITOR
        m_GameObject.name = "x:" + m_LogicPosition.x + " z:" + m_LogicPosition.z + " y:" + m_LogicPosition.y;
#endif
    }

    public Vector3 GetLogicPosition()
    {
        return m_LogicPosition;
    }

    public Vector3 GetArtPosition()
    {
        return m_ArtPosition;
    }

    public void SetTexture(Texture texture)
    {
        m_Material.mainTexture = texture;
    }

    public Color GetColor()
    {
        return m_Material.color;
    }

    public void SetColor(Color color)
    {
        m_Material.color = color;
    }

    public void SetEnableDirection(Vector3 direction)
    {
        m_EnableDirectionsX = (int)direction.x;
        m_EnableDirectionsY = (int)direction.y;
        m_EnableDirectionsZ = (int)direction.z;
    }

    public void UpdateDirectionEnable(Vector3 direction, bool enable)
    {
        if (direction == Vector3.zero)
        {
            return;
        }

        bool curEnable = IsDirectionEnable(direction);
        if (curEnable == enable)
        {
            return;
        }

        if (enable)
        {
            AddEnableDirection(direction);
        }
        else
        {
            DeleteDirection(direction);
        }

        PieceEnableDirectionChangeEventInfo info = new PieceEnableDirectionChangeEventInfo();
        info.LogicPos = m_LogicPosition;
        EventManager.Execute(PieceEnableDirectionChangeEvent, info);
    }

    public void AddEnableDirection(Vector3 direction)
    {
        if (direction.x > 0)
        {
            if (m_EnableDirectionsX == 0)
            {
                m_EnableDirectionsX = 1;
            } else if (m_EnableDirectionsX == -1)
            {
                m_EnableDirectionsX = 2;
            }
        }
        if (direction.x < 0)
        {
            if (m_EnableDirectionsX == 0)
            {
                m_EnableDirectionsX = -1;
            }
            else if (m_EnableDirectionsX == 1)
            {
                m_EnableDirectionsX = 2;
            }
        }

        if (direction.z > 0)
        {
            if (m_EnableDirectionsZ == 0)
            {
                m_EnableDirectionsZ = 1;
            }
            else if (m_EnableDirectionsZ == -1)
            {
                m_EnableDirectionsZ = 2;
            }
        }
        if (direction.z < 0)
        {
            if (m_EnableDirectionsZ == 0)
            {
                m_EnableDirectionsZ = -1;
            }
            else if (m_EnableDirectionsZ == 1)
            {
                m_EnableDirectionsZ = 2;
            }
        }

        if (direction.y > 0)
        {
            m_EnableDirectionsY = 1;
        }
        else if (direction.y < 0)
        {
            m_EnableDirectionsY = -1;
        }

    }

    public void DeleteDirection(Vector3 direction)
    {
        if (direction.x > 0)
        {
            if (m_EnableDirectionsX == 1)
            {
                m_EnableDirectionsX = 0;
            }
            else if (m_EnableDirectionsX == 2)
            {
                m_EnableDirectionsX = -1;
            }
        }
        if (direction.x < 0)
        {
            if (m_EnableDirectionsX == -1)
            {
                m_EnableDirectionsX = 0;
            }
            else if (m_EnableDirectionsX == 2)
            {
                m_EnableDirectionsX = 1;
            }
        }

        if (direction.z > 0)
        {
            if (m_EnableDirectionsZ == 1)
            {
                m_EnableDirectionsZ = 0;
            }
            else if (m_EnableDirectionsZ == 2)
            {
                m_EnableDirectionsZ = -1;
            }
        }
        if (direction.z < 0)
        {
            if (m_EnableDirectionsZ == -1)
            {
                m_EnableDirectionsZ = 0;
            }
            else if (m_EnableDirectionsZ == 2)
            {
                m_EnableDirectionsZ = 1;
            }
        }


        if (direction.y != 0)
        {
            m_EnableDirectionsY = 0;
        }
    }


    public bool IsDirectionEnable(Vector3 direction)
    {
        bool enableMove = true;

        if (direction.x > 0)
        {
            if(m_EnableDirectionsX != 2 && m_EnableDirectionsX != 1)
            {
                enableMove = false;
            }
        }
        else if (direction.x < 0)
        {
            if (m_EnableDirectionsX != 2 && m_EnableDirectionsX != -1)
            {
                enableMove = false;
            }
        }

        if (direction.z > 0)
        {
            if (m_EnableDirectionsZ != 2 && m_EnableDirectionsZ != 1)
            {
                enableMove = false;
            }
        }
        else if (direction.z < 0)
        {
            if (m_EnableDirectionsZ != 2 && m_EnableDirectionsZ != -1)
            {
                enableMove = false;
            }
        }

        if (direction.y > 0)
        {
            if (m_EnableDirectionsY != 1)
            {
                enableMove = false;
            }
        }
        else if (direction.y < 0)
        {
            if (m_EnableDirectionsY != -1)
            {
                enableMove = false;
            }
        }

        return enableMove;
    }


    public void SetEnableMove( bool enableMove)
    {
        m_EnableMove = enableMove;
    }

    public bool GetEnableMove()
    {
        return m_EnableMove;
    }

}
