using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildTerrainTool_PieceController : MonoBehaviour
{
    public Material Material;

    public MeshRenderer MeshRenderer;

    public Color Color;

    [Header("是否下个对象随机颜色")]
    public bool isNextRandonColor = true;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }



}
