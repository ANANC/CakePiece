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
        public string PieceMaterialPath;
        public string SideRootPath;
        public string SideMaterialPath;
        public string DownPath;
        public string UpPath;
        public string BuildingRootPath;
    }

    public class BuildingInfo : CloneHelper.BaseCloneObject  //建筑
    {
        public Vector3 TerrainSize;     //地块大小
        public Vector3 IntervalSize;    //地块间隔大小
        public float SideShiftingValue;    //地块方向块偏移距离
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
        public Color Piece_ArriveAround;    //当前站立地块可以到底的周围地块颜色
        public Color Piece_Other;   //非站立地块颜色
        public Color Piece_End;     //终点地块颜色

        public Color Side_Current;  //当前站立地块的指向片颜色
        public Color Side_Other;    //非站立地块的指向片颜色
    }

    public class SceneArtInfo : CloneHelper.BaseCloneObject //场景表现
    {
        public int FirstFloorShaderLayer;     //首逻辑层的shader层级
        public int FloorShaderLayerInterval;  //逻辑层之间shader层级间隔
    }


    //-- 逻辑数据

    public enum TerrainPieceDirection
    {
        Left,       // x = 1
        Right,      // x = -1
        Forward,    // z = 1
        Back,       // z = -1
        Up,         // y = 1
        Down,       // y = -1
    }

    public class TerrainPieceInfo   //地块基本信息
    {
        public Vector3 LogicPosition;   //逻辑位置
        public Vector3 WorldPosition;   //世界位置

        public TerrainPieceArtInfo ArtInfo; //美术表现

        public Dictionary<TerrainPieceDirection, bool> DirectionFlagDict;   //方向标记列表 dict
        public Dictionary<TerrainPieceDirection, int> DirectionMeasureDict;

        public GameObject GameObject;   //gameobject
        public Transform Transform;     //tranform

        public Material PieceMaterial;  //地块 材质

        public Transform[] SideTransforms;  //方向块列表
        public Material[] SideMaterials;    //方向块 材质列表

        public Transform UpFlagTransform;   //向上标记 transfrom
        public Transform DownFlagTransform; //向下标记 transform

        public Transform BuildingRootTransform; //建筑根节点 transform
    }

    public class TerrainPieceArtInfo    //地块表现信息
    {
        public bool IsShowPiece;        //是否显示地块

        public Dictionary<GameObject, TerrainPieceBuildingInfo> BuildingDict;   //建筑列表

        public bool IsCoverBaseInfo;    //是否覆盖基本信息 使用当前信息
        public Color MyColor;           //自己的颜色
    }

    public class TerrainPieceBuildingInfo   //地块建筑信息
    {
        public string ResourcePath;     //资源路径
        public Vector3 Position;
        public Vector3 Scale;
        public Vector3 Rotation;
    }

    public enum FloorShaderLayer    //逻辑层内shader层层级
    {
        Expand = 0,     //扩展
        Player = 1,     //玩家
        Building = 2,   //建筑
        PieceSide = 3,  //地块方向
        Piece = 4,      //地块
    }

}
