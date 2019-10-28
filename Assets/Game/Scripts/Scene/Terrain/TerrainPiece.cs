using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPiece : BaseSceneObject
{
    /// <summary>
    /// 方向类型
    /// </summary>
    public enum Direction
    {
        Flat,   //平面
        Up,     //上升
        Down,   //下沉
        Not,    //不可移动
    }

    /// <summary>
    /// 空间类型
    /// </summary>
    public enum Space
    {
        Once,   //单一
        Loop,   //循环
    }

    protected Direction m_Direction;
    protected Space m_Space;

    public TerrainPiece(Vector3 position, Direction direction, Space space) : base(position)
    {
        m_Direction = direction;
        m_Space = space;
    }

    public Direction GetDirection()
    {
        return m_Direction;
    }

    public Space GetSpace()
    {
        return m_Space;
    }

}
