using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSceneObject
{
    protected int m_Id;
    protected Vector3 m_Position;

    public BaseSceneObject(int id,Vector3 position)
    {
        m_Id = id;
        m_Position = position;
    }

    public int GetId()
    {
        return m_Id;
    }

    public Vector3 GetPosition()
    {
        return m_Position;
    }
}
