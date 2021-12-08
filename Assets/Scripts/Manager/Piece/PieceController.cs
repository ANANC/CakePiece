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



    public void Init()
    {

    }

    public void UnInit()
    {

    }

    public void Active(GameObject gameObject)
    {
        m_GameObject = gameObject;
        m_Transform = m_GameObject.transform;

        m_Material = m_Transform.Find(MaterialPath).GetComponent<MeshRenderer>().material;

        m_GameObject.SetActive(true);
    }

    public void Dormancy()
    {
        m_GameObject.SetActive(false);

        m_GameObject = null;
        m_Transform = null;
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
}
