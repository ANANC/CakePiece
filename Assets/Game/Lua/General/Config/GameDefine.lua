GameDefine = {}


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

GameDefine.TerrainPieceColor =
{
    [GameDefine.Motion.Not]     = {r = 0, g = 0, b = 0, a = 1},
    [GameDefine.Motion.Flat]    = {r = 0.1, g = 1, b = 1, a = 1},
    [GameDefine.Motion.Up]      = {r = 1, g = 0.1, b = 1, a = 1},
    [GameDefine.Motion.Down]    = {r = 1, g = 1, b = 0.1, a = 1}
}

--地址
GameDefine.Path = 
{
    Prefab = 
    {
        TerrainPiece = "Assets/Game/Resource/Prefab/TerrainPiece.prefab",
        SidePiece = "Assets/Game/Resource/Prefab/SidePiece.prefab",
        Terrain = "Assets/Game/Resource/Prefab/Terrain.prefab",
        Character = "Assets/Game/Resource/Prefab/Character.prefab",
    }
}


--UI
GameDefine.UI = 
{
    MainUI  = "MainUI",
    WinUI   = "WinUI", 
}
