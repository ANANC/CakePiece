GameDefine = {}


--动向
GameDefine.Motion = 
{
    Not     = 0,        --不可移动
    Flat    = 1,        --平面
    Forwark = 2,        --前
    Back    = 3,        --后
    Left    = 4,        --左
    Right   = 5,        --右
    Up      = 6,        --上升
    Down    = 7,        --下沉
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



--地块
GameDefine.TerrainPiece =
{
    -- 可移动方向
    Direction =
    {
        Not     = 0,        --不可移动
        Flat    = 1,        --平面
        Up      = 2,        --上升
        Down    = 3,        --下沉
    },

    --空间
    Space = 
    {
        Once    = 0,    --单一
        Loop    = 1,    --循环   
    }
}

--地形
GameDefine.Terrain =
{
    -- 轴向
    Axial =
    {
        X = 0,  --x轴
        Y = 1,  --y轴
        Z = 2,  --z轴
    }
}

GameDefine.TerrainPieceColor =
{
    [GameDefine.TerrainPiece.Direction.Not]     = {r = 0, g = 0, b = 0, a = 1},
    [GameDefine.TerrainPiece.Direction.Flat]    = {r = 0.1, g = 1, b = 1, a = 1},
    [GameDefine.TerrainPiece.Direction.Up]      = {r = 1, g = 0.1, b = 1, a = 1},
    [GameDefine.TerrainPiece.Direction.Down]    = {r = 1, g = 1, b = 0.1, a = 1}

}

--地址
GameDefine.Path = 
{
    Prefab = 
    {
        TerrainPiece = "Assets/Game/Resource/Prefab/TerrainPiece.prefab",
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
