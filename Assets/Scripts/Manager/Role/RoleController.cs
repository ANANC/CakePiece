using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleController
{
    protected GameObject m_GameObject;
    protected Transform m_Transform;

    protected Vector3 m_LogicPosition;
    protected Vector3 m_ArtPosition;

    public void Init()
    {
        _Init();
    }

    protected virtual void _Init(){ }

    public void UnInit()
    {
        _UnInit();

        m_GameObject = null;
        m_Transform = null;
    }

    protected virtual void _UnInit() { }

    public void SetGameObject(GameObject gameObject)
    {
        m_GameObject = gameObject;
        m_Transform = m_GameObject.transform;
    }

    public void SetPosition(Vector3 logicPosition,Vector3 artPosition)
    {
        m_LogicPosition = logicPosition;
        m_ArtPosition = artPosition;

        m_Transform.localPosition = m_ArtPosition;
    }

    public void SetArtPosition(Vector3 artPosition)
    {
        m_ArtPosition = artPosition;

        m_Transform.localPosition = m_ArtPosition;
    }

    public Vector3 GetLogicPosition()
    {
        return m_LogicPosition;
    }
}
