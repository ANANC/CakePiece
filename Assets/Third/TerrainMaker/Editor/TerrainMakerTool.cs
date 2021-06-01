using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TerrainMakerData;

public class TerrainMakerTool 
{
    //��TerrainPieceDirection����Ӧ��vector3���б� dict
    private Dictionary<TerrainPieceDirection, Vector3> m_Enum2V3DirectionDict = new Dictionary<TerrainPieceDirection, Vector3>
    {
        {TerrainPieceDirection.Left,Vector3.left },
        {TerrainPieceDirection.Right,Vector3.right },
        {TerrainPieceDirection.Forward,Vector3.forward},
        {TerrainPieceDirection.Back,Vector3.back },
    };
    //��vector3����Ӧ��TerrainPieceDirection���б� dict
    private Dictionary<Vector3, TerrainPieceDirection> m_V32EnumDirectironDict = new Dictionary<Vector3, TerrainPieceDirection>
    {
        {Vector3.left, TerrainPieceDirection.Left},
        {Vector3.right, TerrainPieceDirection.Right},
        {Vector3.forward, TerrainPieceDirection.Forward},
        {Vector3.back, TerrainPieceDirection.Back},
    };

    public Dictionary<TerrainPieceDirection, Vector3> Enum2Vector3Direction { get { return m_Enum2V3DirectionDict; } }
    public Dictionary<Vector3, TerrainPieceDirection> V32EnumDirectionDict { get { return m_V32EnumDirectironDict; } }

    private TerrainMakerEditorWindow m_Root;

    public void Init(TerrainMakerEditorWindow root)
    {
        root = m_Root;
    }

    public void UnInit()
    {

    }
}
