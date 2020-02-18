GameDefine = {}

-------------------------- 类型定义 --------------------------

GameDefine.Framerate = 30 --帧率

--动向  （常量）
GameDefine.Motion = 
{
    Not     = 0,        --不可移动
    Flat    = 1,        --平面
    Forward = 2,        --前
    Back    = 3,        --后
    Left    = 4,        --左
    Right   = 5,        --右
    Up      = 6,        --上升
    Down    = 7,        --下沉
}

--方向 （向量）
GameDefine.Direction = 
{
    Forward = Vector3.forward,  --前
    Back    = Vector3.back,     --后
    Left    = Vector3.left,     --左
    Right   = Vector3.right,    --右
    Up      = Vector3.up,       --上
    Down    = Vector3.down,     --下
}
   
--方向
GameDefine.DirectionToMotion = 
{
    [GameDefine.Direction.Forward] = GameDefine.Motion.Forward,     --前
    [GameDefine.Direction.Back] = GameDefine.Motion.Back,           --后
    [GameDefine.Direction.Left] = GameDefine.Motion.Left,           --左
    [GameDefine.Direction.Right] = GameDefine.Motion.Right,         --右
    [GameDefine.Direction.Up] = GameDefine.Motion.Up,               --上
    [GameDefine.Direction.Down] = GameDefine.Motion.Down,           --下
}

GameDefine.MotionToDirection = 
{
    [GameDefine.Motion.Forward] = GameDefine.Direction.Forward,     --前
    [GameDefine.Motion.Back] = GameDefine.Direction.Back,           --后
    [GameDefine.Motion.Left] = GameDefine.Direction.Left,           --左
    [GameDefine.Motion.Right] = GameDefine.Direction.Right,         --右
    [GameDefine.Motion.Up] = GameDefine.Direction.Up,               --上
    [GameDefine.Motion.Down] = GameDefine.Direction.Down,           --下
}

--空间
GameDefine.Space = 
{
    Once    = 0,    --单一
    Loop    = 1,    --循环   
}

-- 轴向
GameDefine.Axial =
{
    X = 0,  --x轴
    Y = 1,  --y轴
    Z = 2,  --z轴
}

-- 属性
GameDefine.Attribute =
{
    Moon    = 0,    --月
    Star    = 1,    --星
    Sun     = 2,    --日
}

-- 阵法
GameDefine.Formation = 
{
    UpGrade = 1,    --升级
    Close   = 2,    --拉近
}

-------------------------- 数值定义 --------------------------

--地址
GameDefine.Path = 
{
    Prefab = 
    {
        TerrainPiece = "Prefab/TerrainPiece",
        SidePiece = "Prefab/SidePiece",
        Terrain = "Prefab/Terrain",
        Character = "Prefab/Character",
    }
}


--UI
GameDefine.UI = 
{
    MainUI  = "MainUI",
    WinUI   = "WinUI", 
    FloorUI = "FloorUI",
}


--颜色
GameDefine.Color =
{
    --层
    Floor = 
    {
        Current = Color.New(1,1,1,1),
        Other = Color.New(0.8,0.8,0.8,0.4),
    },

    --地块
    Piece = 
    {
        Current = Color.New(0.8,0.8,0.8,1),
        End = Color.New(0.82,0.3,0.3,1),   --终点 
    },
    

    --指向片
    Side = 
    {
        Current = Color.New(1,1,0.2,1),   
        Other   = Color.New(0.3,0.3,0.3,1),
    },

    UI = 
    {
        Floor = 
        {
            Current = Color.New(0.82,0.3,0.3,0.5),   
            Other   = Color.New(1,1,1,0.5),
        }
    }
}

--建筑
GameDefine.Building = 
{
    --地块大小
    Size   =           
    {
        Width       = 4,    --宽
        Height      = 4,    --高
        Thickness   = 0.1,  --厚
    },
    --地块大小
    Gap    =           
    {
        Width   = 1,
        Height  = 1, 
    },
    --层高
    FloorHeight = 1.6,       
}

--动画
GameDefine.Tween = 
{
    Originate       = 8,    --起始高度
    Move            = 0.6 * GameDefine.Framerate,    --显示时间（S）
}

--有效范围 等级-格子数
GameDefine.EffectiveRange =
{
    [1] = 2,
    [2] = 4
}