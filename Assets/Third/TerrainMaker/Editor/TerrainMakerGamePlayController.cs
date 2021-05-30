using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TerrainMakerData;

public class TerrainMakerGamePlayController 
{
    private TerrainMakerEditorWindow m_Root;
    private TerrainMakerSceneController m_Scene { get { return m_Root.Scene; } }
    private TerrainMakerTool m_Tool { get { return m_Root.Tool; } }

    public void Init(TerrainMakerEditorWindow root)
    {
        m_Root = root;
    }

    public void UnInit()
    {

    }

    //-- 逻辑

    /// <summary>
    /// 逻辑位置转世界位置
    /// </summary>
    /// <param name="logicPosition"></param>
    /// <returns></returns>
    public Vector3 LogicPositionToWorldPosition(Vector3 logicPosition)
    {
        Vector3 worldPosition = new Vector3(
            logicPosition.x * (m_Scene.Building.TerrainSize.x + m_Scene.Building.IntervalSize.x),
            logicPosition.y * -m_Scene.Building.IntervalSize.y,
            logicPosition.z * (m_Scene.Building.TerrainSize.z + m_Scene.Building.IntervalSize.z)
            );

        return worldPosition;
    }

    /// <summary>
    /// 是否当前站立块周围可以到达的地块
    /// </summary>
    /// <param name="logicPosition"></param>
    /// <returns></returns>
    public bool IsCurrentCanArriveTerrainPiece(Vector3 logicPosition)
    {
        TerrainPieceInfo curTerrainPieceInfo = m_Scene.GetCurrentTerrainPieceInfo();
        Vector3 distance = curTerrainPieceInfo.LogicPosition - logicPosition;

        TerrainPieceDirection terrainPieceDirection;
        if(m_Tool.V32EnumDirectionDict.TryGetValue(distance,out terrainPieceDirection))
        {
            bool enable;
            if(curTerrainPieceInfo.DirectionFlagDict.TryGetValue(terrainPieceDirection,out enable))
            {
                return enable;
            }
        }
        return false;
    }

    //-- 表现

    /// <summary>
    /// 逻辑位置转为动态颜色 站立位置改变，颜色跟随改变
    /// </summary>
    /// <param name="terrainPiece"></param>
    /// <returns></returns>
    public Color LogicPositionToDynamicColor(TerrainPieceInfo terrainPiece)
    {
        bool IsCurLogicPosition = terrainPiece.LogicPosition == m_Scene.Input.CurLogicPosition;
        bool IsEndLogicPosition = terrainPiece.LogicPosition == m_Scene.Input.EndLogicPosition;
        bool IsCurLayer = terrainPiece.LogicPosition.y == m_Scene.Input.CurLogicPosition.y;
        bool IsCover = terrainPiece.ArtInfo.IsCoverBaseInfo;

        //最终地块
        if (IsEndLogicPosition)
        {
            return m_Scene.Color.Piece_End;
        }
        //当前地块
        if (IsCurLogicPosition)
        {
            return m_Scene.Color.Piece_Current;
        }

        Color color = new Color();
        //地块自带颜色
        if (IsCover)
        {
            Color myColor = terrainPiece.ArtInfo.MyColor;
            color = new Color(myColor.r, myColor.g, myColor.b, 1);
        }

        //当前层级
        if (IsCurLayer)
        {
            //使用自带颜色+层级透明度
            if(IsCover)
            {
                color.a = m_Scene.Color.Floor_Current.a;
                return color;
            }
            //可到达的周围地块
            if(IsCurrentCanArriveTerrainPiece(terrainPiece.LogicPosition))
            {
                return m_Scene.Color.Piece_ArriveAround;
            }else
            {
                return m_Scene.Color.Piece_Other;
            }
        }
        //其他层级
        else
        {
            //使用自带颜色+层级透明度
            if (IsCover)
            {
                color.a = m_Scene.Color.Floor_Other.a;
                return color;
            }
            return m_Scene.Color.Floor_Other;
        }

    }

}
