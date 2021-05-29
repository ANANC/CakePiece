using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TerrainMakerData;

public class TerrainMakerGamePlayController 
{
    private TerrainMakerTool m_Root;
    private TerrainMakerSceneController m_Scene { get { return m_Root.Scene; } }

    private Dictionary<Vector3, TerrainPieceDirection> m_V32EnumDirectironDict; //��vector3����Ӧ��TerrainPieceDirection���б� dict

    public void Init(TerrainMakerTool root)
    {
        m_Root = root;

        m_V32EnumDirectironDict = new Dictionary<Vector3, TerrainPieceDirection>()
        {
            {Vector3.left, TerrainPieceDirection.Left},
            {Vector3.right, TerrainPieceDirection.Right},
            {Vector3.forward, TerrainPieceDirection.Forward},
            {Vector3.back, TerrainPieceDirection.Back},

        };
    }

    public void UnInit()
    {

    }

    //-- �߼�

    /// <summary>
    /// �߼�λ��ת����λ��
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
    /// �Ƿ�ǰվ������Χ���Ե���ĵؿ�
    /// </summary>
    /// <param name="logicPosition"></param>
    /// <returns></returns>
    public bool IsCurrentCanArriveTerrainPiece(Vector3 logicPosition)
    {
        TerrainPieceInfo curTerrainPieceInfo = m_Scene.GetCurrentTerrainPieceInfo();
        Vector3 distance = curTerrainPieceInfo.LogixPosition - logicPosition;

        TerrainPieceDirection terrainPieceDirection;
        if(m_V32EnumDirectironDict.TryGetValue(distance,out terrainPieceDirection))
        {
            bool enable;
            if(curTerrainPieceInfo.DirectionFlagDict.TryGetValue(terrainPieceDirection,out enable))
            {
                return enable;
            }
        }
        return false;
    }

    //-- ����

    /// <summary>
    /// �߼�λ��תΪ��̬��ɫ վ��λ�øı䣬��ɫ����ı�
    /// </summary>
    /// <param name="terrainPiece"></param>
    /// <returns></returns>
    public Color LogicPositionToDynamicColor(TerrainPieceInfo terrainPiece)
    {
        bool IsCurLogicPosition = terrainPiece.LogixPosition == m_Scene.Input.CurLogicPosition;
        bool IsEndLogicPosition = terrainPiece.LogixPosition == m_Scene.Input.EndLogicPosition;
        bool IsCurLayer = terrainPiece.LogixPosition.y == m_Scene.Input.CurLogicPosition.y;
        bool IsCover = terrainPiece.ArtInfo.IsCoverBaseInfo;

        //���յؿ�
        if (IsEndLogicPosition)
        {
            return m_Scene.Color.Piece_End;
        }
        //��ǰ�ؿ�
        if (IsCurLogicPosition)
        {
            return m_Scene.Color.Piece_Current;
        }

        Color color = new Color();
        //�ؿ��Դ���ɫ
        if (IsCover)
        {
            Color myColor = terrainPiece.ArtInfo.MyColor;
            color = new Color(myColor.r, myColor.g, myColor.b, 1);
        }

        //��ǰ�㼶
        if (IsCurLayer)
        {
            //ʹ���Դ���ɫ+�㼶͸����
            if(IsCover)
            {
                color.a = m_Scene.Color.Floor_Current.a;
                return color;
            }
            //�ɵ������Χ�ؿ�
            if(IsCurrentCanArriveTerrainPiece(terrainPiece.LogixPosition))
            {
                return m_Scene.Color.Piece_ArriveAround;
            }else
            {
                return m_Scene.Color.Piece_Other;
            }
        }
        //�����㼶
        else
        {
            //ʹ���Դ���ɫ+�㼶͸����
            if (IsCover)
            {
                color.a = m_Scene.Color.Floor_Other.a;
                return color;
            }
            return m_Scene.Color.Floor_Other;
        }

    }

}
