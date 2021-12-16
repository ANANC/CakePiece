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

    public void SetColor(Color color)
    {
        m_Material.color = color;
    }

    public Vector3 GetLogicPosition()
    {
        return m_LogicPosition;
    }

    public Vector3 GetArtPosition()
    {
        return m_ArtPosition;
    }

    public GameObject GetGameObject()
    {
        return m_GameObject;
    }

    public Transform GetTransform()
    {
        return m_Transform;
    }
}
