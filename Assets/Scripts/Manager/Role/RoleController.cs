using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleController
{
    protected int m_Id;

    protected GameObject m_GameObject;
    protected Transform m_Transform;

    protected Vector3 m_LogicPosition;
    protected Vector3 m_ArtPosition;

    protected List<RoleAction> m_ActionList;

    public void Init()
    {
        m_ActionList = new List<RoleAction>();

        _Init();
    }

    protected virtual void _Init(){ }

    public void UnInit()
    {
        _UnInit();

        for(int index = 0;index< m_ActionList.Count;index++)
        {
            m_ActionList[index].UnInit();
        }
        m_ActionList.Clear();

        m_GameObject = null;
        m_Transform = null;
    }

    protected virtual void _UnInit() { }

    public void SetId(int id)
    {
        m_Id = id;
    }

    public int GetId()
    {
        return m_Id;
    }

    public void SetGameObject(GameObject gameObject)
    {
        m_GameObject = gameObject;
        m_Transform = m_GameObject.transform;
    }

    public GameObject GetGameObject()
    {
        return m_GameObject;
    }

    public Transform GetTransform()
    {
        return m_Transform;
    }

    public virtual void SetPosition(Vector3 logicPosition,Vector3 artPosition)
    {
        m_LogicPosition = logicPosition;
        m_ArtPosition = artPosition;

        m_Transform.localPosition = m_ArtPosition;
    }

    public Vector3 GetLogicPosition()
    {
        return m_LogicPosition;
    }

    public Vector3 GetArtPosition()
    {
        return m_ArtPosition;
    }

    public void SetArtPosition(Vector3 artPosition)
    {
        m_ArtPosition = artPosition;

        m_Transform.localPosition = m_ArtPosition;
    }

    public void AddAction(RoleAction action)
    {
        action.Init();

        m_ActionList.Add(action);
    }

    public void DeleteAction(RoleAction action)
    {
        action.UnInit();

        m_ActionList.Remove(action);
    }


}
