using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ͼ�༭�� ���ݽṹ
/// </summary>
public class TerrainMakerData
{   
    //-- ��������

    public class ResourcePathInfo : CloneHelper.BaseCloneObject  //��Դ·��
    {
        public string TerrainPath;      //����
        public string TerrainPiecePath; //�ؿ�
    }

    public class GameObjectPathInfo : CloneHelper.BaseCloneObject   //gameObject��·��
    {
        public string PieceMaterialPath;
        public string SideRootPath;
        public string SideMaterialPath;
        public string DownPath;
        public string UpPath;
    }

    public class BuildingInfo : CloneHelper.BaseCloneObject  //����
    {
        public Vector3 TerrainSize;     //�ؿ��С
        public Vector3 IntervalSize;    //�ؿ�����С
        public float SideShiftingValue;    //�ؿ鷽���ƫ�ƾ���
    }

    public class GamePlayInfo : CloneHelper.BaseCloneObject  //�淨
    {
        public Vector3 BirthLogicPosition;  //�����߼�λ��
        public bool HasEndLogicPosition;    //�Ƿ��н����߼�λ��
        public Vector3 EndLoigcPosition;    //�����߼�λ��
    }

    public class TweenInfo : CloneHelper.BaseCloneObject //����
    {
        public float Originate;     //��ʼ�߶�
        public float MoveSpeed;     //�ƶ��ٶ� ��s��
    }

    public class ColorInfo : CloneHelper.BaseCloneObject  //��ɫ
    {
        public Color Floor_Current; //��ǰվ������ɫ
        public Color Floor_Other;   //��վ������ɫ

        public Color Piece_Current; //��ǰվ���ؿ���ɫ
        public Color Piece_ArriveAround;    //��ǰվ���ؿ���Ե��׵���Χ�ؿ���ɫ
        public Color Piece_Other;   //��վ���ؿ���ɫ
        public Color Piece_End;     //�յ�ؿ���ɫ

        public Color Side_Current;  //��ǰվ���ؿ��ָ��Ƭ��ɫ
        public Color Side_Other;    //��վ���ؿ��ָ��Ƭ��ɫ
    }

    //-- �߼�����

    public enum TerrainPieceDirection
    {
        Left,       // x = 1
        Right,      // x = -1
        Forward,    // z = 1
        Back,       // z = -1
    }

    public class TerrainPieceInfo   //�ؿ������Ϣ
    {
        public Vector3 LogicPosition;   //�߼�λ��
        public Vector3 WorldPosition;   //����λ��

        public TerrainPieceArtInfo ArtInfo; //��������

        public Dictionary<TerrainPieceDirection, bool> DirectionFlagDict;   //�������б� dict

        public GameObject GameObject;   //gameobject
        public Transform Transform;     //tranform

        public Material PieceMaterial;  //�ؿ� ����

        public Transform[] SideTransforms;  //������б�
        public Material[] SideMaterials;    //����� �����б�

        public Transform UpFlagTransform;   //���ϱ�� transfrom
        public Transform DownFlagTransform; //���±�� transform
    }

    public class TerrainPieceArtInfo    //�ؿ������Ϣ
    {
        public bool IsShowPiece;        //�Ƿ���ʾ�ؿ�

        public List<GameObject> BuildingList;   //�����б�

        public bool IsCoverBaseInfo;    //�Ƿ񸲸ǻ�����Ϣ ʹ�õ�ǰ��Ϣ
        public Color MyColor;           //�Լ�����ɫ
    }
}
