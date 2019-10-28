using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSceneObject
{
    protected Vector3 m_Position;

    public BaseSceneObject(Vector3 position)
    {
        m_Position = position;
    }

    public Vector3 GetPosition()
    {
        return m_Position;
    }
}
