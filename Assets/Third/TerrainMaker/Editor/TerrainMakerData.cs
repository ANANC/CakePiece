using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图编辑器 数据结构
/// </summary>
public class TerrainMakerData
{   
    //-- 配置数据

    public class ResourcePathInfo : CloneHelper.BaseCloneObject  //资源路径
    {
        public string TerrainPath;      //地形
        public string TerrainPiecePath; //地块
    }

    public class GameObjectPathInfo : CloneHelper.BaseCloneObject   //gameObject内路径
    {
        public string PiecePath;
        public string SidePath;
        public string DownPath;
        public string UpPath;
    }

    public class BuildingInfo : CloneHelper.BaseCloneObject  //建筑
    {
        public Vector3 TerrainSize;     //地块大小
        public Vector3 IntervalSize;    //地块间隔大小
    }

    public class GamePlayInfo : CloneHelper.BaseCloneObject  //玩法
    {
        public Vector3 BirthLogicPosition;  //出生逻辑位置
        public bool HasEndLogicPosition;    //是否有结束逻辑位置
        public Vector3 EndLoigcPosition;    //结束逻辑位置
    }

    public class TweenInfo : CloneHelper.BaseCloneObject //动画
    {
        public float Originate;     //起始高度
        public float MoveSpeed;     //移动速度 （s）
    }

    public class ColorInfo : CloneHelper.BaseCloneObject  //颜色
    {
        public Color Floor_Current; //当前站立层颜色
        public Color Floor_Other;   //非站立层颜色

        public Color Piece_Current; //当前站立地块颜色
        public Color Piece_Other;   //非站立地块颜色
        public Color Piece_End;     //终点地块颜色

        public Color Side_Current;  //当前站立地块的指向片颜色
        public Color Side_Other;    //非站立地块的指向片颜色
    }

    //-- 逻辑数据

    public enum TerrainPieceDirection
    {
        Left,
        Right,
        Up,
        Down,
    }


    public class TerrainPieceInfo
    {
        public Vector3 LogixPosition;   //逻辑位置
        public Vector3 WorldPosition;   //世界位置

        public bool IsShowPiece;        //是否显示地块
        public List<GameObject> BuildingList;   //建筑列表

        public Dictionary<TerrainPieceDirection, bool> DirectionFlagDict;   //方向标记列表 dict

        public GameObject GameObject;   //gameobject
        public Transform Transform;     //tranform

        public Material PieceMaterial;  //地块 材质

        public Transform[] SideTransforms;  //方向块列表
        public Material[] SideMaterials;    //方向块 材质列表

        public Transform UpFlagTransform;   //向上标记 transfrom
        public Transform DownFlagTransform; //向下标记 transform
    }
}
