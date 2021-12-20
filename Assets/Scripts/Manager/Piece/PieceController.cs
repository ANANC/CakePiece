using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController
{
    private const string MaterialPath = ""; //材质球路径

    private GameObject m_GameObject;
    private Transform m_Transform;

    private Material m_Material;

    private Vector3 m_LogicPosition;
    private Vector3 m_ArtPosition;

    private int m_EnableDirectionsX;
    private int m_EnableDirectionsY;
    private int m_EnableDirectionsZ;

    private List<PieceAction> m_ActionList;


    public void Init()
    {
        m_ActionList = new List<PieceAction>();
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


}
