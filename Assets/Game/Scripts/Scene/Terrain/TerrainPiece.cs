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
        Not,    //不可移动
        Flat,   //平面
        Up,     //上升
        Down,   //下沉
    }

    /// <summary>
    /// 空间类型
    /// </summary>
    public enum Space
    {
        Once,   //单一
        Loop,   //循环
    }

    /// <summary>
    ///     ↗z
    ///    /         
    ///   /-----------/→x
    ///  /    |      /
    /// /-----|-----/
    ///       |
    ///      ↓ y
    /// </summary>
    protected Vector3 m_SpacePosition;
    protected Direction m_Direction;
    protected Space m_Space;

    public TerrainPiece(int id, Vector3 spacePosition, Vector3 position, Direction direction, Space space) : base(id,position)
    {
        m_SpacePosition = spacePosition;
        m_Direction = direction;
        m_Space = space;
    }

    public Vector3 GetSpacePosition()
    {
        return m_SpacePosition;
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
