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
        public string PiecePath;
        public string SidePath;
        public string DownPath;
        public string UpPath;
    }

    public class BuildingInfo : CloneHelper.BaseCloneObject  //����
    {
        public Vector3 TerrainSize;     //�ؿ��С
        public Vector3 IntervalSize;    //�ؿ�����С
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
        public Color Piece_Other;   //��վ���ؿ���ɫ
        public Color Piece_End;     //�յ�ؿ���ɫ

        public Color Side_Current;  //��ǰվ���ؿ��ָ��Ƭ��ɫ
        public Color Side_Other;    //��վ���ؿ��ָ��Ƭ��ɫ
    }

    //-- �߼�����

    public enum TerrainPieceDirection
    {
        Left,
        Right,
        Up,
        Down,
    }


    public class TerrainPieceInfo
    {
        public Vector3 LogixPosition;   //�߼�λ��
        public Vector3 WorldPosition;   //����λ��

        public bool IsShowPiece;        //�Ƿ���ʾ�ؿ�
        public List<GameObject> BuildingList;   //�����б�

        public Dictionary<TerrainPieceDirection, bool> DirectionFlagDict;   //�������б� dict

        public GameObject GameObject;   //gameobject
        public Transform Transform;     //tranform

        public Material PieceMaterial;  //�ؿ� ����

        public Transform[] SideTransforms;  //������б�
        public Material[] SideMaterials;    //����� �����б�

        public Transform UpFlagTransform;   //���ϱ�� transfrom
        public Transform DownFlagTransform; //���±�� transform
    }
}
