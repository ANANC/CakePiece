using ANFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TerrainMakerData;

public class TerrainMakerPieceFactory
{
    private TerrainMakerTool m_Root;
    private TerrainMakerSceneController m_Scene { get { return m_Root.Scene; } }

    private EditorReousrceLoader m_EditorReousrceLoader;

    private GameObject m_ResourceTerrainPiece;  //地块资源GameObject

    public void Init(TerrainMakerTool root)
    {
        m_Root = root;

        m_EditorReousrceLoader = new EditorReousrceLoader();
        m_EditorReousrceLoader.Init();
    }

    public void UnInit()
    {

    }

    /// <summary>
    /// 创建基础地块
    /// </summary>
    /// <param name="logicPosition"></param>
    public TerrainPieceInfo CreateBasisTerrainPiece(Vector3 logicPosition)
    {
        TerrainPieceInfo terrainPieceInfo = new TerrainPieceInfo();

        if (m_ResourceTerrainPiece == null)
        {
            m_ResourceTerrainPiece = m_EditorReousrceLoader.LoadResource<GameObject>(m_Scene.ResourcePath.TerrainPiecePath);
        }

        //logic
        terrainPieceInfo.LogixPosition = logicPosition;

        terrainPieceInfo.DirectionFlagDict = new Dictionary<TerrainPieceDirection, bool>
        {
             {TerrainPieceDirection.Left,true },
             {TerrainPieceDirection.Right,true },
             {TerrainPieceDirection.Forward,true },
             {TerrainPieceDirection.Back,true },
        };

        //art
        GameObject terrainPiece = GameObject.Instantiate(m_ResourceTerrainPiece);
        terrainPiece.name = "x:" + logicPosition.x + " y:" + logicPosition.y + " z:" + logicPosition.z;
        terrainPieceInfo.GameObject = terrainPiece;

        Transform terrainPieceTransform = terrainPiece.transform;
        terrainPieceInfo.Transform = terrainPieceTransform;

        // -- 预制体内的对象
        Transform pieceTransform = terrainPieceTransform.Find(m_Scene.GameObjectPath.PieceMaterialPath);
        Material pieceMaterial = pieceTransform.GetComponent<Renderer>().material;
        terrainPieceInfo.PieceMaterial = pieceMaterial;

        Transform sideRootTransform = terrainPieceTransform.Find(m_Scene.GameObjectPath.SideRootPath);
        int childCount = sideRootTransform.childCount;
        Transform[] sides = new Transform[childCount];
        Material[] sideMaterials = new Material[childCount];
        for (int index = 0; index < sides.Length; index++)
        {
            Transform sideTransform = sideRootTransform.GetChild(index);
            sides[index] = sideTransform;
            Transform sideMaterialPath = sideTransform.Find(m_Scene.GameObjectPath.SideMaterialPath);
            sideMaterials[index] = sideMaterialPath.GetComponent<Renderer>().material;
        }

        Transform upFlagTransform = terrainPieceTransform.Find(m_Scene.GameObjectPath.UpPath);
        terrainPieceInfo.UpFlagTransform = upFlagTransform;

        Transform downFlagTransform = terrainPieceTransform.Find(m_Scene.GameObjectPath.DownPath);
        terrainPieceInfo.DownFlagTransform = downFlagTransform;

        // artInfo
        TerrainPieceArtInfo terrainPieceArtInfo = new TerrainPieceArtInfo();
        terrainPieceArtInfo.IsShowPiece = true;
        terrainPieceArtInfo.IsCoverBaseInfo = false;
        terrainPieceArtInfo.BuildingList = new List<GameObject>();

        terrainPieceInfo.ArtInfo = terrainPieceArtInfo;

        return terrainPieceInfo;
    }

}
