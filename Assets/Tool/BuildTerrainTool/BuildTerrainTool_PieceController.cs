using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildTerrainTool_PieceController : MonoBehaviour
{
    public Material Material;

    public MeshRenderer MeshRenderer;

    public Color Color;

    public Dictionary<Vector3, bool> DirectionDict = new Dictionary<Vector3, bool>()
    {
        {Vector3.left,true },
        {Vector3.right,true },
        {Vector3.forward,true },
        {Vector3.back,true },
        {Vector3.up,false },
        {Vector3.down,false },
    };

}
