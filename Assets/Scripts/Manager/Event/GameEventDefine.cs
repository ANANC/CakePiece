using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventDefine 
{

    // 事件名命名规范：XXXEvent
    // 事件信息命名规范：XXXEventInfo

    //主玩家移动
    public const string MainPlayerMoveEvent = "MainPlayerMoveEvent";    
    public class MainPlayerMoveEventInfo : Stone_EventObject.EventCallbackInfo
    {
        public Vector3 OldLogicPosition;    //旧逻辑位置
        public Vector3 NewLogicPosition;    //新逻辑位置
        public Vector3 NewArtPosition;      //新美术位置
    }

    //地块添加
    public const string PieceCreateEvent = "PieceCreateEvent";
    public class PieceCreateEventInfo : Stone_EventObject.EventCallbackInfo
    {
        public Vector3 LogicPos;    //逻辑位置
    }

    //地块删除
    public const string PieceDestroyEvent = "PieceDestroyEvent";
    public class PieceDestroyEventInfo : Stone_EventObject.EventCallbackInfo
    {
        public Vector3 LogicPos;    //逻辑位置
    }

    //地块方向改变
    public const string PieceEnableDirectionChangeEvent = "PieceEnableDirectionChangeEvent";
    public class PieceEnableDirectionChangeEventInfo : Stone_EventObject.EventCallbackInfo
    {
        public Vector3 LogicPos;    //逻辑位置
    }

    //游戏重置请求
    public const string GameResetRequestEvent = "GameResetRequestEvent";

    //三维空间 层移动表现 进入站立
    public const string ThreeDimensionalSpace_FloorArt_StandInPieceEvent = "ThreeDimensionalSpace_FloorArt_StandInPieceEvent";
    public class ThreeDimensionalSpace_FloorArt_StandInPieceEventInfo : Stone_EventObject.EventCallbackInfo
    {
        public Vector3 LogicPos;    //逻辑位置
    }

    //三维空间 层移动表现 离开站立
    public const string ThreeDimensionalSpace_FloorArt_StandOutPieceEvent = "ThreeDimensionalSpace_FloorArt_StandOutPieceEvent";
    public class ThreeDimensionalSpace_FloorArt_StandOutPieceEventInfo : Stone_EventObject.EventCallbackInfo
    {
        public Vector3 LogicPos;    //逻辑位置
    }

    //三维空间 层移动表现 提升层
    public const string ThreeDimensionalSpace_FloorArt_UpFloorEvent = "ThreeDimensionalSpace_FloorArt_UpFloorEvent"; 
    public class ThreeDimensionalSpace_FloorArt_UpFloorEventInfo : Stone_EventObject.EventCallbackInfo
    {
        public int UpFloor; //层
        public float TweenUpdateTime;    //动画更新时间（单位：s）
        public float TweenMoveLength;    //动画移动距离（单位：米）
    }

    //三维空间 层移动表现 下降层
    public const string ThreeDimensionalSpace_FloorArt_DownFloorEvent = "ThreeDimensionalSpace_FloorArt_DownFloorEvent";  
    public class ThreeDimensionalSpace_FloorArt_DownFloorEventInfo : Stone_EventObject.EventCallbackInfo
    {
        public int DownFloor; //层
        public float TweenUpdateTime;    //动画更新时间（单位：s）
        public float TweenMoveLength;    //动画移动距离（单位：米）
    }
 
    //三维空间 层移动表现 显示层
    public const string ThreeDimensionalSpace_FloorArt_ShowFloorEvent = "ThreeDimensionalSpace_FloorArt_ShowFloorEvent"; 
    public class ThreeDimensionalSpace_FloorArt_ShowFloorEventInfo : Stone_EventObject.EventCallbackInfo
    {
        public int Floor; //层
    }

    //三维空间 层移动表现 隐藏层
    public const string ThreeDimensionalSpace_FloorArt_HideFloorEvent = "ThreeDimensionalSpace_FloorArt_HideFloorEvent";  
    public class ThreeDimensionalSpace_FloorArt_HideFloorEventInfo : Stone_EventObject.EventCallbackInfo
    {
        public int Floor; //层
    }

    //三维空间 层移动表现 根据逻辑位置重置美术位置
    public const string ThreeDimensionalSpace_FloorArt_ResetArtPositionFromLogicEvent = "ThreeDimensionalSpace_FloorArt_ResetArtPositionFromLogicEvent";
    public class ThreeDimensionalSpace_FloorArt_ResetArtPositionFromLogicEventInfo : Stone_EventObject.EventCallbackInfo
    {
        public int Floor; //层
    }

    //三维空间 玩家表现 隐藏
    public const string ThreeDimensionalSpace_PlayerArt_HideEvent = "ThreeDimensionalSpace_PlayerArt_HideEvent";
    public class ThreeDimensionalSpace_PlayerArt_HideEventInfo : Stone_EventObject.EventCallbackInfo
    {
        public int PlayerId; //玩家id
    }

    //三维空间 玩家表现 显示
    public const string ThreeDimensionalSpace_PlayerArt_ShowEvent = "ThreeDimensionalSpace_PlayerArt_ShowEvent";
    public class ThreeDimensionalSpace_PlayerArt_ShowEventInfo : Stone_EventObject.EventCallbackInfo
    {
        public int PlayerId; //玩家id
    }

    //三维空间 玩家表现 美术位置
    public const string ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEvent = "ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEvent";
    public class ThreeDimensionalSpace_PlayerArt_UpdateArtPositionEventInfo : Stone_EventObject.EventCallbackInfo
    {
        public int PlayerId;        //玩家id
        public Vector3 ArtPosition; //美术位置
    }

    //三维空间 玩家表现 美术位置
    public const string ThreeDimensionalSpace_PlayerArt_DownDeathEvent = "ThreeDimensionalSpace_PlayerArt_DownDeathEvent";
    public class ThreeDimensionalSpace_PlayerArt_DownDeathEventInfo : Stone_EventObject.EventCallbackInfo
    {
        public int PlayerId;        //玩家id
        public float DownTime;      //下降时间
        public float DownDistance;  //下降距离
    }
}
